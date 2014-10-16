using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using System.Xml;
using System.IO;


using Antlr.Runtime.Tree;

namespace MVM
{
    /*
     
      uncatches the exception OR sets the exception info
        <get_exception_info>
        <catch_name>'myexception'</catch_name>
        <exception_name>TEMP.exception_name</exception_name>
        <exception_message>TEMP.exception_message</exception_message>
        <exception_trace>TEMP.exception_trace</exception_trace>
        </get_exception_info>
     * 
     * This newModule is not a user newModule it is leveraged through try/catch. It should not be called by the end user.
  */

    class MGetExceptionInfo : IModuleSetup, IModuleRun
    {
        private string catchNameSyntax;
        private string exceptionNameSyntax;
        private string exceptionMessageSyntax;
        private string exceptionTraceSyntax;
        private IReadString catchNameParsed;
        private IWriteString exceptionNameParsed;
        private IWriteString exceptionMessageParsed;
        private IWriteString exceptionTraceParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MGetExceptionInfo m = new MGetExceptionInfo();
            m.catchNameSyntax = me.SelectNodeInnerText("./catch_name","''");
            m.catchNameParsed = mc.ParseSyntax(m.catchNameSyntax);
            m.exceptionNameSyntax = me.SelectNodeInnerText("./exception_name");
            m.exceptionNameParsed = mc.ParseWritableSyntax(m.exceptionNameSyntax);
            m.exceptionMessageSyntax = me.SelectNodeInnerText("./exception_message");
            m.exceptionMessageParsed = mc.ParseWritableSyntax(m.exceptionMessageSyntax);
            m.exceptionTraceSyntax = me.SelectNodeInnerText("./exception_trace");
            m.exceptionTraceParsed = mc.ParseWritableSyntax(m.exceptionTraceSyntax);
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string catchName = this.catchNameParsed.Read(mc);
            string exceptionName = mc.GetCaughtExceptionName();
            if (catchName.Equals("") || catchName.EqualsIgnoreCase(exceptionName))
            {
                string exceptionTrace = mc.GetCaughtExceptionStackTrace();
                string exceptionMessage = mc.GetCaughtExceptionMessage();
                this.exceptionNameParsed.Write(mc, exceptionName);
                this.exceptionMessageParsed.Write(mc, exceptionMessage);
                this.exceptionTraceParsed.Write(mc, exceptionTrace);
            }
            else
            {
                mc.exception = mc.caughtException;
                mc.caughtException = null;
            }
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("get_exception_info:");
        }
    }
}
