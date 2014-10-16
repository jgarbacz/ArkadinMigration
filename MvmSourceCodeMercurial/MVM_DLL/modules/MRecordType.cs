using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;
using Antlr.Runtime;
using Antlr.Runtime.Tree;
using MvmScript;
using NLog;

//- need to require that any and all printing must happen at the end of the last stage...

//- if a rtype does not define <database_lookup> then it cannot fetch parent from db; pass on parent relationships untouched to next stage
// - then final stage will error if parent fields are not null but have no parent, unless can fetch from db

//startup parameters: -phases=phase1 -phases=phase1:phase2 -phases=* -pause -runall

//FIXME: notice fields that are passed but never read, always overwritten (diff type of ignored field)
//  current: ignored when name is not an output db field name
//  case: should NOT ignore when name is an invalid output field but it is an input to a map expression
//  case: should ignore when name is valid output db field that is always overwritten
//  want to ignore when:
//    - field is never used as output field or input to another map expression.. but what if other expression goes nowhere?
//    - need a full tree of expressions that traces the path of each input field to each table field
//    - ignored flag should take into account whether field is used as a map/def input, not just if it appears in an output table
//FIXME: add MC_EARLYDATE, along with structure for GLOBAL.default_xxx that will only validate once at setup time

//TODO: add reverse t_enum_data mappings if needed

//    <joins>
//      <join type="inner">
//        <name>RecordTypeName</name>
//        <field name="nm_login">OBJECT.nm_login</field>
//      </join>
//    </joins>

//      <field_mapping>
//        <map file="xxx"/>
//      </field_mapping>

//  in non-generated part of account mapping, check that parent is valid account type for us

//  hidden fields: don't add them to the rdef, or pre-validate them, but can take them as input if user adds them
//  validation: only do MAPPING ones in pre-validation, along with datatype checks

//  flag for input fields to disable validation on it (e.g. passing in an enum that is already the id_enum_data value)
//  - this ties in with hidden fields

namespace MVM
{
    // "mapping" refers to field_mapping elements and "child_mapping" refers to child_field_mapping elements.
    // The only difference between them is that only child_mapping can use CHILD.x syntax; both domains can
    // use OBJECT.x, PARENT.x, ANCESTOR.x, and the various MC_ functions.
    public enum SyntaxDomain { mapping, child_mapping }
    public enum InputElementType { sysdate, sysday, maxdate, guid, boolean, proc, sequence, mapping, parent_field, object_field, field, constant }
    public enum Visibility { Private, Public }

    public class OutputField
    {
        public string recordType = null;
        public int eventId = 0;
        public int mappingId = 0;
        public string objectSyntax;
        public string objectIdSyntax;
        public string fieldName;
        public AstNode node;
        public bool forcePrivate;
        public List<InputElement> inputs;
        public List<InputElement> allInputs;
        public OutputField(string syntax, string idSyntax, string field, AstNode node)
        {
            this.objectSyntax = syntax;
            this.objectIdSyntax = idSyntax;
            this.fieldName = field;
            this.node = node;
            this.forcePrivate = false;
            this.inputs = new List<InputElement>();
            this.allInputs = new List<InputElement>();
        }
        public bool IsDirectFieldCopy()
        {
            return this.inputs.Count == 1 && this.inputs[0].IsFieldReference() && this.inputs[0].fieldInputs.Count == 1;
        }
        public bool IsDirectParentFieldCopy()
        {
            return this.inputs.Count == 1 && this.inputs[0].IsParentFieldReference() && this.inputs[0].fieldInputs.Count == 1;
        }
        public bool IsConstantAssignment()
        {
            return this.inputs.Count == 1 && this.inputs[0].type == InputElementType.constant;
        }
        public bool IsSafeToSkipValidation()
        {
            // Skip validation on stuff we generate internally like guids, sequences, etc
            return
                (
                    this.inputs.Count == 1 &&
                    this.inputs[0].type.In(InputElementType.guid, InputElementType.sequence, InputElementType.sysdate, InputElementType.sysday, InputElementType.maxdate)
                )
                ||
                (
                    this.inputs.Count == 1 && this.inputs[0].type == InputElementType.parent_field  // PARENT.x
                )
                ||
                (
                    this.inputs.Count == 3 && this.inputs[2].type == InputElementType.mapping  // MC_MAPPING(...)
                )
                ;
        }
    }

    public struct InputElement
    {
        public InputElementType type;
        public AstNode node;
        public List<InputElement> inputs;
        public List<OutputField> fieldInputs;
        public string fieldName;
        public InputElement(InputElementType type, AstNode node)
        {
            this.type = type;
            this.node = node;
            this.fieldName = null;
            this.inputs = new List<InputElement>();
            this.fieldInputs = new List<OutputField>();
        }
        public InputElement(string fieldName, AstNode node)
        {
            if (node.Name.In("OBJECT", "CHILD"))
            {
                this.type = InputElementType.object_field;
            }
            else if (node.Name.Equals("PARENT"))
            {
                this.type = InputElementType.parent_field;
            }
            else
            {
                this.type = InputElementType.field;
            }
            this.node = node;
            this.fieldName = fieldName;
            this.inputs = new List<InputElement>();
            this.fieldInputs = new List<OutputField>();
        }
        public bool IsFieldReference()
        {
            return this.type == InputElementType.object_field;
        }
        public bool IsParentFieldReference()
        {
            return this.type == InputElementType.parent_field;
        }
        public bool IsPrivateByDefault()
        {
            return this.type.In(InputElementType.sequence, InputElementType.guid);
        }
    }

    public class MRecordType: IModuleSetup, IModuleRun
    {
        public string name;
        public string displayName;
        public Visibility visibility;
        private string phase;
        private string stage;
        private string nmspace;
        private string parentNamespace;
        private string baseRecordType;
        private string fileRegex;
        private string selectClause;
        private string fromClause;
        private string parentInclusion;
        private string sortKey;
        private string stableSort;

        private XmlElement baseRecordTypeElem;
        private XmlElement phaseElem;
        private XmlElement stageElem;
        private XmlElement namespaceElem;
        private XmlElement parentNamespaceElem;
        private XmlElement fileRegexElem;
        private XmlElement fieldsElem;
        private XmlElement primaryKeyElem;
        private XmlElement parentKeyElem;
        private XmlElement eventsElem;
        private XmlElement databaseLookupElem;
        private XmlElement parentInclusionElem;
        private XmlElement sortKeyElem;
        private XmlElement stableSortElem;

