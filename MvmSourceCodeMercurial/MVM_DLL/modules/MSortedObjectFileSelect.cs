using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

using Antlr.Runtime.Tree;

namespace MVM
{
    /*
    <sorted_object_file_select>
      <name>TEMP.filename</name>
      <input_file_match>TEMP.filename ~ '*'</input_file_match>
      <cursor>TEMP.csr</cursor>
      <loop>
        ...
      </loop>
    </sorted_object_file_select>
    */

    class MSortedObjectFileSelect : IModuleSetup, IModuleRun
    {
        private CursorSetupCommon cursorSetup;

        // from xml
        private string nameSyntax;
        private string inputFileSyntax;

        // from setup
        private IReadString nameParsed;
        private IReadString inputFileParsed;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MSortedObjectFileSelect m = new MSortedObjectFileSelect();
            m.cursorSetup = new CursorSetupCommon(me, mc);

            // xml extraction
            m.nameSyntax = me.SelectNodeInnerText("./name");
            m.inputFileSyntax = me.SelectNodeInnerText("./input_file_match");

            // parsing
            m.nameParsed = mc.ParseSyntax(m.nameSyntax);
            m.inputFileParsed = mc.ParseSyntax(m.inputFileSyntax);

            // runtime
            run.Add(m);
            m.cursorSetup.AddCursorSubProcs(me, mc, run);
        }

        public void Run(ModuleContext mc)
        {
            var textCmp = MergeableComparer<Text>.Default;
            var textSer = Serializabler<Text>.Default;
            var objSer = mc.mvm.objectDataSerializer;
            string mergeFile = this.nameParsed.Read(mc);
            string inputFiles = this.inputFileParsed.Read(mc);
            List<string> stringList = FileUtils2.GlobToList(inputFiles);
            MergeSortReaderWriter<Text, IObjectData> reader = mc.threadContext.GetMergeSortWriter(mergeFile);
            foreach (string inputFile in stringList)
            {
                MergeableFile<Text, IObjectData> f = new MergeableFile<Text, IObjectData>(Direction.Forward, inputFile, textSer, objSer);
                reader.InsertMergeEnumerable(f);
            }
            var msc = new MergeSortCursor<Text>(mc, this.cursorSetup, reader);
        }
    }

    /// <summary>
    /// Class implementing a merge-sort cursor 
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class MergeSortCursor<K> : CursorCommonLinqEnabled, ICursor
    {
        // set by constructor
        private MergeSortReader<K, IObjectData> reader;

        // constructor
        public MergeSortCursor(ModuleContext mc, ICursorSetupCommon cursorSetup, MergeSortReader<K, IObjectData> reader)
            : base(mc, cursorSetup)
        {
            this.reader = reader;
        }

        public override IObjectData CursorNext()
        {
            IObjectData nextObj = null;
            var looper = this.reader.GetEnumerator();
            if (looper.MoveNext())
            {
                nextObj = looper.Current.Value;
                this.mvm.objectCache.AddOrMergeObject(nextObj);
            }
            return nextObj;
        }

        public override void CursorClear()
        {
            if (this.reader != null) this.reader = null;
        }
    }
}
