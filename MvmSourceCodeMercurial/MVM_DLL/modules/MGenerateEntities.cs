using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;
using Antlr.Runtime;
using Antlr.Runtime.Tree;
using MvmScript;
using NLog;

namespace MVM
{
    public enum EntityMapDomain { parent, self, child }
    public enum EntityMapDirection { all, up, down }
    public enum EntityMapOperation { all, get, put, metadata }
    public enum EntityMapWhen { all, before, after }
    public class MapAction
    {
        public EntityMapDirection direction;
        public EntityMapOperation operation;
        public EntityMapWhen when;
        public MapAction(XmlElement actionElem)
        {
            string strDirection = actionElem.SelectNodeInnerText("./direction", "all");
            if (strDirection != null) this.direction = (EntityMapDirection)Enum.Parse(typeof(EntityMapDirection), strDirection, true);

            string strOperation = actionElem.SelectNodeInnerText("./operation", "all");
            if (strOperation != null) this.operation = (EntityMapOperation)Enum.Parse(typeof(EntityMapOperation), strOperation, true);

            string strWhen = actionElem.SelectNodeInnerText("./when", "before");
            if (strWhen != null) this.when = (EntityMapWhen)Enum.Parse(typeof(EntityMapWhen), strWhen, true);
        }
    }

    public class MapType
    {
        public string name;
        public List<MapAction> actions = new List<MapAction>();
        public MapType(XmlElement elem)
        {
            this.name = elem.SelectNodeInnerText("./name");
            foreach (var actionElem in elem.SelectElements("./action"))
            {
                MapAction mapAction = new MapAction(actionElem);
                actions.Add(mapAction);
            }
        }

        /// <summary>
        /// Given the inputs, returns true if there is a matching action.
        /// direction is structural
        /// when is structural (and optional)
        /// operation NOT STRUCTURAL right now. 
        /// if means that we need ability... to add a predicate in front...
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="operation"></param>
        /// <param name="when"></param>
        /// <returns></returns>
        public MapAction GetMatch(EntityMapDirection direction, EntityMapWhen when, EntityMapOperation operation)
        {
            foreach (var action in actions)
            {
                if (action.direction.In(EntityMapDirection.all, direction) || direction == EntityMapDirection.all)
                    if (action.when.In(EntityMapWhen.all, when) || when == EntityMapWhen.all)
                        if (action.operation.In(EntityMapOperation.all, operation) || operation == EntityMapOperation.all)
                            return action;
            }
            return null;
        }
    }

    public class MapFilter
    {
        public Dictionary<string, MapType> mapTypes = new Dictionary<string, MapType>();
        public MapFilter(List<XmlElement> mapTypesList)
        {
            foreach (var elem in mapTypesList)
            {
                MapType mapType = new MapType(elem);
                if (mapTypes.ContainsKey(mapType.name))
                {
                    throw new Exception("Error more than one map type defined with name=[" + mapType.name + "]");
                }
                mapTypes[mapType.name] = mapType;
            }
        }



        public static Regex LiteralMapRegex = new Regex(@"\s*(ENTITY|PARENT|CHILD).(\w+)\s*=\s*['""]\s*");
        public bool IsLiteralMap(string text){
            var isLiteral= LiteralMapRegex.IsMatch(text);
            return isLiteral;
        }

        public string GenerateMapping(IEnumerable<XmlElement> elems, EntityMapDomain domain, EntityMapDirection direction, EntityMapWhen when, EntityMapOperation operation, ModuleContext mc)
        {
            // if direction is up we want to do things in the reverse order.
            IEnumerable<XmlElement> orderedElems = elems;
            if (direction == EntityMapDirection.up) orderedElems = elems.ToList().GetReverse();
            StringBuilder sb = new StringBuilder();

            // generate the elems in the proper order
            foreach (var elem in orderedElems)
            {
                string type = elem.GetAttributeDefault("type", "default");
                if (!this.mapTypes.ContainsKey(type))
                    throw new Exception("Error, unknown map type=[" + type + "]");

                MapType mapType = this.mapTypes[type];
                MapAction mapAction = mapType.GetMatch(direction, when, operation);
                
                // if maptype is default and this is a literal map and direction is not up
                // just generate it.
                bool isLiteralMap = IsLiteralMap(elem.InnerText);
                if (isLiteralMap && type.Equals("default") && direction != EntityMapDirection.up)
                {
                    sb.AppendLine(MGenerateEntities.GenerateMap(mc, elem, elem.InnerText, direction, domain));
                    continue;
                }

                // if there is no action for type type, continue
                if (mapAction == null) 
                    continue; 

                // if the map is broken into put and get
                string cfg = "";
                if (elem.Name.Equals("map") && elem.HasChildElements())
                {
                    XmlElement putElem = elem.SelectSingleElem("./put");
                    if (putElem != null)
                    {
                        string putCfg = MGenerateEntities.GenerateMap(mc, putElem, putElem.InnerText, direction, domain);
                        putCfg = AddCondition("OBJECT.operation eq 'put'", putCfg);
                        cfg += putCfg;
                    }
                    XmlElement getElem = elem.SelectSingleElem("./get");
                    if (getElem != null)
                    {
                        string getCfg = MGenerateEntities.GenerateMap(mc, getElem, getElem.InnerText, direction, domain);
                        getCfg = AddCondition("OBJECT.operation eq 'get' or OBJECT.operation eq 'metadata'", getCfg);
                        cfg += getCfg;
                    }
                }
                // otherwise do the map as usual
                else
                {
                    cfg = MGenerateEntities.GenerateMap(mc, elem, elem.InnerText, direction, domain);
                }
                // add the top level operation condition if necessary.
                if (operation!=EntityMapOperation.all && mapAction.operation != EntityMapOperation.all)
                    cfg = AddCondition("OBJECT.operation eq " + mapAction.operation.ToString().q(), cfg);
                sb.Append(cfg);
            }
            return sb.ToString();
        }
        public string AddCondition(string condition, string cfg)
        {
            cfg = @"<if>
                            <condition>" + condition + @"</condition>
                            <then>
                                " + cfg + @"
                            </then>
                          </if>";
            return cfg;
        }

    }


    /*
      <generate_entities>
        <generate_dir>TEMP.in</generate_dir>
        <entity_glob>TEMP.out</entity_glob>
      </generate_entities>
    */
    public class MGenerateEntities : IModuleSetup, IModuleRun
    {
        private string genDirSyntax;
        private IReadString genDirParsed;
        private string genDmDirSyntax;
        private IReadString genDmDirParsed;

        private string entityGlobSyntax;
        private IReadString entityGlobParsed;

        public static string[] operations = new string[] { "print", "validate", "child","child_from_index", "operation", "result_child", "input_child", "input_operation", "result_operation", "grid" };
        public static Dictionary<string, IGenOperationProc> operationGenerators = new Dictionary<string, IGenOperationProc>();
        static MGenerateEntities()
        {
            operationGenerators["validate"] = new GenValidateProc();
            operationGenerators["print"] = new GenPrintProc();
        }

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            string defaultGenerateEntitiesDir = mc.schedulerMaster.GetGeneratedDir(mc.structuralNameSpace, "generated_entities_config");
            string defaultGenerateDmDir = mc.schedulerMaster.GetGeneratedDir(mc.structuralNameSpace, "generated_domain_model");
            MGenerateEntities m = new MGenerateEntities();
            m.genDirSyntax = me.SelectNodeInnerText("./generate_dir", defaultGenerateEntitiesDir.q());
            m.genDirParsed = mc.ParseSyntax(m.genDirSyntax);
            m.genDmDirSyntax = me.SelectNodeInnerText("./generate_domain_model_dir", defaultGenerateDmDir.q());
            m.genDmDirParsed = mc.ParseSyntax(m.genDmDirSyntax);
            // for externally configured entities
            m.entityGlobSyntax = me.SelectNodeInnerText("./entities_glob", "GLOBAL.entities_glob");
            m.entityGlobParsed = mc.ParseSyntax(m.entityGlobSyntax);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string genDir = this.genDirParsed.Read(mc);
            string genDmDir = this.genDmDirParsed.Read(mc);
            string entityGlob = this.entityGlobParsed.Read(mc);

            // lookup and organize maptype definitions
            List<XmlElement> mapTypesList = (List<XmlElement>)mc.globalContext.GetNamedClassInst("entity_map_types", new List<XmlElement>());
            if (mapTypesList.Count == 0)
                throw new Exception("Did not find any map_types defined");
            MapFilter mapFilter = new MapFilter(mapTypesList);

            // Get all the entity xml elements, including both groups and definitions:
            List<XmlElement> entityElems = new List<XmlElement>();
            entityElems.AddRange(GetEmbeddedEntityElems());
            entityElems.AddRange(GetGlobEntityElems(entityGlob));
            List<XmlElement> entitiesList = (List<XmlElement>)mc.globalContext.GetNamedClassInst("entities", new List<XmlElement>());
            entityElems.AddRange(entitiesList);
            List<XmlElement> entityGroupsList = (List<XmlElement>)mc.globalContext.GetNamedClassInst("entity_groups", new List<XmlElement>());
            entityElems.AddRange(entityGroupsList);
            if (entityElems.Count == 0)
                throw new Exception("Did not find any entities/entity_groups to generate");

            // organize entity groups and definitions by name
            Dictionary<string, XmlElement> entityGroups = OrganizeEntityGroupsByName(entityElems);
            Dictionary<string, XmlElement> entityDefs = OrganizeEntityDefsByName(entityElems);

            // apply groups information to the definitions
            ApplyEntityGroupsToDefs(entityGroups, entityDefs);

            // assign ids to grids/operations/children 
            AssignEntityPartIds(entityDefs);

            // generate the entity procs
            StringBuilder procs = new StringBuilder("<procs>".AppendLine());
            foreach (var entity in entityDefs)
            {
                XmlElement entityDef = entity.Value;
                string self = entity.Key;
                string selfLc = self.ToLower();
                string selfUc = self.ToUpper();
                string parentMapDownProc = "parent_" + selfLc + "_map_down";
                string parentMapUpProc = "parent_" + selfLc + "_map_up";
                string selfMapDownProc = "self_" + selfLc + "_map_down";
                string selfMapUpProc = "self_" + selfLc + "_map_up";
                string callEntityProc = selfLc + "_call_entity";
                procs.AppendLine(GenerateCallEntityProc(mc, entityDef, callEntityProc, parentMapDownProc, selfMapDownProc));
                foreach (XmlElement elem in entityDef.SelectNodes("./child|input_child|result_child|./grid/child"))
                    procs.Append(GenerateChildMapping(mc, mapFilter, entityDef, elem));
                procs.Append(GenerateParentMapDownProc(mc, mapFilter, entityDef, parentMapDownProc));
                procs.Append(GenerateParentMapUpProc(mc, mapFilter, entityDef, parentMapUpProc));
                procs.Append(GenerateSelfMapDownProc(mc, mapFilter, entityDef, selfMapDownProc));
                procs.Append(GenerateSelfMapUpProc(mc, mapFilter, entityDef, selfMapUpProc));
                foreach (var opElem in entityDef.GetChildElems())
                {
                    if (MGenerateEntities.operationGenerators.ContainsKey(opElem.Name))
                    {
                        IGenOperationProc genOpProc = MGenerateEntities.operationGenerators[opElem.Name];
                        procs.AppendLine(genOpProc.GenerateOperationProc(mc, entityDef, opElem));
                    }
                }
            }
            procs.AppendLine(GenerateEntityParentMapUpProc(mc, entityDefs));
            procs.AppendLine(GenerateEntitySelfMapUpProc(mc, entityDefs));
            procs.AppendLine(GenerateRegisterMvmEntities(mc, entityDefs));
            procs.AppendLine(@"</procs>");
            string procFileName = Path.Combine(genDir, "entity_procs.xml");
            File.WriteAllText(procFileName, procs.ToString());
            mc.ReadXmlConfigFromFiles(procFileName);
        }

        private static int entityPartSeq = 1;
        private static void AssignEntityPartIds(Dictionary<string, XmlElement> entityDefs)
        {
            foreach (XmlElement entityDef in entityDefs.Values)
            {
                foreach (XmlElement elem in entityDef.GetElementsByTagName("*"))
                {
                    elem.SetAttribute("entityPartSeq", (++entityPartSeq).ToString());
                }
            }
        }

