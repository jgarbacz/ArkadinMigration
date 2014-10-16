using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Linq;

namespace MVM
{
    public class XmlConfigParser
    {
        public bool VALIDATE_XML = System.Environment.OSVersion.Platform.ToString().ToLower().StartsWith("win");
        private XmlSchema xmlSchema;
        private XmlDocument xmlDocument;
        private readonly ValidationEventHandler validationHandler = new ValidationEventHandler(MyValidationEventHandler);
        private readonly XmlReaderSettings xmlSettings = new XmlReaderSettings();
           
        public List<XmlElement> moduleDeclarations = new List<XmlElement>();
        public List<XmlElement> moduleTypes = new List<XmlElement>();
        public Dictionary<string, string[]> moduleCategories = new Dictionary<string, string[]>();

        public readonly MvmEngine mvm;
        public XmlConfigParser(MvmEngine mvm)
        {
            this.mvm = mvm;
        }

        /// <summary>
        /// After all dynamic module info has been loaded, we can create and load the module XSD
        /// </summary>
        public void LoadXSD()
        {
            // get and compile the xml schema
            Assembly assbly = Assembly.GetExecutingAssembly();
            string resourceName = "MVM.modules.ProcModuleSchema.xml";
            Stream strm = assbly.GetManifestResourceStream(resourceName);
            if (strm == null) throw new Exception("Error, cannot find embedded resource [" + resourceName + "]");

            // To avoid warnings from the special attributes like datatype, mode, et al. that aren't sanctioned by XSD,
            // we use an empty namespace of WildcardSchema in the xml file, then replace it here with the real schema.
            string origDoc = new StreamReader(strm).ReadToEnd();
            Regex r = new Regex("xmlns:xs=['\"]([^'\"])+['\"]");
            origDoc = r.Replace(origDoc, "xmlns:xs=\"http://www.w3.org/2001/XMLSchema\"", 1);
            XmlDocument doc = new XmlDocument();
            byte[] docBytes = Encoding.ASCII.GetBytes(origDoc);
            doc.Load(new MemoryStream(docBytes));
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");

            // Locate the first group, where we will insert the second declaration
            XmlElement schema = doc.SelectSingleElemNS("./xs:schema", nsmgr);
            XmlElement group = schema.SelectSingleElemNS("./xs:group", nsmgr);
            XmlElement groupChoice = group.SelectSingleElemNS("./xs:choice", nsmgr);

            // Insert the module declarations
            foreach (var dec in this.moduleDeclarations)
            {
                var copy = doc.ImportNode(dec, true);
                schema.PrependChild(copy);
                var copy2 = copy.Clone();
                groupChoice.PrependChild(copy2);
            }

            // Insert the module types
            foreach (var type in this.moduleTypes)
            {
                var copy = doc.ImportNode(type, true);
                schema.AppendChild(copy);
            }

            // Write out module documentation
            this.AnalyzeModules(schema);

            // Convert the XML to an XSD
            XmlSchemaSet schemaSet = new XmlSchemaSet();
            using (XmlReader xmlReader = XmlReader.Create(new StringReader(schema.OuterXml)))
            {
                try
                {
                    XmlSchema x = XmlSchema.Read(xmlReader, validationHandler);
                    schemaSet.Add(x);
                    schemaSet.Compile();
                }
                catch (Exception e)
                {
                    throw new Exception("Error converting the following XML to XSD: [" + schema.OuterXml + "]", e);
                }
            }
            foreach (XmlSchema schema1 in schemaSet.Schemas())
            {
                this.xmlSchema = schema1;
                break;
            }
            // load the xml xmlSettings
            xmlSettings.Schemas.Add(this.xmlSchema);
            xmlSettings.ValidationType = ValidationType.Schema;
            xmlSettings.ValidationEventHandler += validationHandler;

            this.xmlDocument = new XmlDocument();
            this.xmlDocument.PreserveWhitespace = true;
            this.xmlDocument.Schemas.Add(this.xmlSchema);

            this.moduleDeclarations = null;
            this.moduleTypes = null;
        }

        /// <summary>
        /// Adds a schema module declaration to the xsd.
        /// </summary>
        public void AddModuleDeclaration(XmlElement declaration)
        {
            this.moduleDeclarations.Add(declaration);
        }

