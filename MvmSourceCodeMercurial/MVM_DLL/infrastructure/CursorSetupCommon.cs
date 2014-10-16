using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using NLog;
namespace MVM
{
    /**
      * Provides setup functionality for cursors
      * cursor
      * cursor_value
      * cursor_delete - next|clear
      * cursor_type - standard|erd
     * 
     * 
     * we know the usage cursor is going to give a mix of global and external objects.
     * global ones better not get removed. exteral ones will get removed. for now i can
     * put them all in global.
     * 
     * there is also, global where the object is already in global... and global where an object
     * is being generated into global...
     * 
     */


    public class CursorSetupCommon : ICursorSetupCommon
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        public static string[] pipelineFuncs = new string[] { "order_by", "where", "group_by", "distinct", "top", "last", "execute" ,"pipe_row"};
        public static string[] cursorFuncs = new string[] { "loop", "run", "parallel", "then", "else" };

        
        //public string cursorSyntaxDefaultSyntax { get; set; } 
        public string cursorSyntax { get; set; }
        public IWriteString writeCursorParsed { get; set; }
        public IReadString readCursorParsed { get; set; }
        public string GetCursorOid(ModuleContext mc) { 
            return this.readCursorParsed.Read(mc); 
        }
        public void SetCursorOid(ModuleContext mc,string cursorOid) { 
            this.writeCursorParsed.Write(mc,cursorOid); 
        }


        public string cursorValueSyntax { get; set; }
        public IReadString cursorValueParsed { get; set; }
        public string cursorValueDefault { get; set; }
        public string GetCursorValue(ModuleContext mc) {
            if(this.cursorValueParsed!=null) return this.cursorValueParsed.Read(mc);
            return this.cursorValueDefault;
        }

        public string cursorDeleteSyntax { get; set; }
        public IReadString cursorDeleteParsed { get; set; }
        public string cursorDeleteDefault { get; set; } // clear' or 'never'
        public string GetCursorDelete(ModuleContext mc)
        {
            if (this.cursorDeleteParsed != null) return this.cursorDeleteParsed.Read(mc);
            return this.cursorDeleteDefault;
        }

        public string cursorTypeSyntax { get; set; }
        public IReadString cursorTypeParsed { get; set; }
        public string cursorTypeDefault { get; set; } // or  'erd'
        public string GetCursorType(ModuleContext mc) {
            if (this.cursorTypeParsed != null) return this.cursorTypeParsed.Read(mc);
            return this.cursorTypeDefault;
        }

        public string cursorTypeFeedbackNameSyntax { get; set; }
        public IReadString cursorTypeFeedbackNameParsed { get; set; }
        public string cursorTypeFeedbackNameDefault { get; set; }
        public string GetCursorTypeFeedbackName(ModuleContext mc) {
            if (this.cursorTypeFeedbackNameParsed != null) return this.cursorTypeFeedbackNameParsed.Read(mc);
            return this.cursorTypeFeedbackNameDefault;
        }

        public string cursorInstIdSyntax{get;set;}
        public IReadString readCursorInstIdParsed { get; set; }
        public IWriteString writeCursorInstIdParsed { get; set; }
        public string GetCursorInstId(ModuleContext mc)
        {
            if (this.readCursorInstIdParsed != null) return this.readCursorInstIdParsed.Read(mc);
            return null;
        }
        public void SetCursorInstId(ModuleContext mc,string cursorInstId)
        {
            if (this.writeCursorInstIdParsed != null) 
                this.writeCursorInstIdParsed.Write(mc,cursorInstId);
        }
        public bool needsPipeline =false;
        public bool NeedsHeaderObject { get; set; }
        public string isEofSyntax;


