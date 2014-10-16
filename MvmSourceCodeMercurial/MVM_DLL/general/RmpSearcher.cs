using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

namespace MVM
{
    /// <summary>
    /// For browsing RMP. All properties are lazy instanciated and remember their results.
    /// Class is not threadContext safe.
    /// </summary>
    public class RmpSearcher
    {

        public class MsixPtype
        {
            public string paramtable_name;
            public string column_name;
            public string display_name;
            public string is_condition;
            public string column_operator;
            public string is_operator;
            public string has_operator_name;
            public string is_action;
            public string column_type;
            public string enum_namespace;
            public string enum_type;
            public string column_length;
            public string is_required;
            public string default_value;
        }

        public static void Test()
        {
            RmpSearcher rmp = new RmpSearcher(@"D:\MetraTech\RMP");

            Console.WriteLine("PtServiceDefFiles:");
            rmp.PtServiceDefFiles.WriteLines();

            foreach (var doc in rmp.PtServiceDefXmlDocs)
            {
               
                Console.WriteLine(doc.OuterXml);
            }

            Console.WriteLine("PriceableItemFiles:");
            rmp.PriceableItemFiles.WriteLines();

            Console.WriteLine("AccountTypeFiles:");
            rmp.AccountTypeFiles.WriteLines();
            Console.WriteLine("AccountTypes:");
            rmp.AccountTypes.WriteLines();

            Console.WriteLine("AvServiceDefFiles:");
            rmp.AvServiceDefFiles.WriteLines();
            Console.WriteLine("AvServiceDefs:");
            rmp.AvServiceDefs.WriteLines();

            Console.WriteLine("AccountTypeViewsMap:");
            foreach (var accountType in rmp.AccountTypeViewsMap.Keys)
            {
                Console.WriteLine("accountType=" + accountType);
                foreach (var entry in rmp.AccountTypeViewsMap[accountType])
                {
                    ServiceDef sd = rmp.GetAvServiceDefObj(entry.serviceDef);
                    FieldDef fd = sd.GetEnumPartOfKeyFieldDef();
                    Console.WriteLine("..viewname=" + entry.viewName + ", servicedef=" + entry.serviceDef+", poc="+(fd!=null? fd.name:"NOPE"));

                }
            }
        }

        /// <summary>
        /// The RMP directory
        /// </summary>
        public string rmpDir;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rmpDir"></param>
        public RmpSearcher(string rmpDir)
        {
            this.rmpDir = rmpDir;
        }


        #region Extended properties

        // D:\MetraTech\RMP\Extensions\SystemConfig\config\ExtendedProp\*
        private List<string> extendedPropertyFiles;
        public List<string> ExtendedPropertyFiles
        {
            get
            {
                if (this.extendedPropertyFiles == null)
                    this.extendedPropertyFiles = FileUtils2.GlobToList(this.rmpDir + @"\Extensions\SystemConfig\config\ExtendedProp\*.msixdef");
                return this.extendedPropertyFiles;
            }
        }
        private Dictionary<string, ServiceDef> epTableNameServiceDefMap;
        public Dictionary<string, ServiceDef> EpTableNameServiceDefMap
        {
            get
            {
                if (epTableNameServiceDefMap == null)
                {
                    epTableNameServiceDefMap = new Dictionary<string, ServiceDef>();
                    foreach (var xmlDoc in this.EpServiceDefXmlDocs)
                    {
                        string serviceDefName = xmlDoc.DocumentElement.SelectNodeInnerText("//defineservice/name");
                        string shortName = serviceDefName.Substring(serviceDefName.LastIndexOf('/') + 1);
                        string tableName = "t_ep_" + shortName.ToLower();
                        epTableNameServiceDefMap[tableName] = new ServiceDef(xmlDoc.DocumentElement);
                    }
                }
                return epTableNameServiceDefMap;
            }
        }

        public ServiceDef GetEpServiceDefByTableName(string tableName)
        {
            if (!epTableNameServiceDefMap.ContainsKey(tableName))
            {
                throw new Exception("Cannot locate service definition for extended property table [" + tableName + "]");
            }
            return EpTableNameServiceDefMap[tableName];
        }

