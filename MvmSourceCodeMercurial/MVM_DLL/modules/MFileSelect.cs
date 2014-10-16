using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;
using System.Linq;

using Antlr.Runtime.Tree;

namespace MVM
{
    /*

default format:
<format record_delim='system_newline'>
	<record name='value'/>
</format>

<file_select>
<input_file></input_file>
<record_offset_tag>TEMP.offset</record_offset_tag>  <~-- optionally return offset/length of record -->
<record_length_tag>TEMP.length</record_length_tag>
<ctrl>'C:\\_ROB\\mvm\\test_ctrl.txt</ctrl>
     -or-
<format name='ACCOUNT_FORMAT'/>
     -or-
<format record_delim='SYSTEM.newline()' default='NULL'>
	<record name='value'/>
	<field name='value1'/>
    <field name='true'/>
	<field name='value2'>'somedefault'</field>
	<add_field name='some_added_field'>OBJECT.blah+1</field>
</format>
    
<cursor>TEMP.c</cursor>
<then/>
<loop/>
<else/>
</file_select>     

     */

    class MFileSelect : IModuleSetup, IModuleRun
    {
        private CursorSetupCommon cursorSetup;

        // from xml
        private string inputFileSyntax;
        private string inputDirSyntax;
        private string inputMatchSyntax;
        private string recordOffsetSyntax;
        private string recordLengthSyntax;
        private string encodingSyntax;
        private string cursorTypeSyntax;
        private string ctrlSyntax;
        private XmlElement formatElem;
        private string cursorType = null;

        // from setup
        private List<string> fieldNames = new List<string>();
        private IReadString inputFileParsed;
        private IReadString inputDirParsed;
        private IReadString inputMatchParsed;
        private IReadString recordOffsetParsed;
        private IReadString recordLengthParsed;
        private IReadString encodingParsed;
        private IReadString cursorTypeParsed;

        private IReadString ctrlParsed;
        private List<IReadString> fieldNamesParsed = new List<IReadString>();

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MFileSelect m = new MFileSelect();
            m.cursorSetup = new CursorSetupCommon(me, mc);
            // xml extraction
            m.inputDirSyntax = me.SelectNodeInnerText("./input_dir");
            m.inputFileSyntax = me.SelectNodeInnerText("./input_file");
            m.inputMatchSyntax = me.SelectNodeInnerText("./input_match");
            m.recordOffsetSyntax = me.SelectNodeInnerText("./record_offset_tag");
            m.recordLengthSyntax = me.SelectNodeInnerText("./record_length_tag");
            m.encodingSyntax = me.SelectNodeInnerText("./encoding");
            m.cursorTypeSyntax = me.SelectNodeInnerText("./cursor_type");
            m.formatElem = me.SelectSingleElem("./format");
            m.ctrlSyntax = me.SelectNodeInnerText("./ctrl");

            // parsing
            m.inputFileParsed = mc.ParseSyntax(m.inputFileSyntax);
            m.inputDirParsed = mc.ParseSyntax(m.inputDirSyntax);
            m.inputMatchParsed = mc.ParseSyntax(m.inputMatchSyntax);
            m.recordOffsetParsed = mc.ParseSyntax(m.recordOffsetSyntax);
            m.recordLengthParsed = mc.ParseSyntax(m.recordLengthSyntax);
            m.encodingParsed = mc.ParseSyntax(m.encodingSyntax);
            m.cursorTypeParsed = mc.ParseSyntax(m.cursorTypeSyntax);
            m.ctrlParsed = mc.ParseSyntax(m.ctrlSyntax);

            if (m.formatElem != null && m.formatElem.GetAttribute("type").Equals("packed"))
            {
                // special case object files
                bool blocking = m.formatElem.GetAttribute("blocking", "false").Equals("true");
                bool rewind = m.formatElem.GetAttributeDefaulted("rewind", "true").Equals("true");
                run.Add(new PackedFileReader(m.cursorSetup, m.inputFileParsed, blocking, rewind));
            }
            else
            {
                if (m.cursorTypeParsed != null)
                {
                    // new array format
                    m.cursorType = m.cursorTypeParsed.Read(mc);
                }
                run.Add(m);
            }

            // add cursor extras
            m.cursorSetup.AddCursorSubProcs(me, mc, run);
        }

