using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MVM
{
    /// <summary>
    /// Defines the type of procContext 
    /// </summary>
    public enum ProcType
    {
        Standard,
        Catch,
        Finally
    }

    /// <summary>
    /// This stores all of the static information we have about a proc. This has nothing to do with
    /// the parsing of its newModules. This information is static and does not change. ProcDefinitions
    /// are responsible for storing compiled information and it changes as the procContext is altered at
    /// runtime. ProcInst (soon to be named ProcInst stores the runtime instance of a procContext call).
    /// </summary>
    public class ProcInfo
    {
        public readonly SchedulerMaster schedulerMaster;
        public readonly int procId;
        public readonly string nameSpace;
        public readonly string localName;
        public readonly string procName;
        public readonly XmlElement procElem;
        public readonly string structuralNameSpace; 
        public readonly IConfigLocator location;
        public readonly List<XmlElement> paramElems;
        public readonly ProcType procType;
        public readonly bool isEntryPoint;
        public readonly MvmEngine mvm;
        public readonly XmlElement procdoc = null;
        public readonly string[] categories = { };
        public readonly string description = null;
        public ProcInfo(SchedulerMaster schedulerMaster, string nameSpace, string localName, XmlElement procElem, string structuralNameSpace, IConfigLocator location)
        {
            this.schedulerMaster = schedulerMaster;
            this.mvm = this.schedulerMaster.mvm;
            this.procId = this.schedulerMaster.GetNextProcId(this);
            this.nameSpace = nameSpace;
            if (this.nameSpace.IsNullOrEmpty()) throw new Exception("Internal error in ProcInfo for localName=[" + localName + "] namespace must not be null or ''");
            this.localName = localName;
            this.procName = this.nameSpace + "." + this.localName;
            this.procElem = procElem;
            this.structuralNameSpace = structuralNameSpace;
            this.location = location;
            this.paramElems = procElem.SelectElements("./param");
            if (this.procElem.GetAttribute("work_type").Equals("catch"))
                this.procType = ProcType.Catch;
            else if (this.procElem.GetAttribute("work_type").Equals("finally"))
                this.procType = ProcType.Finally;
            else this.procType = ProcType.Standard;

            // Look for proc metadata in the prior <proc_info>
            if (this.procElem.PreviousSibling != null && this.procElem.PreviousSibling.LocalName.Equals("proc_info"))
            {
                this.procdoc = (XmlElement)this.procElem.PreviousSibling;
                XmlElement cat = this.procdoc.SelectSingleElem("./category");
                if (cat != null)
                {
                    this.categories = cat.InnerText.Split('/');
                    if (this.categories.Length > 2)
                    {
                        throw new Exception("Cannot have more than two categories for proc " + this.procName);
                    }
                }
                else
                {
                    this.categories = new string[1] { "Miscellaneous" };
                }
                XmlElement desc = this.procdoc.SelectSingleElem("./description");
                if (desc != null)
                {
                    this.description = desc.InnerText;
                }
            }

            // procs with procContext childElem procContext that aren't explicitly not entry points get IsEntryPoint=true
            // procs that explicity with is_entry=true are get IsEntryPoint=true
            if (this.procElem.LocalName.Equals("proc") && !(this.procElem.GetAttribute("is_entry").Equals("false")))
            {
                this.isEntryPoint = true;
            }
            else if (this.procElem.GetAttribute("is_entry").Equals("true"))
            {
                this.isEntryPoint = true;
            }
            else
            {
                this.isEntryPoint = false;
            }
        }

        // searches up for the extensions/XXX dir this is under
        public string GetExtensionDir()
        {
            string procPath = this.location.GetLocation();
            string output = MvmEngine.GetExtensionDir(procPath);
            return output;

        }

        public bool HasReturnValue
        {
            get
            {
                return this.paramElems
                .Where(p => p.GetAttribute("name").Equals("return_value") && p.GetAttribute("mode").Equals("out")).Any();
            }
        }
         public bool IsPipeRow
        {
            get
            {
                return paramElems.Where(x => x.GetAttribute("name").Equals("pipe_cursor")).Count() == 1;
            }
        }
    }
}
