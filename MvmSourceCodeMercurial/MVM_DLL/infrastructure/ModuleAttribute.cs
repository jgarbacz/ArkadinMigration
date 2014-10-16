using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.IO;
using System.Reflection;


namespace MVM
{
    public class ModuleAttribute : Attribute
    {
        // Turns on and off xml validation
        public static bool VALIDATE_XML = System.Environment.OSVersion.Platform.ToString().ToLower().StartsWith("win");

        public static ValidationEventHandler handler = new ValidationEventHandler(MyValidationEventHandler);

        public static Dictionary<string, ModuleAttribute> moduleAttributeLookup = new Dictionary<string, ModuleAttribute>();

        // Instance members
        public string xmlString { get; set; }
        private XmlElement xmlElement;
        public string[] categories = null;
        public string desc = null;
        public string description = null;
        public List<XmlElement> tests = new List<XmlElement>();
        public List<XmlElement> examples = new List<XmlElement>();

        public Dictionary<string, string> modes = new Dictionary<string, string>();
        public Dictionary<string, string> defaults = new Dictionary<string, string>();
        public Dictionary<string, string> types = new Dictionary<string, string>();

        public List<string> moduleNames = new List<string>();

        // Attribute constructor
        public ModuleAttribute(string xmlString)
        {
            this.xmlString = xmlString;

            var moduleAttrElem = this.GetXmlElement();
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(moduleAttrElem.OwnerDocument.NameTable);
            nsmgr.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");

            // Register the module name(s) with the handler
            string moduleName = null;
            foreach (XmlElement elem in moduleAttrElem.SelectNodes("./name"))
            {
                moduleName = elem.InnerText.Trim();
                moduleNames.Add(moduleName);
            }

            XmlElement cat = moduleAttrElem.SelectSingleElem("./doc/category");
            if (cat != null)
            {
                categories = cat.InnerText.Split('/');
                if (categories.Length > 2)
                {
                    throw new Exception("Cannot have more than two categories for module " + moduleName);
                }
            }
            else
            {
                categories = new string[1] { "Miscellaneous" };
            }
            XmlElement d = moduleAttrElem.SelectSingleElem("./doc/desc");
            if (d != null)
            {
                desc = d.InnerText;
            }
            XmlElement dd = moduleAttrElem.SelectSingleElem("./doc/description");
            if (dd != null)
            {
                description = dd.InnerText;
            }
            foreach (XmlElement test in moduleAttrElem.SelectElements("./test"))
            {
                bool isTest = test.GetAttributeDefaulted("example_only", "").IsNullOrEmpty();
                foreach (XmlElement elem in test.SelectElements("./*"))
                {
                    if (isTest)
                    {
                        tests.Add(elem);
                    }
                    examples.Add(elem);
                }
            }
        }

        // Parse the xml giving good error messages.
        public XmlElement GetXmlElement()
        {
            if (this.xmlElement != null) return this.xmlElement;
            try
            {
                this.xmlElement = ParseInputXmlString(this.xmlString);
            }
            catch (Exception e)
            {
                string message = "Invalid ModuleAttribute(xml): cannot parse xml=[".AppendLine() + this.xmlString.NumberLines().AppendLine() + "]. Parse error=[" + e.Message + "]";
                throw new Exception(message, e);
            }
            return this.xmlElement;
        }
        
        // Parses the xml for the newModule attribute.
        private static XmlElement ParseInputXmlString(string xmlString)
        {
            XmlSchema xs = GetModuleAttributeXmlSchema();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Schemas.Add(xs);
            if (VALIDATE_XML)
            {
                settings.ValidationType = ValidationType.Schema;
                settings.ValidationEventHandler += handler;
            }
            StringReader stringReader = new StringReader(xmlString);
            XmlReader vr = XmlReader.Create(stringReader, settings);
            XmlDocument doc = new XmlDocument();
            doc.Load(vr);
            return doc.DocumentElement;
        }
   
        // Returns the xml schema which is an embedded resource
        public static XmlSchema GetModuleAttributeXmlSchema()
        {
            // Pull out and compile the embedded xml schema file
            Assembly assbly = Assembly.GetExecutingAssembly();
            string resourceName = "MVM.modules.SchemaModuleAttribute.xsd";
            Stream strm = assbly.GetManifestResourceStream(resourceName);
            if (strm == null) throw new Exception("Error, cannot find embedded resource [" + resourceName + "]");
            XmlSchemaSet schemaSet = new XmlSchemaSet();
            XmlSchema x = XmlSchema.Read(strm, handler);
            schemaSet.Add(x);
            schemaSet.Compile();
            foreach (XmlSchema schema1 in schemaSet.Schemas())
            {
                return schema1;
            }
            throw new Exception("Error in GetModuleAttributeXmlSchema()");
        }

        public void Register(MvmEngine mvm,ConstructorInfo defaultConstructor, Type type)
        {
            if (this.moduleNames.Count == 0)
            {
                throw new Exception("Must define module name for xsd: [" + this.xmlString + "]");
            }

            var moduleAttrElem = this.GetXmlElement();
            IModuleSetup ms = (IModuleSetup)defaultConstructor.Invoke(null);

            BaseModuleSetup bs = ms as BaseModuleSetup;
            if (bs == null)
            {
                return;
            }
            bs.moduleAttribute = this;

            // Register the module name(s) with the handler
            foreach (var moduleName in this.moduleNames)
            {
                if (mvm.procLoader.moduleMap.ContainsKey(moduleName))
                {
                    throw new Exception("Duplicate module name " + moduleName);
                }
                mvm.procLoader.moduleMap[moduleName] = ms;
            }

            ModuleAttribute.moduleAttributeLookup[type.Name] = this;

            // Register the xsd info
            int i = 0;
            foreach (XmlElement elem in moduleAttrElem.SelectNodes("./xsd/*"))
            {
                bool isComplexType = elem.FirstChildElem() != null || !elem.Name.Equals("xs:element");
                string moduleType = "xs:string";
                if (isComplexType)
                {
                    if (i == 0 && elem.GetAttribute("name").IsNullOrEmpty())
                    {
                        moduleType = this.moduleNames[0] + "_type";
                        elem.SetAttribute("name", moduleType);
                    }
                    mvm.procLoader.AddModuleType(elem);
                }
                if (i == 0)
                {
                    foreach (var name in this.moduleNames)
                    {
                        XmlElement declarationElem = isComplexType ? moduleAttrElem.OwnerDocument.CreateElement("xs:element", "http://www.w3.org/2001/XMLSchema") : elem;
                        declarationElem.SetAttribute("name", name);
                        if (!declarationElem.HasAttribute("type"))
                        {
                            declarationElem.SetAttribute("type", moduleType);
                        }
                        mvm.procLoader.AddModuleDeclaration(declarationElem);
                    }
                    mvm.procLoader.AddModuleName(this.moduleNames[i], this.categories);
                }
                i++;
            }
        }

        /// <summary>
        /// Xml Validation handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void MyValidationEventHandler(object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Warning)
            {
                 throw new Exception(e.Message);
            }
            else if (e.Severity == XmlSeverityType.Error)
            {
                string message="Error parsing xml at line_no=[" + e.Exception.LineNumber+"] pos=[" + e.Exception.LinePosition+"] error=[" + e.Message+"]";
                throw new Exception(message);
            }
        }
    }

    public class ModuleSchemaInfo
    {
        public XmlElement moduleDeclaration;
        public XmlElement moduleType;
        public ModuleSchemaInfo(XmlElement decl, XmlElement type)
        {
            moduleDeclaration = decl;
            moduleType = type;
        }
    }
}
