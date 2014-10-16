using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using NLog;
namespace MVM
{
    /// <summary>
    /// Builds on queue buffered file to expose a binary file queue that work in the buffered file system.
    /// </summary>
    public class BinaryQueueBufferedFile : IBufferedFile
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        public readonly BufferedFileSystem bufferedFileSystem;
        public readonly string fileName;
        private readonly QueueBufferedFile queueBufferedFile;
        public int BufferBytes { get; private set; }

        public bool Blocking
        {
            get
            {
                return this.queueBufferedFile.Blocking;
            }
            set
            {
                this.queueBufferedFile.Blocking = value;
            }
        }

        /// <summary>
        /// Instanciates the file
        /// </summary>
        /// <param name="bufferedFileSystem"></param>
        /// <param name="fileName"></param>
        public BinaryQueueBufferedFile(BufferedFileSystem bfs, string fileName,bool isPersistent, object state)
        {
            this.bufferedFileSystem = bfs;
            this.fileName = fileName;
            this.queueBufferedFile = new QueueBufferedFile(this.bufferedFileSystem,this.fileName, bfs.writeBufferStartBytes, bfs.writeBufferIncrBytes, bfs.writeBufferMaxBytes, bfs.readBufferBytes,isPersistent, state);
        }


        /// <summary>
        /// Flushes the file to disk
        /// </summary>
        public object FlushToFile(bool keepState)
        {
            //logger.Debug("Flushing " + this.fileName);
            if (keepState)
            {
                if (this.BinaryWriter != null)
                {
                    this.BinaryWriter.Flush();
                }
            }
            return this.queueBufferedFile.FlushToFile(keepState);
        }

        
        /// <summary>
        /// To read from the buffered file
        /// </summary>
        public BinaryReader BinaryReader
        {
            get {
                if (this._binaryReaderValue == null)
                {
                    this._binaryReaderValue = new BinaryReader(this.queueBufferedFile.stream);
                }
                return this._binaryReaderValue;
            }
        }
        private BinaryReader _binaryReaderValue;

        
        /// <summary>
        /// To write to the buffered file
        /// </summary>
        public BinaryWriter BinaryWriter
        {
            get
            {
                if (this._binaryWriterValue == null)
                {
                    this._binaryWriterValue = new BinaryWriter(this.queueBufferedFile.stream);
                }
                return this._binaryWriterValue;
            }
        }
        private BinaryWriter _binaryWriterValue;

        /// <summary>
        /// Commits data that has been written so the reader can access it.
        /// </summary>
        public void Commit()
        {
            this.queueBufferedFile.Commit();
        }

        /// <summary>
        /// Indicates no more data will be written to the queue.
        /// </summary>
        public void SetEof()
        {
            this.queueBufferedFile.Eof = true;
        }

        /// <summary>
        /// Make next read start at beginning.
        /// </summary>
        public void Rewind()
        {
            this.queueBufferedFile.Rewind();
        }
    }
}
