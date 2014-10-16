using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;

using System.Diagnostics;

namespace MVM
{
    /*
     
    <system_command_select>
    <execute>cmd1</execute>
              :
    <execute>cmd2</execute>
    <return_code>TEMP.return_code</return_code>
    <output>TEMP.std_out_err</output>
    <std_out>TEMP.std_out</std_out>
    <std_err>TEMP.std_err</std_err>
    // you get 1 type of cursor for loop. If you list more then one type you need to do you own cleanup.
    <std_out_cursor>TEMP.std_out_csr</std_out_cursor>
    <std_err_cursor>TEMP.std_err_csr</std_err_cursor>
    <cursor>TEMP.std_out_err_csr</cursor>
    <cursor_value>'value'</cursor_value>
    <cursor_command>'value'</cursor_command>
    <cursor_command_no>'value'</cursor_command_no>
    <loop>
        <print>OBJECT(TEMP.std_out_err)</print>
    </loop>
    </system_command_select>
   */

    class MSystemCommandSelect: IModuleSetup,IModuleRun
    {
        // from xml
        private List<string> commandSyntax;
        private string returnCodeSyntax;
        private string outputSyntax;
        private string stdOutSyntax;
        private string stdErrSyntax;
       // private string cursorSyntax;
        private string cursorValueSyntax;
        private string stdOutCursorSyntax;
        private string stdErrCursorSyntax;
        private string cursorCommandSyntax;
        private string cursorCommandNoSyntax;

        // from setup
        private List<IReadString> commandParsed;
        private IWriteString returnCodeParsed;
        private IWriteString outputParsed;
        private IWriteString stdOutParsed;
        private IWriteString stdErrParsed;
        //private IWriteString cursorParsed;
        private IWriteString stdOutCursorParsed;
        private IWriteString stdErrCursorParsed;
        private IReadString cursorCommandParsed;
        private IReadString cursorCommandNoParsed;
        private IReadString cursorValueParsed;


        private CursorSetupCommon cursorSetup;
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MSystemCommandSelect m = new MSystemCommandSelect();
            m.cursorSetup = new CursorSetupCommon(me, mc);
            m.commandSyntax=me.SelectNodesInnerText("./command");
            m.returnCodeSyntax = me.SelectNodeInnerText("./return_code");
            m.outputSyntax=me.SelectNodeInnerText("./output");
            m.stdOutSyntax = me.SelectNodeInnerText("./std_out");
            m.stdErrSyntax = me.SelectNodeInnerText("./std_err");
            m.stdOutCursorSyntax = me.SelectNodeInnerText("./std_out_cursor");
            m.stdErrCursorSyntax = me.SelectNodeInnerText("./std_err_cursor");
            m.cursorCommandSyntax = me.SelectNodeInnerText("./cursor_command","'command'");
            m.cursorCommandNoSyntax = me.SelectNodeInnerText("./cursor_command_no","'command_no'");
            m.cursorValueSyntax = me.SelectNodeInnerText("./cursor_value", "'value'");
           
            m.commandParsed = mc.ParseSyntax(m.commandSyntax);
            m.returnCodeParsed = mc.ParseWritableSyntax(m.returnCodeSyntax);
            m.outputParsed = mc.ParseWritableSyntax(m.outputSyntax);
            m.stdOutParsed = mc.ParseWritableSyntax(m.stdOutSyntax);
            m.stdErrParsed = mc.ParseWritableSyntax(m.stdErrSyntax);
            m.stdOutCursorParsed = mc.ParseWritableSyntax(m.stdOutCursorSyntax);
            m.stdErrCursorParsed = mc.ParseWritableSyntax(m.stdErrCursorSyntax);
            m.cursorCommandParsed = mc.ParseSyntax(m.cursorCommandSyntax);
            m.cursorCommandNoParsed = mc.ParseSyntax(m.cursorCommandNoSyntax);
            m.cursorValueParsed = mc.ParseSyntax(m.cursorValueSyntax);

            run.Add(m);
            m.cursorSetup.AddCursorSubProcs(me, mc, run);
        }

        public void Run(ModuleContext mc)
        {
            
            string cursorCommand = this.cursorCommandParsed.Read(mc);
            string cursorCommandNo = this.cursorCommandNoParsed.Read(mc);
            string cursorValue = this.cursorValueParsed.Read(mc);
            var csr = new DosCommandCursor(mc, this.cursorSetup, cursorValue,cursorCommand, cursorCommandNo, this.commandParsed);
        }
    }

    // runs commands and gives you the output
    // writes to csr field: value(the outputline), command, command_no
    // does not yet support std error
    // optionally redirects stdErr to stdOut
    public class DosCommandCursor : CursorCommonLinqEnabled, ICursor
    {
        public CmdConsole cmdConsole;
        public string csrOid;
        public string cursorValue;
        public string cursorCommand;
        public string cursorCommandNo;
        public List<IReadString> executeCmdsParsed;
        public List<string> executeCmds=new List<string>();
        public int commandNo = -1;
        public IEnumerator<string> enumerator;
        public DosCommandCursor(ModuleContext mc, CursorSetupCommon setup, string cursorValue,string cursorCommand, string cursorCommandNo, List<IReadString> executeCmds)
            : base(mc, setup)
        {
            this.cmdConsole = new CmdConsole();
            this.cursorValue = cursorValue;
            this.cursorCommand = cursorCommand;
            this.cursorCommandNo = cursorCommandNo;
            this.executeCmdsParsed = executeCmds;
            this.orderedFieldNames = new List<string>();
            this.orderedFieldNames.Add(cursorValue);
            this.orderedFieldNames.Add(cursorCommand);
            this.orderedFieldNames.Add(cursorCommandNo);
            foreach (var cmd in this.executeCmdsParsed) this.executeCmds.Add(cmd.Read(mc));
        }

        public override IObjectData CursorNext()
        {
           
                // if more output, just return that
                if (this.enumerator != null && this.enumerator.MoveNext())
                {
                    using (var csrObj = this.CreateNewObject())
                    {
                        csrObj[this.cursorValue] = this.enumerator.Current;
                        return csrObj;
                    }
                }
                // if we have more commands, advance to next command
                if (this.commandNo < (this.executeCmds.Count - 1))
                {
                    using (var csrObj = this.CreateNewObject())
                    {
                        commandNo++;
                        string command = this.executeCmds[commandNo];
                        csrObj[this.cursorCommand] = command;
                        this.enumerator = this.cmdConsole.Execute(command).GetEnumerator();
                        csrObj[this.cursorCommandNo] = commandNo.ToString();
                        if (this.enumerator.MoveNext() && this.enumerator.Current!=null)
                        {
                            csrObj[this.cursorValue] = this.enumerator.Current;
                        }
                        else
                        {
                            csrObj[this.cursorValue] = "";
                        }
                        return csrObj;
                    }
                }
                // no more commands or output so set eof
                return null;
        }

        public override void CursorClear()
        {
            this.cmdConsole.Close();
        }
    }
}