        /// <summary>
        /// Adds a schema module type to the xsd.
        /// </summary>
        public void AddModuleType(XmlElement type)
        {
            this.moduleTypes.Add(type);
        }

        /// <summary>
        /// Adds a schema module name for documentation purposes
        /// </summary>
        public void AddModuleName(string name, string[] category)
        {
            this.moduleCategories[name] = category;
        }

        public XmlElement ModulesDocumentation = null;

        /// <summary>
        /// Writes out documentation for each module that has a ModuleAttribute decoration.
        /// </summary>
        public void AnalyzeModules(XmlElement schema)
        {
            XmlDocument newDoc = new XmlDocument();
            XmlElement outputNode = newDoc.CreateElement("junk");
            XmlElement sortedModules = newDoc.CreateElement("modules");
            newDoc.AppendChild(sortedModules);
            Dictionary<string, XmlElement> moduleOutput = new Dictionary<string, XmlElement>();
            Dictionary<string, XmlElement> typeIndex = new Dictionary<string, XmlElement>();
            Dictionary<string, bool> typeVisited = new Dictionary<string, bool>();
            foreach (XmlElement elem in schema.SelectNodes("./*"))
            {
                string name = elem.GetAttribute("name");
                if (name.IsNullOrEmpty())
                {
                    throw new Exception("Top-level xsd elements must have names: [" + elem.OuterXml + "]");
                }
                if (typeIndex.ContainsKey(name))
                {
                    throw new Exception("Duplicate type in xsd index: " + name);
                }
                typeIndex[name] = elem;
            }

            Dictionary<string, List<XmlElement>> byCategory = new Dictionary<string, List<XmlElement>>();
            foreach (var kv in this.moduleCategories)
            {
                // Traverse the XSD to build up the docs for each module
                int maxModDepth = 0;
                int choiceCtr = 1;
                TraverseModule(kv.Key, 0, 0, ref maxModDepth, ref choiceCtr, (BaseModuleSetup)this.mvm.procLoader.moduleMap[kv.Key], outputNode, typeIndex, typeVisited, typeIndex[kv.Key]);
                moduleOutput[kv.Key] = (XmlElement)outputNode.LastChild;
                string cat = kv.Value.Join(',');
                if (byCategory.ContainsKey(cat))
                {
                    byCategory[cat].Add((XmlElement)outputNode.LastChild);
                }
                else
                {
                    byCategory[cat] = new List<XmlElement>();
                    byCategory[cat].Add((XmlElement)outputNode.LastChild);
                }
            }
            foreach (var kv in from kv in byCategory orderby kv.Key ascending select kv)
            {
                // Sort the modules by category
                XmlElement category = sortedModules.CreateElement("category_group");
                category.SetAttribute("name", kv.Key);
                foreach (var elem in from e in kv.Value orderby e.GetAttribute("name") select e)
                {
                    category.AppendChild(elem);
                }
                sortedModules.AppendChild(category);
            }

            // store this on the class so we can get it later if we want to autodoc
            this.ModulesDocumentation = sortedModules;
            
            //foreach (var ma in ModuleAttribute.moduleAttributeLookup)
            //{
            //    // Run all module unit tests that we know about
            //    foreach (var test in ma.Value.tests)
            //    {
            //    }
            //}

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(newDoc.NameTable);
            nsmgr.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");
            foreach (var e in schema.SelectElementsNS("descendant::*", nsmgr))
            {
                if (e.HasAttribute("mode"))
                {
                    e.RemoveAttribute("mode");
                }
                if (e.HasAttribute("default"))
                {
                    e.RemoveAttribute("default");
                }
                if (e.HasAttribute("description"))
                {
                    e.RemoveAttribute("description");
                }
                if (e.HasAttribute("values"))
                {
                    e.RemoveAttribute("values");
                }
                if (e.HasAttribute("datatype"))
                {
                    e.RemoveAttribute("datatype");
                }
                if (e.HasAttribute("norecurse"))
                {
                    e.RemoveAttribute("norecurse");
                }
            }
            try
            {
                schema.PrettyPrint(Path.GetTempPath() + Path.DirectorySeparatorChar + this.mvm.nodeId.ToString() + ".MVM.ProcModuleSchema.xml");
            }
            catch
            {
                // Ignore exceptions here
            }
        }

