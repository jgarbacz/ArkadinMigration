// TODO: clean up unneeded references
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Xml;
//using System.IO;
//using Antlr.Runtime.Tree;

namespace MVM
{
    /*
     <get_server_data>
        <memory>y</memory>
        <index>n</index>
        <buffers>y</buffers>
        <objects>y</objects>
        <output>TEMP.foo</output>
     </get_server_data>
     */

    // Get a string-based summary of current server state
    class MGetServerData : IModuleSetup, IModuleRun
    {
        // from setup
        private IWriteString outputParsed;
        
        private bool inputGetMemory;
        private bool inputGetIndexes;
        private bool inputGetBuffers;
        private bool inputGetObjects;
        //private IWriteString outputParsed;

        private const long bytesInMB = 1024 * 1024;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MGetServerData m = new MGetServerData();

            m.inputGetMemory = (me.SelectNodeInnerText("./memory") == "y") ? true : false;
            m.inputGetIndexes = (me.SelectNodeInnerText("./index") == "y") ? true : false;
            m.inputGetBuffers = (me.SelectNodeInnerText("./buffers") == "y") ? true : false;
            m.inputGetObjects = (me.SelectNodeInnerText("./objects") == "y") ? true : false;

            m.outputParsed = mc.ParseWritableSyntax(me.SelectNodeInnerText("./output"));

            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            Process winProcess = Process.GetCurrentProcess();

            StringBuilder SB = new StringBuilder();
            SB.Append("Main Window Title: ");
            SB.Append(winProcess.MainWindowTitle);
            SB.Append("\n");
            SB.Append("MVM Node Id: ");
            SB.Append(mc.mvm.nodeId.ToString());
            SB.Append("\n");

            if (this.inputGetMemory)
            {
                SB.AppendLine("MEMORY DETAILS");
                SB.Append("Physical memory allocated: ");
                SB.Append(megaBytes(winProcess.WorkingSet64));
                SB.Append("MB\n");
                SB.Append("Virtual memory allocated: ");
                SB.Append(megaBytes(winProcess.VirtualMemorySize64));
                SB.Append("MB\n");
                SB.Append(" - Paged virtual memory allocated: ");
                SB.Append(megaBytes(winProcess.PagedMemorySize64));
                SB.Append("\nPrivate memory allocated: ");
                SB.Append(megaBytes(winProcess.PrivateMemorySize64));
                SB.Append("MB\n");
            }

            // stole from MVM.DumpMemory(), fyi
            if (this.inputGetBuffers)
            {
                SB.AppendLine("BUFFERED FILE CACHE");
                if (mc.globalContext.bfs == null || mc.globalContext.bfs.bufferedFileCache == null)
                {
                    SB.AppendLine("Buffered file cache not used.");
                }
                else
                {
                    SB.Append("Num items in cache: ");
                    SB.Append(mc.mvm.globalContext.bfs.bufferedFileCache.NumItemsInCached.ToString());
                    SB.Append("\n");
                    SB.Append("Num items in saved state: ");
                    SB.Append(mc.mvm.globalContext.bfs.bufferedFileCache.NumItemsSavedState.ToString());
                    SB.Append("\n");
                    SB.Append("Num items locked: ");
                    SB.Append(mc.mvm.globalContext.bfs.bufferedFileCache.NumLockedItems.ToString());
                    SB.Append("\n");
                    SB.Append("Num items unlocked: ");
                    SB.Append(mc.mvm.globalContext.bfs.bufferedFileCache.NumUnLockedItems.ToString());
                    SB.Append("\n");
                }
            }

            // stole from MVM.DumpMemory(), fyi
            if (this.inputGetObjects)
            {
                SB.AppendLine("OBJECT INVENTORY");
                SB.Append("Total number of objects: ");
                SB.Append(mc.mvm.objectCache.objects.Count);
                SB.Append("\n");
                Dictionary<string, int> objTypeCount = new Dictionary<string, int>();
                foreach (var obj in mc.mvm.objectCache.objects.Values)
                {
                    if (!objTypeCount.ContainsKey(obj.objectType)) objTypeCount[obj.objectType] = 0;  // create entry
                    objTypeCount[obj.objectType]++;  // append to entry count
                }
                // add to StringBuilder
                foreach (string s in objTypeCount.Keys)
                {
                    SB.Append(s);
                    SB.Append(" type objects: ");
                    SB.Append(objTypeCount[s].ToString());
                    SB.Append("\n");
                }
            }

            // stole from MVM.DumpMemory()
            if (this.inputGetIndexes)
            {
                SB.AppendLine("INDEX LISTING AND DETAILS");
                SB.Append("Global monitors: ");
                SB.Append(mc.mvm.globalContext.monitors.Count.ToString());
                SB.Append("\n");
                SB.Append("Indexes: ");
                var globalIndexMap = mc.mvm.globalContext.namedClassInstMap.UnsafeGetInnerDictionary();
                SB.Append(globalIndexMap.Count.ToString());
                SB.Append("\n");
                // Need to specify which indexes you want
                // loop through the array of indexes
                //globalIndexMap.Values.
                foreach (var elem in globalIndexMap)
                {
                    int myCount = 0;
                    if (elem.Value.GetType().ToString().Equals("MVM.MemoryIndex"))
                    {
                        MemoryIndex memidx = (MemoryIndex)elem.Value;
                        foreach (var row in memidx.index.Values) myCount += row.Count;
                        SB.Append("Memory Index ");
                        SB.Append(elem.Key.ToString());
                        SB.Append(": ");
                        SB.Append(myCount.ToString());
                        SB.Append("\n");
                    }
                    else if (elem.Value.GetType().ToString().Equals("MVM.MemoryIndexSync"))
                    {
                        MemoryIndexSync memsync = (MemoryIndexSync)elem.Value;
                        foreach (var row in memsync.index.Values) myCount += row.Count;
                        SB.Append("Memory Index Sync ");
                        SB.Append(elem.Key.ToString());
                        SB.Append(": ");
                        SB.Append(myCount.ToString());
                        SB.Append("\n");
                    }
                    else if (elem.Value.GetType().ToString().Equals("MVM.NestedMemoryIndex"))
                    {
                        NestedMemoryIndex nest = (NestedMemoryIndex)elem.Value;
                        myCount = nest.GetCount(nest.GetOrderedKeyFields());
                        SB.Append("Nested Memory Index ");
                        SB.Append(elem.Key.ToString());
                        SB.Append(": ");
                        SB.Append(myCount.ToString());
                        SB.Append("\n");
                    }
                    else
                    {
                        SB.Append(elem.Value.GetType());
                        SB.Append(" ");
                        SB.Append(elem.Key);
                        SB.Append(": ???\n");
                    }
                }
            }

            outputParsed.Write(mc, SB.ToString());
        }

        private static string megaBytes(long bytes)
        {
            return (bytes / bytesInMB).ToString();
        }
    }
}