        private static void ApplyEntityGroupsToDefs(Dictionary<string, XmlElement> entityGroups, Dictionary<string, XmlElement> entityDefs)
        {
            foreach (XmlElement entityDef in entityDefs.Values)
            {
                // assign the default entity group.
                if (entityDef.SelectSingleElem("./entity_group") == null)
                {
                    entityDef.AppendTextElement("entity_group", "default");
                }
                // lookup the entity group
                string entityGroup = entityDef.SelectNodeInnerText("entity_group");
                if (!entityGroups.ContainsKey(entityGroup))
                {
                    throw new Exception("Entity group=[" + entityGroup + "] not defined. Errored entity:".AppendLine() + entityDef.OuterXml);
                }
                XmlElement groupDef = entityGroups[entityGroup];


                // By default we want the groups maps to be lower priority then the entities own maps
                // mean they should happen before so entity will overwrite the group by default on the way 
                // down and the again on the way back up. So if you do not specify before or after you get
                // before
                XmlElement entityParentMapping = entityDef.SelectSingleElem("./parent");
                if (entityParentMapping == null)
                {
                    entityParentMapping = entityDef.CreateElement("parent");
                    entityParentMapping = entityDef.AppendChildElement(entityParentMapping);
                }
                XmlElement groupParentMapping = groupDef.SelectSingleElem("./parent");
                if (groupParentMapping != null)
                {
                    foreach (XmlElement map in groupParentMapping.SelectNodes("print|map|map_before|map_up|map_up_before|map_down|map_down_before").ToList().GetReverse())
                    {
                        string tag = map.LocalName.Replace("_before", "");
                        entityParentMapping.PrependChildImport(map.CloneButChangeTagName(tag, true));
                    }
                    foreach (XmlElement map in groupParentMapping.SelectNodes("print|map_after|map_up_after|map_down_after"))
                    {
                        string tag = map.LocalName.Replace("_after", "");
                        entityParentMapping.AppendChildImport(map.CloneButChangeTagName(tag, true));
                    }
                }

                // same thing for self.
                XmlElement entitySelfMapping = entityDef.SelectSingleElem("./self");
                if (entitySelfMapping == null)
                {
                    entitySelfMapping = entityDef.CreateElement("self");
                    entitySelfMapping = entityDef.AppendChildElement(entitySelfMapping);
                }
                XmlElement groupSelfMapping = groupDef.SelectSingleElem("./self");
                if (groupSelfMapping != null)
                {
                    foreach (XmlElement map in groupSelfMapping.SelectNodes("map|map_before|map_up|map_up_before|map_down|map_down_before").ToList().GetReverse())
                    {
                        string tag = map.LocalName.Replace("_before", "");
                        entitySelfMapping.PrependChildImport(map.CloneButChangeTagName(tag, true));
                    }
                    foreach (XmlElement map in groupSelfMapping.SelectNodes("map_after|map_up_after|map_down_after"))
                    {
                        string tag = map.LocalName.Replace("_after", "");
                        entitySelfMapping.AppendChildImport(map.CloneButChangeTagName(tag, true));
                    }
                }

                // Any operations in then entity group happen after each entities' own child/operations
                foreach (XmlElement operation in groupDef.SelectNodes("./operation|operation_after|input_operation|input_operation_after|result_operation|result_operation_after"))
                {
                    string tag = operation.LocalName.Replace("_after", "");
                    entityDef.AppendChildImport(operation.CloneButChangeTagName(tag, true));
                }
                foreach (XmlElement operation in groupDef.SelectNodes("./operation_before|input_operation_before|result_operation_before").ToList().GetReverse())
                {
                    string tag = operation.LocalName.Replace("_before", "");
                    entityDef.PrependChildImport(operation.CloneButChangeTagName(tag, true));
                }
            }
        }

        private static Dictionary<string, XmlElement> OrganizeEntityDefsByName(List<XmlElement> entityElems)
        {
            Dictionary<string, XmlElement> entityDefs = new Dictionary<string, XmlElement>();
            foreach (XmlElement entityDef in entityElems.Where(x => x.LocalName.Equals("entity")))
            {
                string entityName = entityDef.SelectNodeInnerText("./name").ToLower();
                if (entityDefs.ContainsKey(entityName))
                {
                    string msg = "Error more then one entity is named [" + entityName + "].";
                    msg = msg.AppendLine("first match:");
                    msg = msg.AppendLine(entityDefs[entityName].OuterXml);
                    msg = msg.AppendLine("second match:");
                    msg = msg.AppendLine(entityDef.OuterXml);
                    throw new Exception(msg);
                }
                entityDefs[entityName] = entityDef;
            }
            return entityDefs;
        }
        private static Dictionary<string, XmlElement> OrganizeEntityGroupsByName(List<XmlElement> entityElems)
        {
            Dictionary<string, XmlElement> entityGroups = new Dictionary<string, XmlElement>();
            foreach (XmlElement entityGroup in entityElems.Where(x => x.LocalName.Equals("entity_group")))
            {
                string groupName = entityGroup.SelectNodeInnerText("./name").ToLower();
                if (entityGroups.ContainsKey(groupName))
                {
                    string msg = "Error more then one entity_group is named [" + groupName + "].";
                    msg = msg.AppendLine("first match:");
                    msg = msg.AppendLine(entityGroups[groupName].OuterXml);
                    msg = msg.AppendLine("second match:");
                    msg = msg.AppendLine(entityGroup.OuterXml);
                    throw new Exception(msg);
                }
                entityGroups[groupName] = entityGroup;
            }
            return entityGroups;
        }
        public static List<XmlElement> GetGlobEntityElems(string entityGlob)
        {
            List<XmlElement> entityElems = new List<XmlElement>();
            if (!entityGlob.Equals(""))
            {
                foreach (var file in FileUtils2.Glob(entityGlob))
                {
                    var doc = MyXml.ParseXmlFile(file);
                    // add in filename/extension information to all entity nodes
                    string extensionDir = MvmEngine.GetExtensionDir(file);
                    foreach (XmlElement elem in doc.DocumentElement.SelectElements("//entity_group|entity"))
                    {
                        elem.AppendTextElement("file_name", file);
                        elem.AppendTextElement("extension_dir", extensionDir);
                        entityElems.Add(elem);
                    }

                }
            }
            return entityElems;
        }

        public static List<XmlElement> GetEmbeddedEntityElems()
        {
            List<XmlElement> entityElems = new List<XmlElement>();
            var entryAssembly = Assembly.GetEntryAssembly();
            foreach (var file in entryAssembly.GetManifestResourceNames())
            {
                //Console.WriteLine("Checking:" + resourceName);
                if (file.matches(@".*\.entities\..*\.xml"))
                {
                    Console.WriteLine("Loading:" + file);
                    Stream strm = entryAssembly.GetManifestResourceStream(file);
                    StreamReader reader = new StreamReader(strm);
                    string xmlString = reader.ReadToEnd();
                    var doc = MyXml.ParseXmlString(xmlString);
                    // add in filename/extension information to all entity nodes
                    string extensionDir = MvmEngine.GetExtensionDir(file);
                    foreach (XmlElement elem in doc.DocumentElement.SelectElements("//entity_group|entity"))
                    {
                        elem.AppendTextElement("file_name", file);
                        elem.AppendTextElement("extension_dir", extensionDir);
                        entityElems.Add(elem);
                    }
                }
            }
            return entityElems;
        }