        /// <summary>
        /// Recurses through module elements to build a documentation tree.
        /// </summary>
        private void TraverseModule(string firstName, int depth, int modDepth, ref int maxModDepth, ref int choiceCtr, BaseModuleSetup module, XmlElement outputNode, Dictionary<string, XmlElement> typeIndex, Dictionary<string, bool> typeVisited, XmlElement currElem)
        {
            bool writeDepth = false;
            bool needOuterGroup = false;
            bool needInnerGroups = false;
            XmlElement outerGroup = null;
            string currName = currElem.GetAttribute("name");
            if (depth > 0 && currName.Equals(firstName))
            {
                return;
            }
            if (currElem.Name.Equals("xs:element"))
            {
                if (modDepth > maxModDepth)
                {
                    maxModDepth = modDepth;
                }
                if (currName.IsNullOrEmpty())
                {
                    throw new Exception("All xs:elements must have name attributes: [" + currElem.OuterXml + "]");
                }
                XmlElement firstChild = currElem.FirstChildElem();
                string type = currElem.GetAttribute("type");
                if (type.IsNullOrEmpty() && firstChild == null)
                {
                    throw new Exception("Must define type attribute for xsd element: [" + currElem.OuterXml + "]");
                }
                string mode = depth == 0 ? currElem.GetAttribute("mode") : currElem.GetAttributeDefaulted("mode", "in");
                string datatype = currElem.GetAttribute("datatype");
                if (datatype.IsNullOrEmpty() && type.StartsWith("xs:"))
                {
                    datatype = "string";
                }
                string defaultVal = currElem.GetAttribute("default");
                string description = currElem.GetAttribute("description");
                string values = currElem.GetAttribute("values");
                string min = currElem.GetAttribute("minOccurs");
                string max = currElem.GetAttribute("maxOccurs");
                XmlElement newElem = outputNode.CreateElement("element");
                modDepth++;
                outputNode.AppendChild(newElem);
                newElem.SetAttribute("name", currName);
                if (!mode.IsNullOrEmpty())
                {
                    newElem.SetAttribute("mode", mode);
                }
                if (!datatype.IsNullOrEmpty())
                {
                    newElem.SetAttribute("datatype", datatype);
                    module.moduleAttribute.types[currName] = datatype;
                }
                if (!min.IsNullOrEmpty())
                {
                    newElem.SetAttribute("min", min);
                }
                if (!max.IsNullOrEmpty())
                {
                    newElem.SetAttribute("max", max);
                }
                module.moduleAttribute.modes[currName] = mode;
                if (depth == 0)
                {
                    ModuleAttribute mod = module.moduleAttribute;
                    if (mod.desc != null)
                    {
                        XmlElement d = newElem.CreateElement("module_desc");
                        d.InnerText = mod.desc;
                        newElem.AppendChild(d);
                    }
                    if (mod.description != null)
                    {
                        XmlElement d = newElem.CreateElement("module_description");
                        d.InnerText = mod.description;
                        newElem.AppendChild(d);
                        writeDepth = true;
                    }
                }
                if (!description.IsNullOrEmpty())
                {
                    XmlElement d = newElem.CreateElement("description");
                    d.InnerText = description;
                    newElem.AppendChild(d);
                }
                if (type.StartsWith("xs:"))
                {
                    if (!defaultVal.IsNullOrEmpty())
                    {
                        XmlElement d = newElem.CreateElement("default");
                        d.InnerText = defaultVal;
                        newElem.AppendChild(d);
                        newElem.SetAttribute("min", "0");
                        currElem.SetAttribute("minOccurs", "0");
                        module.moduleAttribute.defaults[currName] = defaultVal;
                    }
                    if (!values.IsNullOrEmpty())
                    {
                        foreach (var val in values.Split(','))
                        {
                            string[] kv = val.Split(':');
                            XmlElement v = newElem.CreateElement("value");
                            v.SetAttribute("name", kv[0]);
                            v.InnerText = kv[1];
                            newElem.AppendChild(v);
                        }
                    }
                }
                else if (firstChild == null)
                {
                    currElem = typeIndex[type];
                    typeVisited[type] = true;
                    if (currElem.HasAttribute("description"))
                    {
                        XmlElement desc = outputNode.CreateElement("description");
                        desc.InnerText = currElem.GetAttribute("description");
                        outputNode.AppendChild(desc);
                    }
                    if (currElem.HasAttribute("norecurse"))
                    {
                        return;
                    }
                }
                else if (firstChild != null)
                {
                }
                outputNode = newElem;
            }
            else if (currElem.Name.Equals("xs:attribute"))
            {
                XmlElement attr = outputNode.CreateElement("attribute");
                attr.SetAttribute("name", currElem.GetAttribute("name"));
                if (currElem.HasAttribute("datatype"))
                {
                    attr.SetAttribute("datatype", currElem.GetAttribute("datatype"));
                }
                if (currElem.HasAttribute("description"))
                {
                    XmlElement desc = attr.CreateElement("description");
                    desc.InnerText = currElem.GetAttribute("description");
                    attr.AppendChild(desc);
                }
                attr.SetAttribute("use", currElem.GetAttributeDefaulted("use", "optional"));
                outputNode.AppendChild(attr);
                outputNode = attr;
            }
            else if (currElem.Name.Equals("xs:enumeration"))
            {
                XmlElement v = outputNode.CreateElement("value");
                v.SetAttribute("name", currElem.GetAttribute("value"));
                if (currElem.HasAttribute("description"))
                {
                    v.InnerText = currElem.GetAttribute("description");
                }
                outputNode.AppendChild(v);
            }
            else if (currElem.Name.Equals("xs:choice"))
            {
                needInnerGroups = true;
                outerGroup = outputNode.CreateElement("choice");
                outerGroup.SetAttribute("id", (choiceCtr++).ToString());  // save a unique id for later use in the XSL
            }
            else if (currElem.Name.Equals("xs:group"))
            {
                string gref = currElem.GetAttribute("ref");
                if (!gref.IsNullOrEmpty())
                {
                    currElem = typeIndex[gref];
                    typeVisited[gref] = true;
                    if (currElem.HasAttribute("norecurse"))
                    {
                        return;
                    }
                }
                needOuterGroup = true;
            }
            else if (currElem.Name.In("xs:sequence", "xs:all"))
            {
                needOuterGroup = true;
            }
            bool gotChoices = false;
            if (needOuterGroup)
            {
                outerGroup = outputNode.CreateElement("group");
            }
            if (currElem.Name.In("xs:sequence", "xs:all", "xs:group", "xs:choice"))
            {
                if (currElem.HasAttribute("minOccurs"))
                {
                    outerGroup.SetAttribute("min", currElem.GetAttribute("minOccurs"));
                }
                if (currElem.HasAttribute("maxOccurs"))
                {
                    outerGroup.SetAttribute("max", currElem.GetAttribute("maxOccurs"));
                }
            }
            foreach (XmlElement elem in currElem.SelectNodes("./*"))
            {
                if (needInnerGroups)
                {
                    XmlElement innerGroup = outerGroup.CreateElement("group");
                    TraverseModule(firstName, depth + 1, modDepth, ref maxModDepth, ref choiceCtr, module, innerGroup, typeIndex, typeVisited, elem);
                    if (innerGroup.HasChildElements())
                    {
                        List<XmlElement> cc;
                        for (cc = innerGroup.GetChildElems(); cc.Count == 1 && cc[0].Name.Equals("group") && !innerGroup.HasAttributes; cc = innerGroup.GetChildElems())
                        {
                            innerGroup = cc[0];
                        }
                        outerGroup.AppendChild(innerGroup);
                        gotChoices = true;
                    }
                }
                else
                {
                    TraverseModule(firstName, depth + 1, modDepth, ref maxModDepth, ref choiceCtr, module, needOuterGroup ? outerGroup : outputNode, typeIndex, typeVisited, elem);
                }
            }
            if (gotChoices || (needOuterGroup && outerGroup.HasChildElements()))
            {
                List<XmlElement> cc;
                for (cc = outerGroup.GetChildElems(); cc.Count == 1 && cc[0].Name.Equals("group") && !outerGroup.HasAttributes; cc = outerGroup.GetChildElems())
                {
                    outerGroup = cc[0];
                }
                outputNode.AppendChild(outerGroup);
            }
            if (writeDepth && outputNode.Name.Equals("element"))
            {
                XmlElement depthElem = outputNode.CreateElement("module_depth");
                depthElem.InnerText = maxModDepth.ToString();
                outputNode.AppendChild(depthElem);
            }
        }