        public bool noReturn = false;
        public CursorSetupCommon(XmlElement me, ModuleContext mc)
        {
            // if this module is not <select> and it has any pipelined functions
            // then we want to rewrite it as <select>
            this.needsPipeline= 
                !me.LocalName.Equals("select")
                &&
                me.LocalName.EndsWith("_select")
                &&
                me.GetChildElems().Where(e => e.LocalName.In(pipelineFuncs)).Any();

            // if this needs to be pipelined, then give make it use a cursor inst id and do 
            // not let it use the passed cursor object as this belongs to the last operator 
            // in the pipeline.
            if (needsPipeline)
            {
                this.cursorInstIdSyntax = "TEMP." + mc.GetGenSym(me.LocalName + "_is_pipelined");
                this.cursorSyntax = null;
            }
            // otherwise, respect the passed instance id and/or cursor object id.
            else
            {
                // use the one that is there
                this.cursorInstIdSyntax = me.SelectNodeInnerText("cursor_inst_id");
                if (this.cursorInstIdSyntax.IsNullOrEmpty())
                {
                    this.cursorInstIdSyntax = "TEMP." + mc.GetGenSym("IterGenCsrInstId");
                }
                this.cursorSyntax = me.SelectNodeInnerText("./cursor");
                // if they want to work with the cursor object id and this is
                // not pipelined, then we need a header object.
                if (this.cursorSyntax.NotNullOrEmpty())
                {
                    NeedsHeaderObject = true;
                }
            }
            //logger.Info(me.LocalName + " using cursor_inst_id syntax =" + this.cursorInstIdSyntax);

            // generate a temp var to capture is_eof
            this.isEofSyntax = "TEMP." + mc.GetGenSym("is_eof");

            this.writeCursorParsed = mc.ParseWritableSyntax(this.cursorSyntax);
            this.readCursorParsed = mc.ParseSyntax(this.cursorSyntax);
           
            this.readCursorInstIdParsed = mc.ParseSyntax(this.cursorInstIdSyntax);
            this.writeCursorInstIdParsed = mc.ParseWritableSyntax(this.cursorInstIdSyntax);

            this.cursorValueSyntax = me.SelectNodeInnerText("./cursor_value");
            this.cursorValueParsed = mc.ParseSyntax(this.cursorValueSyntax);
            this.cursorDeleteSyntax = me.SelectNodeInnerText("./cursor_delete");
            this.cursorDeleteParsed = mc.ParseSyntax(this.cursorDeleteSyntax);
            this.cursorTypeSyntax = me.SelectNodeInnerText("./cursor_type");
            this.cursorTypeParsed = mc.ParseSyntax(this.cursorTypeSyntax);
            this.cursorTypeFeedbackNameSyntax = me.SelectNodeInnerText("./cursor_cluster");
            this.cursorTypeFeedbackNameParsed = mc.ParseSyntax(this.cursorTypeFeedbackNameSyntax);
            
            // default values
            this.cursorValueDefault = "value";
            this.cursorDeleteDefault = "next";
            this.cursorTypeDefault = "basic";
            this.cursorTypeFeedbackNameDefault = "current";

            // look for passed no-return flags
            if (me.HasChildElement("cursor"))
            {
                if (me.SelectSingleElem("./cursor").GetAttribute("no_return").Equals("1"))
                    this.noReturn = true;
        }
            if (me.HasChildElement("cursor_inst_id"))
            {
                if (me.SelectSingleElem("./cursor_inst_id").GetAttribute("no_return").Equals("1"))
                    this.noReturn = true;
            }

        }


