using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using NLog;
namespace MVM
{
    /// <summary>
    /// This class provides buffered access for one sequential 1 reader and 1 sequential streamWriter
    /// </summary>
    public class QueueBufferedFile
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// When files are flushed, we store their state.
        /// </summary>
        private class QueueState
        {
            public long writeCommitPos;
            public long readLogicalPos;
            public long writeSkipBytes;
            public bool blocking;
        }

        /// <summary>
        /// This is the stream to read and write
        /// </summary>
        public readonly Stream stream;

        /// <summary>
        /// File name should this queue persist
        /// </summary>
        public readonly string fileName;

        /// <summary>
        /// Makes reads block.
        /// </summary>
        public bool Blocking = false;
        public readonly BufferedFileSystem bufferedFileSystem;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fileName"></param>
        public QueueBufferedFile(BufferedFileSystem bufferedFileSystem, string fileName, int writeBufferStartBytes, int writeBufferIncrBytes, int writeBufferMaxBytes, int readBufferBytes, bool isPersistent, object stateObject)
        {
            this.bufferedFileSystem = bufferedFileSystem;
            this.fileName = fileName;
            this.stream = new QueueBufferedFileStream(this);
            this.writeBufInc = writeBufferIncrBytes;
            this.writeBufMax = writeBufferMaxBytes;
            this.writeBufStartBytes = writeBufferStartBytes;
            this.preferedReadSize = readBufferBytes;
            this.IsPersistent = isPersistent;
            this.writeBuf = null;
            this.writeBufLen = 0;
            this.readBufLen = 0;
            this.readBufCommitted = -1;
            if (stateObject != null) this.SetQueueState(stateObject as QueueState);
        }

        // rewinds to the beginning of the file for reading.
        public void Rewind()
        {
            this.readLogicalPos = 0;
            this.readBufPos = 0;
            this.readBufCommitted = -1;
            this.readBuf = null;
            this.readBufLen = 0;
        }

        /// <summary>
        /// When eof is set read with return whatever it has and not block. This
        /// needs to wake up any waiting readers.
        /// </summary>
        public bool Eof
        {
            get
            {
                return this._eofValue;
            }
            set
            {
                lock (this.commitLock)
                {
                    _eofValue = value;
                    Monitor.PulseAll(this.commitLock);
                }
            }
        }
        private bool _eofValue = false;


        // flag indicates if we should persist all writes
        public readonly bool IsPersistent;
        // pos in file of next byte to be written
        private long writeLogicalPos;
        // position of last commited byte, written to by write(), read by read().
        private object commitLock = new object();
        private long _writeCommitPosValue;
        private long writeCommitPos
        {
            get { lock (this.commitLock) { return _writeCommitPosValue; } }
            set { lock (this.commitLock) { _writeCommitPosValue = value; Monitor.Pulse(this.commitLock); } }
        }
        // write buffer
        private byte[] writeBuf;
        // logical position in file of the first byte of the write buffer.
        private long writeBufLogicalPos;
        // length of the write buffer (same as writeBuf.Length).
        private int writeBufLen;
        // size of first write buffer
        private int writeBufInc;
        // increment to increase the size of the next write buffer
        private int writeBufStartBytes;
        // max size of the write buffer
        private int writeBufMax;
        // position in buffer of next byte to write
        private int writeBufPos;
        // number of bytes that can be written to the current buffer
        private int writeBufLeft { get { return this.writeBufLen - this.writeBufPos; } }
        // number of bytes we used in the write buffer same as the position.
        private int writeBufUsed { get { return this.writeBufPos; } }
        // preferred size to read, might be less
        private int preferedReadSize;
        // logical position of first byte in read buffer.
        private long readBufLogicalPos;
        // logical file positon for next byte to read
        private long readLogicalPos;
        // read buffer
        private byte[] readBuf;
        // last readable offset in buffer
        private int readBufLen;
        // last commited readable offset in buffer.
        private int readBufCommitted;
        // position of next byte to read from buffer
        private int readBufPos;
        // bytes in read buffer we're allowed to read.
        private int readBufLeft { get { return this.readBufCommitted - this.readBufPos + 1; } }


