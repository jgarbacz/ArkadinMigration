using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.IO;

namespace MVM
{
    public static class MyXml
    {
        public static XmlNode AppendChildImport(this XmlNode thisXmlNode, XmlElement thatXmlNode)
        {
            if (thisXmlNode.OwnerDocument.Equals(thatXmlNode.OwnerDocument))
            {
                return thisXmlNode.AppendChild(thatXmlNode);
            }
            else
            {
                var copy = thisXmlNode.OwnerDocument.ImportNode(thatXmlNode, true);
                return thisXmlNode.AppendChild(copy);
            }
        }
        public static XmlNode PrependChildImport(this XmlNode thisXmlNode, XmlElement thatXmlNode)
        {
            if (thisXmlNode.OwnerDocument.Equals(thatXmlNode.OwnerDocument))
            {
                return thisXmlNode.PrependChild(thatXmlNode);
            }
            else
            {
                var copy = thisXmlNode.OwnerDocument.ImportNode(thatXmlNode, true);
                return thisXmlNode.PrependChild(copy);
            }
        }

        public static List<XmlElement> GetElementsByTagNameSelfIncluded(this XmlElement xmlElement, string tagName)
        {
            List<XmlElement> output = new List<XmlElement>();
            if (xmlElement.LocalName.Equals(tagName)) output.Add(xmlElement);
            foreach (XmlElement n in xmlElement.GetElementsByTagName(tagName)) output.Add(n);
            return output;
        }

        public static Dictionary<string, string> GetAttributeDictionary(this XmlElement xmlElement)
        {
            Dictionary<string, string> attr = new Dictionary<string, string>();
            foreach (XmlAttribute a in xmlElement.Attributes)
            {
                attr[a.Name] = a.Value;
            }
            return attr;
        }

        public static Dictionary<string, string> GetChildElementDictionary(this XmlElement xmlElement)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (XmlElement a in xmlElement.GetChildElems())
            {
                dic[a.LocalName] = a.InnerText;
            }
            return dic;
        }

        public static XmlElement CloneButChangeTagName(this XmlElement xmlElement, string tagName, bool deep)
        {
            XmlElement output = xmlElement.CreateElement(tagName);
            foreach (XmlNode n in xmlElement.ChildNodes)
            {
                output.AppendChild(n.CloneNode(true));
            }
            foreach (XmlAttribute a in xmlElement.Attributes)
            {
                output.SetAttribute(a.Name, a.Value);
            }
            return output;
        }
        /// <summary>
        /// Returns true if the xmlElement has mixed content
        /// </summary>
        /// <param name="xmlElement"></param>
        /// <returns></returns>
        public static bool HasMixedContent(this XmlElement xmlElement)
        {
            return xmlElement.HasChildXmlText() && xmlElement.HasChildElements();
        }
        /// <summary>
        /// Returns true if the xmlElement has any child xml elements.
        /// </summary>
        /// <param name="xmlElement"></param>
        /// <returns></returns>
        public static bool HasChildElements(this XmlElement xmlElement)
        {
            return xmlElement.GetChildElems().Count != 0;
        }
        /// <summary>
        /// Returns true if the xmlElement has atleast one direct child element with the specified tag.
        /// </summary>
        /// <param name="xmlElement"></param>
        /// <returns></returns>
        public static bool HasChildElement(this XmlElement xmlElement, string tagName)
        {
            return xmlElement.GetChildElems().Where(e => e.LocalName.Equals(tagName)).Any();
        }

        /// <summary>
        /// Returns true if the xmlElement has any child xml text.
        /// </summary>
        /// <param name="xmlElement"></param>
        /// <returns></returns>
        public static bool HasChildXmlText(this XmlElement xmlElement)
        {
            return xmlElement.GetChildXmlText().Take(1).Count() > 0;
        }

        public static void AppendTextElement(this XmlElement xmlElement, string tag, string text)
        {
            var textElem = xmlElement.CreateTextElement(tag, text);
            xmlElement.AppendChild(textElem);
        }
        public static XmlElement CreateTextElement(this XmlNode xmlNode, string tag, string text)
        {
            var elem = xmlNode.CreateElement(tag);
            var textNode = xmlNode.CreateTextNode(text);
            elem.AppendChild(textNode);
            return elem;
        }
        public static XmlElement CreateElement(this XmlNode xmlNode, string tag)
        {
            var doc = xmlNode.OwnerDocument;
            var elem = doc.CreateElement(tag);
            return elem;
        }
        public static XmlText CreateTextNode(this XmlNode xmlNode, string text)
        {
            var doc = xmlNode.OwnerDocument;
            var node = doc.CreateTextNode(text);
            return node;
        }
        public static string GetAttributeDefaulted(this XmlElement xmlElement, string attribute, string defaultValue)
        {
            if (xmlElement.HasAttribute(attribute)) return xmlElement.GetAttribute(attribute);
            return defaultValue;
        }