        private List<XmlDocument> epServiceDefXmlDocs;
        public List<XmlDocument> EpServiceDefXmlDocs
        {
            get
            {
                if (this.epServiceDefXmlDocs == null)
                {
                    epServiceDefXmlDocs = new List<XmlDocument>();
                    foreach (string f in this.ExtendedPropertyFiles)
                    {
                        XmlDocument doc = MyXml.ParseXmlFile(f);
                        epServiceDefXmlDocs.Add(doc);
                    }
                }
                return epServiceDefXmlDocs;
            }
        }
       




        #endregion



        #region Priceable items

        private List<string> priceableItemFiles;
        public List<string> PriceableItemFiles
        {
            get
            {
                if (this.priceableItemFiles == null) 
                    this.priceableItemFiles = FileUtils2.GlobToList(this.rmpDir + @"\Extensions\*\config\PriceableItems\*.xml");
                return this.priceableItemFiles;
            }
        }

    
       
#endregion


        # region param tables
        private List<string> ptServiceDefFiles;
        public List<string> PtServiceDefFiles
        {
            get
            {
                if (this.ptServiceDefFiles == null)
                    this.ptServiceDefFiles = FileUtils2.GlobToList(this.rmpDir + @"\Extensions\*\config\ParamTable\*\*.msixdef");
                return this.ptServiceDefFiles;
            }
        }

        private Dictionary<string, ServiceDef> ptTableNameServiceDefMap;
        public Dictionary<string, ServiceDef> PtTableNameServiceDefMap
        {
            get
            {
                if (ptTableNameServiceDefMap == null)
                {
                    ptTableNameServiceDefMap = new Dictionary<string, ServiceDef>();
                    foreach (var xmlDoc in this.PtServiceDefXmlDocs)
                    {
                        string serviceDefName = xmlDoc.DocumentElement.SelectNodeInnerText("//defineservice/name");
                        string shortName = serviceDefName.Substring(serviceDefName.LastIndexOf('/')+1);
                        string tableName = "t_pt_" + shortName.ToLower();
                        ptTableNameServiceDefMap[tableName] = new ServiceDef(xmlDoc.DocumentElement);
                    }
                }
                return ptTableNameServiceDefMap;
            }
        }
        public bool HasPtServiceDefByTableName(string tableName)
        {
            if (!PtTableNameServiceDefMap.ContainsKey(tableName))
            {
                return false;
            }
            return true;
        }
        public ServiceDef GetPtServiceDefByTableName(string tableName)
        {
            if (!PtTableNameServiceDefMap.ContainsKey(tableName))
            {
                throw new Exception("Cannot locate service definition for parameter table ["+tableName+"]");
            }
            return PtTableNameServiceDefMap[tableName];
        }

        private List<XmlDocument> ptServiceDefXmlDocs;
        public List<XmlDocument> PtServiceDefXmlDocs
        {
            get
            {
                if (this.ptServiceDefXmlDocs == null)
                {
                    ptServiceDefXmlDocs = new List<XmlDocument>();
                    foreach (string f in this.PtServiceDefFiles)
                    {
                        XmlDocument doc = MyXml.ParseXmlFile(f);
                        ptServiceDefXmlDocs.Add(doc);
                    }
                }
                return ptServiceDefXmlDocs;
            }
        }

       



        #endregion


        #region svc Tables

        private List<string> svcServiceDefFiles;
        public List<string> SvcServiceDefFiles
        {
            get
            {
                if (this.svcServiceDefFiles == null)
                    this.svcServiceDefFiles = FileUtils2.GlobToList(this.rmpDir + @"\Extensions\*\config\Service\*\*.msixdef");
                return this.svcServiceDefFiles;
            }
        }

        private Dictionary<string, ServiceDef> svcTableNameServiceDefMap;
        public Dictionary<string, ServiceDef> SvcTableNameServiceDefMap
        {
            get
            {
                if (svcTableNameServiceDefMap == null)
                {
                    svcTableNameServiceDefMap = new Dictionary<string, ServiceDef>();
                    foreach (var xmlDoc in this.SvcServiceDefXmlDocs)
                    {
                        string serviceDefName = xmlDoc.DocumentElement.SelectNodeInnerText("//defineservice/name");
                        string shortName = serviceDefName.Substring(serviceDefName.LastIndexOf('/') + 1);
                        string tableName = "t_svc_" + shortName.ToLower();
                        svcTableNameServiceDefMap[tableName] = new ServiceDef(xmlDoc.DocumentElement);
                    }
                }
                return svcTableNameServiceDefMap;
            }
        }
        public bool HasSvcServiceDefByTableName(string tableName)
        {
            if (!SvcTableNameServiceDefMap.ContainsKey(tableName))
            {
                return false;
            }
            return true;
        }
        public ServiceDef GetSvcServiceDefByTableName(string tableName)
        {
            if (!SvcTableNameServiceDefMap.ContainsKey(tableName))
            {
                throw new Exception("Cannot locate service definition for parameter table [" + tableName + "]");
            }
            return SvcTableNameServiceDefMap[tableName];
        }