        /// <summary>
        /// If this file was closed, resume update stateful info
        /// </summary>
        private void SetQueueState(QueueState queueState)
        {

            this.Blocking = queueState.blocking;
            this.writeCommitPos = queueState.writeCommitPos;
            this.readLogicalPos = queueState.readLogicalPos;
            this.writerSkipBytes = queueState.writeSkipBytes;
            this.writeBufLogicalPos = this.writeCommitPos + 1; // needed or read doesn't work.
            //logger.Debug("resumed state for queue file={0} commitThru={1} nextRead={2} writeSkip={3}", this.fileName, writeCommitPos, readLogicalPos,writerSkipBytes);

            FileInfo fileInfo;
            try
            {
                fileInfo = new FileInfo(this.fileName);
            }
            catch (Exception e)
            {
                string msg = "Cannot get file info for file [" + this.fileName + "]: " + e.Message;
                logger.Fatal(msg);
                throw new Exception(msg, e);
            }


            // if the file exists, assume everything in the file is commited and nothing is read
            if (fileInfo.Exists)
            {
                this.writeCommitPos = fileInfo.Length - 1;
                this.readLogicalPos = 0;
                this.writerSkipBytes = 0;
                //logger.Debug("The file {0} has no saved state so assume reading from beginning pos={1} and committed through last byte offset {2}:" + this.fileName, this.readLogicalPos,this.writeCommitPos);
                return;
            }

            // otherwise this is a new file so byte offset 0 is not even commited.
            this.writeCommitPos = -1;
            this.readLogicalPos = 0;
            this.writerSkipBytes = 0;
            //logger.Debug("intial state for queue file={0} commitThru={1} nextRead={2} writeSkip={3}", this.fileName, writeCommitPos, readLogicalPos,writerSkipBytes);

        }

        /// <summary>
        /// Saves the queue state for when a file is flushed.
        /// </summary>
        private QueueState GetQueueState()
        {
            QueueState queueState = new QueueState() { writeCommitPos = this.writeCommitPos, readLogicalPos = this.readLogicalPos, writeSkipBytes = this.writerSkipBytes, blocking = this.Blocking };
            logger.Debug("saving state for queue file={0} commitThru={1} nextRead={2},writeSkip={3},blocking={4}", this.fileName, writeCommitPos, readLogicalPos, this.writerSkipBytes, this.Blocking);
            return queueState;
        }


        // Only instanciate the fileStream when we need it. Make sure this is synchronous.
        private object _fileStreamLock = new object();
        private FileStream _fileStreamValue;
        private FileStream fileStream
        {
            get
            {
                if (this._fileStreamValue == null)
                {
                    lock (this._fileStreamLock)
                    {
                        if (this._fileStreamValue == null)
                        {
                            // TBD: get unbuffered file to work!!
                            //this._fileStreamValue=UnbufferedFileLoader.GetUnbufferedFileStream(this.fileName,FileAccess.ReadWriteNext,FileShare.ReadWriteNext,FileMode.Append);

                            FileInfo fileInfo = new FileInfo(this.fileName);
                            fileInfo.Directory.CreateIfNotThere();
                            this._fileStreamValue = new FileStream(
                                this.fileName,
                                FileMode.OpenOrCreate,
                                FileAccess.ReadWrite,
                                FileShare.ReadWrite
                                );
                        }
                    }
                }
                return this._fileStreamValue;
            }
        }
        // close file stream, returning true if it was open
        private bool CloseFileStream()
        {
            if (this._fileStreamValue != null)
            {
                lock (this._fileStreamLock)
                {
                    this._fileStreamValue.Close();
                    this._fileStreamValue = null;
                    return true;
                }
            }
            return false;
        }

        public long WritePosition
        {
            get
            {
                return this.writeLogicalPos;
            }
        }
        public long ReadPosition
        {
            get
            {
                return this.readLogicalPos;
            }
        }


        /// <summary>
        /// Provides an audit point up to where the reader is able to read
        /// </summary>
        public void Commit()
        {
            this.writeCommitPos = this.writeLogicalPos - 1;
            //logger.Debug(this.fileName + " committed thru byte " + this.writeCommitPos);
        }