        public void Run(ModuleContext mc)
        {
            // get our list of files
            List<string> files = new List<string>();
            string inputDir = this.inputDirParsed != null ? this.inputDirParsed.Read(mc) : null;
            if (this.inputFileParsed != null)
            {
                string fileName = this.inputFileParsed.Read(mc);
                if (inputDir == null)
                {
                    files.Add(fileName);
                }
                else
                {
                    DirectoryInfo root = new DirectoryInfo(inputDir);
                    foreach (var elem in root.GetFiles(fileName, SearchOption.TopDirectoryOnly))
                        files.Add(fileName);
                }
            }
            if (this.inputDirParsed != null && this.inputMatchParsed != null)
            {
                string inputMatch = this.inputMatchParsed.Read(mc);
                if (inputDir == null) throw new Exception("error, input_match expects input dir to be set");
                DirectoryInfo root = new DirectoryInfo(inputDir);
                foreach (var elem in root.GetFiles(inputMatch, SearchOption.TopDirectoryOnly))
                    files.Add(elem.FullName);
            }

            string recordOffset = this.recordOffsetParsed != null ? this.recordOffsetParsed.Read(mc) : null;
            string recordLength = this.recordLengthParsed != null ? this.recordLengthParsed.Read(mc) : null;
            string encoding = this.encodingParsed != null ? this.encodingParsed.Read(mc) : null;

            // if we're using a oracle sqlldr ctrl file
            if (this.ctrlParsed != null)
            {
                // read ctrl file and get format info
                string ctrl = this.ctrlParsed.Read(mc);
                OracleDumpCtrlFile ctrlFile;
                try
                {
                    ctrlFile = new OracleDumpCtrlFile(ctrl);
                }
                catch (Exception e)
                {
                    throw new Exception("Error, cannot do file_select. ctrl file:" + this.ctrlSyntax + "=" + ctrl + " is not valid", e);
                }
                string fieldDelim = ctrlFile.fieldDelim;
                string recordDelim = ctrlFile.recordDelim;
                //Console.WriteLine("fd=" + fieldDelim.InterpolateEscapesReverse());
                //Console.WriteLine("rd=" + recordDelim.InterpolateEscapesReverse());
                List<string> fieldNames = ctrlFile.GetOrderedFieldNames();
                var fc = new DelimitedRecordCursor(mc, cursorSetup, files, fieldDelim, recordDelim, fieldNames, recordOffset, recordLength, encoding, false, this.cursorType);

                return;
            }

            // if there is no format element specified, assume reading a file with the default newline
            if (formatElem == null)
            {
                string recordDelim = FileUtils2.DefaultNewline;
                string cursorValue = "value";
                var fc = new DelimitedFileCursor(mc, this.cursorSetup, files, recordDelim, cursorValue, recordOffset, recordLength, encoding, false, this.cursorType);
                return;
            }

            // if there is a record delimiter defined and no field delimiter defined and a single field/record
            // TBD: distinguish from fixed length b/c you can have fixed length with a record delim.
            if (!formatElem.GetAttribute("record_delim").Equals("") && formatElem.GetAttribute("field_delim").Equals("") && formatElem.GetChildElems().Count == 1 && !formatElem.GetChildElems()[0].HasAttribute("length"))
            {
                string recordDelim = formatElem.GetAttribute("record_delim");
                string cursorValue = formatElem.GetChildElems()[0].GetAttribute("name");

                var fc = new DelimitedFileCursor(mc, this.cursorSetup, files, recordDelim, "value", recordOffset, recordLength, encoding, false, this.cursorType);
                return;
            }

            // if there is a field delim defined, we are doing delimited parsing
            if (!formatElem.GetAttribute("field_delim").Equals(""))
            {

                string fieldDelim = formatElem.GetAttribute("field_delim");
                string recordDelim = formatElem.GetAttributeDefaulted("record_delim", FileUtils2.DefaultNewline);
                bool trim = formatElem.GetAttribute("trim").Equals("true");
                if (formatElem.GetAttribute("format_in_header").Equals("true"))
                {
                    var fc = new DelimitedRecordCursorWithFormatHeader(mc, this.cursorSetup, files, fieldDelim, recordDelim, recordOffset, recordLength, encoding, trim, this.cursorType);
                }
                else
                {
                    List<string> orderedFieldNames = formatElem.SelectElements("./field").Select(n => n.GetAttribute("name")).ToList();
                    var fc = new DelimitedRecordCursor(mc, this.cursorSetup, files, fieldDelim, recordDelim, orderedFieldNames, recordOffset, recordLength, encoding, trim, this.cursorType);
                }
                return;
            }

            // need to support format!!!
            throw new Exception("This format is not supported ".AppendLine() + ":" + formatElem.OuterXml.AppendLine());
        }

        public class PackedFileReader : IModuleRun
        {
            IReadString fileNameParsed;
            //IWriteString cursorParsed;
            bool blocking;
            bool rewind;
            private ICursorSetupCommon cursorSetup;
            public PackedFileReader(ICursorSetupCommon cursorSetup, IReadString fileNameParsed, bool blocking, bool rewind)
            {
                this.cursorSetup = cursorSetup;
                this.fileNameParsed = fileNameParsed;
                this.blocking = blocking;
                this.rewind = rewind;
            }
            public void Run(ModuleContext mc)
            {
                string fileName = this.fileNameParsed.Read(mc);
                using (var f = mc.globalContext.bfs.GetObjectQueue(fileName, true))
                {
                    var bqueue = (f.value as ObjectQueueBufferedFile);
                    if (rewind) bqueue.Rewind();
                }
                var fc = new PackedFileCursor(mc, cursorSetup, fileName, blocking);
            }
        }
    }
}