        private List<XmlDocument> svcServiceDefXmlDocs;
        public List<XmlDocument> SvcServiceDefXmlDocs
        {
            get
            {
                if (this.svcServiceDefXmlDocs == null)
                {
                    svcServiceDefXmlDocs = new List<XmlDocument>();
                    foreach (string f in this.SvcServiceDefFiles)
                    {
                        XmlDocument doc = MyXml.ParseXmlFile(f);
                        svcServiceDefXmlDocs.Add(doc);
                    }
                }
                return svcServiceDefXmlDocs;
            }
        }

        #endregion



        # region Account Types

        private List<string> accountTypeFiles;
        public List<string> AccountTypeFiles
        {
            get
            {
                if (this.accountTypeFiles == null) 
                    this.accountTypeFiles = FileUtils2.GlobToList(this.rmpDir + @"\Extensions\Account\config\AccountType\*.xml");
                return this.accountTypeFiles;
            }
        }

        private List<XmlDocument> accountTypeXmlDocs;
        public List<XmlDocument> AccountTypeXmlDocs
        {
            get
            {
                if (this.accountTypeXmlDocs == null)
                {
                    accountTypeXmlDocs = new List<XmlDocument>();
                    foreach (string f in this.AccountTypeFiles)
                    {
                        XmlDocument doc = MyXml.ParseXmlFile(f);
                        accountTypeXmlDocs.Add(doc);
                    }
                }
                return accountTypeXmlDocs;
            }
        }

        private List<string> accountTypes;
        public List<string> AccountTypes
        {
            get
            {
                if (this.accountTypes == null)
                {
                    this.accountTypes = new List<string>();
                    foreach (var xDoc in this.AccountTypeXmlDocs)
                    {
                        foreach (var accountType in xDoc.SelectNodesInnerText("//AccountType/Name"))
                        {
                            this.accountTypes.Add(accountType);
                        }
                    }
                }
                return this.accountTypes;
            }
        }

        private Dictionary<string, XmlDocument> accountTypeXmlDocMap;
        public Dictionary<string, XmlDocument> AccountTypeXmlDocMap
        {
            get
            {
                if (this.accountTypeXmlDocMap == null)
                {
                    accountTypeXmlDocMap = new Dictionary<string, XmlDocument>();
                    foreach (var xDoc in this.accountTypeXmlDocs)
                    {
                        foreach (var at in xDoc.SelectNodesInnerText("//AccountType/Name"))
                        {
                            this.accountTypeXmlDocMap[at] = xDoc;
                        }
                    }
                }
                return this.accountTypeXmlDocMap;
            }
        }

        public XmlDocument GetAccountTypeXmlDoc(string accountType)
        {
            return this.AccountTypeXmlDocMap[accountType];
        }

        

        private Dictionary<string, List<AccountTypeViewEntry>> accountTypeViewsMap;
        private Dictionary<string, List<AccountTypeViewEntry>> AccountTypeViewsMap
        {
            get
            {
                if (this.accountTypeViewsMap == null)
                {
                    this.accountTypeViewsMap = new Dictionary<string, List<AccountTypeViewEntry>>();
                    foreach (var accountType in this.AccountTypeXmlDocMap.Keys)
                    {
                        if (!this.accountTypeViewsMap.ContainsKey(accountType)) this.accountTypeViewsMap[accountType] = new List<AccountTypeViewEntry>();
                        var xDoc = this.AccountTypeXmlDocMap[accountType];
                        List<string> accountTypeViews = new List<string>();
                        foreach (XmlElement adapterSet in xDoc.SelectNodes("//AccountViews/AdapterSet"))
                        {
                            
                            string serviceDef = adapterSet.SelectNodeInnerText("./ConfigFile");
                            //string viewName = adapterSet.SelectNodeInnerText("./Name"); /* Name is NOT the view name. It is junk. */
                            string viewName = serviceDef.Substring(serviceDef.LastIndexOf('/')+1); 
                            accountTypeViewsMap[accountType].Add(new AccountTypeViewEntry(viewName, serviceDef));
                        }
                    }
                }
                return this.accountTypeViewsMap;
            }
        }