        /// <summary>
        /// Closing will flush to disk and close real filestream
        /// </summary>
        public object FlushToFile(bool keepState)
        {
            //logger.Debug("Flushing file {0} writeLogicalPos={1}, readLogicalPos={2}, writeCommitPos={3} persistent={4} partiallyCommit={5} partiallyRead={6} ", this.fileName, this.writeLogicalPos, this.readLogicalPos, this.writeCommitPos,this.IsPersistent,this.IsPartiallyCommitted,this.IsPartiallyRead);

            // if keep state is false, just close the file handle and return null
            if (!keepState)
            {
                this.CloseFileStream();
                return null;
            }


            // If file is partially commited or partially read, we need to saved the state in memory so 
            // we can resume it properly when it is opened.
            object state = null;
            if (this.IsPartiallyCommitted || this.IsPartiallyRead)
            {
                state = this.GetQueueState();
            }
            else
            {
                //logger.Debug("skip saving state on {0} writeLogicalPos={1}, readLogicalPos={2}, writeCommitPos={3} ", this.fileName, this.writeLogicalPos, this.readLogicalPos, this.writeCommitPos);
            }



            // if the file is partially commited, or partially read, or persistent, we need to make sure
            // the write buffer is flushed and a file is fully persisted on disk.
            if (this.IsPartiallyCommitted || this.IsPartiallyRead || this.IsPersistent)
            {
                // flush the write buffer
                this.FlushWriteBuffer();

                // if the filestream is open close it or create an empty file if it is not there.
                if (!this.CloseFileStream())
                {
                    FileInfo f = new FileInfo(this.fileName);
                    f.Directory.CreateIfNotThere();
                    if (!f.Exists)
                    {
                        //logger.Debug("creating empty persistent file: " + f.FullName);
                        f.Create().Close();
                    }
                }
            }

            // otherwise the file is temporary, so close it if it is open and delete it
            // if it exists.
            else
            {
                // if the filestream is open close it.
                this.CloseFileStream();
                // if the file exists delete it.
                FileInfo f = new FileInfo(this.fileName);
                if (f.Exists)
                {
                    //logger.Debug("deleting temporary file: " + f.FullName);
                    f.Delete();
                }
                else
                {
                    //logger.Debug("temporary file never existed: " + f.FullName);
                }
            }
            return state;
        }

        public bool IsPartiallyCommitted
        {
            // if the last byte we wrote is greater than the last committed byte, then the file is partially committed
            get { return (this.writeLogicalPos - 1) > this.writeCommitPos; }
        }

        public bool IsPartiallyRead
        {
            // if the last byte we read is before the last committed byte, then the file is partially read
            get { return (this.readLogicalPos - 1) < this.writeCommitPos; }
        }


        /// <summary>
        /// Writes to the QueueBufferedFile
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public void Write(byte[] buffer, int offset, int count)
        {
            //logger.Debug("write({0})", count);
            if (count > buffer.Length - offset) throw new Exception("count extends past buffer end");
            while (count > 0)
            {
                // count fits in buffer, write it to buffer
                if (count <= this.writeBufLeft)
                {
                    Buffer.BlockCopy(buffer, offset, writeBuf, writeBufPos, count);
                    this.AdvanceWritePos(count);
                    return;
                }
                // otherwise count extends past buffer, fill remaining buffer if any
                if (this.writeBufLeft > 0)
                {
                    Buffer.BlockCopy(buffer, offset, writeBuf, this.writeBufPos, this.writeBufLeft);
                    count -= this.writeBufLeft;
                    offset += this.writeBufLeft;
                    this.AdvanceWritePos(this.writeBufLeft);
                }
                // try to persist current buffer if needed
                this.TryPersistWriteBuffer();
                // get a new one
                this.GetNextWriteBuffer();
            }
        }