        /// <summary>
        /// Add support for: run, then, loop, parallel
        /// build pipeline if using: where, groupby, distinct.
        /// </summary>
        /// <param name="me"></param>
        /// <param name="mc"></param>
        /// <param name="run"></param>
        public void AddCursorSubProcs(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            // if this is pipelined, generate the pipeline, otherwise work directly on this cursor
            if (this.needsPipeline)
            {
                var pLcursorSyntax = me.SelectNodeInnerText("./cursor");
                var pLcursorInstIdSyntax = me.SelectNodeInnerText("cursor_inst_id");
                if (pLcursorInstIdSyntax.IsNullOrEmpty())
                    pLcursorInstIdSyntax = "TEMP." + mc.GetGenSym("IterGenCsrInstId");
                XmlElement pipelineElem=me.CreateElement("select");
                pipelineElem.AppendTextElement("input_cursor_inst_id",this.cursorInstIdSyntax);
                // add the pipelined functions
                foreach (var elem in me.GetChildElems().SkipWhile(e => !e.LocalName.In(pipelineFuncs))){
                    elem.ParentNode.RemoveChild(elem);
                    pipelineElem.AppendChildElement(elem);
                }
                // add the cursor and cursor inst id as passed to this module
                pipelineElem.AppendTextElement("cursor", pLcursorSyntax);
                pipelineElem.AppendTextElement("cursor_inst_id", pLcursorInstIdSyntax);
                // add the cursor procs
                foreach (var elem in me.GetChildElems().Where(e => e.LocalName.In(cursorFuncs)))
                {
                    elem.ParentNode.RemoveChild(elem);
                    pipelineElem.AppendChildElement(elem);
                }
                // inline the generated config.
                run.Add(mc.GetModuleRun(pipelineElem));
                return;
            }

            // flag so we know if we already advance to the first row of the cursor.
            bool didFirstRow = false;

            // see what sub procs we need to execute
            XmlDocument doc = me.OwnerDocument;

            // everything we add should be put in a try catch where we close the cursor in finally
            XmlElement tryElem = doc.CreateElement("try");
            XmlElement tryConfigElem = tryElem.AppendChildElement(doc.CreateElement("config"));
            XmlElement tryFinallyElem = tryElem.AppendChildElement(doc.CreateElement("finally"));

            // deal w/ parallel as a preprocess macro
            XmlElement parallelElem = me.SelectSingleElem("./parallel");
            if (parallelElem != null)
            {
                // pull or default producer attributes from parallel
                string cursor = me.SelectNodeInnerText("cursor", "TEMP.csr");
                string cursorTempObject = parallelElem.GetAttributeDefaulted("cursor_temp_object", "TEMP." + mc.GetGenSym("csr_tmp_obj"));
                string batchSize = parallelElem.GetAttributeDefaulted("batch_size", "1");
                string copyCursorToTemp = parallelElem.GetAttributeDefaulted("copy_cursor_to_temp", "1");
                string copyCursorToTempXml = "";
                if (copyCursorToTemp.Equals("1"))
                {
                    copyCursorToTempXml =
                    "<copy_cursor_to_temp>" +
                    "<cursor>" + cursorTempObject + "</cursor>" +
                    (parallelElem.GetAttributeDefaulted("prefix", "").Equals("1") ? "<prefix>" + cursorTempObject + "</cursor>" : "") +
                    (parallelElem.GetAttributeDefaulted("suffix", "").Equals("1") ? "<suffix>" + cursorTempObject + "</suffix>" : "") +
                    "</copy_cursor_to_temp>";
                }

                // set the newModule element to our macro version
                XmlElement macroMe = (XmlElement)me.CloneNode(true);
                string xml =
                  "<run>" +
                  "<multi_thread_cursor>" +
                  "<cursor>" + cursor + "</cursor>" +
                  "<cursor_temp_object>" + cursorTempObject + "</cursor_temp_object>" +
                  "<batch_size>" + batchSize + "</batch_size>" +
                  "<run>" +
                  copyCursorToTempXml +
                  parallelElem.InnerXml +
                  "</run>" +
                  "</multi_thread_cursor>" +
                  "</run>";
                tryConfigElem.AppendChildImport(MyXml.ParseXmlStringGetElement(xml));
            }


            // Now just do what we normally do
            XmlElement runElem = me.SelectSingleElem("./run");
            XmlElement thenElem = me.SelectSingleElem("./then");
            XmlElement loopElem = me.SelectSingleElem("./loop");
            XmlElement elseElem = me.SelectSingleElem("./else");

            // if we have a run childElem, do that first
            if (runElem != null)
            {
                //run.Add(mc.schedulerMaster.GetModuleRun(runElem));
                tryConfigElem.AppendChildImport(runElem);
            }

            // if we have a then or else, need the if
            if (thenElem != null || elseElem != null)
            {
                AppendFirstRow(tryConfigElem, ref didFirstRow);
                XmlElement ifElem = doc.CreateElement("if");
                XmlElement conditionElem = doc.CreateElement("condition");
                conditionElem.InnerText = this.isEofSyntax + " ne 1";
                ifElem.AppendChild(conditionElem);
                if (thenElem != null)
                {
                    ifElem.AppendChild(thenElem);
                }
                else if (loopElem != null)
                {
                    thenElem = doc.CreateElement("then");
                    ifElem.AppendChild(thenElem);
                }
                if (elseElem != null) ifElem.AppendChild(elseElem);
                tryConfigElem.AppendChildImport(ifElem);
            }

            // if we have loop, interate with while loop
            if (loopElem != null)
            {
                AppendFirstRow(tryConfigElem, ref didFirstRow);
                XmlElement whileElem = doc.CreateElement("while");
                XmlElement conditionElem = doc.CreateElement("condition");
                conditionElem.InnerText = this.isEofSyntax+" ne 1";
                AppendCursorNextElement(loopElem);
                whileElem.AppendChild(conditionElem);
                whileElem.AppendChild(loopElem);
                if (thenElem != null)
                {
                    thenElem.AppendChild(whileElem);
                }
                else
                {
                    tryConfigElem.AppendChildImport(whileElem);
                }
            }
            // if we had any cursor sub procs, clear the cursor, else it is up to the user.
            if (runElem != null || loopElem != null || thenElem != null || elseElem != null)
            {
                AppendCursorClearElement(tryFinallyElem);
            }
            // add the try/catch
            run.Add(mc.GetModuleRun("<do>"+this.isEofSyntax+"=''</do>"));

            
            // if we are in a no_return situation remove the try config. This could be handled
            // more elegantly but for now just alter what is currently working so we do no break the
            // base case.
            if (this.noReturn)
            {
                // select out the config elem
                // select out the 
                XmlElement blockElem = doc.CreateElement("block");
                foreach (XmlElement elem in tryElem.SelectNodes("./config/*"))
                {
                    blockElem.AppendChildElement(elem);
                }
                foreach (XmlElement elem in tryElem.SelectNodes("./finally/*"))
                {
                    blockElem.AppendChildElement(elem);
                }
                run.Add(mc.GetModuleRun(blockElem));
                return;
            }

            run.Add(mc.GetModuleRun(tryElem));
        }

        public XmlElement AppendCursorClearElement(XmlElement parentElem)
        {
            XmlElement cursorClearElem = parentElem.CreateElement("cursor_clear");
           
                XmlElement cursorInstIdElem = parentElem.CreateElement("cursor_inst_id");
                cursorInstIdElem.InnerText = cursorInstIdSyntax;
                cursorClearElem.AppendChild(cursorInstIdElem);
           
            return parentElem.AppendChildImport(cursorClearElem) as XmlElement;
        }
        
        public XmlElement AppendCursorNextElement(XmlElement parentElem)
        {
            XmlElement cursorNextElem = parentElem.CreateElement("cursor_next");
            cursorNextElem.AppendTextElement("cursor_inst_id", this.cursorInstIdSyntax);
            cursorNextElem.AppendTextElement("cursor", this.cursorSyntax);
            cursorNextElem.AppendTextElement("is_eof", this.isEofSyntax);
            return parentElem.AppendChildImport(cursorNextElem) as XmlElement;
        }

        public XmlElement AppendFirstRow(XmlElement parentElem,ref bool didFirstRow)
        {
            if (!didFirstRow)
            {
                didFirstRow = true;
                return AppendCursorNextElement(parentElem);
            }
            return parentElem;
        }

    }
}