        private static Regex trueRegex = new Regex(@"t|1|y|on", RegexOptions.IgnoreCase);
        private static Regex falseRegex = new Regex(@"f|0|n|off", RegexOptions.IgnoreCase);

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            // TEMP CODE
            Logger logger = LogManager.GetCurrentClassLogger();
            
            XmlElement parent = null;
            XmlNode current_node = null;
            MRecordType m = new MRecordType();
            m.name = me.GetAttribute("name").Trim();

            Dictionary<string, RecordTypeNode> allNodes;
            if (mc.globalContext.HasNamedClassInst("MC_RECORD_TYPE_NODES"))
            {
                allNodes = (Dictionary<string, RecordTypeNode>)mc.globalContext.GetNamedClassInst("MC_RECORD_TYPE_NODES");
            }
            else
            {
                allNodes = new Dictionary<string, RecordTypeNode>();
                mc.globalContext.SetNamedClassInst("MC_RECORD_TYPE_NODES", allNodes);
            }

            // save the record type
            RecordTypeNode node = new RecordTypeNode(mc, m, me);
            if (allNodes.ContainsKey(m.name))
            {
                string exists = me.GetAttributeDefaulted("exists", "error").Trim();
                if (exists.EqualsIgnoreCase("return"))
                {
                    return;
                }
                throw new Exception("Record type [" + m.name + "] was defined twice!");
            }
            allNodes[m.name] = node;

            m.baseRecordTypeElem = me.SelectSingleElem("./base_record_type");
            current_node = m.baseRecordTypeElem;
            if (m.baseRecordTypeElem == null)
            {
                current_node = me.CreateTextElement("base_record_type", "");
                me.PrependChildImport((XmlElement)current_node);
            }
            m.baseRecordType = me.SelectNodeInnerText("./base_record_type", "").Trim();

            // inherit from base record type
            if (!m.baseRecordType.Equals(""))
            {
                parent = allNodes[m.baseRecordType].GetXml();
            }

            string visibility = me.GetAttributeDefaulted("visibility", "").Trim();
            if (visibility.IsNullOrEmpty() && parent != null)
            {
                visibility = parent.GetAttributeDefaulted("visibility", "").Trim();
            }
            m.visibility = visibility.EqualsIgnoreCase("private") ? Visibility.Private : Visibility.Public;
            
            string display = me.GetAttributeDefaulted("display", "").Trim();
            if (!display.IsNullOrEmpty())
            {
                ;
            }
            m.displayName = display.IsNullOrEmpty() ? m.name : display;

            m.phaseElem = me.SelectSingleElem("./phase");
            if (m.phaseElem != null)
            {
                current_node = m.phaseElem;
            }
            if (m.phaseElem == null && parent != null)
            {
                m.phaseElem = parent.SelectSingleElem("./phase");
                if (m.phaseElem != null)
                {
                    var copy = me.OwnerDocument.ImportNode(m.phaseElem, true);
                    me.InsertAfter(copy, current_node);
                    current_node = copy;
                }
            }
            m.phase = me.SelectNodeInnerText("./phase", "").Trim();

            m.stageElem = me.SelectSingleElem("./stage");
            if (m.stageElem != null)
            {
                current_node = m.stageElem;
            }
            if (m.stageElem == null && parent != null)
            {
                m.stageElem = parent.SelectSingleElem("./stage");
                if (m.stageElem != null)
                {
                    var copy = me.OwnerDocument.ImportNode(m.stageElem, true);
                    me.InsertAfter(copy, current_node);
                    current_node = copy;
                }
            }
            m.stage = me.SelectNodeInnerText("./stage", "").Trim();

            m.namespaceElem = me.SelectSingleElem("./namespace");
            if (m.namespaceElem != null)
            {
                current_node = m.namespaceElem;
            }
            if (m.namespaceElem == null && parent != null)
            {
                m.namespaceElem = parent.SelectSingleElem("./namespace");
                if (m.namespaceElem != null)
                {
                    var copy = me.OwnerDocument.ImportNode(m.namespaceElem, true);
                    me.InsertAfter(copy, current_node);
                    current_node = copy;
                }
            }
            m.nmspace = me.SelectNodeInnerText("./namespace", "").Trim();
            if (m.nmspace.IsNullOrEmpty())
            {
                m.nmspace = m.name;
            }

            m.fileRegexElem = me.SelectSingleElem("./file_regex");
            if (m.fileRegexElem != null)
            {
                current_node = m.fileRegexElem;
            }
            if (m.fileRegexElem == null && parent != null)
            {
                m.fileRegexElem = parent.SelectSingleElem("./file_regex");
                if (m.fileRegexElem != null)
                {
                    var copy = me.OwnerDocument.ImportNode(m.fileRegexElem, true);
                    me.InsertAfter(copy, current_node);
                    current_node = copy;
                }
            }
            m.fileRegex = me.SelectNodeInnerText("./file_regex", "").Trim();

            m.fieldsElem = me.SelectSingleElem("./fields");
            if (m.fieldsElem != null)
            {
                current_node = m.fieldsElem;
            }
            if (m.fieldsElem == null && parent != null)
            {
                m.fieldsElem = parent.SelectSingleElem("./fields");
                if (m.fieldsElem != null)
                {
                    var copy = me.OwnerDocument.ImportNode(m.fieldsElem, true);
                    me.InsertAfter(copy, current_node);
                    current_node = copy;
                }
            }
            
            m.primaryKeyElem = me.SelectSingleElem("./primary_key");
            if (m.primaryKeyElem != null)
            {
                current_node = m.primaryKeyElem;
            }
            if (m.primaryKeyElem == null && parent != null)
            {
                m.primaryKeyElem = parent.SelectSingleElem("./primary_key");
                if (m.primaryKeyElem != null)
                {
                    var copy = me.OwnerDocument.ImportNode(m.primaryKeyElem, true);
                    me.InsertAfter(copy, current_node);
                    current_node = copy;
                }
            }

            m.parentKeyElem = me.SelectSingleElem("./parent_key");
            if (m.parentKeyElem != null)
            {
                current_node = m.parentKeyElem;
            }
            if (m.parentKeyElem == null && parent != null)
            {
                m.parentKeyElem = parent.SelectSingleElem("./parent_key");
                if (m.parentKeyElem != null)
                {
                    var copy = me.OwnerDocument.ImportNode(m.parentKeyElem, true);
                    me.InsertAfter(copy, current_node);
                    current_node = copy;
                }
            }