        public static Regex LineInfoRegex = new Regex(@"\s*_lineinfo=['""][^'""]+['""]");

        /// <summary>
        /// Parses the xml string and return the document element or throw an
        /// exception on error.
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public XmlElement ParseXmlString(string xmlString)
        {
            lock (this)
            {
                if (this.mvm.addXmlHashes)
                {
                    this.mvm.AddStringHash(xmlString);
                }

                // This could be generated code, so strip out the line numbers to pass XSD validation
                string newXmlString = LineInfoRegex.Replace(xmlString, "");
                string error = this.LoadXml(newXmlString);
                if (error != null)
                {
                    throw new Exception(error);
                }
                error = this.Validate();
                if (error != null)
                {
                    error = this.ValidatingLoadXml(newXmlString);
                    throw new Exception(error);
                }
                // Reload the original one with the line info
                this.LoadXml(xmlString);
                AddLineInfo(this.xmlDocument, "unknown");
                PreprocessXml(this.xmlDocument);
                return this.xmlDocument.DocumentElement;
            }
        }
        
        /// <summary>
        /// Parses the xml file and return the document element. If there
        /// is an error then we will return xml like this:
        /// <error>
        /// <error_message>here is why parsing failed...</error_message>
        /// <procContext name='xxxx' namespace='yyyy'/>
        /// <procContext name='xxxx' namespace='yyyy'/>
        /// </error>
        /// </summary>
        /// <param name="xmlFileName"></param>
        /// <returns></returns>
        public XmlElement ParseXmlFile(string xmlFileName)
        {
            lock (this)
            {
                StreamReader sr = new StreamReader(xmlFileName);
                string xmlString = sr.ReadToEnd();
                if (this.mvm.addXmlHashes)
                {
                    this.mvm.AddFileHash(xmlFileName, xmlString);
                }

                string newXmlString = LineInfoRegex.Replace(xmlString, "");
                string error = this.Load(xmlFileName, new MemoryStream(Encoding.UTF8.GetBytes(newXmlString)));
                if (error != null)
                {
                    return ProcessErrors(xmlFileName,error);
                }
                error = this.Validate();
                if (error != null)
                {
                    error = this.ValidatingLoad(xmlFileName);
                    return ProcessErrors(xmlFileName,error);
                }
                // Reload the original one with the line info
                
                this.Load(xmlFileName, new MemoryStream(Encoding.UTF8.GetBytes(xmlString)));
                AddLineInfo(this.xmlDocument, xmlFileName);
                PreprocessXml(this.xmlDocument);
                return this.xmlDocument.DocumentElement;
            }
        }

