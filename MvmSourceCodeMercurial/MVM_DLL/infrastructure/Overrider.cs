using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;

/*
 * Given a field name, this class will output the syntax we should use to get the value.
 * It supports named, pattern, object_id overrides, first hit wins.
 *
 *  Named override:
 *  <field name="my_date">'20090101000000'</field>
 *  
 *  Pattern override:
 *  <regex pattern="^(my_)(.*)$">(TEMP.$2 ne '' ? TEMP.$2 : OBJECT.$2)</regex>
 *  
 *  Object_id override:
 *  <object_id>TEMP.oid</object_id> 
 */
namespace MVM
{
    public class Overrider
    {
        List<IOverride> overrides = new List<IOverride>();

        public Overrider(XmlElement parentXmlElement)
        {
            foreach (XmlElement elem in parentXmlElement.SelectElements("./field|param|regex|object_id"))
            {
                if (elem.LocalName.In("field", "param")) this.overrides.Add(new NamedOverride(elem.GetAttribute("name"), elem.InnerText));
                else if (elem.LocalName.Equals("regex")) this.overrides.Add(new PatternOverride(elem.GetAttribute("pattern"), elem.InnerText));
                else if (elem.LocalName.Equals("object_id")) this.overrides.Add(new ObjectIdOverride(elem.InnerText));
                else throw new Exception("unexpected element:" + elem.LocalName);
            }
        }

        public string GetSyntax(string fieldName)
        {
            foreach (IOverride iOverride in this.overrides)
            {
                string result = iOverride.GetSyntax(fieldName);
                if (result != null) return result;
            }
            return "OBJECT." + fieldName;
        }

        interface IOverride
        {
            string GetSyntax(string fieldName);
        }

        class NamedOverride : IOverride
        {
            readonly string name;
            readonly string value;
            public NamedOverride(string name, string value)
            {
                this.name = name;
                this.value = value;
            }
            public string GetSyntax(string fieldName)
            {
                if (name.Equals(fieldName)) return value;
                return null;
            }
        }
        class ObjectIdOverride : IOverride
        {
            readonly string objectId;

            public ObjectIdOverride(string objectId)
            {
                this.objectId = objectId;
            }
            public string GetSyntax(string fieldName)
            {
                return "OBJECT(" + this.objectId + ")." + fieldName;
            }
        }
        class PatternOverride : IOverride
        {
            readonly string pattern;
            readonly string replacement;
            readonly Regex regex;
            public PatternOverride(string pattern, string replacement)
            {
                this.pattern = pattern;
                this.replacement = replacement;
                this.regex = new Regex(pattern);
            }
            public string GetSyntax(string fieldName)
            {
                if (regex.IsMatch(fieldName))
                {
                    return regex.Replace(fieldName, replacement);
                }
                return null;
            }
        }
    }
}
