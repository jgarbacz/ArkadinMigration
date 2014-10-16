using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    [Module(@"
        <module_config>
            <name>analyze_record_types</name>
            <xsd xmlns:xs='http://www.w3.org/2001/XMLSchema'>
                <xs:complexType>
                    <xs:attribute name='name' type='xs:string' use='optional'/>
                </xs:complexType>
            </xsd>
            <doc>
                <category>MetraConvert</category>
                <description>Analyzes all record types and links them together</description>
            </doc>
        </module_config>
    ")]
    public class MAnalyzeRecordTypes : BaseModuleSetup, IModuleRun
    {
        public static ModuleContext mc;
        public NestedMemoryIndex IdxTableFields;
        public IIndex IdxRecordFields;
        public IIndex IdxEventMappings;
        public IIndex IdxValidation;
        public Dictionary<string, string> recordsSeen;
        public Dictionary<string, RecordTypeNode> allNodes;

        public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MAnalyzeRecordTypes.mc = mc;
            MAnalyzeRecordTypes m = new MAnalyzeRecordTypes();

            m.allNodes = (Dictionary<string, RecordTypeNode>)mc.globalContext.GetNamedClassInst("MC_RECORD_TYPE_NODES");
            Dictionary<string, string> childParentMap = new Dictionary<string, string>();
            foreach (var kv in m.allNodes)
            {
                foreach (var e in kv.Value.events)
                {
                    if (e.category == EventNode.EventCategory.record)
                    {
                        childParentMap[e.name] = "";
                    }
                }
            }

            // Recursively build the node tree starting with each top-level record
            m.recordsSeen = new Dictionary<string, string>();
            m.IdxTableFields = (NestedMemoryIndex)mc.globalContext.GetNamedClassInst("MC_TABLE_FIELDS");
            m.IdxRecordFields = (IIndex)mc.globalContext.GetNamedClassInst("MC_RECORD_FIELDS");
            m.IdxEventMappings = (IIndex)mc.globalContext.GetNamedClassInst("MC_RECORD_EVENT_MAPPINGS");
            m.IdxValidation = (IIndex)mc.globalContext.GetNamedClassInst("MC_RECORD_VALIDATION");
            foreach (var kv in m.allNodes)
            {
                if (!childParentMap.ContainsKey(kv.Value.name))
                {
                    kv.Value.BuildNode(mc, m, new MappingSet(), null);
                }
            }

            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
        }
    }

    public class EventNode
    {
        public enum EventCategory { mapping, record, table }
        public string name;
        public EventCategory category;
    }

    public class MappingEventNode : EventNode
    {
        public SyntaxDomain domain;
        public List<OutputField> setFields = new List<OutputField>();
        public Dictionary<string, string> setFieldsIdx = new Dictionary<string, string>();
        public MappingEventNode(SyntaxDomain domain)
        {
            this.name = "mapping";
            this.domain = domain;
            this.category = EventCategory.mapping;
        }
        public void addMapping(OutputField f)
        {
            this.setFields.Add(f);
            this.setFieldsIdx[f.fieldName] = f.objectSyntax;
        }
    }

    public class SpawnNode : EventNode
    {
        public SpawnNode realNode;
        public MappingEventNode spawnMapping = new MappingEventNode(SyntaxDomain.child_mapping);
        public List<DatabaseField> dbFields = new List<DatabaseField>();
        public Dictionary<string, List<DatabaseField>> dbFieldsIdx = new Dictionary<string, List<DatabaseField>>();
        public SpawnNode(string name)
        {
            this.name = name;
            this.realNode = this;
        }
        public void setTable(RecordTypeDbTable t)
        {
            throw new Exception("Should never call SpawnNode.setTable!");
        }
        public void AddDbField(DatabaseField field)
        {
            dbFields.Add(field);
            dbFieldsIdx.GetAddValueDefaulted(field.ToString(), new List<DatabaseField>()).Add(field);
        }
        public List<DatabaseField> GetDbFields() { return null; }
        public Dictionary<string, List<DatabaseField>> GetDbFieldsIdx() { return null; }
        public void BuildNode(ModuleContext mc, MAnalyzeRecordTypes m, MappingSet mappedFields, RecordTypeNode parent)
        {
            throw new Exception("Should never call SpawnNode.BuildNode!");
        }
    }

    public class SpawnRecordNode : SpawnNode
    {
        public SpawnRecordNode(string name)
            : base(name)
        {
            this.category = EventCategory.record;
        }
        new public List<DatabaseField> GetDbFields()
        {
            return this.realNode.dbFields;
        }
        new public Dictionary<string, List<DatabaseField>> GetDbFieldsIdx()
        {
            return this.realNode.dbFieldsIdx;
        }
        new public void BuildNode(ModuleContext mc, MAnalyzeRecordTypes m, MappingSet mappedFields, RecordTypeNode parent)
        {
            RecordTypeNode childNode = m.allNodes[this.name];
            this.realNode = childNode;
            childNode.BuildNode(mc, m, mappedFields, parent);
        }
    }

    public class SpawnTableNode : SpawnNode
    {
        public RecordTypeDbTable table;
        public SpawnTableNode(string name)
            : base(name)
        {
            this.category = EventCategory.table;
        }
        new public void setTable(RecordTypeDbTable t)
        {
            this.table = t;
        }
        new public List<DatabaseField> GetDbFields()
        {
            return this.dbFields;
        }
        new public Dictionary<string, List<DatabaseField>> GetDbFieldsIdx()
        {
            return this.dbFieldsIdx;
        }
        new public void BuildNode(ModuleContext mc, MAnalyzeRecordTypes m, MappingSet mappedFields, RecordTypeNode parent)
        {
            // TODO: should use IIndex interface methods here to select from MC_TABLE_FIELDS...
            RecordTypeDbTable childTable = this.table;
            object objPtr = null;
            Dictionary<string, object> dicPtr = m.IdxTableFields.index;
            dicPtr.TryGetValue(childTable.db, out objPtr);
            dicPtr = objPtr as Dictionary<string, object>;
            dicPtr.TryGetValue(childTable.table, out objPtr);
            dicPtr = objPtr as Dictionary<string, object>;
            foreach (var kv in dicPtr)
            {
                DatabaseField fld = new DatabaseField(childTable.db, childTable.table, kv.Key);
                if (!(childTable.ignoreFields.ContainsKey(kv.Key) || dbFieldsIdx.ContainsKey(fld.ToString())))
                {
                    List<string[]> values = kv.Value as List<string[]>;
                    fld.Set("type", values[0][0]);
                    fld.Set("length", values[0][1]);
                    fld.Set("scale", values[0][2]);
                    fld.Set("nullable", MRecordType.IsTrue(values[0][3]) ? "true" : (MRecordType.IsFalse(values[0][3]) ? "false" : ""));
                    fld.Set("default", values[0][4]);
                    fld.Set("enum", values[0][5]);
                    fld.Set("regex", values[0][6]);
                    fld.Set("alias", values[0][7]);
                    fld.Set("required", MRecordType.IsTrue(fld.Get("nullable")) ? "false" : (MRecordType.IsFalse(fld.Get("nullable")) ? "true" : ""));
                    AddDbField(fld);
                }
            }
        }
    }

    public class RecordTypeNode : SpawnNode
    {
        public XmlElement xml;
        public MRecordType rtype;
        //public XmlElement interfaceXml;
        public bool isCore = false;
        public bool isProcessed = false;
        //public List<RecordTypeNode> parents = new List<RecordTypeNode>();
        //public List<RecordTypeNode> children = new List<RecordTypeNode>();
        public Dictionary<string, string> parentChildKeys = new Dictionary<string, string>();
        public List<RecordTypeField> recordFields = new List<RecordTypeField>();
        public Dictionary<string, List<RecordTypeField>> recordFieldsIdx = new Dictionary<string, List<RecordTypeField>>();
        public List<EventNode> events = new List<EventNode>();
        public MappingSet currentMappingSet = null;
        public RecordTypeNode currentParent = null;

        public RecordTypeNode(ModuleContext mc, MRecordType rt, XmlElement me)
            : base(rt.name)
        {
            this.name = rt.name;
            this.rtype = rt;
            this.xml = me;
            this.category = EventCategory.record;
            //this.interfaceXml = (XmlElement)this.xml.CloneNode(false);
        }

        public string StubName()
        {
            return this.name.ToLower();
        }

        public XmlElement GetXml()
        {
            return this.xml;
        }

        public void AddRecordField(RecordTypeField field)
        {
            if (!recordFieldsIdx.ContainsKey(field.identifier))
            {
                recordFields.Add(field);
            }
            recordFieldsIdx.GetAddValueDefaulted(field.identifier, new List<RecordTypeField>()).Add(field);
        }

        public static void BuildFieldMappings(List<OutputField> setFields, MappingSet mappedFields, MappingSet childMappedFields)
        {
            foreach (var setField in setFields)
            {
                // FIXME: this doesn't track references for PARENT.x
                MappingSet myFields;
                if (setField.node.Name.Equals("CHILD") && childMappedFields != null)
                {
                    myFields = childMappedFields;
                }
                else if (setField.node.Name.Equals("OBJECT"))
                {
                    myFields = mappedFields;
                }
                else
                {
                    continue;
                }

                List<InputElement> added = new List<InputElement>();
                foreach (var inputElement in setField.inputs)
                {
                    // Each input element will already have its own complete list of inputs
                    if (inputElement.IsFieldReference())
                    {
                        OutputField outfld = null;
                        if (childMappedFields != null && inputElement.node.Name.Equals("CHILD") && childMappedFields.TryGetLatest(inputElement.fieldName, out outfld))
                        {
                            added.AddRange(outfld.inputs);
                        }
                        else if (inputElement.node.Name.Equals("OBJECT") && mappedFields.TryGetLatest(inputElement.fieldName, out outfld))
                        {
                            added.AddRange(outfld.inputs);
                        }
                        if (outfld != null)
                        {
                            inputElement.fieldInputs.Add(outfld);
                        }
                    }
                }

                setField.allInputs.AddRange(added);
                myFields.addMapping(setField.fieldName, setField);
            }
        }

        // TODO: currently we only need allInputs because when it's populated it's not recursing through direct field copies.
        // Should rearrange things to do what we're doing here...
        public void TraceField(ModuleContext mc, MAnalyzeRecordTypes m, OutputField outfield, DatabaseField dbField)
        {
            if (outfield.IsDirectFieldCopy())
            {
                this.TraceField(mc, m, outfield.inputs[0].fieldInputs[0], dbField);
            }
            else if (this.currentParent != null && outfield.IsDirectParentFieldCopy())
            {
                this.currentParent.TraceField(mc, m, outfield.inputs[0].fieldInputs[0], dbField);
                // FIXME: for MAPPER, CONTACT, etc, PARENT.id_acc does not trace back to the sequence to be a private field
            }
            else
            {
                // Mark this mapping site for validation
                Dictionary<string, string> values = new Dictionary<string, string>();
                List<string> keys = new List<string>();
                keys.Add(outfield.recordType);
                keys.Add(outfield.eventId.ToString());
                keys.Add(outfield.mappingId.ToString());

                /* TODO: should notice when a field is mapped to multiple db fields with different type signatures.  Right now, last one wins. */

                values["validate_db"] = dbField.db;
                values["validate_table"] = dbField.table;
                values["validate_field"] = dbField.field;

                m.IdxEventMappings.IndexUpdate(mc, keys, values);
            }
        }

        new public void BuildNode(ModuleContext mc, MAnalyzeRecordTypes m, MappingSet mappedFields, RecordTypeNode parent)
        {
            if (m.recordsSeen.ContainsKey(this.name))
            {
                throw new Exception("[" + this.name + "] creates circular reference in record types configuration: " + m.recordsSeen.Keys.JoinStrings("|"));
            }

            //if (parent != null)
            //{
            //    this.parents.Add(parent);
            //}
            if (this.isProcessed)  // we've already built from this node down
            {
                return;
            }
            m.recordsSeen[this.name] = "";
            this.isProcessed = true;

            MappingSet mapped = new MappingSet(mappedFields);
            foreach (var child in this.events)
            {
                if (child.category == EventCategory.mapping)
                {
                    MappingEventNode mm = child as MappingEventNode;
                    BuildFieldMappings(mm.setFields, mapped, null);
                }
                else
                {
                    SpawnNode childNode = child as SpawnNode;
                    MappingSet childMapped = new MappingSet(mapped);

                    BuildFieldMappings(childNode.spawnMapping.setFields, mapped, childMapped);
                    this.currentMappingSet = mapped;
                    this.currentParent = parent;

                    List<DatabaseField> childDbFields;
                    Dictionary<string, List<DatabaseField>> childDbFieldsIdx;
                    if (child.category == EventCategory.record)
                    {
                        SpawnRecordNode c = childNode as SpawnRecordNode;
                        c.BuildNode(mc, m, childMapped, this);
                        childDbFields = c.GetDbFields();
                        childDbFieldsIdx = c.GetDbFieldsIdx();
                    }
                    else
                    {
                        SpawnTableNode c = childNode as SpawnTableNode;
                        c.BuildNode(mc, m, childMapped, this);
                        childDbFields = c.GetDbFields();
                        childDbFieldsIdx = c.GetDbFieldsIdx();
                    }

                    foreach (var childField in childDbFields)
                    {
                        DatabaseField newChildField = new DatabaseField(childField);
                        if (this.rtype.visibility == Visibility.Private)
                        {
                            newChildField.visibility = Visibility.Private;
                        }
                        OutputField outfield;
                        if (childMapped.TryGetLatest(newChildField.identifier, out outfield))
                        {
                            foreach (var input in outfield.allInputs)
                            {
                                if (input.IsPrivateByDefault())
                                {
                                    newChildField.visibility = Visibility.Private;
                                }
                            }
                            if (child.category == EventCategory.table)
                            {
                                this.TraceField(mc, m, outfield, newChildField);
                            }
                        }
                        else if (child.category == EventCategory.table)
                        {
                            List<string> keys = new List<string>();
                            keys.Add(this.name);
                            keys.Add(newChildField.identifier);
                            keys.Add(newChildField.db);
                            keys.Add(newChildField.table);
                            keys.Add(newChildField.field);
                            m.IdxValidation.IndexInsert(mc, keys);
                        }
                        if (MRecordType.IsTrue(newChildField.Get("required")) && childMapped.exists(newChildField.identifier))
                        {
                            newChildField.Set("required", "");
                        }
                        if (newChildField.visibility == Visibility.Public && !dbFieldsIdx.ContainsKey(newChildField.ToString()))
                        {
                            AddDbField(newChildField);
                        }
                    }
                    //children.Add(childNode);
                }
            }

            XmlElement fields = this.xml.SelectSingleElem("./fields");
            if (fields != null)
            {
                // only print an interface file if fields were defined for the record type
                //fields = (XmlElement)fields.CloneNode(false);
                //this.interfaceXml.PrependChild(fields);

                // Overlay field info from db on top of configured fields
                List<XmlElement> fieldsToAdd = new List<XmlElement>();
                Dictionary<string, string> seenRecFields = new Dictionary<string, string>();
                foreach (var fld in dbFields)
                {
                    RecordTypeField recField = ResolveRecordField(fld);
                    bool dbRequired = MRecordType.IsTrue(fld.Get("required"));
                    if (recField != null)
                    {
                        bool recRequired = MRecordType.IsTrue(recField.Get("required"));
                        if (dbRequired && !recRequired)
                        {
                            recField.Set("required", "true");
                        }
                        else if (recRequired && !dbRequired)
                        {
                            recField.Set("required", "false");
                        }
                        if (seenRecFields.ContainsKey(recField.identifier))
                        {
                            continue;
                        }
                    }

                    if (recField != null)
                    {
                        recField.OverlayProperties(fld, this);
                        recField.UpdateIndex(mc, m.IdxRecordFields, this.name);
                        seenRecFields[recField.identifier] = "";
                    }
                    else
                    {
                        XmlElement toBeAdded = null;
                        recField = AddMissingField(fld, fields, out toBeAdded);
                        fieldsToAdd.Add(toBeAdded);
                        seenRecFields[recField.identifier] = "";
                    }
                }
                foreach (var fld in recordFields)
                {
                    if (!(seenRecFields.ContainsKey(fld.identifier) || this.parentChildKeys.ContainsKey(fld.identifier)))
                    {
                        fld.Set("ignored", "true");
                    }
                    else
                    {
                        fld.Set("ignored", "");
                    }
                }
                fieldsToAdd.Sort(CompareXmlFields);
                foreach (var fld in fieldsToAdd)
                {
                    fields.AppendChild(fld);
                }

                // print out the xml tree for the record type
                string directory = mc.globalContext["record_types_directory"];
                if (!directory.Equals(""))
                {
                    string fileName = System.IO.Path.Combine(directory, this.name + ".xml");
                    this.xml.RemoveAttribute("exists");
                    XmlElement procXml = this.xml.CreateElement("proc");
                    procXml.SetAttribute("name", this.name);
                    procXml.SetAttribute("run_after", "setup_metranet_record_types");
                    procXml.AppendChild(this.xml);
                    procXml.PrettyPrint(fileName);
                    //this.interfaceXml.PrettyPrint(fileName);
                }
            }

            m.recordsSeen.Remove(this.name);
        }

        public static int CompareXmlFields(XmlElement a, XmlElement b)
        {
            bool reqA = MRecordType.IsTrue(a.SelectNodeInnerText("./required"));
            bool reqB = MRecordType.IsTrue(b.SelectNodeInnerText("./required"));
            if (reqA && !reqB)
            {
                return -1;
            }
            else if (reqB && !reqA)
            {
                return 1;
            }
            return a.GetAttribute("name").CompareTo(b.GetAttribute("name"));
        }

        public RecordTypeField ResolveRecordField(DatabaseField dbField)
        {
            // We must match on the alias, if there is one
            if (recordFieldsIdx.ContainsKey(dbField.identifier))
            {
                return recordFieldsIdx[dbField.identifier][0];
            }
            return null;
        }

        public RecordTypeField AddMissingField(DatabaseField field, XmlElement fieldsElement, out XmlElement newElem)
        {
            RecordTypeField newField = new RecordTypeField(field, null);
            newField.Set("missing", "true");
            this.AddRecordField(newField);
            newElem = newField.ToXml(fieldsElement);
            return newField;
        }
    }

    public class RecordTypeDbTable
    {
        public string db;
        public string table;
        public Dictionary<string, string> ignoreFields = new Dictionary<string, string>();
        public Dictionary<string, string> enumFields = new Dictionary<string, string>();

        public RecordTypeDbTable(string db, string table)
        {
            this.db = db;
            this.table = table;
        }
    }

    public class RecordTypeField
    {
        public string field;
        public XmlElement element;
        public Dictionary<string, string> properties;
        public bool createdFromDb = false;

        public RecordTypeField(string field, XmlElement elem)
        {
            this.field = field;
            this.element = elem;
            this.properties = new Dictionary<string, string>();
        }

        public RecordTypeField(DatabaseField dbField, XmlElement elem)
            : this(dbField.field, elem)
        {
            this.createdFromDb = true;
            foreach (var prop in dbField.properties)
            {
                this.Set(prop.Key, prop.Value);
            }
        }

        public string identifier
        {
            get
            {
                string alias = this.Get("alias");
                if (!alias.IsNullOrEmpty())
                {
                    return alias;
                }
                return this.field;
            }
        }

        public string Get(string property)
        {
            return properties.GetValueDefaulted(property, "");
        }

        public string Set(string property, string value)
        {
            properties[property] = value;
            if (this.element != null)
            {
                XmlElement elem = this.element.SelectSingleElem("./" + property);
                if (value.IsNullOrEmpty())
                {
                    if (elem != null)
                    {
                        this.element.RemoveChild(elem);
                    }
                }
                else
                {
                    if (elem == null)
                    {
                        elem = this.element.CreateElement(property);
                        elem.InnerText = value;
                        this.element.AppendChild(elem);
                    }
                    else
                    {
                        elem.InnerText = value;
                    }
                }
            }
            return value;
        }

        public void Clear()
        {
            properties.Clear();
        }

        public void OverlayProperties(RecordTypeField other, RecordTypeNode node)
        {
            if (this.createdFromDb)
            {
                return;
            }
            foreach (var prop in other.properties)
            {
                if (prop.Key.Equals("alias"))
                {
                    continue;
                }
                if (!prop.Value.Equals(""))
                {
                    this.Set(prop.Key, prop.Value);
                }
            }

            // This is a ridiculous hack, but don't have a better way to set id_paramtable
            // at the moment. Can't do what we do with accounts and id_type because we need
            // the id_paramtable early enough to be part of the key.
            if (node.name.StartsWith("core_subscription_rate_t_pt_") || node.name.StartsWith("core_group_subscription_rate_t_pt_"))
            {
                if (this.field.Equals("id_paramtable"))
                {
                    string ptName = node.name.Substring(node.name.IndexOf("rate_t_pt_") + 10).ToLower();
                    IIndex enumIndex = (IIndex)MAnalyzeRecordTypes.mc.mvm.globalContext.GetNamedClassInst("ENUMS_BY_NAME");
                    List<string> keys = new List<string>();
                    keys.Add("mc/id_pt_names");
                    keys.Add(ptName);
                    Dictionary<string, string> values = new Dictionary<string, string>();
                    values["id_enum_data"] = "";
                    string retval = enumIndex.IndexGet(MAnalyzeRecordTypes.mc, keys, values);
                    this.Set("default", "'" + values["id_enum_data"] + "'");
                    this.Set("missing", "true");
                }
            }
        }

        public void UpdateIndex(ModuleContext mc, IIndex idx, string recTypeName)
        {
            List<string> orderedKeyValues = new List<string>() { recTypeName, this.field };
            Dictionary<string, string> updateValues = new Dictionary<string, string>();
            foreach (var prop in this.properties)
            {
                if (!prop.Value.IsNullOrEmpty())
                {
                    updateValues[prop.Key] = prop.Value;
                }
            }
            string retval = idx.IndexUpdate(mc, orderedKeyValues, updateValues);
        }

        public XmlElement ToXml(XmlElement parent)
        {
            XmlElement elem = parent.CreateElement("field");
            string alias = this.Get("alias");
            if (!alias.IsNullOrEmpty())
            {
                elem.SetAttribute("name", alias);
            }
            else
            {
                elem.SetAttribute("name", this.field);
            }
            foreach (var prop in properties)
            {
                if (prop.Key.Equals("alias"))
                {
                    continue;
                }
                if (!prop.Value.IsNullOrEmpty())
                {
                    XmlElement tag = elem.CreateElement(prop.Key);
                    tag.InnerText = prop.Value;
                    elem.AppendChild(tag);
                }
            }
            return elem;
        }

        public override string ToString()
        {
            return field;
        }
    }

    public class DatabaseField : RecordTypeField
    {
        public string db;
        public string table;
        public Visibility visibility;

        public DatabaseField(string db, string table, string field)
            : base(field, null)
        {
            this.db = db;
            this.table = table;
            this.visibility = Visibility.Public;
        }

        public DatabaseField(DatabaseField dbField)
            : this(dbField.db, dbField.table, dbField.field)
        {
            foreach (var prop in dbField.properties)
            {
                this.Set(prop.Key, prop.Value);
            }
        }

        public override string ToString()
        {
            return db + "." + table + "." + field;
        }
    }

    public class MappingSet
    {
        public List<OutputField> mapList;
        public Dictionary<string, List<OutputField>> mapLookup;

        public MappingSet()
        {
            mapList = new List<OutputField>();
            mapLookup = new Dictionary<string, List<OutputField>>();
        }

        public MappingSet(MappingSet orig)
        {
            mapList = new List<OutputField>(orig.mapList);
            mapLookup = new Dictionary<string, List<OutputField>>(orig.mapLookup);
        }

        public void addMapping(string name, OutputField mapping)
        {
            mapList.Add(mapping);
            mapLookup.GetAddValueDefaulted(name, new List<OutputField>()).Add(mapping);
        }

        public bool exists(string name)
        {
            return mapLookup.ContainsKey(name);
        }

        public bool TryGetLatest(string name, out OutputField latest)
        {
            List<OutputField> outlist;
            if (mapLookup.TryGetValue(name, out outlist))
            {
                latest = outlist[outlist.Count - 1];
                return true;
            }
            latest = null;
            return false;
        }
    }
}