            bool eventsOverride = false;
            m.eventsElem = me.SelectSingleElem("./events");
            if (m.eventsElem != null)
            {
                current_node = m.eventsElem;
            }
            if (parent != null)
            {
                if (m.eventsElem == null)
                {
                    m.eventsElem = parent.SelectSingleElem("./events");
                    if (m.eventsElem != null)
                    {
                        var copy = me.OwnerDocument.ImportNode(m.eventsElem, true);
                        me.InsertAfter(copy, current_node);
                        current_node = copy;
                    }
                }
                else
                {
                    // Merge the parent events with our own events.  Field_mapping elements have an optional
                    // before_parent attribute that tells us whether to insert before the parent's events or
                    // after them (which is the default).  Could extend this to create_record, or make the
                    // ordering more flexible if we need to.
                    eventsOverride = IsTrue(m.eventsElem.GetAttributeDefault("override", "false"));
                    XmlElement parentEvents = parent.SelectSingleElem("./events");
                    if (parentEvents != null && !eventsOverride)
                    {
                        List<XmlElement> childNodes = m.eventsElem.GetChildElems();
                        foreach (var pe in parentEvents.GetChildElems().GetReverse())
                        {
                            var copy = pe.Clone();
                            m.eventsElem.PrependChild(copy);
                        }
                        if (childNodes.Count > 0)
                        {
                            foreach (var child in childNodes)
                            {
                                if (IsTrue(child.GetAttributeDefault("before_parent", "false")))
                                {
                                    m.eventsElem.RemoveChild(child);
                                    m.eventsElem.PrependChild(child);
                                }
                            }
                        }
                    }
                    current_node = m.eventsElem;
                }
            }

            m.databaseLookupElem = me.SelectSingleElem("./database_lookup");
            if (m.databaseLookupElem != null)
            {
                current_node = m.databaseLookupElem;
            }
            if (m.databaseLookupElem == null && parent != null)
            {
                m.databaseLookupElem = parent.SelectSingleElem("./database_lookup");
                if (m.databaseLookupElem != null)
                {
                    var copy = me.OwnerDocument.ImportNode(m.databaseLookupElem, true);
                    me.InsertAfter(copy, current_node);
                    current_node = copy;
                }
            }
            m.selectClause = me.SelectNodeInnerText("./database_lookup/select_fields", "");
            m.fromClause = me.SelectNodeInnerText("./database_lookup/from", "");

            m.parentInclusionElem = me.SelectSingleElem("./parent_inclusion");
            if (m.parentInclusionElem != null)
            {
                current_node = m.parentInclusionElem;
            }
            if (m.parentInclusionElem == null && parent != null)
            {
                m.parentInclusionElem = parent.SelectSingleElem("./parent_inclusion");
                if (m.parentInclusionElem != null)
                {
                    var copy = me.OwnerDocument.ImportNode(m.parentInclusionElem, true);
                    me.InsertAfter(copy, current_node);
                    current_node = copy;
                }
            }
            m.parentInclusion = me.SelectNodeInnerText("./parent_inclusion", "").Trim();

            m.sortKeyElem = me.SelectSingleElem("./sort_key");
            if (m.sortKeyElem != null)
            {
                current_node = m.sortKeyElem;
            }
            if (m.sortKeyElem == null && parent != null)
            {
                m.sortKeyElem = parent.SelectSingleElem("./sort_key");
                if (m.sortKeyElem != null)
                {
                    var copy = me.OwnerDocument.ImportNode(m.sortKeyElem, true);
                    me.InsertAfter(copy, current_node);
                    current_node = copy;
                }
            }
            m.sortKey = me.SelectNodeInnerText("./sort_key", "").Trim();

            m.stableSortElem = me.SelectSingleElem("./stable_sort");
            if (m.stableSortElem != null)
            {
                current_node = m.stableSortElem;
            }
            if (m.stableSortElem == null && parent != null)
            {
                m.stableSortElem = parent.SelectSingleElem("./stable_sort");
                if (m.stableSortElem != null)
                {
                    var copy = me.OwnerDocument.ImportNode(m.stableSortElem, true);
                    me.InsertAfter(copy, current_node);
                    current_node = copy;
                }
            }
            m.stableSort = me.SelectNodeInnerText("./stable_sort", "").Trim();

            m.parentNamespaceElem = me.SelectSingleElem("./parent_key");
            if (m.parentNamespaceElem != null)
            {
                m.parentNamespace = m.parentNamespaceElem.GetAttributeDefault("namespace", "").Trim();
                if (m.parentNamespace.Equals(""))
                {
                    throw new Exception("Parent_key for record type [" + m.name + "] must define parent namespace");
                }
            }
            else
            {
                m.parentNamespace = "";
            }

            List<string> orderedFieldValues = new List<string>();
            InsertRecordType(mc, true, m.name, m.nmspace, m.parentNamespace, m.baseRecordType, m.phase, m.stage, m.fileRegex, m.selectClause, m.fromClause, m.parentInclusion, m.sortKey, m.stableSort, "", "", "", m.displayName);
            
            // insert the record type stage info
            IIndex recTypeStagesIdx = (IIndex)mc.globalContext.GetNamedClassInst("MC_STAGE_INPUT_RECORDS");
            orderedFieldValues.Clear();
            orderedFieldValues.Add(m.phase);
            orderedFieldValues.Add(m.stage);
            orderedFieldValues.Add(m.name);
            recTypeStagesIdx.IndexInsert(mc, orderedFieldValues);

            // insert the child row
            IIndex recChildrenIdx = (IIndex)mc.globalContext.GetNamedClassInst("MC_RECORD_CHILDREN");
            orderedFieldValues.Clear();
            orderedFieldValues.Add(m.parentNamespace);
            orderedFieldValues.Add(m.name);
            recChildrenIdx.IndexInsert(mc, orderedFieldValues);

            // insert parent/child keys
            this.InsertKeys(mc, m, node, "MC_RECORD_CHILD_KEYS", me.SelectSingleElem("./primary_key"));
            this.InsertKeys(mc, m, node, "MC_RECORD_PARENT_KEYS", me.SelectSingleElem("./parent_key"));

            // insert events
            IIndex map_idx = (IIndex)mc.globalContext.GetNamedClassInst("MC_RECORD_EVENT_MAPPINGS");
            IIndex table_idx = (IIndex)mc.globalContext.GetNamedClassInst("MC_RECORD_TABLES");
            IIndex attr_idx = (IIndex)mc.globalContext.GetNamedClassInst("MC_TABLE_FIELD_ATTRIBUTES");
            this.InsertEvents(mc, m, node, "MC_RECORD_EVENTS", map_idx, table_idx, attr_idx, me.SelectSingleElem("./events"));
            
