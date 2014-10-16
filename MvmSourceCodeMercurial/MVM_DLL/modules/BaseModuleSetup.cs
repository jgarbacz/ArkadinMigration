using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    // Base class for all modules that want easy access to ModuleAttribute decoration
    public abstract class BaseModuleSetup : IModuleSetup
    {
        public ModuleAttribute moduleAttribute = null;
        public List<string> preModules = null;
        public List<string> postModules = null;
        public Dictionary<string, bool> objectIds = null;

        public BaseModuleSetup()
        {
            Type t = this.GetType();
            ModuleAttribute.moduleAttributeLookup.TryGetValue(t.Name, out moduleAttribute);
            if (moduleAttribute == null)
            {
                foreach (var attr in t.GetCustomAttributes(true))
                {
                    moduleAttribute = attr as ModuleAttribute;
                }
                ModuleAttribute.moduleAttributeLookup[t.Name] = moduleAttribute;
            }
        }


        public void SetupWriteString(XmlElement me, ModuleContext mc, string name, out string syntax, out IWriteString parsed)
        {
            syntax = this.SelectSingleNode(me, name);
            parsed = mc.ParseWritableSyntax(syntax);
        }

        public void SetupWriteString(XmlElement me, ModuleContext mc, out string syntax, out IWriteString parsed)
        {
            syntax = me.InnerText;
            parsed = mc.ParseWritableSyntax(syntax);
        }

        public void SetupReadString(XmlElement me, ModuleContext mc, string name, out string syntax, out IReadString parsed)
        {
            syntax = this.SelectSingleNode(me, name);
            parsed = mc.ParseSyntax(syntax);
        }

        public void SetupReadString(XmlElement me, ModuleContext mc, out string syntax, out IReadString parsed)
        {
            syntax = me.InnerText;
            parsed = mc.ParseSyntax(syntax);
        }

        public void SetupReadStringLiteral(XmlElement me, ModuleContext mc, string name, out string syntax, out string parsed)
        {
            syntax = this.SelectSingleNode(me, name);
            if (!mc.IsLiteralString(syntax))
            {
                throw new Exception("Error, expecting string literal for parameter " + name + ", not [" + syntax + "]");
            }
            parsed = mc.GetLiteralString(syntax);
        }



        public void SetupReadString(XmlElement me, ModuleContext mc, string name, out List<string> syntaxList, out List<IReadString> parsedList)
        {
            syntaxList = new List<string>();
            parsedList = new List<IReadString>();
            foreach(var x in this.SelectMultipleNodes(me,"name")){
                var syntax = this.SelectSingleNode(me, name);
                var parsed = mc.ParseSyntax(syntax);
                syntaxList.Add(syntax);
                parsedList.Add(parsed);
            }
        }

        public string SelectSingleNode(XmlElement me, string name)
        {
            if (name.StartsWith("./"))
            {
                name = name.Substring(2);
            }
            string value = me.SelectNodeInnerText(name);
            if (moduleAttribute != null)
            {
                string def = moduleAttribute.defaults.GetValueOrNull(name);
                if (!def.IsNullOrEmpty())
                {
                    value = me.SelectNodeInnerText(name, def);
                }
                string type = moduleAttribute.types.GetValueOrNull(name);
                if (!type.IsNullOrEmpty() && type.Equals("object_id"))
                {
                    if (objectIds == null)
                    {
                        objectIds = new Dictionary<string, bool>();
                    }
                    objectIds[value] = true;
                }
            }
            return value;
        }

        public List<string> SelectMultipleNodes(XmlElement me, string name)
        {
            return me.SelectNodesInnerText(name);
        }

        //public IReadString ParseSyntax(ModuleContext mc, string syntax)
        //{
        //    if (moduleAttribute != null)
        //    {
        //        string type = moduleAttribute.types.GetValueOrNull(syntax);
        //        if (!type.IsNullOrEmpty() && type.Equals("object_id"))
        //        {
        //            if (preModules == null)
        //            {
        //                preModules = new List<string>();
        //            }
        //            string temp = mc.GetGenSym("spawn");
        //            StringBuilder spawn = new StringBuilder();
        //            spawn.AppendLine("<spawn>");
        //            spawn.AppendLine("  <object_type>'TEST'</object_type>");
        //            spawn.AppendLine("  <object_id>TEMP." + temp + "</object_type>");
        //            spawn.AppendLine("</spawn>");
        //            preModules.Add(spawn.ToString());
        //            return mc.ParseSyntax("TEMP." + temp);
        //        }
        //    }
        //    return mc.ParseSyntax(syntax);
        //}

        //public IWriteString ParseWritableSyntax(ModuleContext mc, string name)
        //{
        //    if (moduleAttribute != null)
        //    {
        //        string mode = moduleAttribute.modes.GetValueOrNull(name);
        //        if (!mode.IsNullOrEmpty() && mode.Contains("out"))
        //        {
        //            return me.SelectNodeInnerText(name, def);
        //        }
        //    }
        //    return null;
        //}

        //public void Add(List<IModuleRun> run)
        //{
        //}

        #region IModuleSetup Members

        public abstract void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run);

        #endregion
    }
}
