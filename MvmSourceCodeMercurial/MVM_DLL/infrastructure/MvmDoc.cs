using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Reflection;
using System.IO;
namespace MVM
{
    public class MvmDoc
    {
        public readonly MvmEngine mvm;
        public MvmDoc(MvmEngine mvm)
        {
            this.mvm = mvm;
        }

        public Dictionary<string, string[]> procCategories = new Dictionary<string, string[]>();
        public Dictionary<string, string[]> functionCategories = new Dictionary<string, string[]>();
        public Dictionary<string, string[]> moduleCategories;

        public void GenerateXmlDoc(string fileName)
        {
            if (fileName.IsNullOrEmpty())
            {
                fileName = this.mvm.documentationDir + Path.DirectorySeparatorChar + "MVM.AutoDoc.xml";
            }

            // Write out the raw autodoc xml file
            XmlDocument autoDoc = new XmlDocument();
            XmlElement autodocElem = autoDoc.CreateElement("mvm_auto_doc");
            XmlElement procs = this.DocumentMvmProcs();
            autodocElem.AppendChildImport(procs);
            XmlElement modules = this.DocumentMvmModules();
            autodocElem.AppendChildImport(modules);
            XmlElement functions = this.DocumentMvmFunctions();
            autodocElem.AppendChildImport(functions);
            autodocElem.PrettyPrint(fileName);

            // Write the module/function/proc sections in asciidoc format
            string functionStylesheet = this.mvm.documentationDir + Path.DirectorySeparatorChar + "functions2doc.xsl";
            XmlReader functionReader = XmlReader.Create(new StringReader(functions.OuterXml));
            XPathDocument funcXPathDoc = new XPathDocument(functionReader);
            XslCompiledTransform funcXslTrans = new XslCompiledTransform();
            funcXslTrans.Load(functionStylesheet);
            XmlTextWriter funcWriter = new XmlTextWriter(this.mvm.documentationDir + Path.DirectorySeparatorChar + "MVM.FunctionDocumentation.txt", null);
            funcXslTrans.Transform(funcXPathDoc, null, funcWriter);

            string procStylesheet = this.mvm.documentationDir + Path.DirectorySeparatorChar + "procs2doc.xsl";
            XmlReader procReader = XmlReader.Create(new StringReader(procs.OuterXml));
            XPathDocument procXPathDoc = new XPathDocument(procReader);
            XslCompiledTransform procXslTrans = new XslCompiledTransform();
            procXslTrans.Load(procStylesheet);
            XmlTextWriter procWriter = new XmlTextWriter(this.mvm.documentationDir + Path.DirectorySeparatorChar + "MVM.ProcDocumentation.txt", null);
            procXslTrans.Transform(procXPathDoc, null, procWriter);

            string moduleStylesheet = this.mvm.documentationDir + Path.DirectorySeparatorChar + "modules2doc.xsl";
            XmlReader moduleReader = XmlReader.Create(new StringReader(modules.OuterXml));
            XPathDocument moduleXPathDoc = new XPathDocument(moduleReader);
            XslCompiledTransform moduleXslTrans = new XslCompiledTransform();
            moduleXslTrans.Load(moduleStylesheet);
            XmlTextWriter moduleWriter = new XmlTextWriter(this.mvm.documentationDir + Path.DirectorySeparatorChar + "MVM.ModuleDocumentation.txt", null);
            moduleXslTrans.Transform(moduleXPathDoc, null, moduleWriter);
        }

        public XmlElement DocumentMvmProcs()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement procsElem = doc.CreateElement("procs");
            Dictionary<string, XmlElement> procsByCategory = new Dictionary<string, XmlElement>();

            IEnumerable<ProcInfo> procs;
            mvm.workMgr.schedulerMaster.GetDocumentedProcs(out procs);
            foreach (var procinfo in from procinfo in procs orderby procinfo.localName ascending select procinfo)
            {
                XmlElement procElement = procsElem.CreateElement("proc");
                procElement.SetAttribute("name", procinfo.localName);
                procElement.SetAttribute("namespace", procinfo.nameSpace);
                procElement.SetAttribute("location", procinfo.location.GetLocation());
                XmlElement memberElement = procElement.CreateElement("member");
                procElement.AppendChild(memberElement);
                XmlElement summaryElement = memberElement.CreateElement("summary");
                summaryElement.InnerText = procinfo.description;
                memberElement.AppendChild(summaryElement);

                this.procCategories[procinfo.localName] = procinfo.categories;
                string cat = procinfo.categories.Join('/');

                XmlElement categoryElement;
                if (procsByCategory.ContainsKey(cat))
                {
                    categoryElement = procsByCategory[cat];
                }
                else
                {
                    categoryElement = procsElem.CreateElement("category_group");
                    categoryElement.SetAttribute("name", cat);
                    procsByCategory[cat] = categoryElement;
                }

                foreach (var param in procinfo.paramElems)
                {
                    XmlElement paramElement = memberElement.CreateElement("param");
                    paramElement.SetAttribute("name", param.GetAttribute("name"));
                    paramElement.SetAttribute("mode", param.GetAttributeDefaulted("mode", "in"));
                    if (param.HasAttribute("default"))
                    {
                        paramElement.SetAttribute("default", param.GetAttribute("default"));
                    }
                    if (param.HasAttribute("description"))
                    {
                        paramElement.SetAttribute("description", param.GetAttribute("description"));
                    }
                    if (param.HasAttribute("datatype"))
                    {
                        paramElement.SetAttribute("datatype", param.GetAttribute("datatype"));
                    }
                    memberElement.AppendChild(paramElement);
                }

                XmlElement returnsElement = memberElement.CreateElement("returns");
                memberElement.AppendChild(returnsElement);
                categoryElement.AppendChild(procElement);
            }
            foreach (var catElem in from kv in procsByCategory orderby kv.Key ascending select kv.Value)
            {
                procsElem.AppendChild(catElem);
            }
            return procsElem;
        }

