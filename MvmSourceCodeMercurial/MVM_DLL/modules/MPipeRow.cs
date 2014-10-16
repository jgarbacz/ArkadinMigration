using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using NLog;
namespace MVM
{
    /// <summary>
    /// <pipe_row>TEMP.any_oid</pipe_row>
    /// </summary>
    class MPipeRow : IModuleSetup, IModuleRun
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        private string oidSyntax;
        private IReadString oidParsed;
        private string pipeCursorSyntax = "TEMP.pipe_cursor";
        private IReadString pipeCursorParsed;
        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MPipeRow m = new MPipeRow();
            m.oidSyntax = me.InnerText;
            m.oidParsed = mc.ParseSyntax(m.oidSyntax);
            m.pipeCursorParsed = mc.ParseSyntax(m.pipeCursorSyntax);
            run.Add(m);
            run.Add(new MYield());
        }

        public void Run(ModuleContext mc)
        {
            // pipe_row is like cursor.next() on the pipe cursor so it 
            // get the pipe cursor
            string pipeCursorInstId = this.pipeCursorParsed.Read(mc);
            PipeCursor pipeCursor =mc.globalContext.GetNamedClassInst(pipeCursorInstId) as PipeCursor;

            // get the object being piped
            string oid = this.oidParsed.Read(mc);

            // if oid is '' then just pipe a new null_row row object. This object is created
            // off the pipe cursor so it fully belongs to the pipe cursor.
            if (oid.Equals(""))
            {
                using (IObjectData pipeCsrObj = pipeCursor.CreateNewObject())
                {
                    //logger.Trace("[pipe_row] piping null_row_object oid="+pipeCsrObj.objectId);
                    pipeCsrObj["null_row"] = "1";
                    pipeCursor.SetNextObject(pipeCsrObj.objectId);
                    return;
                }
            }
            // otherwise, divorce this object from this cursor and tie it to the pipe cursor,
            // then, create a new stub object that points to this cursor.
            else
            { 
                //logger.Info("[pipe_row] piping [" + oid + "]");
                // set the next object on the pipecursor
                pipeCursor.SetNextObject(oid);
            }
        }
    }

}