            // insert record fields
            IIndex recTypeFieldsIdx = (IIndex)mc.globalContext.GetNamedClassInst("MC_RECORD_FIELDS");
            m.fieldsElem = me.SelectSingleElem("./fields");
            if (m.fieldsElem != null)
            {
                foreach (var fld in m.fieldsElem.SelectElements("./field"))
                {
                    string field = fld.GetAttribute("name");
                    string nullable = fld.SelectNodeInnerText("./nullable", "");
                    RecordTypeField newField = new RecordTypeField(field, fld);
                    newField.Set("type", fld.SelectNodeInnerText("./type", ""));
                    newField.Set("length", fld.SelectNodeInnerText("./length", ""));
                    newField.Set("scale", fld.SelectNodeInnerText("./scale", ""));
                    newField.Set("nullable", IsTrue(nullable) ? "true" : (IsFalse(nullable) ? "false" : ""));
                    newField.Set("format", fld.SelectNodeInnerText("./format", ""));
                    newField.Set("enum", fld.SelectNodeInnerText("./enum", ""));
                    newField.Set("regex", fld.SelectNodeInnerText("./regex", ""));
                    newField.Set("default", fld.SelectNodeInnerText("./default", ""));
                    newField.Set("override", IsTrue(fld.SelectNodeInnerText("./override", "")) ? "true" : "");
                    newField.Set("alias", fld.SelectNodeInnerText("./alias", ""));
                    newField.Set("selected", IsTrue(fld.SelectNodeInnerText("./selected", "true")) ? "" : "false");
                    newField.Set("ignored", IsTrue(fld.SelectNodeInnerText("./ignored", "")) ? "true" : "");
                    newField.Set("missing", IsTrue(fld.SelectNodeInnerText("./missing", "")) ? "true" : "");
                    newField.Set("target", fld.SelectNodeInnerText("./target", ""));
                    newField.Set("required", IsTrue(fld.SelectNodeInnerText("./required", "")) ? "true" : "");
                    newField.Set("data_type", fld.SelectNodeInnerText("./data_type", ""));
                    node.AddRecordField(newField);

                    orderedFieldValues.Clear();
                    orderedFieldValues.Add(m.name);
                    orderedFieldValues.Add(field);
                    orderedFieldValues.Add(newField.Get("type"));
                    orderedFieldValues.Add(newField.Get("length"));
                    orderedFieldValues.Add(newField.Get("scale"));
                    orderedFieldValues.Add(newField.Get("nullable"));
                    orderedFieldValues.Add(newField.Get("format"));
                    orderedFieldValues.Add(newField.Get("enum"));
                    orderedFieldValues.Add(newField.Get("regex"));
                    orderedFieldValues.Add(newField.Get("default"));
                    orderedFieldValues.Add(newField.Get("override"));
                    orderedFieldValues.Add(newField.Get("alias"));
                    orderedFieldValues.Add(newField.Get("selected"));
                    orderedFieldValues.Add(newField.Get("ignored"));
                    orderedFieldValues.Add(newField.Get("missing"));
                    orderedFieldValues.Add(newField.Get("target"));
                    orderedFieldValues.Add(newField.Get("required"));
                    orderedFieldValues.Add(newField.Get("data_type"));
                    recTypeFieldsIdx.IndexInsert(mc, orderedFieldValues);
                }
            }

            // print out the xml tree
            string directory = mc.globalContext["gen_record_types_directory"];
            if (directory.Length > 0 && !mc.globalContext["mc_done_core_record_types"].Equals("1"))
            {
                string fileName = System.IO.Path.Combine(directory, m.name + ".txt");
                me.PrettyPrint(fileName);
            }

            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
        }

        public int GetNextNamedId(ModuleContext mc, string name)
        {
            int id;
            string temp_id = mc.globalContext[name];
            if (temp_id.Equals("")) temp_id = "0";
            id = temp_id.ToInt() + 1;
            mc.globalContext[name] = id.ToString();
            return id;
        }

        public void InsertRecordType(ModuleContext mc, bool error, string name, string nmspace, string parent, string basert, string phase, string stage, string regex, string select, string from, string inclusion, string sortkey, string stableSort, string db, string table, string printed, string displayName)
        {
            IIndex recTypesIdx = (IIndex)mc.globalContext.GetNamedClassInst("MC_RECORD_TYPES");
            List<string> key = new List<string>();
            key.Add(name);
            Dictionary<string, string> values = new Dictionary<string, string>();
            if (recTypesIdx.IndexGet(mc, key, values).Equals("1"))
            {
                if (error)
                {
                    throw new Exception("Record type [" + name + "] was defined twice!");
                }
                else
                {
                    return;
                }
            }

            // get a new record type id
            int id = this.GetNextNamedId(mc, "_mc_record_type_id");

            // insert the record type
            List<string> orderedFieldValues = new List<string>();
            orderedFieldValues.Add(name);
            orderedFieldValues.Add(id.ToString());
            orderedFieldValues.Add(nmspace);
            orderedFieldValues.Add(parent);
            orderedFieldValues.Add(basert);
            orderedFieldValues.Add(phase);
            orderedFieldValues.Add(stage);
            orderedFieldValues.Add(regex);
            orderedFieldValues.Add(select);
            orderedFieldValues.Add(from);
            orderedFieldValues.Add(inclusion);
            orderedFieldValues.Add(sortkey);
            orderedFieldValues.Add(stableSort);
            orderedFieldValues.Add(db);
            orderedFieldValues.Add(table);
            orderedFieldValues.Add(printed);
            orderedFieldValues.Add("");
            orderedFieldValues.Add("");
            orderedFieldValues.Add(displayName);
            recTypesIdx.IndexInsert(mc, orderedFieldValues);
            
            // insert the record type id
            IIndex recTypesByIdIdx = (IIndex)mc.globalContext.GetNamedClassInst("MC_RECORD_TYPES_BY_ID");
            orderedFieldValues.Clear();
            orderedFieldValues.Add(id.ToString());
            orderedFieldValues.Add(name);
            recTypesByIdIdx.IndexInsert(mc, orderedFieldValues);
        }

        public void InsertKeys(ModuleContext mc, MRecordType m, RecordTypeNode node, string index, XmlElement e)
        {
            if (e == null) return;
            IIndex idx = (IIndex)mc.globalContext.GetNamedClassInst(index);
            List<string> orderedFieldValues = new List<string>();
            foreach (var elem in e.SelectElements("./field"))
            {
                string name = elem.GetAttribute("name");
                orderedFieldValues.Clear();
                orderedFieldValues.Add(m.name);
                orderedFieldValues.Add(name);
                orderedFieldValues.Add(elem.InnerText);
                idx.IndexInsert(mc, orderedFieldValues);
                node.parentChildKeys[name] = "";
            }
        }