        public string GetProcs(string procName)
        {
            XmlElement procsElem = DocumentMvmProcs();
            StringBuilder output = new StringBuilder();
            if (procName.IsNullOrEmpty())
            {
                PrintByCategory(this.procCategories, output);
            }
            else
            {
                foreach (var procElem in procsElem.SelectElements("//proc"))
                {
                    if (procElem.GetAttribute("name").EqualsIgnoreCase(procName))
                    {
                        output.AppendLine(procElem.PrettyString());
                        break;
                    }
                }
            }
            string result = output.ToString().Nvl("Unknown proc: [" + procName + "]");
            return result;
        }

        /// <summary>
        /// Documents MvmExported functions
        /// </summary>
        public XmlElement DocumentMvmFunctions()
        {
            Dictionary<string, XmlElement> functionsByCategory = new Dictionary<string, XmlElement>();
            XmlDocument doc = new XmlDocument();
            XmlElement functionsElem = doc.CreateElement("functions");
            foreach (var fi in from fi in CallFunction.GetMvmFunctionInfo() orderby fi.Key ascending select fi)
            {
                string functionName = fi.Key;
                MethodInfo functionInfo = fi.Value;

                try
                {
                    // find the csharp doc.
                    XmlComments comments = new XmlComments(functionInfo);
                    XmlNode commentsNode = comments.AllComments;
                    if (commentsNode is XmlDocument)
                    {
                        throw new Exception("Error, you cannot export an mvm function [" + functionInfo + "] without writing csharp documentation on it.");
                    }
                    XmlElement functionElement = functionsElem.CreateElement("function");
                    functionElement.SetAttribute("name", functionName);
                    commentsNode = functionsElem.OwnerDocument.ImportNode(commentsNode, true);

                    XmlElement commentNode = commentsNode.SelectSingleElem("./summary");
                    if (commentNode != null)
                    {
                        commentNode.InnerText = commentNode.InnerText.Trim();
                    }

                    int paramCount = 0;
                    Dictionary<string, XmlElement> paramElems = new Dictionary<string, XmlElement>();
                    foreach (var param in ((XmlElement)commentsNode).SelectElements("./param"))
                    {
                        paramCount++;
                        paramElems[param.GetAttribute("name")] = param;
                    }
                    MethodInfo method = CallFunction.ResolveReflectedFunction(functionName, paramCount);
                    ParameterInfo[] parameters = method.GetParameters();
                    foreach (var paramInfo in parameters.Concat(method.ReturnParameter))
                    {
                        XmlElement param;
                        string paramName = paramInfo.Name;
                        string typeName = paramInfo.ParameterType.Name;
                        if (paramName.IsNullOrEmpty())
                        {
                            if (typeName.Equals("Void"))
                            {
                                continue;
                            }
                            paramName = "[return value]";
                            param = ((XmlElement)commentsNode).SelectSingleElem("./returns");
                            if (param == null)
                            {
                                //throw new Exception("Documentation comments for function " + functionName + " do not mention return value");
                                param = commentsNode.CreateElement("returns");
                                commentsNode.AppendChild(param);
                            }
                            param.SetAttribute("name", paramName);
                            param.SetAttribute("mode", "return");
                        }
                        else
                        {
                            if (!paramElems.ContainsKey(paramName))
                            {
                                //throw new Exception("Documentation comments for function " + functionName + " do not mention parameter " + paramName);
                                param = commentsNode.CreateElement("param");
                                commentsNode.AppendChild(param);
                                param.SetAttribute("name", paramName);
                                paramElems[paramName] = param;
                            }
                            param = paramElems[paramName];
                            if (paramInfo.IsOut)
                            {
                                param.SetAttribute("mode", "out");
                            }
                            else
                            {
                                param.SetAttribute("mode", "in");
                            }
                        }
                        if (paramInfo.DefaultValue != null)
                        {
                            param.SetAttribute("default", paramInfo.DefaultValue.ToString());
                        }
                        if (typeName.Equals("ModuleContext") || paramName.Equals("_named_arguments_w"))
                        {
                            param.SetAttribute("ignore", "1");
                        }
                        else if (paramName.Equals("_named_arguments"))
                        {
                            param.SetAttribute("name", "[named arguments]");
                            param.SetAttribute("datatype", "[named arguments]");
                        }
                        else if (paramName.Equals("_positional_arguments"))
                        {
                            param.SetAttribute("name", "[arguments]");
                            param.SetAttribute("datatype", "[arguments]");
                        }
                        else
                        {
                            param.SetAttribute("datatype", typeName);

                            if (!param.HasAttribute("description") && !param.InnerText.IsNullOrEmpty())
                            {
                                param.SetAttribute("description", param.InnerText);
                            }
                            if (param.GetAttribute("description").IsNullOrEmpty())
                            {
                                //throw new Exception("Function " + functionName + " has no description of parameter " + paramName);
                            }
                        }
                    }

                    XmlElement catElem = commentsNode.SelectSingleElem("//category");
                    string[] cat;
                    if (catElem != null)
                    {
                        cat = catElem.InnerText.Split('/');
                    }
                    else
                    {
                        cat = new string[1] { "Miscellaneous" };
                    }
                    this.functionCategories[functionName] = cat;
                    string catj = cat.Join(',');
                    XmlElement categoryElement;
                    if (functionsByCategory.ContainsKey(catj))
                    {
                        categoryElement = functionsByCategory[catj];
                    }
                    else
                    {
                        categoryElement = functionsElem.CreateElement("category_group");
                        categoryElement.SetAttribute("name", catj);
                        functionsByCategory[catj] = categoryElement;
                    }

                    // tbd: use info from the comments node + reflection info to build a more logical view of the function

                    functionElement.AppendChild(commentsNode);
                    categoryElement.AppendChild(functionElement);
                }
                catch (Exception e)
                {
                    mvm.Log("warning, autodoc failed for function [" + functionName + "]: " + e.Message);
                }
            }
            foreach (var catElem in from kv in functionsByCategory orderby kv.Key ascending select kv.Value)
            {
                functionsElem.AppendChild(catElem);
            }
            return functionsElem;
        }

