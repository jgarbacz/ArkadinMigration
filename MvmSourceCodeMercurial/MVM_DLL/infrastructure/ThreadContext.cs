using System;
using System.Collections.Generic;

using System.Text;
using System.IO;

namespace MVM
{
    public class ThreadContext :IDisposable
    {
        public readonly GlobalContext globalContext;
        private readonly IObjectData threadObjectData;

        public ThreadContext(GlobalContext globalContext,int workerNo)
        {
            this.globalContext = globalContext;

            ObjectDataStringHash obj = ObjectDataStringHash.GetThreadObjectData(globalContext.workMgr.objectCache);
            obj.objectId = "THREAD_" + workerNo;
            obj["object_id"] = "THREAD" + workerNo;
            obj["object_type"] = "THREAD";
            obj["worker_no"] = workerNo.ToString();
            threadObjectData=obj;
        }


        #region Manage THREAD.field

        public string this[string fieldName]
        {
            get
            {
                return this.threadObjectData[fieldName];
            }
            set
            {
                this.threadObjectData[fieldName] = value;
            }
        }

        #endregion

        #region StreamReaders

        // store my filehandles for the threadContext here
        public Dictionary<string, StreamReader> streamReaders = new Dictionary<string, StreamReader>();
        public StreamReader GetStreamReader(string fileName)
        {
            if (streamReaders.ContainsKey(fileName)) return streamReaders[fileName];
            StreamReader sr = new StreamReader(fileName);
            streamReaders[fileName] = sr;
            return sr;
        }
        public StreamReader GetStreamReader(string fileName, Encoding encoding)
        {
            if (streamReaders.ContainsKey(fileName)) return streamReaders[fileName];
            StreamReader sr = new StreamReader(fileName, encoding);
            streamReaders[fileName] = sr;
            return sr;
        }

        // Closes a stream streamReader, returns true if it did.
        public bool CloseStreamReader(string fileName)
        {
            if (streamReaders.ContainsKey(fileName))
            {
                var sr = streamReaders[fileName];
                sr.Close();
                streamReaders.Remove(fileName);
                return true;
            }
            return false;
        }

        // closes the stream readers
        public void DisposeStreamReaders()
        {
            for (; ; )
            {
                string fileName = streamReaders.GetFirstKey();
                if (fileName == null) break;
                this.CloseStreamReader(fileName);
            }
        }
        #endregion

        #region MergeSorts

        // store my filehandles for the threadContext here
        public Dictionary<string, MergeSortReaderWriter<Text, IObjectData>> mergeSorts = new Dictionary<string, MergeSortReaderWriter<Text, IObjectData>>();
        public MergeSortReaderWriter<Text, IObjectData> GetMergeSortWriter(string fileName)
        {
            if (mergeSorts.ContainsKey(fileName)) return mergeSorts[fileName];
            string dir = Path.GetDirectoryName(fileName);
            string prefix = globalContext["node_id"];
            var textCmp = MergeableComparer<Text>.Default;
            var textSer = Serializabler<Text>.Default;
            MergeSortReaderWriter<Text, IObjectData> ms = new MergeSortReaderWriter<Text, IObjectData>(100, 100000, dir, prefix, textCmp, textSer, this.globalContext.workMgr.mvm.objectDataSerializer);
            mergeSorts[fileName] = ms;
            return ms;
        }
        public bool CloseMergeSortWriter(string fileName, bool flush)
        {
            if (mergeSorts.ContainsKey(fileName))
            {
                var ms = mergeSorts[fileName];
                if (flush)
                {
                    ms.FlushToFile(fileName);
                }
                mergeSorts.Remove(fileName);
                return true;
            }
            return false;
        }
        public void DisposeMergeSortWriters()
        {
            for (; ; )
            {
                string fileName = mergeSorts.GetFirstKey();
                if (fileName == null) break;
                this.CloseMergeSortWriter(fileName, true);
            }
        }

        #endregion

        #region StreamWriters
        
        // store my filehandles for the threadContext here
        public Dictionary<string, StreamWriter> streamWriters = new Dictionary<string, StreamWriter>();
        public StreamWriter GetStreamWriter(string fileName)
        {
            if(streamWriters.ContainsKey(fileName)) return streamWriters[fileName];
            StreamWriter sw = new StreamWriter(fileName);
            streamWriters[fileName] = sw;
            return sw;
        }
        public StreamWriter GetStreamWriter(string fileName,Encoding encoding)
        {
            if (streamWriters.ContainsKey(fileName)) return streamWriters[fileName];
            StreamWriter sw = new StreamWriter(fileName,false,encoding);
            streamWriters[fileName] = sw;
            return sw;
        }

        // Closes a stream streamWriter, returns true if it did.
        public bool CloseStreamWriter(string fileName)
        {
            if (streamWriters.ContainsKey(fileName))
            {
                var sw=streamWriters[fileName];
                sw.Close();
                streamWriters.Remove(fileName);
                return true;
            }
            return false;
        }

        // closes the stream writers
        public void DisposeStreamWriters()
        {
            for (; ; )
            {
                string fileName=streamWriters.GetFirstKey();
                if (fileName == null) break;
                this.CloseStreamWriter(fileName);
            }
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            this.DisposeStreamReaders();
            this.DisposeStreamWriters();
            this.DisposeMergeSortWriters();
        }

        #endregion
    }
}