        public List<AccountTypeViewEntry> GetAccountTypeAccountViews(string accountType)
        {
            return AccountTypeViewsMap[accountType];
        }

        #endregion

        #region Account Views

        private List<string> avServiceDefFiles;
        public List<string> AvServiceDefFiles
        {
            get
            {
                if (this.avServiceDefFiles == null) 
                    this.avServiceDefFiles = FileUtils2.GlobToList(this.rmpDir + @"\Extensions\Account\config\AccountView\*\*.msixdef");
                return this.avServiceDefFiles;
            }
        }

        private List<XmlDocument> avServiceDefXmlDocs;
        public List<XmlDocument> AvServiceDefXmlDocs
        {
            get
            {
                if (this.avServiceDefXmlDocs == null)
                {
                    avServiceDefXmlDocs = new List<XmlDocument>();
                    foreach (string f in this.AvServiceDefFiles)
                    {
                        XmlDocument doc = MyXml.ParseXmlFile(f);
                        avServiceDefXmlDocs.Add(doc);
                    }
                }
                return avServiceDefXmlDocs;
            }
        }

        private List<string> avServiceDefs;
        public List<string> AvServiceDefs
        {
            get
            {
                if (this.avServiceDefs == null)
                {
                    this.avServiceDefs = new List<string>();
                    foreach (var xDoc in this.AvServiceDefXmlDocs)
                    {
                        foreach (var serviceDef in xDoc.SelectNodesInnerText("//defineservice/name"))
                        {
                            this.avServiceDefs.Add(serviceDef);
                        }
                    }
                }
                return this.avServiceDefs;
            }
        }

        private Dictionary<string, XmlDocument> avServiceDefXmlDocMap;
        public Dictionary<string, XmlDocument> AvServiceDefXmlDocMap
        {
            get
            {
                if (this.avServiceDefXmlDocMap == null)
                {
                    avServiceDefXmlDocMap = new Dictionary<string, XmlDocument>();
                    foreach (var xDoc in this.AvServiceDefXmlDocs)
                    {
                        foreach (var sd in xDoc.SelectNodesInnerText("//defineservice/name"))
                        {
                            this.avServiceDefXmlDocMap[sd] = xDoc;
                        }
                    }
                }
                return avServiceDefXmlDocMap;
            }
        }

        public XmlDocument GetAvServiceDefXmlDoc(string serviceDef)
        {
            return this.AvServiceDefXmlDocMap[serviceDef];
        }


        private Dictionary<string, ServiceDef> avServiceDefObjMap;
        public Dictionary<string, ServiceDef> AvServiceDefObjMap
        {
            get
            {
                if (this.avServiceDefObjMap == null)
                {
                    avServiceDefObjMap = new Dictionary<string, ServiceDef>();
                    foreach (string serviceDefName in this.AvServiceDefXmlDocMap.Keys)
                    {
                        XmlDocument serviceDefXmlDoc = this.AvServiceDefXmlDocMap[serviceDefName];
                        this.avServiceDefObjMap[serviceDefName.ToLower()] = new ServiceDef(serviceDefXmlDoc.DocumentElement);
                    }
                }
                return avServiceDefObjMap;
            }
        }


        public ServiceDef GetAvServiceDefObj(string serviceDef)
        {
            return this.AvServiceDefObjMap[serviceDef.ToLower()];
        }