        public void InsertEvents(ModuleContext mc, MRecordType m, RecordTypeNode node, string index, IIndex map_idx, IIndex tbl_idx, IIndex attr_idx, XmlElement e)
        {
            if (e == null) return;
            IIndex idx = (IIndex)mc.globalContext.GetNamedClassInst(index);
            List<string> orderedFieldValues = new List<string>();
            foreach (var fld in e.GetChildElems())
            {
                int id = this.GetNextNamedId(mc, "_mc_event_id");
                string pass = fld.GetAttributeDefaulted("pass", "1");
                if (fld.Name.Equals("create_record"))
                {
                    string db = fld.GetAttributeDefault("database", "").ToUpper();
                    string table = fld.GetAttributeDefault("table", "");
                    string db_rectype = "MC_TABLE_" + db + "_" + table.ToUpper();
                    string recType = fld.GetAttributeDefault("type", db_rectype);
                    SpawnNode spawnNode;
                    if (table.Equals(""))
                    {
                        spawnNode = new SpawnRecordNode(recType);
                    }
                    else
                    {
                        spawnNode = new SpawnTableNode(recType);
                    }

                    XmlNode fc = fld.SelectSingleNode("./from_cursor");
                    XmlNode cursorNode = fld.SelectSingleNode("./from_cursor/cursor");
                    string cursorVar = "";
                    if (cursorNode == null)
                    {
                        cursorVar = "TEMP." + mc.GetGenSym("mc");
                        if (fc != null)
                        {
                            XmlElement elem = fc.CreateTextElement("cursor", cursorVar);
                            fc.AppendChild(elem);
                        }
                    }
                    else
                    {
                        cursorVar = cursorNode.InnerText;
                    }

                    string condition = fld.SelectNodeInnerText("./condition", "");
                    string inherit = fld.SelectNodeInnerText("./inherit_fields", "false");
                    if (inherit.EqualsIgnoreCase("true"))
                    {
                        inherit = "1";
                    }
                    else if (inherit.EqualsIgnoreCase("false"))
                    {
                        inherit = "0";
                    }
                    orderedFieldValues.Clear();
                    orderedFieldValues.Add(m.name);
                    orderedFieldValues.Add(pass);
                    orderedFieldValues.Add(id.ToString());
                    orderedFieldValues.Add(fld.Name);
                    orderedFieldValues.Add(recType);
                    orderedFieldValues.Add(db);
                    orderedFieldValues.Add(table);
                    orderedFieldValues.Add(inherit);
                    orderedFieldValues.Add(fc == null ? "" : fc.InnerXml);
                    orderedFieldValues.Add(cursorVar);
                    orderedFieldValues.Add(condition);
                    idx.IndexInsert(mc, orderedFieldValues);
                    this.InsertMappings(mc, m, node, map_idx, id, cursorVar, fld.SelectSingleElem("./child_field_mapping"), spawnNode.spawnMapping);

                    if (!table.Equals(""))
                    {
                        string selected = IsTrue(fld.GetAttributeDefault("selected", "true")) ? "1" : "";
                        orderedFieldValues.Clear();
                        orderedFieldValues.Add(m.name);
                        orderedFieldValues.Add(table);
                        orderedFieldValues.Add(db);
                        orderedFieldValues.Add(selected);
                        orderedFieldValues.Add("");
                        orderedFieldValues.Add("");
                        tbl_idx.IndexInsert(mc, orderedFieldValues);
                        this.InsertRecordType(mc, false, db_rectype, "", "", "", m.phase, m.stage, "", "", "", "", "", "", db, table, "1", m.displayName);

                        RecordTypeDbTable dbTable = new RecordTypeDbTable(db, table);

                        Dictionary<string, string> flds = new Dictionary<string, string>();
                        Dictionary<string, string> ignores = new Dictionary<string, string>();
                        Dictionary<string, string> enums = new Dictionary<string, string>();
                        Dictionary<string, string> aliases = new Dictionary<string, string>();
                        foreach (var ignore in fld.SelectElements("./ignore_field"))
                        {
                            flds[ignore.InnerText] = "";
                            ignores[ignore.InnerText] = ignore.InnerText;
                        }
                        foreach (var fe in fld.SelectElements("./enum"))
                        {
                            flds[fe.GetAttribute("name")] = "";
                            enums[fe.GetAttribute("name")] = fe.InnerText;
                        }
                        foreach (var alias in fld.SelectElements("./alias"))
                        {
                            flds[alias.GetAttribute("name")] = "";
                            aliases[alias.GetAttribute("name")] = alias.InnerText;
                        }
                        foreach (var f in flds)
                        {
                            string fname = f.Key.Trim();
                            orderedFieldValues.Clear();
                            orderedFieldValues.Add(db);
                            orderedFieldValues.Add(table);
                            orderedFieldValues.Add(fname);
                            orderedFieldValues.Add(ignores.GetValueDefaulted(f.Key, ""));
                            orderedFieldValues.Add(enums.GetValueDefaulted(f.Key, ""));
                            orderedFieldValues.Add(aliases.GetValueDefaulted(f.Key, ""));
                            attr_idx.IndexInsert(mc, orderedFieldValues);
                            if (ignores.ContainsKey(f.Key))
                            {
                                dbTable.ignoreFields[fname] = "";
                            }
                            if (enums.ContainsKey(f.Key))
                            {
                                dbTable.enumFields[fname] = enums[f.Key];
                            }
                        }
                        (spawnNode as SpawnTableNode).setTable(dbTable);
                    }
                    node.events.Add(spawnNode);
                }
                else if (fld.Name.Equals("field_mapping"))
                {
                    orderedFieldValues.Clear();
                    orderedFieldValues.Add(m.name);
                    orderedFieldValues.Add(pass);
                    orderedFieldValues.Add(id.ToString());
                    orderedFieldValues.Add(fld.Name);
                    orderedFieldValues.Add("");
                    orderedFieldValues.Add("");
                    orderedFieldValues.Add("");
                    orderedFieldValues.Add("");
                    orderedFieldValues.Add("");
                    orderedFieldValues.Add("");
                    idx.IndexInsert(mc, orderedFieldValues);

                    MappingEventNode mapNode = new MappingEventNode(SyntaxDomain.mapping);
                    node.events.Add(mapNode);
                    this.InsertMappings(mc, m, node, map_idx, id, "TEMP." + mc.GetGenSym("mc"), fld, mapNode);
                }
                else
                {
                    throw new Exception("unexpected event type: " + fld.Name);
                }
            }
        }

