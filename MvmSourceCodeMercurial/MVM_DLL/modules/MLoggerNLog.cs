using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.Diagnostics;
using System.Data;

using NLog;

namespace MVM
{
    class MLoggerNLog: IModuleRun
    {
        private string msgSyntax;
        private IReadString msgParsed;
        private LogLevel msgLevel;
        private string msgLiteral; // if this is set, it just prints this literal value and does not eval the syntax
        private string localName;
        private Logger nlogger;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run, string loggerOid, string msgSyntax, string msgLiteral, LogLevel msgLevel)
        {
            MLoggerNLog m = new MLoggerNLog();
            string nameSpace=mc.NameSpace;
            string entryProc = "XML." + nameSpace + ".MyProc";
            m.nlogger = LogManager.GetLogger(entryProc);
            m.msgSyntax = msgSyntax;
            m.msgLiteral = msgLiteral;
            if(m.msgLiteral==null) m.msgParsed = mc.ParseSyntax(msgSyntax);
            m.msgLevel = msgLevel;

            m.localName = mc.LocalName;

            // Trim the local name.  Here's an example of 
            // a local name that will be trimmed to the slash:
            // print_object/try_config[ModuleOrder(1,0,3,1,0,1)]

            int indexOfSlash = m.localName.IndexOf('/');
            if (indexOfSlash != -1)
            {
                m.localName = m.localName.Substring(0, indexOfSlash);
            }

            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string logMessage = "";
            if (this.msgLiteral != null)
            {
                logMessage = msgLiteral;
            }
            else
            {
                logMessage = msgParsed.Read(mc);
            }
            this.nlogger.Log(this.msgLevel, "[" + localName + "] " + logMessage);
            if (this.msgLevel == LogLevel.Fatal)
            {
                throw new Exception(logMessage);
            }
        }
        }
    }