        /// <summary>
        /// Expects a node list of element with attribute name and some inner text value. Returns a dictionary
        /// that maps the name to the inner text.
        /// </summary>
        /// <param name="nodeList"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ToNameTextDictionary(this XmlNodeList elementNodeList)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();
            if (elementNodeList == null) return output;
            foreach (XmlElement n in elementNodeList)
            {
                string name = n.GetAttribute("name");
                string text = n.InnerText;
                output[name] = text;
            }
            return output;
        }

        /// <summary>
        /// Returns a list of string with the results of selecting the innerText from all the nodes in the xpath.
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public static List<string> SelectNodesInnerText(this XmlNode xmlNode, string xpath)
        {
            List<string> output = new List<string>();
            foreach (XmlNode n in xmlNode.SelectNodes(xpath))
            {
                output.Add(n.InnerText);
            }
            return output;
        }

        /// <summary>
        /// Selects node and returns innerText or defaultValue if not found
        /// </summary>
        /// <param name="xmlElement"></param>
        /// <param name="xpath"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string SelectNodeInnerText(this XmlElement xmlElement, string xpath, string defaultValue)
        {
            string output = SelectNodeInnerText(xmlElement, xpath);
            if (output == null) return defaultValue;
            return output;
        }

        public static string GetAttributeDefault(this XmlElement xmlElement, string attribute, string defaultValue)
        {
            if (xmlElement.HasAttribute(attribute)) return xmlElement.GetAttribute(attribute);
            return defaultValue;
        }

        /// <summary>
        /// Selects node and returns innerText or null if not found
        /// </summary>
        /// <param name="xmlElement"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public static List<XmlElement> SelectElements(this XmlElement xmlElement, string xpath)
        {
            List<XmlElement> result = new List<XmlElement>();
            if (xmlElement == null) return result;
            foreach (XmlElement elem in xmlElement.SelectNodes(xpath)) result.Add(elem);
            return result;
        }

        public static List<XmlElement> SelectElementsNS(this XmlElement xmlElement, string xpath, XmlNamespaceManager nsmgr)
        {
            List<XmlElement> result = new List<XmlElement>();
            if (xmlElement == null) return result;
            foreach (XmlElement elem in xmlElement.SelectNodes(xpath, nsmgr)) result.Add(elem);
            return result;
        }

        /// <summary>
        /// Selects node and returns innerText or null if not found
        /// </summary>
        /// <param name="xmlElement"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public static string SelectNodeInnerText(this XmlElement xmlElement, string xpath)
        {
            XmlNode n = xmlElement.SelectSingleNode(xpath);
            if (n == null) return null;
            return n.InnerText;
        }
        /// <summary>
        /// Returns the first child XmlElement of the passed node or null.
        /// </summary>
        /// <param name="XmlNode"></param>
        /// <returns></returns>
        public static XmlElement FirstChildElem(this XmlElement XmlNode)
        {
            XmlNode n = XmlNode.FirstChild;
            while (n != null)
            {
                if (n.NodeType.Equals(XmlNodeType.Element)) return (XmlElement)n;
                n = n.NextSibling;
            }
            return null;
        }
        /// <summary>
        /// Returns the next sibling XmlElement of the passed node or null.
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        public static XmlElement NextSiblingElem(this XmlNode xmlNode)
        {
            xmlNode = xmlNode.NextSibling;
            while (xmlNode != null)
            {
                if (xmlNode.NodeType.Equals(XmlNodeType.Element)) return (XmlElement)xmlNode;
                xmlNode = xmlNode.NextSibling;
            }
            return null;
        }
        /// <summary>
        /// Selects and returns the first instance of the specified element as an XmlElement
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public static XmlElement SelectSingleElem(this XmlNode xmlNode, string xpath)
        {
            return (XmlElement)xmlNode.SelectSingleNode(xpath);
        }
        public static XmlElement SelectSingleElemNS(this XmlNode xmlNode, string xpath, XmlNamespaceManager nsmgr)
        {
            return (XmlElement)xmlNode.SelectSingleNode(xpath, nsmgr);
        }

        /// <summary>
        /// Returns list of child XmlElement of current node
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        public static List<XmlElement> GetChildElems(this XmlNode xmlNode)
        {
            List<XmlElement> output = new List<XmlElement>();
            foreach (XmlNode n in xmlNode.ChildNodes)
            {
                if (n.IsElement())
                {
                    output.Add((XmlElement)n);
                }
            }
            return output;
        }
        /// <summary>
        /// Returns list of child XmlElement of current node
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        public static IEnumerable<XmlText> GetChildXmlText(this XmlNode xmlNode)
        {
            foreach (XmlNode n in xmlNode.ChildNodes)
            {
                if (n.IsText())
                    yield return (XmlText)n;
            }
        }


        /// <summary>
        /// Returns true if current not is of XmlElement type
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        public static bool IsElement(this XmlNode xmlNode)
        {
            return xmlNode.NodeType == XmlNodeType.Element;
        }
        /// <summary>
        /// Returns true if current not is of Text type
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        public static bool IsText(this XmlNode xmlNode)
        {
            return xmlNode.NodeType == XmlNodeType.Text;
        }

        /// <summary>
        /// Formats and prints the element to a file
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        public static void PrettyPrint(this XmlElement elem, string fileName)
        {
            string p = elem.PrettyString();
            string pp = XmlConfigParser.LineInfoRegex.Replace(p, "");
            if (fileName == null)
            {
                pp.ToString().WriteToConsole();
            }
            else
            {
                pp.ToString().WriteToFile(fileName);
            }
        }

        /// <summary>
        /// Formats and return the xml as a string
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        public static string PrettyString(this XmlElement elem)
        {
            using (StringWriter swriter = new StringWriter())
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreWhitespace = true;
                XmlReader reader = XmlReader.Create(new XmlNodeReader(elem), settings);
                XmlTextWriter writer = new XmlTextWriter(swriter);
                writer.Formatting = Formatting.Indented;
                writer.Indentation = 2;
                writer.IndentChar = ' ';
                writer.WriteNode(reader, true);
                return swriter.ToString();
            }
        }

        // parses an xml file (no validation) and returns the document
        public static XmlDocument ParseXmlFile(string xmlFileName)
        {
            XmlTextReader tr = new XmlTextReader(xmlFileName);
            XmlReaderSettings settings = new XmlReaderSettings();
            XmlReader vr = XmlReader.Create(tr, settings);
            XmlDocument doc = new XmlDocument();

            try
            {
                doc.Load(vr);
            }
            catch (XmlException e)
            {
                string message = "Cannot parse xml file [" + xmlFileName + "]. Error on line=" + e.LineNumber + ", pos=" + e.LinePosition;
                throw new Exception(message, e);
            }
            catch (Exception e)
            {
                string message = "Cannot parse xml file [" + xmlFileName + "].";
                throw new Exception(message, e);
            }
            finally
            {
                tr.Close();
            }
            return doc;
        }

        public static XmlElement AppendChildElement(this XmlElement elem, XmlElement childXmlElement)
        {
            return (XmlElement)elem.AppendChildImport(childXmlElement);
        }

        public static XmlElement ParseXmlStringGetElement(string xmlString)
        {
            return ParseXmlString(xmlString).DocumentElement;
        }

        public static XmlDocument ParseXmlString(string xmlString)
        {
            StringReader stringReader = new StringReader(xmlString);
            XmlReaderSettings settings = new XmlReaderSettings();
            XmlReader xr = XmlReader.Create(stringReader, settings);
            XmlDocument doc = new XmlDocument();

            try
            {
                doc.Load(xr);
            }
            catch (XmlException e)
            {
                string message = "Cannot parse xml string [" + xmlString + "]. Error on line=" + e.LineNumber + ", pos=" + e.LinePosition;
                throw new Exception(message, e);

            }
            catch (Exception e)
            {
                string message = "Cannot parse xml string [" + xmlString + "].";
                throw new Exception(message, e);
            }
            return doc;
        }

        public static List<XmlNode> ToList(this XmlNodeList nodeList)
        {
            List<XmlNode> output = new List<XmlNode>();
            if (nodeList == null) return output;
            foreach (XmlNode n in nodeList)
            {
                output.Add(n);
            }
            return output;
        }
    }
}