        public void InsertMappings(ModuleContext mc, MRecordType m, RecordTypeNode node, IIndex idx, int id, string childCursor, XmlElement e, MappingEventNode mapNode)
        {
            if (e == null) return;
            int map_id = 1;
            List<string> orderedFieldValues = new List<string>();
            foreach (var fld in e.GetChildElems())
            {
                if (fld.Name.In("map", "def"))
                {
                    SyntaxDomain domain = e.Name.Equals("field_mapping") ? SyntaxDomain.mapping : SyntaxDomain.child_mapping;
                    string overridden = fld.Name.Equals("map") ? "1" : "";
                    string mapping = fld.InnerText;
                    string newMapping;
                    string condition = fld.GetAttributeDefault("condition", "");
                    string newCondition = "";
                    string assignedVariable = "";
                    string assignedObjectId = "";
                    string assignedField = "";
                    string assignedConstant = "";
                    string validate = "run";

                    // Invoke the parser to resolve references like CHILD.x, PARENT.x, etc
                    {
                        string prepare;
                        string access;
                        RecordTypeSyntaxInterpreter interpreter = new RecordTypeSyntaxInterpreter(mc, mapping, domain, childCursor);
                        interpreter.Interpret(out prepare, out access);
                        newMapping = prepare.AppendLine(access);
                        if (interpreter.outputElements.Count == 1)
                        {
                            OutputField f = interpreter.outputElements[0];
                            foreach (var inputElem in interpreter.inputElements)
                            {
                                // For now, force any MC_GUID or MC_SEQUENCE field to be private by default (normally these are internal fields)
                                if (inputElem.IsPrivateByDefault())
                                {
                                    f.forcePrivate = true;
                                }
                                f.inputs.Add(inputElem);
                                f.allInputs.Add(inputElem);
                            }
                            f.recordType = m.name;
                            f.eventId = id;
                            f.mappingId = map_id;
                            assignedVariable = f.objectSyntax;
                            assignedObjectId = f.objectIdSyntax;
                            assignedField = f.fieldName;
                            if (f.IsConstantAssignment())
                            {
                                assignedConstant = interpreter.inputElements[0].node.InnerText;
                                validate = "setup";
                            }
                            else if (f.IsSafeToSkipValidation())
                            {
                                validate = "";
                            }

                            mapNode.addMapping(f);
                        }
                        else
                        {
                            throw new Exception("Cannot have more than one output element in mapping [" + mapping + "]");
                        }
                    }

                    if (!condition.Equals(""))
                    {
                        string prepare;
                        string access;
                        RecordTypeSyntaxInterpreter interpreter = new RecordTypeSyntaxInterpreter(mc, condition, domain, childCursor);
                        interpreter.Interpret(out prepare, out access);
                        newCondition = prepare.AppendLine(access);
                    }

                    orderedFieldValues.Clear();
                    orderedFieldValues.Add(m.name);
                    orderedFieldValues.Add(id.ToString());
                    orderedFieldValues.Add(map_id++.ToString());
                    orderedFieldValues.Add(newMapping);
                    orderedFieldValues.Add(overridden);
                    orderedFieldValues.Add(newCondition);
                    orderedFieldValues.Add(assignedVariable);
                    orderedFieldValues.Add(assignedObjectId);
                    orderedFieldValues.Add(assignedField);
                    orderedFieldValues.Add(assignedConstant);
                    orderedFieldValues.Add(validate);
                    orderedFieldValues.Add("");
                    orderedFieldValues.Add("");
                    orderedFieldValues.Add("");
                    // A few fields initialized to null here
                    idx.IndexInsert(mc, orderedFieldValues);
                }
                else if (fld.Name.Equals("debug"))
                {
                    // Pass an arbitrary statement through without any analysis of it
                    orderedFieldValues.Clear();
                    orderedFieldValues.Add(m.name);
                    orderedFieldValues.Add(id.ToString());
                    orderedFieldValues.Add(map_id++.ToString());
                    orderedFieldValues.Add(fld.InnerXml);
                    orderedFieldValues.Add("");
                    orderedFieldValues.Add("");
                    orderedFieldValues.Add("");
                    orderedFieldValues.Add("");
                    orderedFieldValues.Add("");
                    orderedFieldValues.Add("");
                    orderedFieldValues.Add("");
                    orderedFieldValues.Add("");
                    orderedFieldValues.Add("");
                    orderedFieldValues.Add("");
                    // A few fields initialized to null here
                    idx.IndexInsert(mc, orderedFieldValues);
                }
            }
        }

        /// <summary>
        /// Tests a string for truth
        /// </summary>
        /// <param name="text">Input string</param>
        /// <returns>0|1</returns>
        /// <category>Strings and Scalars</category>
        [MvmExport("is_true")]
        public static bool IsTrue(string text)
        {
            if (text.IsNullOrEmpty())
            {
                return false;
            }
            return trueRegex.IsMatch(text);
        }