        #endregion


       
    
    }
    public class ServiceDef
    {
        public XmlElement serviceDefElem;
        public string name;
        public string description;
        public List<FieldDef> fieldDefs = new List<FieldDef>();
        public string fileName;
        public FileInfo fileInfo;
        public string tableName;
        public static ServiceDef GetServiceDef(string msixDefFile)
        {
            XmlDocument msixDefDoc = MyXml.ParseXmlFile(msixDefFile);
            string serviceDefName = msixDefDoc.DocumentElement.SelectNodeInnerText("//defineservice/name");
            string shortName = serviceDefName.Substring(serviceDefName.LastIndexOf('/') + 1);
            //string tableName = "t_pt_" + shortName.ToLower();
            ServiceDef serviceDef= new ServiceDef(msixDefDoc.DocumentElement);
            serviceDef.fileName = msixDefFile;
            serviceDef.fileInfo = new FileInfo(serviceDef.fileName);
            DirectoryInfo nameSpaceDir = serviceDef.fileInfo.Directory;
            DirectoryInfo typeDir = nameSpaceDir.Parent;
            if (typeDir.Name.EqualsIgnoreCase("ParamTable")) serviceDef.tableName = "t_pt_" + shortName.ToLower();
            else if (typeDir.Name.EqualsIgnoreCase("ProductView")) serviceDef.tableName = "t_pv_" + shortName.ToLower();
            else if (typeDir.Name.EqualsIgnoreCase("AccountView")) serviceDef.tableName = "t_av_" + shortName.ToLower();
            return serviceDef;
        }
        public ServiceDef(XmlElement serviceDefElem)
        {
            this.serviceDefElem = serviceDefElem;
            this.name = serviceDefElem.SelectNodeInnerText("./name");
            this.description = serviceDefElem.SelectNodeInnerText("./description");
            foreach (XmlElement fieldDefElem in serviceDefElem.SelectNodes("./ptype")) this.fieldDefs.Add(new FieldDef(fieldDefElem));
        }
        public List<FieldDef> GetEnumFields()
        {
            List<FieldDef> results = new List<FieldDef>();
            foreach (var f in this.fieldDefs)
            {
                if (f.type.Equals("enum"))
                {
                    results.Add(f);
                }
            }
            return results;
        }
        public FieldDef GetFieldDefForColumnName(string columnName)
        {
            foreach (var fd in this.fieldDefs)
            {
                if (fd.columnName.EqualsIgnoreCase(columnName)) return fd;
            }
            return null;
        }
        public bool ContainsColumnName(string columnName)
        {
            return GetFieldDefForColumnName(columnName) != null;
        }
        

        // returns field entityDef if there is a single enum field that is part of key, This is for things like t_av_contact.
        public FieldDef GetEnumPartOfKeyFieldDef()
        {
            var nodes = this.serviceDefElem.SelectNodes("./ptype[@partofkey and ./type=\"enum\"] ");
            if (nodes.Count > 1)
            {
                return null;
                //throw new Exception("Error, not setup to handle more than one account view field with partofkey=true, see service definition: ".AppendLine() + serviceDefElem.OwnerDocument.OuterXml);
            }
            foreach (XmlElement elem in nodes)
            {
               return new FieldDef(elem);
            }
            return null;
        }
        
    }
    public class FieldDef
    {
        public string name;
        public string columnName;
        public string type;
        public string enumSpace;
        public string enumType;
        public string length;
        public string required;
        public string defaultValue;
        public string description;
        public string isOperator;
        public string columnOperator;
        public string displayName;
        public string isCondition;
        public string hasOperatorName;
        public string isAction;
        public string hasOperatorPerRule;
       
        public FieldDef(XmlElement fieldDefElem)
        {
           
            this.name = fieldDefElem.SelectNodeInnerText("./dn");
            this.columnName = "c_" + this.name;
            this.type = fieldDefElem.SelectNodeInnerText("./type");
            if (type.Equals("enum"))
            {
                this.enumSpace = fieldDefElem.SelectSingleElem("./type").GetAttribute("EnumSpace");
                this.enumType = fieldDefElem.SelectSingleElem("./type").GetAttribute("EnumType");
            }
            this.length = fieldDefElem.SelectNodeInnerText("./length");
            this.required = fieldDefElem.SelectNodeInnerText("./required");
            this.defaultValue = fieldDefElem.SelectNodeInnerText("./defaultvalue");
            this.description = fieldDefElem.SelectNodeInnerText("./description");
            this.isOperator = fieldDefElem.GetAttribute("operator");
            this.columnOperator = fieldDefElem.GetAttribute("column_operator");
            this.isAction = fieldDefElem.GetAttribute("action");
            this.hasOperatorName = "";
            this.displayName = fieldDefElem.GetAttribute("display_name");
            this.hasOperatorPerRule = fieldDefElem.GetAttribute("operator_per_rule");
            this.isCondition = fieldDefElem.GetAttribute("condition");
        }
    }

    public class AccountTypeViewEntry
    {
        public string viewName;
        public string serviceDef;
        public AccountTypeViewEntry(string viewName, string serviceDef)
        {
            this.viewName = viewName;
            this.serviceDef = serviceDef;
        }
    }
}