        public string GenerateRegisterMvmEntities(ModuleContext mc, Dictionary<string, XmlElement> entityDefs)
        {
            StringBuilder procs = new StringBuilder();
            procs.AppendLine(
@"<proc name='register_mvm_entities'>
<startup>
    <define_memory_index>
        <index>'MVM_ENTITIES'</index>
        <key_field>'entity_name'</key_field>
        <field>'is_public'</field>
        <field>'file_name'</field>
        <field>'extension_dir'</field>
    </define_memory_index>
 
");
            foreach (var entity in entityDefs)
            {
                XmlElement entityElem = entity.Value;
                string isPublic = entityElem.GetAttributeDefaulted("is_public", "false");
                string fileName = entityElem.SelectNodeInnerText("./file_name", "unspecified");
                string extensionDir = entityElem.SelectNodeInnerText("./extension_dir", "unspecified");
                string entityName = entity.Key;
                procs.AppendLine(@"
    <index_insert>
    <index>'MVM_ENTITIES'</index>
    <field name='entity_name'>'" + entityName + @"'</field>
    <field name='is_public'>'" + isPublic + @"'</field>
    <field name='file_name'>'" + fileName + @"'</field>
    <field name='extension_dir'>'" + extensionDir + @"'</field>
    </index_insert>
    ");
            }
            procs.AppendLine("</startup></proc>");
            return procs.ToString();
        }



        public string GenerateEntityParentMapUpProc(ModuleContext mc, Dictionary<string, XmlElement> entityDefs)
        {
            StringBuilder procs = new StringBuilder();
            procs.AppendLine(
@"<proc name='entity_map_up'>
    <param name='child_oid'/>
    <param name='parent_oid'/>
    <do>TEMP.target_prefix=OBJECT(OBJECT(TEMP.parent_oid).static_oid).target_prefix</do>

    <debug switch='log_entities'>'entity_map_up of '~TEMP.child_oid~'/'~OBJECT(TEMP.child_oid).object_type~' to '~TEMP.parent_oid~'/'~OBJECT(TEMP.parent_oid).object_type</debug>
    
    <debug switch='log_entities'>'child before:'</debug>
    <if>
    <condition>GLOBAL.log_entities eq 1</condition>
    <then>
        <call_proc_for_object>
        <name>'debug_entity'</name>
        <object_id>TEMP.child_oid</object_id>
        </call_proc_for_object>
    </then>
    </if>

    <do>TEMP.child_ot=OBJECT(TEMP.child_oid).object_type</do>
    <if>");
            foreach (var self in entityDefs.Keys)
            {
                string selfUc = self.ToUpper();
                string selfLc = self.ToLower();
                string mapSelfToParentProc = "parent_" + selfLc + "_map_up";
                procs.AppendLine(
@"  
    <condition>TEMP.child_ot eq '" + selfUc + @"'</condition>
    <then>

        <!-- add grouping and labeling  if specified -->
        <do>TEMP.entity_apply_field_label=OBJECT(TEMP.child_oid).entity_apply_field_label</do>
        <if>
        <condition>TEMP.entity_apply_field_label ne ''</condition>
        <then>
            <call_proc_for_current_object>
            <name>'entity_apply_field_label'</name>
            <param name='field_label'>TEMP.entity_apply_field_label</param>
            </call_proc_for_current_object>
        </then>
        </if>

        <do>TEMP.map_up_before_proc=OBJECT(TEMP.child_oid).map_up_before_proc</do>
        <if>
        <condition>TEMP.map_up_before_proc ne ''</condition>
        <then>
            <call_dynamic_proc_for_current_object>
            <name>TEMP.map_up_before_proc</name>
            <object_id>TEMP.child_oid</object_id>
            <param name='parent_oid'>TEMP.parent_oid</param>
            </call_dynamic_proc_for_current_object>
        </then>
        </if>
        <call_proc_for_object>
        <name>'" + mapSelfToParentProc + @"'</name>
        <object_id>TEMP.child_oid</object_id>
        <param name='parent_oid'>TEMP.parent_oid</param>
        </call_proc_for_object>
        <do>TEMP.map_up_after_proc=OBJECT(TEMP.child_oid).map_up_after_proc</do>
        <if>
        <condition>TEMP.map_up_after_proc ne ''</condition>
        <then>
            <call_dynamic_proc_for_current_object>
            <name>TEMP.map_up_after_proc</name>
            <object_id>TEMP.child_oid</object_id>
            <param name='parent_oid'>TEMP.parent_oid</param>
            </call_dynamic_proc_for_current_object>
        </then>
        </if>
    </then>");
            }
            procs.AppendLine(
@"  <else>
        <fatal>'unexpected child object type='~OBJECT(TEMP.child_oid).object_type</fatal>
    </else>
    </if>
    <debug switch='log_entities'>'parent after:'</debug>
    <if>
    <condition>GLOBAL.log_entities eq 1</condition>
    <then>
        <call_proc_for_object>
        <name>'debug_entity'</name>
        <object_id>TEMP.parent_oid</object_id>
        </call_proc_for_object>
    </then>
    </if>

</proc>");
            return procs.ToString();
        }


        public string GenerateEntitySelfMapUpProc(ModuleContext mc, Dictionary<string, XmlElement> entityDefs)
        {
            StringBuilder procs = new StringBuilder();
            procs.AppendLine(
@"<proc name='entity_self_map_up'>
    <param name='child_oid'/>
    <debug switch='log_entities'>'entity_self_map_up of '~TEMP.child_oid~'/'~OBJECT(TEMP.child_oid).object_type</debug>
    <if>
    <condition>GLOBAL.log_entity_self_mapping eq 1</condition>
    <then>
        <debug>'self before:'</debug>
        <call_proc_for_object>
        <name>'debug_entity'</name>
        <object_id>TEMP.child_oid</object_id>
        </call_proc_for_object>
    </then>
    </if>
    <do>TEMP.child_ot=OBJECT(TEMP.child_oid).object_type</do>
    <if>");
            foreach (var self in entityDefs.Keys)
            {
                string selfUc = self.ToUpper();
                string selfLc = self.ToLower();
                string mapSelfToParentProc = "self_" + selfLc + "_map_up";
                procs.AppendLine(
@"  
    <condition>TEMP.child_ot eq '" + selfUc + @"'</condition>
    <then>
        <call_proc_for_object>
        <name>'" + mapSelfToParentProc + @"'</name>
        <object_id>TEMP.child_oid</object_id>
        <param name='parent_oid'>TEMP.parent_oid</param>
        </call_proc_for_object>
    </then>");
            }
            procs.AppendLine(
@"  <else>
        <fatal>'unexpected child object type='~OBJECT(TEMP.child_oid).object_type</fatal>
    </else>
    </if>
    <if>
    <condition>GLOBAL.log_entity_self_mapping eq 1</condition>
    <then>
        <debug>'self before:'</debug>
        <call_proc_for_object>
        <name>'debug_entity'</name>
        <object_id>TEMP.child_oid</object_id>
        </call_proc_for_object>
    </then>
    </if>
</proc>");
            return procs.ToString();
        }

        public string GenerateParentMapDownProc(ModuleContext mc, MapFilter mapFilter, XmlElement entityDef, string procName)
        {
            StringBuilder procs = new StringBuilder();
            procs.AppendLine(
@"<proc name='" + procName + @"'>
    <param name='parent_oid'/>
    <info switch='log_entity_map'>'map_down from '~TEMP.parent_oid~'/'~OBJECT(TEMP.parent_oid).object_type~' to '~OBJECT.object_id~'/'~OBJECT.object_type</info>
");
            procs.Append(
                   mapFilter.GenerateMapping(
                       entityDef.SelectSingleElem("./parent").GetChildElems(),
                       EntityMapDomain.parent,
                       EntityMapDirection.down,
                       EntityMapWhen.all,
                       EntityMapOperation.all,
                       mc
                   )
               );
            procs.AppendLine(@"
</proc>");
            return procs.ToString();
        }

        public string GenerateParentMapUpProc(ModuleContext mc, MapFilter mapFilter, XmlElement entityDef, string procName)
        {
            StringBuilder procs = new StringBuilder();
            procs.AppendLine(
@"<proc name='" + procName + @"'>
    <param name='parent_oid'/>
    <do>TEMP.target_prefix=OBJECT(OBJECT(TEMP.parent_oid).static_oid).target_prefix</do>
    <info switch='log_entity_map'>'map_up from '~OBJECT.object_id~'/'~OBJECT.object_type~' to '~TEMP.parent_oid~'/'~OBJECT(TEMP.parent_oid).object_type~',target_prefix='~TEMP.target_prefix</info>
");
            procs.Append(
                   mapFilter.GenerateMapping(
                       entityDef.SelectSingleElem("./parent").GetChildElems(),
                       EntityMapDomain.parent,
                       EntityMapDirection.up,
                       EntityMapWhen.all,
                       EntityMapOperation.all,
                       mc
                   )
               );

            procs.AppendLine(@"
</proc>");
            return procs.ToString();
        }



        public string GenerateSelfMapDownProc(ModuleContext mc, MapFilter mapFilter, XmlElement entityDef, string procName)
        {
            StringBuilder procs = new StringBuilder();
            procs.AppendLine(
@"<proc name='" + procName + @"'>
    <info switch='log_self_map'>'self_map_down for '~OBJECT.object_id~'/'~OBJECT.object_type</info>
");
            procs.Append(
                   mapFilter.GenerateMapping(
                       entityDef.SelectSingleElem("./self").GetChildElems(),
                       EntityMapDomain.self,
                       EntityMapDirection.down,
                       EntityMapWhen.all,
                       EntityMapOperation.all,
                       mc
                   )
               );
            procs.AppendLine(@"
</proc>");
            return procs.ToString();
        }


        public string GenerateSelfMapUpProc(ModuleContext mc, MapFilter mapFilter, XmlElement entityDef, string procName)
        {
            StringBuilder procs = new StringBuilder();
            procs.AppendLine(
@"<proc name='" + procName + @"'>
    <info switch='log_self_map'>'self_map_up for '~OBJECT.object_id~'/'~OBJECT.object_type</info>
");
            procs.Append(
                   mapFilter.GenerateMapping(
                       entityDef.SelectSingleElem("./self").GetChildElems(),
                       EntityMapDomain.self,
                       EntityMapDirection.up,
                       EntityMapWhen.all,
                       EntityMapOperation.all,
                       mc
                   )
               );
            procs.AppendLine(@"
</proc>");
            return procs.ToString();
        }

        public string GenerateDoLoadDynamicEntities()
        {
            StringBuilder procs = new StringBuilder();
            procs.AppendLine(@"
  <c> make sure there is a dynamic version of self in DYNAMIC_ENTITIES </c>
    <index_select>
    <index>'DYNAMIC_ENTITIES'</index>
    <field name='static_oid'>OBJECT.object_id</field>
    <cursor>TEMP.csr</cursor>
    <then>
        <do>TEMP.dynamic_oid=OBJECT(TEMP.csr).dynamic_oid</do>
        <debug switch='log_entities'>'Looks like you preloaded dynamic entities for type='~OBJECT(TEMP.dynamic_oid).object_type</debug>
    </then>
    <loop>
        <do>TEMP.dynamic_oid=OBJECT(TEMP.csr).dynamic_oid</do>
        <debug switch='log_entities'>'LOOP Looks like you preloaded dynamic entities for type='~OBJECT(TEMP.dynamic_oid).object_type</debug>
        <if>
            <condition>TEMP.dynamic_oid eq OBJECT.object_id</condition>
            <then>
                <fatal>'Error, static object cannot point to itself as a dynamic object in DYNAMIC_ENTITIES. Offending oid/type='~OBJECT.object_id~'/'~OBJECT.object_type</fatal>
            </then>
        </if>
        <do>OBJECT(TEMP.dynamic_oid).static_oid=OBJECT.object_id</do>
    </loop>
    <else>
        <call_proc_for_current_object>
        <name>'create_dynamic_entity'</name>
        <param name='static_oid'>OBJECT.object_id</param>
        <param name='dynamic_oid'>TEMP.my_new_obj</param>
        </call_proc_for_current_object>        
    </else>
    </index_select>
");
            return procs.ToString();
        }


        public string GenerateCallEntityProc(ModuleContext mc, XmlElement entityDef, string callEntityProc, string parentMapDownProc, string selfMapDownProc)
        {
            StringBuilder procs = new StringBuilder();
            procs.AppendLine(
@"<proc name='" + callEntityProc + @"'>
    <param name='parent_oid' default='""""'/>
    <debug switch='log_entities'>'object '~OBJECT.object_id~'/'~OBJECT.object_type~' in " + callEntityProc + @"('~TEMP.parent_oid~'/'~OBJECT(TEMP.parent_oid).object_type~')'</debug>
    ");
            procs.AppendLine(GenerateDoParentMapDown(entityDef, parentMapDownProc));
            procs.AppendLine(GenerateDoSelfMapDown(entityDef, selfMapDownProc));
            procs.AppendLine(GenerateDoLoadDynamicEntities());
            // Generate do all the operations and children
            //foreach (XmlElement elem in entityDef.SelectNodes("./validate|child|operation|result_child|input_child|input_operation|result_operation|grid"))
            foreach (XmlElement elem in entityDef.GetChildElems().Where(c => c.Name.In(operations)))
            {
                if (elem.LocalName.In("input_operation"))
                    procs.Append(GenerateDoInputOperation(elem));
                else if (elem.LocalName.In("result_operation", "operation"))
                    procs.Append(GenerateDoResultOperation(elem));
                else if (elem.LocalName.In("result_child"))
                    procs.Append(GenerateDoResultChild(entityDef, elem));
                else if (elem.LocalName.In("input_child", "child"))
                    procs.Append(GenerateDoInputChild(entityDef, elem));
                else if (elem.LocalName.In("child_from_index"))
                    procs.Append(GenerateDoInputChildFromIndex(entityDef, elem));
                else if (MGenerateEntities.operationGenerators.ContainsKey(elem.LocalName))
                    procs.Append(GenerateDoResultOperation(mc, entityDef, elem));
                else if (elem.LocalName.In("grid"))
                    procs.Append("");
                //    procs.Append(GenerateDoGrid(entityDef,elem));
                else
                    throw new Exception("unexpected:" + elem.OuterXml);
            }
            procs.AppendLine(
@"
    </proc>");
            return procs.ToString();
        }
        public string GenerateDoParentMapDown(XmlElement def, string parentMapDownProc)
        {
            StringBuilder procs = new StringBuilder();
            procs.AppendLine(@"
            <if>
    <condition>TEMP.parent_oid ne ''</condition>
    <then>
        <debug switch='log_entities'>'parent before:'</debug>
        <if>
        <condition>GLOBAL.log_entities eq 1</condition>
        <then>
            <call_proc_for_object>
            <name>'debug_entity'</name>
            <object_id>TEMP.parent_oid</object_id>
            </call_proc_for_object>
        </then>
        </if>

        <!-- do>TEMP.map_down_before_proc=OBJECT(TEMP.parent_oid).map_down_before_proc</do -->
        <do>TEMP.map_down_before_proc=OBJECT.map_down_before_proc</do>
        <if>
            <condition>TEMP.map_down_before_proc ne ''</condition>
            <then>
            <call_dynamic_proc_for_current_object>
              <name>TEMP.map_down_before_proc</name>
              <param name='parent_oid'>TEMP.parent_oid</param>
            </call_dynamic_proc_for_current_object>
            </then>
        </if>
        <call_proc_for_current_object>
        <name>'" + parentMapDownProc + @"'</name>
        <param name='parent_oid'>TEMP.parent_oid</param>
        </call_proc_for_current_object>
        <!--do>TEMP.map_down_after_proc=OBJECT(TEMP.parent_oid).map_down_after_proc</do -->
        <do>TEMP.map_down_after_proc=OBJECT.map_down_after_proc</do>
        <if>
            <condition>TEMP.map_down_after_proc ne ''</condition>
            <then>
            <call_dynamic_proc_for_current_object>
              <name>TEMP.map_down_after_proc</name>
              <param name='parent_oid'>TEMP.parent_oid</param>
            </call_dynamic_proc_for_current_object>
            </then>
        </if>
        <debug switch='log_entities'>'child after:'</debug>
        <if>
        <condition>GLOBAL.log_entities eq 1</condition>
        <then>
            <call_proc_for_object>
            <name>'debug_entity'</name>
            <object_id>OBJECT.object_id</object_id>
            </call_proc_for_object>
        </then>
        </if>

    </then>
    <else>
        <debug switch='log_entities'>'skip map_down since parent_id is null'</debug>
    </else>
    </if>");
            return procs.ToString();
        }



        public string GenerateDoSelfMapDown(XmlElement def, string selfMapDownProc)
        {
            StringBuilder procs = new StringBuilder();
            procs.AppendLine(@"
            <if>
    <condition>TEMP.parent_oid ne ''</condition>
    <then>
        <debug switch='log_entities'>'parent before:'</debug>
        <if>
        <condition>GLOBAL.log_entities eq 1</condition>
        <then>
            <call_proc_for_object>
            <name>'debug_entity'</name>
            <object_id>TEMP.parent_oid</object_id>
            </call_proc_for_object>
        </then>
        </if>
        
        <call_proc_for_current_object>
        <name>'" + selfMapDownProc + @"'</name>
        <param name='parent_oid'>TEMP.parent_oid</param>
        </call_proc_for_current_object>

        <debug switch='log_entities'>'child after:'</debug>
        <if>
        <condition>GLOBAL.log_entities eq 1</condition>
        <then>
            <call_proc_for_object>
            <name>'debug_entity'</name>
            <object_id>OBJECT.object_id</object_id>
            </call_proc_for_object>
        </then>
        </if>

    </then>
    <else>
        <debug switch='log_entities'>'skip map_down since parent_id is null'</debug>
    </else>
    </if>");
            return procs.ToString();
        }

        // adds condition if top_level is true
        private string AddTopLevelConditions(string config, XmlElement elem)
        {
            if (elem.GetAttribute("top_level").Equals("true"))
            {
                return
         @"
        <!-- top level only -->
        <if>
        <condition>TEMP.parent_oid eq ''</condition>
        <then>
" + config + @"
        </then>
        </if>";
            }
            return config;
        }

        // predicate means no condition.
        private string AddOperationConditions(string config, XmlElement elem, string operationField)
        {
            List<string> conditions = new List<string>();
            if (elem.HasAttribute("name")) conditions.Add(operationField + " eq '" + elem.GetAttribute("name") + "'");
            if (elem.GetAttribute("put").Equals("true")) conditions.Add(operationField + " eq 'put'");
            if (elem.GetAttribute("get").Equals("true")) conditions.Add(operationField + " eq 'get'");
            if (elem.GetAttribute("metadata").Equals("true")) conditions.Add(operationField + " eq 'metadata'");
            if (conditions.Count > 0)
            {
                return
         @"
        <if>
        <condition>" + conditions.Join(" or ") + @"</condition>
        <then>
" + config + @"
        </then>
        </if>";
            }
            return config;
        }

        public string GenerateDoInputOperation(XmlElement elem)
        {
            StringBuilder procs = new StringBuilder();
            int entityPartSeq = elem.GetAttribute("entityPartSeq").ToInt();
            string operationProc = elem.InnerText.ToLower();

            string callOperatorConfig = @"
            <info switch='log_operator_call'>'..call operation " + operationProc + @" for '~OBJECT.object_id~'/'~OBJECT.object_type</info>
            <call_proc_for_current_object>
            <name>'" + operationProc + @"'</name>
            </call_proc_for_current_object>
            <debug switch='log_entity_operator_results'>'..operator results:'</debug>
            <if>
                <condition>GLOBAL.log_entity_operator_results eq 1</condition>
                <then>
                    <call_proc_for_current_object>
                    <name>'debug_dynamic_entities'</name>
                    </call_proc_for_current_object>
                </then>
            </if>
        ";
            callOperatorConfig = AddOperationConditions(callOperatorConfig, elem, "OBJECT.operation");
            callOperatorConfig = AddTopLevelConditions(callOperatorConfig, elem);
            procs.AppendLine(callOperatorConfig);
            return procs.ToString();
        }
        public string GenerateDoResultOperation(XmlElement elem)
        {
            StringBuilder procs = new StringBuilder();
            int entityPartSeq = elem.GetAttribute("entityPartSeq").ToInt();
            string operationProc = elem.InnerText.ToLower();
            string callOperatorConfig = @"
            <debug switch='log_operator_call'>'..call operation " + operationProc + @" for '~TEMP.dynamic_self_oid~'/'~OBJECT(TEMP.dynamic_self_oid).object_type</debug>
            <call_proc_for_object>
            <name>'" + operationProc + @"'</name>
            <object_id>TEMP.dynamic_self_oid</object_id>
            </call_proc_for_object>
            <debug switch='log_entity_operator_results'>'..operator results:'</debug>
            <if>
            <condition>GLOBAL.log_entity_operator_results eq 1</condition>
            <then>
                <call_proc_for_object>
                <name>'debug_entity'</name>
                <object_id>TEMP.dynamic_self_oid</object_id>
                </call_proc_for_object>
            </then>
            </if>
";
            callOperatorConfig = AddOperationConditions(callOperatorConfig, elem, "OBJECT(TEMP.dynamic_self_oid).operation");
            callOperatorConfig = @"
    <proc_select>
    <name>'select_snap_dynamic_entities'</name>
    <param name='static_oid'>OBJECT.object_id</param>
    <cursor>TEMP.csr</cursor>
    <loop>
        <do>TEMP.dynamic_self_oid=OBJECT(TEMP.csr).dynamic_oid</do>
" + callOperatorConfig + @"

    </loop>
    </proc_select>";
            callOperatorConfig = AddTopLevelConditions(callOperatorConfig, elem);
            procs.AppendLine(callOperatorConfig);
            return procs.ToString();
        }

        public static string GetOperationProcName(XmlElement opElem)
        {
            int entityPartSeq = opElem.GetAttribute("entityPartSeq").ToInt();
            string opName = opElem.Name;
            string opProcName = "op_" + opName + "_" + entityPartSeq;
            return opProcName;
        }

        public string GenerateDoResultOperation(ModuleContext mc, XmlElement entityDef, XmlElement opElem)
        {
            string opProcName = GetOperationProcName(opElem);
            string output =
                @"
                <c>
                    " + opElem.OuterXml + @"
                </c>
                <proc_select>
                <name>'select_snap_dynamic_entities'</name>
                <param name='static_oid'>OBJECT.object_id</param>
                <cursor>TEMP.csr</cursor>
                <loop>
                    <do>TEMP.dynamic_oid=OBJECT(TEMP.csr).dynamic_oid</do>
                    <call_proc_for_object>
                    <name>'" + opProcName + @"'</name>
                    <object_id>TEMP.dynamic_oid</object_id>
                    </call_proc_for_object>
                </loop>
                </proc_select>";
            return output;
        }


        public string GenerateDoInputChildFromIndex(XmlElement entityDef, XmlElement childElem)
        {

            StringBuilder procs = new StringBuilder();
            string index_name = childElem.InnerText;
            string childObjectType = "";
            string callChildEntityProc = childObjectType.ToLower() + "_call_entity";
            procs.AppendLine(
@"
<!-- begin child_from_index " + childObjectType + @"-->
        <do>TEMP.dynamic_self_oid=OBJECT.object_id</do>
<print>'HEY I GOT HERE'</print>
<index_select>
<index>"+index_name.q()+ @"</index>
<cursor>TEMP.child_csr</cursor>
<loop>
<do>TEMP.child_oid=OBJECT(TEMP.child_csr).oid</do>
<do>TEMP.child_object_type=OBJECT(TEMP.child_oid).object_type</do>
<do>TEMP.child_entity_proc=lc(TEMP.child_object_type)~'_call_entity'</do>
        <spawn>
        <object_type>TEMP.child_object_type</object_type>
        <object_id>TEMP.static_child_oid</object_id>
        </spawn>

<inherit_object>
<source>TEMP.child_oid</source>
<target>TEMP.static_child_oid</target>
</inherit_object>

        <do>OBJECT(TEMP.static_child_oid).operation=OBJECT.operation</do>
        <info switch='log_entity_children'>'..spawned input_child entity '~TEMP.static_child_oid~'/'~OBJECT(TEMP.static_child_oid).object_type</info>
        " + /*GenerateDoChildMapDown(entityDef, childElem) +*/ @"
        <call_dynamic_proc_for_object>
        <name>TEMP.child_entity_proc</name>
        <object_id>TEMP.static_child_oid</object_id>
        <param name='parent_oid'>OBJECT.object_id</param>
        </call_dynamic_proc_for_object>
        <info switch='log_entity_children'>'..call push_children_to_parents for '~TEMP.static_child_oid~'/'~OBJECT(TEMP.static_child_oid).object_type~' to '~OBJECT.object_id~'/'~OBJECT.object_type</info>
        <call_proc_for_object>
        <name>'push_children_to_parents'</name>
        <object_id>OBJECT.object_id</object_id>
        <param name='static_child_oid'>TEMP.static_child_oid</param>
        <param name='static_parent_oid'>OBJECT.object_id</param>
        </call_proc_for_object>
        <index_remove>
        <index>'DYNAMIC_ENTITIES'</index>
        <field name='static_oid'>TEMP.static_child_oid</field>
        </index_remove>
</loop>
</index_select>
<!-- end child_from_index " + childObjectType + @"-->
");
            return procs.ToString();
        }


        public string GenerateDoInputChild(XmlElement entityDef, XmlElement childElem)
        {

            StringBuilder procs = new StringBuilder();
            string childObjectType = (childElem.HasChildElements() ? childElem.SelectNodeInnerText("./name") : childElem.InnerText).ToUpper();
            string callChildEntityProc = childObjectType.ToLower() + "_call_entity";
            procs.AppendLine(
@"
<!-- begin child "+childObjectType+@"-->
        <do>TEMP.dynamic_self_oid=OBJECT.object_id</do>
        <spawn>
        <object_type>'" + childObjectType + @"'</object_type>
        <object_id>TEMP.static_child_oid</object_id>
        </spawn>
        <do>OBJECT(TEMP.static_child_oid).operation=OBJECT.operation</do>
        <info switch='log_entity_children'>'..spawned input_child entity '~TEMP.static_child_oid~'/'~OBJECT(TEMP.static_child_oid).object_type</info>
        " + GenerateDoChildMapDown(entityDef, childElem) + @"
        <call_proc_for_object>
        <name>'" + callChildEntityProc + @"'</name>
        <object_id>TEMP.static_child_oid</object_id>
        <param name='parent_oid'>OBJECT.object_id</param>
        </call_proc_for_object>
        <info switch='log_entity_children'>'..call push_children_to_parents for '~TEMP.static_child_oid~'/'~OBJECT(TEMP.static_child_oid).object_type~' to '~OBJECT.object_id~'/'~OBJECT.object_type</info>
        <call_proc_for_object>
        <name>'push_children_to_parents'</name>
        <object_id>OBJECT.object_id</object_id>
        <param name='static_child_oid'>TEMP.static_child_oid</param>
        <param name='static_parent_oid'>OBJECT.object_id</param>
        </call_proc_for_object>
        <index_remove>
        <index>'DYNAMIC_ENTITIES'</index>
        <field name='static_oid'>TEMP.static_child_oid</field>
        </index_remove>
<!-- end child " + childObjectType + @"-->
");
            return procs.ToString();
        }

        public string GenerateDoResultChild(XmlElement entityDef, XmlElement childElem)
        {

            StringBuilder procs = new StringBuilder();
            string childObjectType = (childElem.HasChildElements() ? childElem.SelectNodeInnerText("./name") : childElem.InnerText).ToUpper();
            string callChildEntityProc = childObjectType.ToLower() + "_call_entity";
            procs.AppendLine(
@"
    <proc_select>
    <name>'select_snap_dynamic_entities'</name>
    <param name='static_oid'>OBJECT.object_id</param>
    <cursor>TEMP.csr</cursor>
    <loop>
        <do>TEMP.dynamic_self_oid=OBJECT(TEMP.csr).dynamic_oid</do>
        <spawn>
        <object_type>'" + childObjectType + @"'</object_type>
        <object_id>TEMP.static_child_oid</object_id>
        </spawn>
        <do>OBJECT(TEMP.static_child_oid).operation=OBJECT.operation</do>
        <info switch='log_entity_children'>'..spawned result_child entity '~TEMP.static_child_oid~'/'~OBJECT(TEMP.static_child_oid).object_type</info>
        " + GenerateDoChildMapDown(entityDef, childElem) + @"
        <call_proc_for_object>
        <name>'" + callChildEntityProc + @"'</name>
        <object_id>TEMP.static_child_oid</object_id>
        <param name='parent_oid'>TEMP.dynamic_self_oid</param>
        </call_proc_for_object>
        <info switch='log_entity_children'>'..call push_children_to_parents for '~TEMP.static_child_oid~'/'~OBJECT(TEMP.static_child_oid).object_type~' to '~OBJECT.object_id~'/'~OBJECT.object_type</info>
        <call_proc_for_object>
        <name>'push_children_to_parents'</name>
        <object_id>OBJECT.object_id</object_id>
        <param name='static_child_oid'>TEMP.static_child_oid</param>
        <param name='static_parent_oid'>OBJECT.object_id</param>
        </call_proc_for_object>
        <index_remove>
        <index>'DYNAMIC_ENTITIES'</index>
        <field name='static_oid'>TEMP.static_child_oid</field>
        </index_remove>
    </loop>
    </proc_select>");
            return procs.ToString();
        }

        private static string GenerateDoChildMapDown(XmlElement parentElem, XmlElement childElem)
        {
            StringBuilder procs = new StringBuilder();
            int entityPartSeq = childElem.GetAttribute("entityPartSeq").ToInt();
            string selfLc = parentElem.SelectNodeInnerText("./name").ToLower();
            string child;
            string childLc;
            string childUc;
            string doChildMapDownBeforeProc = "";
            string doChildMapDownAfterProc = "";
            string doChildMapUpBeforeProc = "";
            string doChildMapUpAfterProc = "";
            if (childElem.HasChildElements())
            {
                child = childElem.SelectNodeInnerText("./name");
                childLc = child.ToLower();
                childUc = child.ToUpper();
                string childMapDownBeforeProc = selfLc + "_to_" + childLc + "_" + entityPartSeq + "_map_down_before";
                string childMapDownAfterProc = selfLc + "_to_" + childLc + "_" + entityPartSeq + "_map_down_after";
                string childMapUpBeforeProc = childLc + "_to_" + selfLc + "_" + entityPartSeq + "_map_up_before";
                string childMapUpAfterProc = childLc + "_to_" + selfLc + "_" + entityPartSeq + "_map_up_after";
                doChildMapDownBeforeProc = "<do>OBJECT(TEMP.static_child_oid).map_down_before_proc='" + childMapDownBeforeProc + "'</do>";
                doChildMapDownAfterProc = "<do>OBJECT(TEMP.static_child_oid).map_down_after_proc='" + childMapDownAfterProc + "'</do>";
                doChildMapUpBeforeProc = "<do>OBJECT(TEMP.static_child_oid).map_up_before_proc='" + childMapUpBeforeProc + "'</do>";
                doChildMapUpAfterProc = "<do>OBJECT(TEMP.static_child_oid).map_up_after_proc='" + childMapUpAfterProc + "'</do>";
            }
            else
            {
                child = childElem.InnerText;
                childLc = child.ToLower();
                childUc = child.ToUpper();
            }


            // apply label and group if any
            string fieldLabel = childElem.GetAttribute("field_label");
            if (!fieldLabel.Equals(""))
                procs.AppendLine("<do>OBJECT(TEMP.static_child_oid).entity_apply_field_label='" + fieldLabel + "'</do>");

            procs.AppendLine(doChildMapDownBeforeProc);
            procs.AppendLine(doChildMapDownAfterProc);
            procs.AppendLine(doChildMapUpBeforeProc);
            procs.AppendLine(doChildMapUpAfterProc);
            return procs.ToString();
        }


        public string GenerateDoGrid(XmlElement entityDef, XmlElement gridElem)
        {
            StringBuilder procs = new StringBuilder();
            int entityPartSeq = gridElem.GetAttribute("entityPartSeq").ToInt();
            string gridKeyFieldsProc = gridElem.SelectNodeInnerText("grid_key_fields_proc");
            string gridName = gridElem.GetAttribute("name");
            procs.AppendLine(
@"
    <debug switch='log_entities'>'select a snapshot of dynamic entities for static_oid=['~OBJECT.object_id~']'</debug>
    <proc_select>
    <name>'select_snap_dynamic_entities'</name>
    <param name='static_oid'>OBJECT.object_id</param>
    <cursor>TEMP.snap_csr</cursor>
    <loop>
        <do>TEMP.dynamic_self_oid=OBJECT(TEMP.snap_csr).dynamic_oid</do>
        <print>'got dynamic_self_oid='~TEMP.dynamic_self_oid</print>
        <if>
            <condition>OBJECT.operation eq 'get'</condition>
            <then>
                <debug switch='log_entities'>'calling proc_select on=['~" + gridKeyFieldsProc + @"~']'</debug>
                <proc_select>
                <name>" + gridKeyFieldsProc + @"</name>
                <cursor>TEMP.gridkeys</cursor>
                <loop>
                     <print>" + gridKeyFieldsProc + @"~': in loop'</print>
");
            foreach (XmlElement childElem in gridElem.SelectNodes("./child"))
            {
                string childObjectType = (childElem.HasChildElements() ? childElem.SelectNodeInnerText("./name") : childElem.InnerText).ToUpper();
                string callChildEntityProc = childObjectType.ToLower() + "_call_entity";
                procs.AppendLine(@"
                    <spawn>
                    <object_type>'" + childObjectType + @"'</object_type>
                    <object_id>TEMP.static_child_oid</object_id>
                    </spawn>
                    <debug switch='log_entities'>'..spawned child_entity '~TEMP.static_child_oid~'/'~OBJECT(TEMP.static_child_oid).object_type</debug>

                    <debug switch='log_entities'>'..write cursor gridkeys onto dynamic_self_oid='~TEMP.dynamic_self_oid</debug>
                    <inherit_cursor>
                    <cursor>TEMP.gridkeys</cursor>
                    <object_id>TEMP.dynamic_self_oid</object_id>
                    </inherit_cursor>
                    " + GenerateDoChildMapDown(entityDef, childElem) + @"
                    <call_proc_for_object>
                    <name>'" + callChildEntityProc + @"'</name>
                    <object_id>TEMP.static_child_oid</object_id>
                    <param name='parent_oid'>TEMP.dynamic_self_oid</param>
                    </call_proc_for_object>
                    
                    <call_proc_for_object>
                    <name>'get_grid_prefix'</name>
                    <object_id>OBJECT.object_id</object_id>
                    <param name='grid_key_cursor'>TEMP.gridkeys</param>
                    <param name='grid_name'>'" + gridName + @"'</param>
                    <param name='grid_prefix'>TEMP.target_prefix</param>
                    </call_proc_for_object>
                    <debug switch='log_entities'>'..seting grid target_prefix=['~TEMP.target_prefix~'] on static_parent_oid=['~OBJECT.object_id~']'</debug>
                    <do>OBJECT.target_prefix=TEMP.target_prefix</do>
            
                    <debug switch='log_entities'>'..call push_children_to_parents for '~TEMP.static_child_oid~'/'~OBJECT(TEMP.static_child_oid).object_type~' to '~OBJECT.object_id~'/'~OBJECT.object_type</debug>
                    <call_proc_for_object>
                    <name>'push_children_to_parents'</name>
                    <object_id>OBJECT.object_id</object_id>
                    <param name='static_child_oid'>TEMP.static_child_oid</param>
                    <param name='static_parent_oid'>OBJECT.object_id</param>
                    </call_proc_for_object>
                    <index_remove>
                    <index>'DYNAMIC_ENTITIES'</index>
                    <field name='static_oid'>TEMP.static_child_oid</field>
                    </index_remove>
");
            }
            procs.AppendLine(@"
                </loop>
                </proc_select>
            </then>
            <condition>OBJECT.operation eq 'put' or OBJECT.operation eq 'metadata'</condition>
            <then>
                <proc_select>
                <name>'select_gridkeys'</name>
                <cursor>TEMP.gridkeys</cursor>
                <loop>
                    <print>'IN CURSOR LOOP too'</print>
                </loop>
                </proc_select>
            </then>
        </if>
    </loop>
    </proc_select>");
            return procs.ToString();
        }

        public string GenerateChildMapping(ModuleContext mc, MapFilter mapFilter, XmlElement entityDef, XmlElement elem)
        {
            StringBuilder procs = new StringBuilder();
            int entityPartSeq = elem.GetAttribute("entityPartSeq").ToInt();
            string selfLc = entityDef.SelectNodeInnerText("./name").ToLower();

            entityPartSeq = elem.GetAttribute("entityPartSeq").ToInt();
            string child;
            if (elem.HasChildElements())
            {
                child = elem.SelectNodeInnerText("./name");
                string childLc = child.ToLower();
                string childUc = child.ToUpper();
                // map down before
                string childMapDownBeforeProc = selfLc + "_to_" + childLc + "_" + entityPartSeq + "_map_down_before";
                procs.AppendLine(
@"<proc name='" + childMapDownBeforeProc + @"'>
    <param name='parent_oid'/>
    <info switch='log_entity_map'>'map_down_before from '~TEMP.parent_oid~'/'~OBJECT(TEMP.parent_oid).object_type~' to '~OBJECT.object_id~'/'~OBJECT.object_type</info>

<!-- print>'operation==='~OBJECT.operation</print -->
");

                procs.Append(
                    mapFilter.GenerateMapping(
                        elem.GetChildElems().Where(e => !e.Name.Equals("name")),
                        EntityMapDomain.child,
                        EntityMapDirection.down,
                        EntityMapWhen.before,
                        EntityMapOperation.all,
                        mc
                    )
                );

                procs.AppendLine(@"
</proc>");
                // map down after
                string childMapDownAfterProc = selfLc + "_to_" + childLc + "_" + entityPartSeq + "_map_down_after";
                procs.AppendLine(
@"<proc name='" + childMapDownAfterProc + @"'>
    <param name='parent_oid'/>
    <info switch='log_entity_map'>'map_down_after from '~TEMP.parent_oid~'/'~OBJECT(TEMP.parent_oid).object_type~' to '~OBJECT.object_id~'/'~OBJECT.object_type</info>
");
                procs.Append(
                    mapFilter.GenerateMapping(
                        elem.GetChildElems().Where(e => !e.Name.Equals("name")),
                        EntityMapDomain.child,
                        EntityMapDirection.down,
                        EntityMapWhen.after,
                        EntityMapOperation.all,
                        mc
                    )
                );

                procs.AppendLine(@"
</proc>");
                // map up before
                string childMapUpBeforeProc = childLc + "_to_" + selfLc + "_" + entityPartSeq + "_map_up_before";
                procs.AppendLine(
@"<proc name='" + childMapUpBeforeProc + @"'>
    <param name='parent_oid'/>
    <do>TEMP.target_prefix=OBJECT(OBJECT(TEMP.parent_oid).static_oid).target_prefix</do>
    <info switch='log_entity_map'>'map_up_before from '~OBJECT.object_id~'/'~OBJECT.object_type~' to '~TEMP.parent_oid~'/'~OBJECT(TEMP.parent_oid).object_type~',target_prefix='~TEMP.target_prefix</info>
");
                procs.Append(
                   mapFilter.GenerateMapping(
                       elem.GetChildElems().Where(e => !e.Name.Equals("name")),
                       EntityMapDomain.child,
                       EntityMapDirection.up,
                       EntityMapWhen.before,
                       EntityMapOperation.all,
                       mc
                   )
               );

                procs.AppendLine(@"
</proc>");
                // GENERATE MAP UP AFTER
                string childMapUpAfterProc = childLc + "_to_" + selfLc + "_" + entityPartSeq + "_map_up_after";
                procs.AppendLine(
@"<proc name='" + childMapUpAfterProc + @"'>
    <param name='parent_oid'/>
    <do>TEMP.target_prefix=OBJECT(OBJECT(TEMP.parent_oid).static_oid).target_prefix</do>
    <info switch='log_entity_map'>'map_up_after from '~OBJECT.object_id~'/'~OBJECT.object_type~' to '~TEMP.parent_oid~'/'~OBJECT(TEMP.parent_oid).object_type~',target_prefix='~TEMP.target_prefix</info>

");
                procs.Append(
                   mapFilter.GenerateMapping(
                       elem.GetChildElems().Where(e => !e.Name.Equals("name")),
                       EntityMapDomain.child,
                       EntityMapDirection.up,
                       EntityMapWhen.after,
                       EntityMapOperation.all,
                       mc
                   )
               );

                procs.AppendLine(@"
</proc>");

            }
            return procs.ToString();
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("generate entities:");
        }



        /// <summary>
        /// Interprets syntax and returns config to do the syntax
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="mapElem"></param>
        /// <param name="map"></param>
        /// <param name="mapUp"></param>
        /// <param name="childMap"></param>
        /// <returns></returns>
        public static string GenerateMap(ModuleContext mc, XmlElement mapElem, string map, EntityMapDirection mapDirection, EntityMapDomain mapDomain)
        {
            string output = null;
            string prepareConfig;
            string accessSyntax;

            // Special case print
            if (mapElem.Name.StartsWith("print"))
            {
                StatementInterpreter interpreter = new StatementInterpreter(mc, map, mapDomain, mapDirection);
                interpreter.Interpret(out prepareConfig, out accessSyntax);
                output = output.AppendLine(prepareConfig);
                output = output.AppendLine("<print>" + accessSyntax + "</print>");
                return output;
            }

            // do map the new way...
            if (output == null)
            {
                StatementInterpreter interpreter = new StatementInterpreter(mc, map, mapDomain, mapDirection);
                interpreter.Interpret(out prepareConfig, out accessSyntax);
                output = output.AppendLine(prepareConfig);
                output = output.AppendLine(accessSyntax);
            }
            if (output != null)
            {
                output = "<info switch='log_entity_map'>" + "\"" + mapElem.Name + " " + mapDirection + ":" + mapElem.InnerText + "\"" + "</info>".AppendLine(output);
                return output;
            }
            throw new Exception("Error, cannot process [" + map + "]");
        }

        // LEAVE AROUND FOR REFERENCE SO WE CAN FINISH CODING GRID SOMEDAY
        public static string NOT_USED_GenMapConfigForGrid(XmlElement mapElem, string srcObject, string srcOid, string srcField, string tgtObject, string tgtOid, string tgtField)
        {
            srcField = srcField.ToLower();
            tgtField = tgtField.ToLower();
            StringBuilder output = new StringBuilder();
            output.AppendLine(@"
                                <call_proc_for_current_object>
                                <name>'map_to_grid'</name>
                                <param name='target_prefix'>TEMP.target_prefix</param>
                                <param name='target_oid'>" + tgtOid + @"</param>
                                <param name='target_field'>'" + tgtField + @"'</param>
                                <param name='source_oid'>" + srcOid + @"</param>
                                <param name='source_field'>'" + srcField + @"'</param>
                                </call_proc_for_current_object>
                       ");
            if (mapElem.HasAttributes)
            {
                output.AppendLine(@"
                                <call_proc_for_current_object>
                                <name>'add_metadata'</name>
                                <param name='static_oid'>" + tgtOid + @"</param>
                                <param name='field_name'>'" + tgtField + @"'</param>
                                <param name='datatype'>'" + mapElem.GetAttribute("datatype") + @"'</param>
                                <param name='namespace'>'" + mapElem.GetAttribute("namespace") + @"'</param>
                                <param name='length'>'" + mapElem.GetAttribute("length") + @"'</param>
                                <param name='rate_type'>'" + mapElem.GetAttribute("rate_type") + @"'</param>
                                <param name='unique_rate_row'>'" + mapElem.GetAttribute("unique_rate_row") + @"'</param>
                                </call_proc_for_current_object>
                       ");
            }
            else
            {
                output.AppendLine(@"
                                <call_proc_for_current_object>
                                <name>'copy_metadata'</name>
                                <param name='src_static_oid'>" + srcOid + @"</param>
                                <param name='src_field_name'>'" + srcField + @"'</param>
                                <param name='tgt_static_oid'>" + tgtOid + @"</param>
                                <param name='tgt_field_name'>'" + tgtField + @"'</param>
                                </call_proc_for_current_object>
                        ");
            }
            return output.ToString();
        }
    }


    /// <summary>
    /// This class is used for interpreting entity syntax. It is for 1 time use.
    /// </summary>
    public class StatementInterpreter
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static AstElement ParseSyntax(string stmt)
        {
            AstNode syn = AstNode.ParseStatement(stmt);
            //if (syn != null)Console.WriteLine((syn as ITree).ToStringTree());
            if (syn.IsNil)
                syn = (AstNode)syn.Children().First();
            AstElement elem = syn as AstElement;
            //Console.WriteLine(elem.OuterAst);
            return elem;
        }

        public ModuleContext mc;
        public string statement;
        public AstElement statementElem;
        public EntityMapDirection mapDirection;
        public EntityMapDomain mapType;
        public EntityMapDirection mapPersective = EntityMapDirection.down;
        public StatementInterpreter(ModuleContext mc, string statement, EntityMapDomain mapType, EntityMapDirection mapDirection)
        {
            this.mc = mc;
            this.statement = statement;
            this.mapType = mapType;
            this.mapDirection = mapDirection;
        }
        public string selfName
        {
            get
            {
                switch (mapType)
                {
                    case EntityMapDomain.child: return null;
                    case EntityMapDomain.self: return "ENTITY";
                    case EntityMapDomain.parent: return null;
                    default: return null;
                }
            }
        }
        public string parentName
        {
            get
            {
                switch (mapType)
                {
                    case EntityMapDomain.child: return "ENTITY";
                    case EntityMapDomain.self: return null;
                    case EntityMapDomain.parent: return "PARENT";
                    default: return null;
                }
            }
        }
        public string childName
        {
            get
            {
                switch (mapType)
                {
                    case EntityMapDomain.child: return "CHILD";
                    case EntityMapDomain.self: return null;
                    case EntityMapDomain.parent: return "ENTITY";
                    default: return null;
                }
            }
        }

        public void Interpret(out string prepare, out string access)
        {
            try
            {
                statementElem = ParseSyntax(this.statement);
            }
            catch (Exception e)
            {
                throw new Exception("Cannot parse entity syntax [" + this.statement + "]", e);
            }
            try
            {
                this.InterpretSyntax(statementElem, out prepare, out access);
            }
            catch (Exception e)
            {
                Console.WriteLine(statementElem.OuterAst);
                throw new Exception("Invalid entity syntax [" + this.statement + "]", e);
            }
        }

        public void GetEntityObjectSyntax(string entitySyntax, out string objectSyntax, out string objectIdSyntax)
        {
            if (mapType == EntityMapDomain.parent)
            {
                if (entitySyntax.Equals("PARENT"))
                {
                    objectSyntax = "OBJECT(TEMP.parent_oid)";
                    objectIdSyntax = "TEMP.parent_oid";
                    return;
                }
                if (entitySyntax.Equals("ENTITY"))
                {
                    objectSyntax = "OBJECT";
                    objectIdSyntax = "OBJECT.object_id";
                    return;
                }
                if (entitySyntax.Equals("CHILD")) throw new Exception("[CHILD] has no meaning in a parent block. You should use [ENTITY] to refer to the child.");
            }
            else if (mapType == EntityMapDomain.self)
            {
                if (entitySyntax.Equals("PARENT")) throw new Exception("[PARENT] has no meaning in a self block.");
                if (entitySyntax.Equals("ENTITY"))
                {
                    objectSyntax = "OBJECT";
                    objectIdSyntax = "OBJECT.object_id";
                    return;
                }
                if (entitySyntax.Equals("CHILD")) throw new Exception("[CHILD] has no meaning in a self block.");
            }
            else if (mapType == EntityMapDomain.child)
            {
                if (entitySyntax.Equals("PARENT")) throw new Exception("[PARENT] has no meaning in a child block. You should use [ENTITY] to refer to the child's parent.");
                if (entitySyntax.Equals("ENTITY"))
                {
                    objectSyntax = "OBJECT(TEMP.parent_oid)";
                    objectIdSyntax = "TEMP.parent_oid";
                    return;
                }
                if (entitySyntax.Equals("CHILD"))
                {
                    objectSyntax = "OBJECT";
                    objectIdSyntax = "OBJECT.object_id";
                    return;
                }
            }
            throw new Exception("unexpected");
        }

        public bool IsTwoWayFunction(AstElement elem)
        {
            string funcName = elem.Name;
            ProcInfo procInfo = GetProcInfo(funcName);
            if (procInfo == null) return false;
            var hasDirection = procInfo.paramElems.Where(p => p.GetAttribute("name").Equals("direction")).Any();
            var hasOperation = procInfo.paramElems.Where(p => p.GetAttribute("name").Equals("operation")).Any();
            if (hasDirection && hasOperation) return true;
            return false;
        }

        public ProcInfo GetProcInfo(string funcName)
        {
            if (SyntaxMaster.IsBinaryOperator(funcName)) return null;
            try
            {
                ProcInfo procInfo = mc.GetProcInfo(funcName);
                return procInfo;
            }
            catch (Exception e)
            {
                logger.Debug("GetProcInfo(" + funcName + ") returning NULL: " + e.Message);
                return null;
            }
        }
        public bool IsOneWayFunction(AstElement elem)
        {
            if (IsTwoWayFunction(elem)) return false;
            string funcName = elem.Name;
            ProcInfo procInfo = GetProcInfo(funcName);
            if (procInfo == null) return false;
            return true;
#if false
            // must have an out parameter called return_value
            var hasReturn = procInfo.paramElems
                .Where(p => p.GetAttribute("name").Equals("return_value") && p.GetAttribute("mode").Equals("out")).Any();
            return hasReturn;
#endif
        }

        public bool IsOneWayFunctionWithReturn(AstElement elem)
        {
            if (SyntaxMaster.IsBinaryOperator(elem.Name)) return false;
            string funcName = elem.Name;
            // look up the function
            ProcInfo procInfo = mc.GetProcInfo(funcName);
            // must have an out parameter called return_value
            var hasReturn = procInfo.paramElems
                .Where(p => p.GetAttribute("name").Equals("return_value") && p.GetAttribute("mode").Equals("out")).Any();
            return hasReturn;
        }
        public bool IsBinaryOperator(AstElement elem)
        {
            return SyntaxMaster.IsBinaryOperator(elem.Name);
        }

        public bool IsFunction(AstElement elem)
        {
            if (elem.HasParameters && !IsBinaryOperator(elem)) return true;
            return false;

        }
        public bool IsEntityField(AstElement elem)
        {
            AstElement entityElem = elem;
            if (entityElem.Name.EqualsIgnoreCase("as")) entityElem = elem.GetParameterSubElements().First();
            if (entityElem.Name.In(this.parentName, this.childName, this.selfName))
            {
                var fieldElem = entityElem.DotProperty;
                if (fieldElem != null)
                {
                    if (!fieldElem.HasChildren)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool IsParentField(AstElement elem)
        {
            if (!IsEntityField(elem)) return false;
            AstElement entityElem = elem;
            if (entityElem.Name.EqualsIgnoreCase("as")) entityElem = elem.GetParameterSubElements().First();
            return entityElem.Name.Equals(this.parentName);
        }
        public bool IsChildField(AstElement elem)
        {
            if (!IsEntityField(elem)) return false;
            AstElement entityElem = elem;
            if (entityElem.Name.EqualsIgnoreCase("as")) entityElem = elem.GetParameterSubElements().First();
            return entityElem.Name.Equals(this.childName);
        }

        public void GetEntityFieldSyntax(AstElement elem, out string objectSyntax, out string objectIdSyntax, out string fieldNameSyntax, out string datatype)
        {
            objectIdSyntax = null;
            fieldNameSyntax = null;
            objectSyntax = null;
            datatype = null;
            if (!this.IsEntityField(elem)) return;
            AstElement entityElem = elem;
            if (entityElem.Name.EqualsIgnoreCase("as"))
            {
                entityElem = elem.GetParameterSubElements().ElementAt(0);
                var datatypeElem = elem.GetParameterSubElements().ElementAt(1);
                datatype = datatypeElem.Name;
                while (datatypeElem.HasDotProperty)
                {
                    datatypeElem = datatypeElem.DotProperty;
                    datatype += "." + datatypeElem;
                }
            }
            this.GetEntityObjectSyntax(entityElem.Name, out objectSyntax, out objectIdSyntax);
            fieldNameSyntax = "'" + entityElem.DotProperty.Name + "'";
        }

        public bool IsEntityFieldMetadata(AstElement elem)
        {
            if (elem.Name.In("ENTITY", "PARENT", "CHILD"))
            {
                var fieldElem = elem.DotProperty;
                if (fieldElem != null)
                {
                    var metadataElem = fieldElem.DotProperty;
                    if (metadataElem != null)
                    {
                        if (!fieldElem.HasChildren)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void GetEntityFieldMetadataInfo(AstElement elem, out string objectSyntax, out string objectIdSyntax, out string fieldNameSyntax, out string metadataName)
        {
            objectIdSyntax = null;
            objectSyntax = null;
            fieldNameSyntax = null;
            metadataName = null;
            if (!this.IsEntityFieldMetadata(elem)) return;
            AstElement entityElem = elem;
            AstElement fieldElem = elem.DotProperty;
            AstElement metadataElem = fieldElem.DotProperty;
            this.GetEntityObjectSyntax(entityElem.Name, out objectSyntax, out objectIdSyntax);
            fieldNameSyntax = fieldElem.Name.q();
            metadataName = metadataElem.Name;
        }

        /// <summary>
        /// Interprets the syntax and returns code to prepare for evaluation and code to access the value.
        /// Prepare code is to call the proc where as the access value is TEMP.field syntax of the output 
        /// of the proc.
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="prepare"></param>
        /// <param name="access"></param>
        private void InterpretSyntax(AstElement elem, out string prepare, out string access)
        {
            prepare = "";
            access = "";

            // deal with assignment in map context
            if (elem.Name.Equals("="))
            {
                AstElement lvalueElem = elem.GetParameterSubElements().ElementAt(0);
                AstElement rvalueElem = elem.GetParameterSubElements().ElementAt(1);

                // Handle ANY.field.metaproperty=anything
                if (IsEntityFieldMetadata(lvalueElem))
                {
                    // get the metafield info
                    string tgtObjSyntax;
                    string tgtOidSyntax;
                    string tgtFldSyntax;
                    string tgtMetaFldName;
                    GetEntityFieldMetadataInfo(lvalueElem, out tgtObjSyntax, out tgtOidSyntax, out tgtFldSyntax, out tgtMetaFldName);
                    // process the rvalue
                    string rvaluePrepare;
                    string rvalueAccess;
                    InterpretSyntax(rvalueElem, out rvaluePrepare, out rvalueAccess);
                    // build the result
                    prepare = prepare.AppendLine(rvaluePrepare);
                    prepare = prepare.AppendLine(@"
                            <call_proc_for_current_object>
                              <name>'update_metadata'</name>
                              <param name='static_oid'>" + tgtOidSyntax + @"</param>
                              <param name='field_name'>TEMP.target_prefix~" + tgtFldSyntax + @"</param>
                              <param name='" + tgtMetaFldName + "'>" + rvalueAccess + @"</param>
                            </call_proc_for_current_object>
                        ");
                    return;
                }

                // Handle ANY.field.metadata=ANY.field.metadata (metadata only copy)
                // TBD

                // Handle ANY.field=ANY.field        
                if (IsEntityField(lvalueElem) && IsEntityField(rvalueElem))
                {
                    // Special case entity to entity maps to 'do the right thing'
                    // Change C=P to P=C on up
                    // Change P=C to C=P on down
                    // Change P.x=P.y to P.y=P.x when direction is not the perspective
                    // Change C.x=C.y to C.y=C.x when direction is not the perspective
                    if (this.mapDirection == EntityMapDirection.up && IsChildField(lvalueElem) && IsParentField(rvalueElem))
                        MyObject.SwapValues(ref lvalueElem, ref rvalueElem);
                    else if (this.mapDirection == EntityMapDirection.down && IsParentField(lvalueElem) && IsChildField(rvalueElem))
                        MyObject.SwapValues(ref lvalueElem, ref rvalueElem);
                    else if (this.mapDirection == EntityMapDirection.down && IsParentField(lvalueElem) && IsParentField(rvalueElem))
                        MyObject.SwapValues(ref lvalueElem, ref rvalueElem);
                    else if (this.mapDirection == EntityMapDirection.down && IsChildField(lvalueElem) && IsChildField(rvalueElem))
                        MyObject.SwapValues(ref lvalueElem, ref rvalueElem);

                    string tgtObjSyntax, tgtOidSyntax, tgtFldSyntax, tgtDatatype;
                    GetEntityFieldSyntax(lvalueElem, out tgtObjSyntax, out tgtOidSyntax, out tgtFldSyntax, out tgtDatatype);

                    string srcObjSyntax, srcOidSyntax, srcFldSyntax, srcDatatype;
                    GetEntityFieldSyntax(rvalueElem, out srcObjSyntax, out srcOidSyntax, out srcFldSyntax, out srcDatatype);
                    string tgtField = tgtFldSyntax.StripQuotes();
                    string srcField = srcFldSyntax.StripQuotes();
                    // copy the field 
                    prepare =
                        @"
                        <call_proc_for_current_object>
                        <name>'do_entity_map'</name>
                        <param name='target_prefix'>TEMP.target_prefix</param>
                        <param name='target_oid'>" + tgtOidSyntax + @"</param>
                        <param name='target_field'>" + tgtFldSyntax + @"</param>
                        <param name='source_oid'>" + srcOidSyntax + @"</param>
                        <param name='source_field'>" + srcFldSyntax + @"</param>
                        </call_proc_for_current_object>
";

                    // if direction is up and operation is metadata then need to error when copying null metadata
                    if (this.mapDirection == EntityMapDirection.up && tgtDatatype == null)
                    {
                        prepare = prepare.AppendLine(@"
                    <if>
                        <condition>OBJECT(" + srcOidSyntax + @").operation eq 'metadata'</condition>
                        <then>
                            <call_proc_for_current_object>
                            <name>'has_metadata'</name>
                            <param name='src_static_oid'>" + srcOidSyntax + @"</param>
                            <param name='src_field_name'>" + srcFldSyntax + @"</param>
                            <param name='result'>TEMP.has_metadata</param>
                            </call_proc_for_current_object>
                            <if>
                                <condition>TEMP.has_metadata</condition>
                                <then>
                                    <call_proc_for_current_object>
                                    <name>'copy_metadata'</name>
                                    <param name='src_static_oid'>" + srcOidSyntax + @"</param>
                                    <param name='src_field_name'>" + srcFldSyntax + @"</param>
                                    <param name='tgt_static_oid'>" + tgtOidSyntax + @"</param>
                                    <param name='tgt_field_name'>TEMP.target_prefix~" + tgtFldSyntax + @"</param>
                                    </call_proc_for_current_object>
                                </then>
                                <else>
                                    <do>TEMP.error_message='ERROR, cannot map_up src oid='~" + srcOidSyntax + @"~' field=" + srcField + @" to tgt oid='~" + tgtOidSyntax + @"~' field=" + tgtField + @" with no metadata.'</do>
                                    <print>TEMP.error_message</print>
                                    <call_proc_for_object>
                                    <name>'error_field'</name>
                                    <object_id>" + tgtOidSyntax + @"</object_id>
                                    <param name='field_name'>TEMP.target_prefix~" + tgtFldSyntax + @"</param>
                                    <param name='error'>TEMP.error_message</param>
                                    </call_proc_for_object>
                                </else>
                            </if>
                        </then>
                        <else>
                            <call_proc_for_current_object>
                            <name>'copy_metadata'</name>
                            <param name='src_static_oid'>" + srcOidSyntax + @"</param>
                            <param name='src_field_name'>" + srcFldSyntax + @"</param>
                            <param name='tgt_static_oid'>" + tgtOidSyntax + @"</param>
                            <param name='tgt_field_name'>TEMP.target_prefix~" + tgtFldSyntax + @"</param>
                            </call_proc_for_current_object>
                        </else>
                    </if>
                   ");
                    }
                    else
                    {
                        prepare = prepare.AppendLine(@"
                                <call_proc_for_current_object>
                                <name>'copy_metadata'</name>
                                <param name='src_static_oid'>" + srcOidSyntax + @"</param>
                                <param name='src_field_name'>" + srcFldSyntax + @"</param>
                                <param name='tgt_static_oid'>" + tgtOidSyntax + @"</param>
                                <param name='tgt_field_name'>TEMP.target_prefix~" + tgtFldSyntax + @"</param>
                                </call_proc_for_current_object>
                        ");
                    }
                    // If the user put a datatype with 'as datatype' use that.
                    if (tgtDatatype != null)
                    {
                        prepare = prepare.AppendLine(@"
                            <call_proc_for_current_object>
                              <name>'update_metadata'</name>
                              <param name='static_oid'>" + tgtOidSyntax + @"</param>
                              <param name='field_name'>TEMP.target_prefix~" + tgtFldSyntax + @"</param>
                              <param name='datatype'>'" + tgtDatatype + @"'</param>
                            </call_proc_for_current_object>
                        ");
                    }
                    return;
                }

                // Handle ANY.field=literal
                if (IsEntityField(lvalueElem) && rvalueElem.IsLiteral)
                {
                    // Skip unnecessary literal maps
                    // CHILD.field=literal is down only
                    // PARENT.field=literal is up only
                    if (lvalueElem.Name.Equals(this.childName) && rvalueElem.IsLiteral && this.mapDirection == EntityMapDirection.up)
                        return;
                    else if (lvalueElem.Name.Equals(this.parentName) && rvalueElem.IsLiteral && this.mapDirection == EntityMapDirection.down)
                        return;
                    string tgtObjSyntax, tgtOidSyntax, tgtFldSyntax, tgtDatatype;
                    GetEntityFieldSyntax(lvalueElem, out tgtObjSyntax, out tgtOidSyntax, out tgtFldSyntax, out tgtDatatype);
                    string literalValue = rvalueElem.LiteralValue;
                    string literalDatatype = literalValue.IsNumeric() ? "decimal" : "string";
                    prepare = @"
                                <do>" + tgtObjSyntax + "." + tgtFldSyntax.StripQuotes() + "=" + literalValue + @"</do>
                               ";
                    // If the user put a datatype with 'as datatype' use that.
                    if (tgtDatatype == null) tgtDatatype = literalDatatype;
                    if (tgtDatatype != null)
                    {
                        prepare = prepare.AppendLine(@"
                            <call_proc_for_current_object>
                              <name>'update_metadata'</name>
                              <param name='static_oid'>" + tgtOidSyntax + @"</param>
                              <param name='field_name'>TEMP.target_prefix~" + tgtFldSyntax + @"</param>
                              <param name='datatype'>'" + tgtDatatype + @"'</param>
                            </call_proc_for_current_object>
                        ");
                    }
                    return;
                }

                // Handle ANY.field=TwoWayFunc(ANY.x,ANY.y) 
                if (IsEntityField(lvalueElem) && IsTwoWayFunction(rvalueElem))
                {
                    string direction = this.mapDirection == EntityMapDirection.up ? "'up'" : "'down'";

                    var funcName = rvalueElem.Name;
                    var rightTreeArgs = rvalueElem.GetParameterSubElements().ToList();
                    List<string> rightArgValues;
                    List<string> rightArgOids;
                    List<string> rightArgNames;
                    GetTwoWayFunctionArgs(rightTreeArgs, out rightArgOids, out rightArgNames, out rightArgValues);

                    var leftTreeArgs = lvalueElem.AsList();
                    List<string> leftArgValues;
                    List<string> leftArgOids;
                    List<string> leftArgNames;
                    GetTwoWayFunctionArgs(leftTreeArgs, out leftArgOids, out leftArgNames, out leftArgValues);

                    // look up the function
                    ProcInfo procInfo = mc.GetProcInfo(funcName);
                    // tbd: validate the proc is conforming...
                    var lParams = procInfo.paramElems.TakeWhile(x => !x.GetAttribute("name").Equals("direction")).ToList();
                    var rParams = procInfo.paramElems.Skip(lParams.Count + 2).ToList();

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("");
                    sb.AppendLine("<call_proc_for_current_object>");
                    sb.AppendLine("<name>'" + funcName + "'</name>");
                    int leftIdx = -1;
                    for (int i = 0; i < lParams.Count; i++)
                    {
                        leftIdx++;
                        var v_paramElement = lParams[i];
                        string v_paramName = v_paramElement.GetAttribute("name");
                        string v_paramValue = leftArgValues[leftIdx];
                        sb.AppendLine("<param name='" + v_paramName + "'>" + v_paramValue + "</param>");

                        var o_paramElement = lParams[++i];
                        string o_paramName = o_paramElement.GetAttribute("name");
                        string o_paramValue = leftArgOids[leftIdx];
                        sb.AppendLine("<param name='" + o_paramName + "'>" + o_paramValue + "</param>");

                        var n_paramElement = lParams[++i];
                        string n_paramName = n_paramElement.GetAttribute("name");
                        string n_paramValue = leftArgNames[leftIdx];
                        sb.AppendLine("<param name='" + n_paramName + "'>" + n_paramValue + "</param>");
                    }
                    sb.AppendLine("<param name='" + "direction" + "'>" + direction + "</param>");
                    sb.AppendLine("<param name='" + "operation" + "'>" + "OBJECT.operation" + "</param>");
                    int rightIdx = -1;
                    for (int i = 0; i < rParams.Count; i++)
                    {
                        rightIdx++;
                        var v_paramElement = rParams[i];
                        string v_paramName = v_paramElement.GetAttribute("name");
                        string v_paramValue = rightArgValues[rightIdx];
                        sb.AppendLine("<param name='" + v_paramName + "'>" + v_paramValue + "</param>");

                        var o_paramElement = rParams[++i];
                        string o_paramName = o_paramElement.GetAttribute("name");
                        string o_paramValue = rightArgOids[rightIdx];
                        sb.AppendLine("<param name='" + o_paramName + "'>" + o_paramValue + "</param>");

                        var n_paramElement = rParams[++i];
                        string n_paramName = n_paramElement.GetAttribute("name");
                        string n_paramValue = rightArgNames[rightIdx];
                        sb.AppendLine("<param name='" + n_paramName + "'>" + n_paramValue + "</param>");
                    }
                    sb.AppendLine("</call_proc_for_current_object>");
                    prepare = sb.ToString();
                    return;
                }


                // ANY.field=operator
                // ANY.field=OneWayFunction()
                if (IsEntityField(lvalueElem) && (SyntaxMaster.IsBinaryOperator(rvalueElem.Name) || IsOneWayFunctionWithReturn(rvalueElem)))
                {
                    string rvaluePrepare;
                    string rvalueAccess;
                    InterpretSyntax(rvalueElem, out rvaluePrepare, out rvalueAccess);
                    prepare += rvaluePrepare;

                    string tgtObjSyntax, tgtOidSyntax, tgtFldSyntax, tgtDatatype;
                    GetEntityFieldSyntax(lvalueElem, out tgtObjSyntax, out tgtOidSyntax, out tgtFldSyntax, out tgtDatatype);
                    prepare = prepare.AppendLine("<do>" + tgtObjSyntax + "." + tgtFldSyntax.StripQuotes() + "=" + rvalueAccess + @"</do>");

                    // Copy metadata from the best field
                    var metadataFieldElem = GetMetaDataEntityField(rvalueElem);
                    if (metadataFieldElem != null)
                    {
                        string srcObjSyntax, srcOidSyntax, srcFldSyntax, srcDatatype;
                        GetEntityFieldSyntax(metadataFieldElem, out srcObjSyntax, out srcOidSyntax, out srcFldSyntax, out srcDatatype);
                        prepare = prepare.AppendLine(@"
                                <call_proc_for_current_object>
                                <name>'copy_metadata'</name>
                                <param name='src_static_oid'>" + srcOidSyntax + @"</param>
                                <param name='src_field_name'>" + srcFldSyntax + @"</param>
                                <param name='tgt_static_oid'>" + tgtOidSyntax + @"</param>
                                <param name='tgt_field_name'>TEMP.target_prefix~" + tgtFldSyntax + @"</param>
                                </call_proc_for_current_object>
                        ");
                    }

                    // Override the datatype if user specified a 'as datatype'.
                    if (tgtDatatype != null)
                    {
                        prepare = prepare.AppendLine(@"
                            <call_proc_for_current_object>
                              <name>'update_metadata'</name>
                              <param name='static_oid'>" + tgtOidSyntax + @"</param>
                              <param name='field_name'>TEMP.target_prefix~" + tgtFldSyntax + @"</param>
                              <param name='datatype'>'" + tgtDatatype + @"'</param>
                            </call_proc_for_current_object>
                        ");
                    }
                    return;
                }



                throw new Exception("unexpected syntax:" + this.statement);
            }

            // Binary operator
            if (SyntaxMaster.IsBinaryOperator(elem.Name))
            {
                AstElement lvalueElem = elem.GetParameterSubElements().ElementAt(0);
                string lvaluePrepare;
                string lvalueAccess;
                InterpretSyntax(lvalueElem, out lvaluePrepare, out lvalueAccess);

                AstElement rvalueElem = elem.GetParameterSubElements().ElementAt(1);
                string rvaluePrepare;
                string rvalueAccess;
                InterpretSyntax(rvalueElem, out rvaluePrepare, out rvalueAccess);

                prepare = prepare.AppendLine(lvaluePrepare);
                prepare = prepare.AppendLine(rvaluePrepare);
                access = "(" + lvalueAccess + " " + elem.Name + " " + rvalueAccess + ")";
                return;
            }

            // Literals
            if (elem.IsLiteral)
            {
                access = elem.LiteralValue;
                return;
            }

            // ENTITY expression
            if (elem.Name.In("ENTITY", "PARENT", "CHILD"))
            {
                string objectSyntax;
                string objectIdSyntax;
                GetEntityObjectSyntax(elem.Name, out objectSyntax, out objectIdSyntax);
                // ENTITY
                if (!elem.HasDot)
                {
                    access = objectIdSyntax;
                    return;
                }
                // ENTITY.field or ENTITY.field.function() or ENTITY.field.metadata
                if (elem.HasDotProperty)
                {
                    AstElement fieldElem = elem.DotProperty;
                    string fieldName = elem.DotProperty.Name;
                    // ENTITY.field
                    if (!fieldElem.HasDot)
                    {
                        access = objectSyntax + "." + fieldName;
                        return;
                    }
                    // ENTITY.field.metadata 
                    if (fieldElem.HasDotProperty)
                    {
                        string metadata = fieldElem.DotProperty.Name;
                        if (metadata.Equals("field_name"))
                        {
                            access = fieldName.q();
                            return;
                        }
                        else
                        {
                            
                            string metadataTemp = "TEMP." + mc.GetGenSym(metadata);
                            prepare = @"<call_proc_for_object>
                                    <name>'get_metadata'</name>
                                    <object_id>" + objectIdSyntax + @"</object_id>
                                    <param name='static_oid'>" + objectIdSyntax + @"</param>
                                    <param name='field_name'>'" + fieldName + @"'</param>
                                    <param name='" + metadata + @"'>" + metadataTemp + @"</param>
                                    </call_proc_for_object>";
                            access = metadataTemp;
                            return;
                        }
                    }
                    // ENTITY.field.function()
                    if (fieldElem.HasDotFunction)
                    {
                        string fieldFunction = fieldElem.DotFunction.Name;
                        if (fieldFunction.Equals("details"))
                        {
                            string accessTemp = "TEMP." + mc.GetGenSym("details");
                            prepare = @"<call_proc_for_object>
                                    <name>'dump_field_to_string'</name>
                                    <object_id>" + objectIdSyntax + @"</object_id>
                                    <param name='field_name'>'" + fieldName + @"'</param>
                                    <param name='text'>" + accessTemp + @"</param>
                                    </call_proc_for_object>";
                            access = accessTemp;
                            return;
                        }
                        throw new Exception("[" + fieldFunction + "] is not a valid entity field function.");
                    }
                    // TBD
                    throw new Exception("unexpected:" + fieldElem.OuterAst);
                }

                // ENTITY.function()
                if (elem.HasDotFunction)
                {
                    string entityFunction = elem.DotFunction.Name;
                    if (entityFunction.Equals("details"))
                    {
                        string accessTemp = "TEMP." + mc.GetGenSym("details");
                        prepare = @"<call_proc_for_object>
                                    <name>'dump_entity_to_string'</name>
                                    <object_id>" + objectIdSyntax + @"</object_id>
                                    <param name='text'>" + accessTemp + @"</param>
                                    </call_proc_for_object>";
                        access = accessTemp;
                        return;
                    }
                    throw new Exception("[" + entityFunction + "] is not a valid entity function.");
                }
                throw new Exception("unexpected:" + elem.OuterAst);
            }

            // Handle the tagging function called metadata that is actually a marker that
            // means use the metadata from this entity field and is not really a function
            if (elem.HasParameters && elem.Name.Equals("metadata"))
            {
                var passedArgs = elem.GetParameterSubElements().ToList();
                if (passedArgs.Count != 1)
                    throw new Exception("Expecting metadata to have a single entity field argument not [" + passedArgs.Count + "]");
                var innerElem = passedArgs.First();
                InterpretSyntax(innerElem, out prepare, out access);
                return;
            }

            // See if the thing looks like a function call
            if (elem.HasParameters)
            {
                string funcName = elem.Name;
                ProcInfo procInfo = GetProcInfo(funcName);
                if (procInfo == null)
                    throw new Exception("Error [" + funcName + "] is not a valid function name");
                var passedArgs = elem.GetParameterSubElements().ToList();
                int expectedArgCount = procInfo.HasReturnValue ? procInfo.paramElems.Count - 1 : procInfo.paramElems.Count;
                if (passedArgs.Count != expectedArgCount) throw new Exception("Error calling function " + funcName + " expected " + expectedArgCount + " parameters but passed " + passedArgs.Count);
                // Assign a temp var for the return value...
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("");
                sb.AppendLine("<call_proc_for_current_object>");
                sb.AppendLine("<name>'" + funcName + "'</name>");
                int idx = 0;
                foreach (var paramElem in procInfo.paramElems)
                {
                    string paramName = paramElem.GetAttribute("name");
                    string paramValue;
                    if (paramName.Equals("return_value"))
                    {
                        paramValue = "TEMP." + mc.GetGenSym("return_value");
                        access = paramValue;
                    }
                    else
                    {
                        string argAccess, argPrepare;
                        var argElem = passedArgs[idx++];
                        InterpretSyntax(argElem, out argPrepare, out argAccess);
                        prepare = prepare.AppendLine(argPrepare);
                        if (argAccess.Equals("")) throw new Exception("Error:" + argElem.ToCode() + " does not return a value");
                        paramValue = argAccess;
                    }
                    sb.AppendLine("<param name='" + paramName + "'>" + paramValue + "</param>");
                }
                sb.AppendLine("</call_proc_for_current_object>");
                prepare = sb.ToString();
                return;
            }
            throw new Exception("not expected: [" + elem.OuterAst + "]");
        }

        // given an element which can represent an expression, find the left more entity field.
        public AstElement GetLeftMostEntityField(AstElement elem)
        {
            foreach (var node in elem.WalkAllElements())
            {
                if (IsEntityField(node)) return node;
            }
            return null;
        }

        public AstElement GetMetadataTaggedEntityField(AstElement elem)
        {
            foreach (var node in elem.WalkAllElements())
            {
                if (IsFunction(node) && node.Name.Equals("metadata"))
                {
                    return node.GetParameterSubElements().FirstOrDefault();
                }
            }
            return null;
        }

        public AstElement GetMetaDataEntityField(AstElement elem)
        {
            var output = GetMetadataTaggedEntityField(elem);
            if (output == null)
            {
                output = GetLeftMostEntityField(elem);
            }
            return output;
        }

        public List<string> GetTwoWayFunctionArgs(List<AstElement> args, out List<string> argOids, out  List<string> argNames, out  List<string> argValues)
        {
            argNames = new List<string>();
            argOids = new List<string>();
            argValues = new List<string>();
            foreach (var argElem in args)
            {
                if (argElem.IsLiteral)
                {
                    argNames.Add("''");
                    argValues.Add(argElem.LiteralValue);
                    argOids.Add("''");
                }
                else if (argElem.Name.Equals(this.parentName))
                {
                    string entityField = argElem.DotProperty.Name;
                    argNames.Add(entityField.q());
                    argValues.Add("OBJECT(TEMP.parent_oid)." + entityField);
                    argOids.Add("TEMP.parent_oid");
                }
                else if (argElem.Name.Equals(this.childName))
                {
                    string entityField = argElem.DotProperty.Name;
                    argNames.Add(entityField.q());
                    argValues.Add("OBJECT." + entityField);
                    argOids.Add("OBJECT.object_id");
                }
                else
                {
                    throw new Exception("unexpected [" + argElem.ToStringTree() + "]");
                }
            }
            return argValues;
        }
    }

    // Iterface to become and operation
    public interface IGenOperationProc
    {
        string GenerateOperationProc(ModuleContext mc, XmlElement entityDef, XmlElement opElem);
    }

    /// <summary>
    /// Implements print operation
    /// </summary>
    public class GenPrintProc : IGenOperationProc
    {
        public string GenerateOperationProc(ModuleContext mc, XmlElement entityDef, XmlElement opElem)
        {
            string output = "";
            EntityMapDomain mapDomain = EntityMapDomain.self;
            EntityMapDirection mapDirection = EntityMapDirection.all;

            // Generate the erroring config
            string printSyntax = opElem.InnerText;
            StatementInterpreter conditionInterpreter = new StatementInterpreter(mc, printSyntax, mapDomain, mapDirection);
            string printPrepare, printAccess;
            conditionInterpreter.Interpret(out printPrepare, out printAccess);
            output = output.AppendLine(printPrepare);
            output = output.AppendLine(@"<print>" + printAccess + @"</print>");

            // wrap the config up in a proc call
            string opProcName = MGenerateEntities.GetOperationProcName(opElem);
            output =
            @"<proc name='" + opProcName + @"'>
                " + output + @"
            </proc>";
            return output;
        }
    }



    /// <summary>
    /// Implements validate operation
    /// </summary>
    public class GenValidateProc : IGenOperationProc
    {
        public class InputMacro
        {
            public string find;
            public string replace;
            public InputMacro(XmlElement inputElem)
            {
                this.find = inputElem.GetAttribute("name");
                this.replace = inputElem.InnerText;
            }
        }
        public class InputGroup
        {
            List<InputMacro> macros = new List<InputMacro>();
            public InputGroup(List<XmlElement> inputFieldElems)
            {
                foreach (var inputFieldElem in inputFieldElems)
                    this.macros.Add(new InputMacro(inputFieldElem));
            }
            public XmlElement Apply(XmlElement inputElem)
            {
                XmlElement outputElem = (XmlElement)inputElem.CloneNode(true);
                // walk the xml and do replaces.
                foreach (XmlText xmlText in outputElem.SelectNodes("//text()"))
                {
                    string text = xmlText.InnerText;
                    foreach (InputMacro inputMacro in this.macros)
                    {
                        text = text.Replace(inputMacro.find, inputMacro.replace);
                    }
                    xmlText.InnerText = text;
                }
                return outputElem;
            }
        }
        public string GenerateOperationProc(ModuleContext mc, XmlElement entityDef, XmlElement opElem)
        {
            // tbd validate and normalize the input field and input group naming.

            // get the input groups setup. Treat top level input fields as input groups of 1
            List<InputGroup> inputGroups = new List<InputGroup>();
            foreach (XmlElement inputGroupElem in opElem.SelectNodes("./input_group|./input_field"))
            {
                if (inputGroupElem.Name.Equals("input_group"))
                    inputGroups.Add(new InputGroup(inputGroupElem.GetChildElems()));
                else
                    inputGroups.Add(new InputGroup(inputGroupElem.AsList()));
            }

            // generate the config
            StringBuilder sb = new StringBuilder();
            if (inputGroups.Count == 0)
                sb.AppendLine(GenerateOperationCfg(mc, entityDef, opElem));
            else
                foreach (var inputGroup in inputGroups)
                {
                    XmlElement macroOpElem = inputGroup.Apply(opElem);
                    sb.AppendLine(GenerateOperationCfg(mc, entityDef, macroOpElem));
                }

            // wrap the config up in a proc call
            string procName = MGenerateEntities.GetOperationProcName(opElem);
            return
            @"<proc name='" + procName + @"'>
                " + sb.ToString() + @"
            </proc>";
        }

        private string GenerateOperationCfg(ModuleContext mc, XmlElement entityDef, XmlElement opElem)
        {
            string output = "";
            EntityMapDomain mapDomain = EntityMapDomain.self;
            EntityMapDirection mapDirection = EntityMapDirection.all;

            // Get the config to set the error field if one was specified 
            string setErrorFieldCfg = "";
            {
                string errorField = opElem.SelectNodeInnerText("./error_field", "");
                if (!errorField.Equals(""))
                {
                    StatementInterpreter errorFieldInterpreter = new StatementInterpreter(mc, errorField, mapDomain, mapDirection);
                    string errorFieldPrepare, errorFieldAccess;
                    errorFieldInterpreter.Interpret(out errorFieldPrepare, out errorFieldAccess);
                    if (!errorFieldInterpreter.IsEntityField(errorFieldInterpreter.statementElem)) throw new Exception("Error, error_field [" + errorField + "] must be a valid ENTITY.field syntax. ast=" + errorFieldInterpreter.statementElem.OuterAst);
                    string objectSyntax, objectIdSyntax, fieldNameSyntax, datatypeSyntax;
                    errorFieldInterpreter.GetEntityFieldSyntax(errorFieldInterpreter.statementElem, out objectSyntax, out objectIdSyntax, out fieldNameSyntax, out datatypeSyntax);
                    setErrorFieldCfg = @"
                            <call_proc_for_current_object>
                            <name>'set_error_metadata'</name>
                            <param name='static_oid'>" + objectIdSyntax + @"</param>
                            <param name='field_name'>" + fieldNameSyntax + @"</param>
                            <param name='error'>TEMP.entity_error</param>
                            </call_proc_for_current_object>
                        ";
                }
            }

            // Get the severity, default is 'error'
            string severitySyntax = "'[error]'~";
            if (opElem.GetAttribute("severity").Equals("warn"))
                severitySyntax = "'[warn]'~";

            // Get the config to get error message
            string messagePrepare, messageAccess;
            {
                string message = opElem.SelectNodeInnerText("./error_message");
                StatementInterpreter messageInterpreter = new StatementInterpreter(mc, message, mapDomain, mapDirection);
                messageInterpreter.Interpret(out messagePrepare, out messageAccess);
                messageAccess = severitySyntax + messageAccess; // prefix severity
            }

            // Generate the erroring config
            string condition = opElem.SelectNodeInnerText("./error_condition");
            StatementInterpreter conditionInterpreter = new StatementInterpreter(mc, condition, mapDomain, mapDirection);
            string conditionPrepare, conditionAccess;
            conditionInterpreter.Interpret(out conditionPrepare, out conditionAccess);
            output = output.AppendLine(conditionPrepare);
            output = output.AppendLine(@"
                     <if>
                     <condition>OBJECT.operation eq 'put'</condition>
                     <then>
                        <if>
                        <condition>" + conditionAccess + @"</condition>
                        <then>
                            <!-- print>'===================== Validation FAILED'</print -->
                            " + messagePrepare + @"
                            <do>TEMP.entity_error=" + messageAccess + @"</do>
                            <do>OBJECT.entity_error = OBJECT.entity_error ne '' ? OBJECT.entity_error: TEMP.entity_error</do>
                            <print>'entity_error=['~OBJECT.entity_error~'] for oid='~OBJECT.object_id</print>
                            " + setErrorFieldCfg + @"
                        </then>
                        <else>
                            <!-- print>'=================== Validation PASSED'</print -->
                        </else>
                        </if>
                    </then>
                    </if>");
            return output;
        }

    }
}
