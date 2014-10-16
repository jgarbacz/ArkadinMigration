using System;
using System.Collections.Generic;

using System.Text;
using System.IO;

namespace MVM
{
    
    public class FileIndex:IIndex
    {
        public Dictionary<string, int> FieldIndexes { get { return this.fieldIndexes; } }
        private Dictionary<string, int> fieldIndexes = new Dictionary<string, int>();
        public Dictionary<StringArray, long> index;
        public string fileName;
        public string fieldDelim;
        public string recordDelim;
        public List<string> fieldNames;
        //public string keyField;
        public List<string> keyFieldNames=new List<string>();
        public List<int> keyFieldColNos = new List<int>();
        //public int keyFieldCol;

        public string IndexName { get { return this.fileName; } }

        public static FileIndex CreateFileIndex(string fileName, string fieldDelim, string recordDelim, List<string> fieldNames, List<string> keyFieldNames)
        {
            FileIndex fileIndex = new FileIndex();
            fileIndex.fieldDelim = fieldDelim;
            fileIndex.recordDelim = recordDelim;
            fileIndex.fileName = fileName;
            fileIndex.fieldNames = fieldNames;
            fileIndex.keyFieldNames=keyFieldNames;
            foreach (string keyField in keyFieldNames)
            {
                int keyFieldCol = fileIndex.fieldNames.IndexOf(keyField);
                if (keyFieldCol < 0) throw new Exception("Error, cannot index file. Key field=[" + keyField + "] not in format [" + fieldNames.Join(",") + "]");
                fileIndex.keyFieldColNos.Add(keyFieldCol);
            }
             fileIndex.index = BuildIndexOnSortedFile(
                fileIndex.fileName, 
                fileIndex.fieldDelim,
                fileIndex.recordDelim, 
                fileIndex.keyFieldColNos,
                fileIndex.fieldNames);
            return fileIndex;
        }

        // Builds an index on top of a sorted file
        public static Dictionary<StringArray, long> BuildIndexOnSortedFile(string fileName, string fieldDelim,string recordDelim, List<int> colNos,List<string>fieldNames)
        {
            string[] fDelArr = new string[] { fieldDelim };
            Dictionary<StringArray, long> index = new Dictionary<StringArray, long>();
            
            RecordReader tr = new RecordReader(fileName,recordDelim);
            long offset = tr.Position();
            string line;
            long lineNo = 0;
            while ((line = tr.ReadLine()) != null)
            {
                lineNo++; // preincrement
                string[] cols = line.Split(fDelArr, StringSplitOptions.None);
                if (cols.Length != fieldNames.Count)
                {
                    Console.WriteLine("Bad line:" + lineNo + " in [" + fileName + "]");
                    Console.WriteLine("line=[" + line + "]");
                    Console.WriteLine("expected field count=[" + fieldNames.Count + "]");
                    Console.WriteLine("found field count=[" + cols.Length + "]");
                    for(int x=0;x<fieldNames.Count;x++){
                        string name = fieldNames[x];
                        string value = x < cols.Length ? cols[x] : "MISSING";
                        Console.WriteLine(name + "\t[" + value+"]");
                    }
                    for (int x = fieldNames.Count; x < cols.Length; x++)
                    {
                        string value = cols[x];
                        Console.WriteLine("extra:"+x + "\t[" + value + "]");
                    }
                    throw new Exception("Error building index on sorted file see screen output");
                }
                else
                {
                    StringArray key = new StringArray(colNos.Count);
                    for (int i = 0; i < colNos.Count; i++)
                    {
                        try
                        {
                            int colNo = colNos[i];
                            string col = cols[colNo];
                            key[i] = col;
                        }
                        catch (Exception e)
                        {
                            throw new Exception("error", e);
                        }
                    }
                    if (!index.ContainsKey(key)) index[key] = offset;
                }
                offset = tr.Position(); 
            }
            tr.Close();

            //Console.WriteLine("INDEX");
            //foreach (string k in index.Keys)Console.WriteLine(k + "=" + index[k]);
            return index;
        }
        public bool NestedKeys()
        {
            return false;
        }

        public bool UseContext()
        {
            return true;
        }

        public bool IsStatic()
        {
            return true;
        }

        public int GetCount(List<string> keys)
        {
            throw new NotImplementedException();
        }

        public bool IndexGetRow(ModuleContext mc, string[] orderedKeyValues, out string[] values)
        {
            throw new NotImplementedException();
        }

        public string IndexGet(ModuleContext mc, List<string> orderedKeyValues, Dictionary<string, string> values)
        {
            throw new NotImplementedException();
        }

        public List<string> GetOrderedKeyFields()
        {
            return this.keyFieldNames;
        }

        public ICursor IndexSelect(ModuleContext mc, ICursorSetupCommon cursorSetup, List<string> orderedKeyValues, CursorOrder cursorOrder)
        {

            StringArray keyFieldValue = new StringArray(orderedKeyValues.ToArray());
            if (this.index.ContainsKey(keyFieldValue))
            {
                long offset = this.index[keyFieldValue];
                var csr = new FileIndexCursor(mc,cursorSetup,this, offset, fieldDelim, recordDelim, this.fieldNames, keyFieldColNos, orderedKeyValues);
                return csr;
            }
            else
            {
                var csr = new NullCursor(mc, cursorSetup, fieldNames);
                return csr;
            }
        }

        public List<string> GetOrderedValueFields()
        {
            throw new NotImplementedException();
        }


        public string IndexInsertIfNone(ModuleContext mc, List<string> orderedFieldValues)
        {
            throw new NotImplementedException();
        }


        public void IndexInsert(ModuleContext mc, List<string> orderedFieldValues)
        {
            throw new NotImplementedException();
        }

        public string IndexUpdate(ModuleContext mc, List<string> orderedKeyValues, Dictionary<string, string> updateValues)
        {
            throw new NotImplementedException();
        }

        #region IIndex Members


        public string IndexRemove(ModuleContext mc, List<string> orderedKeyValues, IndexRemovalOption removalOption)
        {
            throw new NotImplementedException();
        }

        public string IndexClear(ModuleContext mc)
        {
            throw new NotImplementedException();
        }

        public void IndexClose(ModuleContext mc)
        {
            throw new NotImplementedException();
        }

        public void IndexDrop(ModuleContext mc)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IIndex Members


        public ICursor IndexSelectKeys(ModuleContext mc,ICursorSetupCommon cursorSetup, List<string> notused)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IIndex Members


        public void PushSlice(MvmEngine mvm, string[] orderedKeyValues, SocketHandler socketHandler, bool clearSource, string tgtName, string[] tgtRecordTemplate, List<KeyValuePair<int, int>> tgtIdxSrcIdxMap)
        {
            throw new NotImplementedException();
        }

        public void PushAll(MvmEngine mvm, SocketHandler socketHandler, bool clearSource, string tgtName, string[] tgtRecordTemplate, List<KeyValuePair<int, int>> tgtIdxSrcIdxMap)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