        /// <summary>
        /// Persists the write buffer to disk if needed.
        /// </summary>
        private void TryPersistWriteBuffer()
        {
            if (this.writeBufUsed > 0)
            {
                // if not persisting writes and reader is point to my buffer, i can skip persisting
                bool skipWrite = false;
                if (!IsPersistent)
                {
                    lock (this.nextBufferLock)
                    {
                        if (this.readBufLen > 0 && this.writeBufLogicalPos == this.readBufLogicalPos)
                        {
                            skipWrite = true;
                            this.writerSkipBytes += this.writeBufLen;
                        }
                    }
                }
                if (!skipWrite)
                {
                    this.FlushWriteBuffer();
                }
            }
        }

        private int FlushWriteBuffer()
        {
            if (this.writeBufUsed > 0)
            {
                lock (this.fileStream)
                {
                    this.fileStream.Seek(0, SeekOrigin.End);
                    //logger.Debug("persisting {0} bytes starting on position {1} in {2}", this.writeBufUsed, this.fileStream.Position,this.fileName);
                    //string bufstring = Encoding.ASCII.GetString(this.writeBuf);
                    this.fileStream.Write(this.writeBuf, 0, this.writeBufUsed);
                    this.fileStream.Flush(); // need to flush so reader can read.
                }
            }
            return this.writeBufUsed;
        }

        private void AdvanceWritePos(int count)
        {
            this.writeBufPos += count;
            this.writeLogicalPos += count;
        }

        // a lock for when the reader points to the streamWriter's buffer.
        private object nextBufferLock = new object();

        // number of bytes the streamWriter skipped b/c reader was already pointing to streamWriter's buffer
        private long writerSkipBytes = 0;

        /// <summary>
        /// Updates the write buffer to the next block
        /// RULES:
        /// streamWriter should not create a block that goes past a multiple of increment.
        /// </summary>
        private void GetNextWriteBuffer()
        {
            lock (this.nextBufferLock)
            {
                if (this.writeBufLen == 0)
                {
                    this.writeBufLen = this.writeBufStartBytes;
                    //logger.Debug("set initial write buffer to {0}", this.writeBufLen);
                }
                else
                {
                    if (this.writeBufLen > this.writeBufMax)
                    {
                        this.writeBufLen = this.writeBufMax;
                        //logger.Debug("decreasing write buffer to {0}", this.writeBufLen);
                    }
                    else if (this.writeBufLen < this.writeBufMax)
                    {
                        this.writeBufLen += this.writeBufInc;
                        //logger.Debug("increasing write buffer by {1} to {0}", this.writeBufLen,this.writeBufInc);
                    }
                }
                this.writeBuf = new byte[this.writeBufLen];
                this.writeBufPos = 0;
                this.writeBufLogicalPos = this.writeLogicalPos;
            }
        }