        /// <summary>
        /// Adds line number information to the elements in the document.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public void AddLineInfo(XmlDocument document, string filename)
        {
            // Regular XmlElements don't give us line numbers, but XPathNavigator objects do.  So
            // go through the XPathDocument alongside the XmlDocument and copy the line numbers over.
            // This means we parse the xml twice (we still only read it from disk once), but the
            // impact on startup speed seems negligible.
            XPathDocument doc = new XPathDocument(XmlReader.Create(new StringReader(document.OuterXml)));
            XPathNavigator nav = doc.CreateNavigator();
            XPathNodeIterator iter = nav.Select("//*");
            XmlNodeList allElements = document.SelectNodes("//*");
            int nodeIdx = 0;
            while (iter.MoveNext())
            {
                XPathNavigator root = iter.Current;
                IXmlLineInfo info = root as IXmlLineInfo;
                ((XmlElement)allElements[nodeIdx]).SetAttribute("_lineinfo", filename + "|" + info.LineNumber.ToString());
                nodeIdx++;
            }
        }

        /// <summary>
        /// Pre-processes the xml document to add some syntactic proc sugar that
        /// adds logic for the "run_after" and "object_type" proc attributes.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public void PreprocessXml(XmlDocument document)
        {
            XmlNodeList nodes = document.SelectNodes("./procs");
            if (nodes.Count == 0)
            {
                nodes = document.SelectNodes("./proc");
            }
            else
            {
                nodes = nodes[0].SelectNodes("./proc");
            }
            foreach (XmlNode node in nodes)
            {
                XmlAttributeCollection attrs = node.Attributes;
                XmlNode name = attrs.GetNamedItem("name");
                XmlNode run_after = attrs.GetNamedItem("run_after");
                XmlNode object_type = attrs.GetNamedItem("object_type");
                if (run_after != null)
                {
                    string text = null;
                    if (object_type != null)
                    {
                        text =
                            "<startup>" +
                            " <push_after name='" + run_after.InnerText + "'>" +
                            "  <if>" +
                            "   <condition>OBJECT.object_type eq '" + object_type.InnerText + "'</condition>" +
                            "   <then>" +
                            "    <call_proc_for_current_object>" +
                            "     <name>'" + name.InnerText + "'</name>" +
                            "    </call_proc_for_current_object>" +
                            "   </then>" +
                            "  </if>" +
                            " </push_after>" +
                            "</startup>";
                    }
                    else
                    {
                        text =
                            "<startup>" +
                            " <push_after name='" + run_after.InnerText + "'>" +
                            "  <call_proc_for_current_object>" +
                            "   <name>'" + name.InnerText + "'</name>" +
                            "  </call_proc_for_current_object>" +
                            " </push_after>" +
                            "</startup>";
                    }
                    XmlDocumentFragment fragment = document.CreateDocumentFragment();
                    fragment.InnerXml = text;
                    node.PrependChild(fragment);
                }
            }
        }