        /// <summary>
        /// Tests a string for truth
        /// </summary>
        /// <param name="text">Input string</param>
        /// <returns>0|1</returns>
        /// <category>Strings and Scalars</category>
        [MvmExport("is_false")]
        public static bool IsFalse(string text)
        {
            if (text.IsNullOrEmpty())
            {
                return false;
            }
            return falseRegex.IsMatch(text);
        }
    }

    public class RecordTypeSyntaxInterpreter
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public ModuleContext mc;
        public string statement;
        public string childRef;
        public SyntaxDomain domain;
        public AstElement statementElem;

        public List<InputElement> inputElements = new List<InputElement>();
        public List<OutputField> outputElements = new List<OutputField>();

        public RecordTypeSyntaxInterpreter(ModuleContext mc, string statement, SyntaxDomain domain, string childRef)
        {
            this.mc = mc;
            this.statement = statement;
            this.domain = domain;
            this.childRef = childRef;
        }

        public static AstElement ParseSyntax(string stmt)
        {
            AstNode syn = AstNode.ParseStatement(stmt);
            if (syn.IsNil)
            {
                syn = (AstNode)syn.Children().First();
            }
            AstElement elem = syn as AstElement;
            return elem;
        }

        public void Interpret(out string prepare, out string access)
        {
            try
            {
                statementElem = ParseSyntax(this.statement);
            }
            catch (Exception e)
            {
                throw new Exception("Cannot parse record_type mapping syntax [" + this.statement + "]", e);
            }
            try
            {
                this.InterpretSyntax(statementElem, out prepare, out access, false);
            }
            catch (Exception e)
            {
                throw new Exception("Invalid record_type mapping syntax [" + this.statement + "]", e);
            }
        }

        private void InterpretSyntax(AstElement elem, out string prepare, out string access, bool assigned)
        {
            prepare = "";
            access = "";

            // Object references
            if (IsReference(elem))
            {
                string objectSyntax;
                string objectIdSyntax;
                GetReferenceObjectSyntax(elem, out objectSyntax, out objectIdSyntax);

                // Bare reference to an entity
                if (!elem.HasDot)
                {
                    if (elem.Name.Equals("ANCESTOR"))
                    {
                        throw new Exception("ANCESTOR: invalid syntax");
                    }
                    access = objectIdSyntax;
                    return;
                }

                // SOMETHING.field
                if (elem.HasDotProperty)
                {
                    AstElement fieldElem = elem.DotProperty;
                    string fieldName = elem.DotProperty.Name;

                    if (elem.Name.Equals("ANCESTOR"))
                    {
                        if (assigned)
                        {
                            throw new Exception("cannot assign to ANCESTOR.field");
                        }

                        string temp = "TEMP." + mc.GetGenSym(elem.Name);
                        prepare = prepare.AppendLine(@"
                            <call_proc_for_current_object>
                                <name>'lookup_ancestor_field'</name>
                                <param name='field'>'" + fieldName + @"'</param>
                                <param name='value'>" + temp + @"</param>
                            </call_proc_for_current_object>
                        ");
                        access = temp;
                        return;
                    }
                    else if (objectSyntax.Equals("OBJECT") && elem.HasParameters)
                    {
                        // OBJECT(expression).field
                        AstElement innerElem = elem.GetParameterSubElements().ElementAt(0);
                        string innerPrepare;
                        string innerAccess;
                        InterpretSyntax(innerElem, out innerPrepare, out innerAccess, false);
                        prepare = prepare.AppendLine(innerPrepare);
                        access = objectSyntax + "(" + innerAccess + ")." + fieldName;
                        if (assigned)
                        {
                            // TODO: not currently tracking assignment to OBJECT references for outputElements
                        }
                        else
                        {
                            inputElements.Add(new InputElement(fieldName, elem));
                        }
                        return;
                    }
                    else
                    {
                        access = objectSyntax + "." + fieldName;
                        if (assigned)
                        {
                            outputElements.Add(new OutputField(access, objectIdSyntax, fieldName, elem));
                        }
                        else
                        {
                            inputElements.Add(new InputElement(fieldName, elem));
                        }
                        return;
                    }

                    throw new Exception("unexpected dot syntax: [" + fieldElem.OuterAst + "]");
                }

                throw new Exception("unexpected reference syntax: [" + elem.OuterAst + "]");
            }

            if (assigned)
            {
                throw new Exception("cannot assign to syntax: [" + elem.OuterAst + "]");
            }

            // Assignment and binary operators
            bool isAssignmentOperator = SyntaxMaster.IsAssignmentOperator(elem.Name);
            if (SyntaxMaster.IsBinaryOperator(elem.Name) || isAssignmentOperator)
            {
                AstElement lvalueElem = elem.GetParameterSubElements().ElementAt(0);
                string lvaluePrepare;
                string lvalueAccess;
                InterpretSyntax(lvalueElem, out lvaluePrepare, out lvalueAccess, isAssignmentOperator);

                AstElement rvalueElem = elem.GetParameterSubElements().ElementAt(1);
                string rvaluePrepare;
                string rvalueAccess;
                InterpretSyntax(rvalueElem, out rvaluePrepare, out rvalueAccess, false);

                prepare = prepare.AppendLine(lvaluePrepare);
                prepare = prepare.AppendLine(rvaluePrepare);

                // Special handling of assignment operators, which will ordinarily be the top node in the tree
                if (isAssignmentOperator)
                {
                    if (!IsAssignable(lvalueElem))
                    {
                        throw new Exception("invalid assignment: [" + elem.OuterAst + "]");
                    }
                    access = "<do>" + lvalueAccess + " " + elem.Name + " " + rvalueAccess + "</do>";
                }
                else
                {
                    access = "(" + lvalueAccess + " " + elem.Name + " " + rvalueAccess + ")";
                }

                return;
            }

            // Literals
            if (elem.IsLiteral)
            {
                inputElements.Add(new InputElement(InputElementType.constant, elem));
                access = elem.LiteralValue;
                return;
            }

            // MVM functions
            if (CallFunction.IsMvmFunction(elem.Name))
            {
                // TODO: don't support named arguments in functions yet
                int x = 0;
                access = elem.Name + "(";
                foreach (var param in elem.GetParameterSubElements())
                {
                    string argPrepare;
                    string argAccess;
                    InterpretSyntax(param, out argPrepare, out argAccess, false);
                    prepare = prepare.AppendLine(argPrepare);
                    if (x > 0)
                    {
                        access += ", ";
                    }
                    access += argAccess;
                    x++;
                }
                access += ")";
                return;
            }

            // Special MC functions
            if (IsMCFunction(elem))
            {
                string temp;
                string elemName = elem.Name.ToUpper();
                switch (elemName)
                {
                    case "MC_SYSDATE":
                        access = "GLOBAL.mvm_startup_date";
                        inputElements.Add(new InputElement(InputElementType.sysdate, elem));
                        return;
                    case "MC_SYSDAY":
                        access = "GLOBAL.mvm_startup_day";
                        inputElements.Add(new InputElement(InputElementType.sysday, elem));
                        return;
                    case "MC_MAXDATE":
                        access = "GLOBAL.maxdate";
                        inputElements.Add(new InputElement(InputElementType.maxdate, elem));
                        return;
                    case "MC_GUID":
                        temp = "TEMP." + mc.GetGenSym(elem.Name);
                        prepare = prepare.AppendLine("<get_guid>" + temp + "</get_guid>");
                        access = temp;
                        inputElements.Add(new InputElement(InputElementType.guid, elem));
                        return;
                    case "MC_SEQUENCE":
                    case "MC_PROC":
                        {
                            // Special case for bareword sequence/proc names
                            AstElement valueElem = elem.GetParameterSubElements().ElementAt(0);
                            string valueAccess = valueElem.Name;
                            if (valueElem.IsLiteral ||
                                valueElem.CountBraceNodes + valueElem.CountBracketNodes + valueElem.CountTypeParameterNodes +
                                valueElem.CountChildrenNodes + valueElem.CountDotNodes + valueElem.CountParameterNodes > 0)
                            {
                                throw new Exception("invalid MC sequence/proc name: [" + valueElem.OuterAst + "]");
                            }
                            string procName = valueAccess;

                            List<Tuple<string, string>> arguments = new List<Tuple<string, string>>();
                            if (elemName.Equals("MC_SEQUENCE"))
                            {
                                procName = "sequence_get_next_" + valueAccess;
                                inputElements.Add(new InputElement(InputElementType.sequence, elem));
                            }
                            else
                            {
                                // FIXME: MC_PROC() arguments need to be tracked as inputs; custom procs should be decorated to know the inputs
                                inputElements.Add(new InputElement(InputElementType.proc, elem));
                                int x = 0;
                                foreach (var param in elem.GetParameterSubElements())
                                {
                                    if (x > 0)
                                    {
                                        string argPrepare;
                                        string argAccess;
                                        InterpretSyntax(param, out argPrepare, out argAccess, false);
                                        prepare = prepare.AppendLine(argPrepare);
                                        arguments.Add(new Tuple<string, string>(param.NodeName, argAccess));
                                    }
                                    x++;
                                }
                            }

                            temp = "TEMP." + mc.GetGenSym(elem.Name);
                            prepare = prepare.AppendLine(@"
                                <call_proc_for_current_object>
                                    <name>'" + procName + @"'</name>
                            ");
                            foreach (var arg in arguments)
                            {
                                prepare = prepare.AppendLine(@"
                                    <param name='" + arg.Item1 + "'>" + arg.Item2 + @"</param>
                                ");
                            }
                            prepare = prepare.AppendLine(@"
                                    <param name='value'>" + temp + @"</param>
                                </call_proc_for_current_object>
                            ");
                            access = temp;
                            return;
                        }
                    case "MC_BOOLEAN":
                        {
                            AstElement valueElem = elem.GetParameterSubElements().ElementAt(0);
                            string valuePrepare;
                            string valueAccess;
                            InterpretSyntax(valueElem, out valuePrepare, out valueAccess, false);

                            prepare = prepare.AppendLine(valuePrepare);

                            temp = "TEMP." + mc.GetGenSym(elem.Name);
                            prepare = prepare.AppendLine(@"
                                <call_proc_for_current_object>
                                    <name>'boolean'</name>
                                    <param name='input'>" + valueAccess + @"</param>
                                    <param name='output'>" + temp + @"</param>
                                </call_proc_for_current_object>
                            ");
                            access = temp;
                            inputElements.Add(new InputElement(InputElementType.boolean, elem));
                            return;
                        }
                    case "MC_MAPPING":
                        {
                            AstElement lvalueElem = elem.GetParameterSubElements().ElementAt(0);
                            string lvaluePrepare;
                            string lvalueAccess;
                            InterpretSyntax(lvalueElem, out lvaluePrepare, out lvalueAccess, false);

                            AstElement rvalueElem = elem.GetParameterSubElements().ElementAt(1);
                            string rvaluePrepare;
                            string rvalueAccess;
                            InterpretSyntax(rvalueElem, out rvaluePrepare, out rvalueAccess, false);

                            prepare = prepare.AppendLine(lvaluePrepare);
                            prepare = prepare.AppendLine(rvaluePrepare);

                            temp = "TEMP." + mc.GetGenSym(elem.Name);
                            prepare = prepare.AppendLine(@"
                                <do>" + temp + @" = ''</do>
                                <to_lower>
                                    <input>" + lvalueAccess + @"</input>
                                    <output>TEMP.lowerarg1</output>
                                </to_lower>
                                <to_lower>
                                    <input>" + rvalueAccess + @"</input>
                                    <output>TEMP.lowerarg2</output>
                                </to_lower>
                                <index_select>
                                    <index>'MC_MAPPINGS'</index>
                                    <field name='namespace'>TEMP.lowerarg1</field>
                                    <field name='from'>TEMP.lowerarg2</field>
                                    <cursor>TEMP.ecsr</cursor>
                                    <then>
                                        <do>" + temp + @" = OBJECT(TEMP.ecsr).to</do>
                                    </then>
                                    <else>
                                        <do>OBJECT._mc_error ~= 'Failed lookup with [' ~ TEMP.lowerarg1 ~ ':' ~ TEMP.lowerarg2 ~ ']. '</do>
                                    </else>
                                </index_select>
                            ");
                            access = temp;
                            inputElements.Add(new InputElement(InputElementType.mapping, elem));
                            return;
                        }
                }
                throw new Exception("invalid MC function: [" + elem.OuterAst + "]");
            }

            throw new Exception("unexpected record type syntax: [" + elem.OuterAst + "]");
        }

        public void GetReferenceObjectSyntax(AstElement elem, out string objectSyntax, out string objectIdSyntax)
        {
            string syntax = elem.Name;
            if (domain == SyntaxDomain.mapping && syntax.Equals("CHILD"))
            {
                throw new Exception("CHILD has no meaning in a record type field_mapping block.");
            }
            if (syntax.Equals("ANCESTOR"))
            {
                objectSyntax = "";
                objectIdSyntax = "";
                return;
            }
            if (syntax.Equals("PARENT"))
            {
                objectSyntax = "OBJECT(OBJECT._mc_parent)";
                objectIdSyntax = "OBJECT._mc_parent";
                return;
            }
            if (syntax.Equals("CHILD"))
            {
                if (childRef == null)
                {
                    throw new Exception("Must have child reference for [" + elem.OuterAst + "]");
                }
                objectSyntax = "OBJECT(" + childRef + ")";
                objectIdSyntax = childRef;
                return;
            }
            if (syntax.Equals("OBJECT"))
            {
                objectSyntax = "OBJECT";
                objectIdSyntax = "OBJECT.object_id";
                return;
            }
            if (syntax.Equals("GLOBAL"))
            {
                objectSyntax = "GLOBAL";
                objectIdSyntax = "";
                return;
            }
            throw new Exception("unexpected object reference!");
        }

        public bool IsAssignable(AstElement elem)
        {
            if (this.domain == SyntaxDomain.child_mapping)
            {
                return elem.Name.In("PARENT", "CHILD", "OBJECT", "GLOBAL");
            }
            return elem.Name.In("PARENT", "OBJECT", "GLOBAL");
        }

        public bool IsReference(AstElement elem)
        {
            return elem.Name.In("ANCESTOR", "PARENT", "CHILD", "OBJECT", "GLOBAL");
        }

        public bool IsMCFunction(AstElement elem)
        {
            return elem.Name.StartsWith("MC_");
        }
    }
}