        /// <summary>
        /// Updates the read buffer to the next block.
        /// RULES: 
        /// if streamWriter has a block && reader.block==streamWriter.block
        ///          point reader to streamWriter
        /// else
        ///          instanciate reader block from disk but do not let it go past streamWriter's position
        /// </summary>
        private void GetNextReadBuffer()
        {
            lock (this.nextBufferLock)
            {
                // update the read buffer logical position
                this.readBufLogicalPos = this.readLogicalPos;
                // always read from begining of buffer
                this.readBufPos = 0;
                if (this.writeBufLen > 0 && this.readBufLogicalPos == this.writeBufLogicalPos)
                {
                    this.readBuf = this.writeBuf;
                    this.readBufLen = this.readBuf.Length;
                }
                else
                {
                    long allowedToRead = this.writeBufLogicalPos - this.readLogicalPos;
                    if (allowedToRead > 0)
                    {
                        if (allowedToRead > this.preferedReadSize) allowedToRead = preferedReadSize;
                        this.readBuf = new byte[allowedToRead];
                        lock (this.fileStream)
                        {
                            long readPhysicalPosition = this.readLogicalPos - this.writerSkipBytes;
                            fileStream.Seek(readPhysicalPosition, SeekOrigin.Begin);
                            //logger.Debug("reading {0} bytes from logical pos {1} physical pos {2} in {3}", this.readBuf.Length, this.readLogicalPos, readPhysicalPosition,this.fileName);
                            this.readBufLen = fileStream.Read(this.readBuf, 0, this.readBuf.Length);
                            if (this.readBufLen != allowedToRead) throw new Exception("unexpected");
                        }
                    }
                    else
                    {
                        this.readBufLen = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Sets <code>this.readBufCommited</code> based on <code>this.writeCommitPos</code>
        /// </summary>
        private void SetReadBufCommitted()
        {
            long commitPos = this.writeCommitPos; // locking op
            long lastReadBufPos = this.readBufLogicalPos + this.readBufLen - 1;
            // commit pos >= last logical pos in read buffer, whole buffer is commited
            if (commitPos >= lastReadBufPos)
            {
                this.readBufCommitted = this.readBufLen - 1;
            }
            // commit pos >= first pos logical pos in read buffer, part of buffer committed
            else if (commitPos >= this.readBufLogicalPos)
            {
                this.readBufCommitted = (int)(commitPos - this.readBufLogicalPos);
            }
            else
            {
                this.readBufCommitted = -1;
            }
        }




        /// <summary>
        /// Reads data from the file (or buffers)
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public int Read(byte[] buffer, int offset, int count)
        {
            //logger.Debug("read({0})", count);
            if (count <= 0) return 0;
            int need = count;           // how much we need to read
            int read = 0;               // how much we have read
            for (; ; )
            {
                // No data left in read buffer
                if (this.readBufLeft <= 0)
                {
                    // if read position is at end of readbuffer, we need the next buffer
                    // otherwise, are waiting data in current buffer to commit.
                    if (this.readBufPos > (this.readBufLen - 1))
                    {
                        this.GetNextReadBuffer();
                    }
                    this.SetReadBufCommitted();
                    // if there is still not data return
                    if (this.readBufLeft <= 0)
                    {
                        // if non blocking reads, return what we read.
                        // assume we're at eof if nto blocking.
                        if (!this.Blocking)
                        {
                            this.Eof = true;
                            return read;
                        }
                        // otherwise, we need to wait on a write or eof. 
                        // when we resume, we want to check for eof
                        // and  reevaluate the commited spot.
                        lock (this.commitLock)
                        {
                            // now that we're synchronous, double check
                            this.SetReadBufCommitted();
                            if (this.readBufLeft <= 0)
                            {
                                // if we are at eof, just return what we got
                                if (this.Eof) return read;
                                // otherwise, wait a commit or eof.
                                //logger.Debug("Waiting on data to commit to so I can read {0} more bytes in queue file {1}", need,this.fileName);
                                Monitor.Wait(this.commitLock);
                                //logger.Debug("Reader woke up");
                                continue;
                            }
                        }
                    }
                }
                // Read buffer can fulfill our needs
                if (need <= this.readBufLeft)
                {
                    Buffer.BlockCopy(this.readBuf, this.readBufPos, buffer, offset, need);
                    read += need;
                    this.AdvanceReadPos(need);
                    return read;
                }
                // Read buffer can partially satisfy our needs
                else
                {
                    int readSize = this.readBufLeft;
                    Buffer.BlockCopy(this.readBuf, this.readBufPos, buffer, offset, readSize);
                    read += readSize;
                    need -= readSize;
                    offset += readSize;
                    this.AdvanceReadPos(readSize);
                }
            }
        }

        private void AdvanceReadPos(int count)
        {
            this.readBufPos += count;
            this.readLogicalPos += count;
        }


    }
    /// <summary>
    /// Class for reading and writing to the BufferedQueueFile
    /// </summary>
    public class QueueBufferedFileStream : Stream
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        public readonly QueueBufferedFile fileBuffer;

        public QueueBufferedFileStream(QueueBufferedFile fileBuffer)
        {
            this.fileBuffer = fileBuffer;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return this.fileBuffer.Read(buffer, offset, count);
        }
        public override void Write(byte[] buffer, int offset, int count)
        {
            fileBuffer.Write(buffer, offset, count);
        }
        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Flush()
        {
        }

        public override long Length
        {
            get { throw new NotSupportedException(); }
        }
        public override long Position
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }
    }
}