        public string GetFunctions(string functionName)
        {
            XmlElement functionsElem = DocumentMvmFunctions();
            StringBuilder output = new StringBuilder();
            if (functionName.IsNullOrEmpty())
            {
                PrintByCategory(this.functionCategories, output);
            }
            else
            {
                foreach (var functionElem in functionsElem.SelectElements("//function"))
                {
                    if (functionElem.GetAttribute("name").EqualsIgnoreCase(functionName))
                    {
                        output.AppendLine(functionElem.PrettyString());
                        break;
                    }
                }
            }
            string result = output.ToString().Nvl("Unknown function: [" + functionName + "]");
            return result;
        }

        public XmlElement DocumentMvmModules()
        {
            this.moduleCategories = this.mvm.procLoader.xmlConfigParser.moduleCategories;
            return this.mvm.procLoader.xmlConfigParser.ModulesDocumentation;
        }

        public string GetModules(string moduleName)
        {
            XmlElement modulesElem = DocumentMvmModules();
            StringBuilder output = new StringBuilder();
            if (moduleName.IsNullOrEmpty())
            {
                PrintByCategory(this.moduleCategories, output);
            }
            else
            {
                foreach (XmlElement moduleElem in modulesElem.SelectElements("//element"))
                {
                    if (moduleElem.SelectSingleElem("./module_description") == null)
                    {
                        // This is not the top-level module element
                        continue;
                    }
                    if (moduleElem.GetAttribute("name").EqualsIgnoreCase(moduleName))
                    {
                        // TODO: Write formatted text output rather than the raw xml
                        output.AppendLine(moduleElem.PrettyString());
                        break;
                    }
                }
            }

            string result = output.ToString().Nvl("Unknown module: [" + moduleName + "]");
            return result;
        }

        public void PrintByCategory(Dictionary<string, string[]> catMapping, StringBuilder output)
        {
            var sortedDict = (from entry in catMapping orderby entry.Value.Join("/") ascending, entry.Key ascending select entry).ToDictionary(pair => pair.Key, pair => pair.Value.Join("/"));
            string lastCategory = "";
            foreach (var m in sortedDict)
            {
                if (!lastCategory.Equals(m.Value))
                {
                    output.AppendLine(m.Value + ":");
                    lastCategory = m.Value;
                }
                output.AppendLine("\t" + m.Key);
            }
        }
    }
}