        private XmlElement ProcessErrors(string xmlFileName,string error)
        {
            // since xml parsing failed we can try to grep out the procs
            List<string[]> procs = GrepForProcs(xmlFileName);

            // if we get here we had a parsing error. Try to grep out the procs
            // so upstream can register the procs and error should you try to call them.
            StringBuilder errorXml = new StringBuilder("<error>");
            errorXml.AppendLine("<error_message><![CDATA[" + error + "]]></error_message>");
            foreach (string[] proc in procs)
            {
                string procName = proc[0];
                string nameSpace = proc[1];
                errorXml.Append("<proc name='" + procName + "'");
                if (nameSpace != null) errorXml.Append(" namespace='" + nameSpace + "'");
                errorXml.AppendLine("/>");
            }
            errorXml.AppendLine("</error>");
            XmlDocument errDoc = MyXml.ParseXmlString(errorXml.ToString());
            return errDoc.DocumentElement;
        }

        // validates the document and returns the error message;
        private string Validate()
        {
            StringBuilder errorMessage = new StringBuilder();
            try
            {
                this.xmlDocument.Validate(this.validationHandler);
                return null;
            }
            catch (XmlException e)
            {
                errorMessage.AppendLine("Error on line=" + e.LineNumber + ", pos=" + e.LinePosition);
                errorMessage.AppendLine(e.Message);
            }
            catch (Exception e)
            {
                errorMessage.AppendLine(e.Message);
            }
            return errorMessage.ToString();
        }

        // loads the document and returns the error message;
        private string Load(string xmlFileName, MemoryStream xmlFileStream)
        {
            StringBuilder errorMessage = new StringBuilder();
            try
            {
                this.xmlDocument.Load(xmlFileStream);
                return null;
            }
            catch (XmlException e)
            {
                errorMessage.AppendLine("Error parsing input xml file [" + xmlFileName + "]");
                errorMessage.AppendLine("Error on line=" + e.LineNumber + ", pos=" + e.LinePosition);
                errorMessage.AppendLine(e.Message);
            }
            catch (Exception e)
            {
                errorMessage.AppendLine("Error parsing input xml file [" + xmlFileName + "]");
                errorMessage.AppendLine(e.Message);
            }
            return errorMessage.ToString();
        }
        // expected to error
        private string ValidatingLoad(string xmlFileName)
        {
            StringBuilder errorMessage = new StringBuilder();
            XmlTextReader tr = new XmlTextReader(xmlFileName);
            XmlReader xmlReader = XmlReader.Create(tr, this.xmlSettings);
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(xmlReader);
            }
            catch (XmlException e)
            {
                errorMessage.AppendLine("Error parsing input xml file [" + xmlFileName + "]");
                errorMessage.AppendLine("Error on line=" + e.LineNumber + ", pos=" + e.LinePosition);
                errorMessage.AppendLine(e.Message);
            }
            catch (Exception e)
            {
                errorMessage.AppendLine("Error parsing input xml file [" + xmlFileName + "]");
                errorMessage.AppendLine(e.Message);
            }
            if (errorMessage.ToString().Equals("")) throw new Exception("unexpected");
            return errorMessage.ToString();
        }

        // loads the document and returns the error message;
        private string LoadXml(string xmlString)
        {
            string errorMessage;
            try
            {
                this.xmlDocument.LoadXml(xmlString);
                return null;
            }
            catch (XmlException e)
            {
                errorMessage = "Cannot parse xml=[".AppendLine() + xmlString.NumberLines().AppendLine() + "]. Parse error=[" + e.Message + "]";
            }
            catch (Exception e)
            {
                errorMessage = "Cannot parse xml=[".AppendLine() + xmlString.NumberLines().AppendLine() + "]. Parse error=[" + e.Message + "]";
            }
            return errorMessage;
        }

        // this is expected to error
        private string ValidatingLoadXml(string xmlString)
        {
            string errorMessage=null;
            StringReader stringReader = new StringReader(xmlString);
            XmlReader xmlReader = XmlReader.Create(stringReader, this.xmlSettings);
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(xmlReader);
            }
            catch (XmlException e)
            {
                errorMessage = "Cannot parse xml=[".AppendLine() + xmlString.NumberLines().AppendLine() + "]. Parse error=[" + e.Message + "]";
            }
            catch (Exception e)
            {
                errorMessage = "Cannot parse xml=[".AppendLine() + xmlString.NumberLines().AppendLine() + "]. Parse error=[" + e.Message + "]";
            }
            if(errorMessage==null) throw new Exception("unexpected");
            return errorMessage;
        }

        public static Regex procRegex = new Regex(@"<proc[^>]+>", RegexOptions.IgnoreCase);
        public static Regex procNameRegex = new Regex(@"name\s*=\s*['""](\w+)['""]", RegexOptions.IgnoreCase);
        public static Regex procNameSpaceRegex = new Regex(@"namespace\s*=\s*['""](\w+)['""]", RegexOptions.IgnoreCase);
        /// <summary>
        /// Returns List of ["initNamespaceProcName","nameSpace"]
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="structuralNameSpace"></param>
        /// <returns></returns>
        public static List<string[]> GrepForProcs(string fileName)
        {
            List<string[]> output = new List<string[]>();
            string fileText = File.ReadAllText(fileName);
            MatchCollection mc1 = procRegex.Matches(fileText);
            foreach (Group g1 in mc1)
            {
                string procXml = g1.Value;
                string procName = null;
                string procNameSpace = null;
                MatchCollection mc2 = procNameRegex.Matches(procXml);
                if (mc2.Count > 0)
                {
                    procName = mc2[0].Groups[1].Value;
                    MatchCollection mc3 = procNameSpaceRegex.Matches(procXml);
                    if (mc3.Count > 0) procNameSpace = mc3[0].Groups[1].Value;
                    output.Add(new string[] { procName, procNameSpace });
                }
            }
            return output;
        }

        /// <summary>
        /// Xml Validation handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MyValidationEventHandler(object sender, ValidationEventArgs e)
        {
            string message = "Error parsing xml at line_no=[" + e.Exception.LineNumber + "] pos=[" + e.Exception.LinePosition + "] error=[" + e.Message + "]";
            throw new Exception(message);
        }

//        public static void Test()
//        {
//            XmlConfigParser xcp = new XmlConfigParser();
//            SplitStopwatch sw = new SplitStopwatch();
//            sw.Start();
//            for (int i = 0; i < 100000; i++)
//            {
//                var de=xcp.ParseXmlString(@"<proc name='hi'>
//
//<do>OBJECT.x=OBJECT.y</do>
//
//
//</proc>
//");
//                //Console.WriteLine(de.OuterXml);
//            }
//            sw.Stop();
//            Console.WriteLine("ms="+sw.ElapsedMilliseconds);
           
//        }
    }
}
