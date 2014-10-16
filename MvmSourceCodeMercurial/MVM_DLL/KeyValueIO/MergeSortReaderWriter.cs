using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NLog;
using System.Diagnostics;
namespace MVM
{
    public enum SortOrder { Forward, Reverse, None }

    public class NaturalMergeSortReaderWriter<K, V> : MergeSortReaderWriter<K, V>
        where K : IBinarySerializableComparable<K>, new()
        where V : ISerializable<V>, new()
    {
        public static readonly IComparer<K> defaultComparer =  new DefaultLiveComparer<K>();
        public NaturalMergeSortReaderWriter(int maxOpenFiles, int maxLiveObjects, string tempDir, IMergeableComparer<K> comparer, ISerializer<K> keySerializer, ISerializer<V> valueSerializer)
            : base(
                maxOpenFiles, maxLiveObjects, tempDir, "", (comparer!=null?comparer:MergeableComparer<K>.Default), (keySerializer != null ? keySerializer : Serializabler<K>.Default), (valueSerializer != null ? valueSerializer : Serializabler<V>.Default))
        {
        }
    }
    
    /// <summary>
    /// Provides an abstraction layer maintaining a sorted queue that can 
    /// overflow memory.
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class MergeSortReaderWriter<K, V> : MergeSortReader<K, V>
    {
        private static int tempCtr = 0;

        public MergeableSortingQueue<K, V> sortingQueue; // order = none
        public IMergeEnumerator<K,V> sortingEnumerator { get { return this.sortingQueue.GetMergeEnumerator(); } }
        public Dictionary<string,MergeableFile<K, V>> sortedFiles = new Dictionary<string,MergeableFile<K, V>>(); // as we sort and cut files, they go here

        public MergeableQueue<K, V> sortedQueue; // order = forward
        public IMergeEnumerator<K,V> sortedEnumerator { get { return this.sortedQueue.GetMergeEnumerator(); } }
        public MergeableFile<K, V> forwardFile = null; // forward-sorted rows get appended here

        public MergeableStack<K, V> sortedQueueRev; // order = reverse
        public IMergeEnumerator<K, V> sortedEnumeratorRev { get { return this.sortedQueueRev.GetMergeEnumerator(); } }
        public MergeableFile<K, V> reverseFile = null; // reverse-sorted rows get appended here

        public string tempDir;
        public string prefix;
        public int maxOpenFiles;
        public int maxLiveObjects;
        public int LiveRowCount
        {
            get
            {
                return this.sortingQueue.Count + this.sortedQueue.Count + this.sortedQueueRev.Count;
            }
        }

        public IComparer<RawValue<K>> rawComparer { get { return this.comparer.RawComparer; } }

        public readonly IMergeableComparer<K> comparer;
        public readonly ISerializer<K> keySerializer;
        public readonly ISerializer<V> valueSerializer;

        public MergeSortReaderWriter(
            int maxOpenFiles,
            int maxLiveObjects,
            string tempDir,
            string prefix,
            IMergeableComparer<K> comparer,
            ISerializer<K> keySerializer,
            ISerializer<V> valueSerializer)
            : base(comparer.LiveComparer)
        {
            Debug.Assert(comparer != null);
            Debug.Assert(keySerializer != null);
            Debug.Assert(valueSerializer != null);
            Debug.Assert(tempDir != null);

            this.comparer = comparer;
            this.keySerializer =  keySerializer;
            this.valueSerializer = valueSerializer;
          
            this.maxLiveObjects = maxLiveObjects;
            this.maxOpenFiles = maxOpenFiles;
            this.tempDir = tempDir;
            this.prefix = prefix;
            this.sortedQueue = new MergeableQueue<K, V>();
            this.sortedQueueRev = new MergeableStack<K, V>();
            this.sortingQueue = new MergeableSortingQueue<K, V>(comparer.LiveComparer);
        }

        public string getTempFileName(SortOrder dir)
        {
            string d = "forward.";
            if (dir == SortOrder.Reverse)
            {
                d = "reverse.";
            }
            else if (dir == SortOrder.None)
            {
                d = "unsorted.";
            }
            return (this.prefix.IsNullOrEmpty() ? "" : this.prefix + ".") + d + tempCtr++.ToString();
        }

        // override the reader method so we can lazy sort whatever needs to be lazy sorted.
        public override void InstanciateEnumeratorHook()
        {
            lock (this)
            {
                //logger.Info("FIRING READER/WRITER INSTANCIATE HOOK!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                if (this.LazySorter.Count > 0)
                {
                    if (this.sortedQueue.Count > 0)
                    {
                        throw new Exception("not expecting a mix of lazy sorted data and sortedQueue data");
                    }

                    //logger.Info("lazy sorting {0} items",this.LazySorter.Count);
                    this.LazySorter.Sort(this.liveKeyValuePairComparer);
                    //logger.Info("adding {0} lazy sorting items", this.LazySorter.Count);
                    foreach (var elem in this.LazySorter)
                    {
                        this.Add(elem, SortOrder.Forward);
                    }
                    //logger.Info("done adding {0} lazy sorted items", this.LazySorter.Count);
                    this.LazySorter.Clear();
                    //logger.Info("done clearing lazy sorter");
                }
            }
        }

        public List<KeyValuePair<K, V>> LazySorter=new List<KeyValuePair<K,V>>();

        /// <summary>
        /// Adds an object to the merging enumerator. This may result in objects
        /// being spilled to disk.
        /// </summary>
        /// <param name="kv"></param>
        /// <param name="inOrder"></param>
        public void Add(KeyValuePair<K, V> keyValuePair, SortOrder inOrder)
        {
            //logger.Debug("RW.Add key=" + keyValuePair.Key.ToString());
            // if objects are being added in order use the sortedQueue
            // otherwise use the sortingQueue which sorts them in memory for us.
            if (inOrder == SortOrder.Forward)
            {
                // if the sorted queue is not in the sort tree be sure to add it.
                bool inMergeTree = this.sortedQueue.GetMergeEnumerator().HasNext;
                if (inMergeTree)
                {
                    this.sortedQueue.Add(keyValuePair);
                }
                else
                {
                    if (this.sortedQueue.IsDisposed)
                    {
                        this.sortedQueue.IsDisposed = false;
                        this.sortedQueue.CountConsumed = 0;
                    }
                    this.sortedQueue.Add(keyValuePair);
                    this.InsertMergeEnumerable(this.sortedQueue);
                }
            }
            else if (inOrder == SortOrder.Reverse)
            {
                // if the sorted stack is not in the sort tree be sure to add it.
                bool inMergeTree = this.sortedQueueRev.GetMergeEnumerator().HasNext;
                if (inMergeTree)
                {
                    //tbd - this thrashes on the merging btree... consider being lazy
                    // and not keeping the btree upto date.
                    this.RemoveMergeEnumerator(sortedEnumeratorRev);
                    this.sortedQueueRev.Add(keyValuePair);
                    this.InsertMergeEnumerator(sortedEnumeratorRev);
                }
                else
                {
                    if (this.sortedQueueRev.IsDisposed)
                    {
                        this.sortedQueueRev.IsDisposed = false;
                        this.sortedQueueRev.CountConsumed = 0;
                    }
                    this.sortedQueueRev.Add(keyValuePair);
                    this.InsertMergeEnumerable(this.sortedQueueRev);
                }
            }
            else if (inOrder == SortOrder.None)
            {
                // If we are not currently looping we can add the data to our lazySorter so that we 
                // simply append and sort only when someone gets an enumerator or we spill the lazy
                // sorter to disk.
                if (!this.LoopInProgress)
                {
                    this.LazySorter.Add(keyValuePair);
                    if (this.LazySorter.Count >= this.maxLiveObjects)
                    {
                        logger.Info("Sorting {0} lazy queued objects", this.LazySorter.Count);
                        this.LazySorter.Sort(this.liveKeyValuePairComparer);
                        logger.Info("Spilling {0} lazy queued objects", this.LazySorter.Count);
                        string spillFileName = Path.Combine(this.tempDir, this.getTempFileName(SortOrder.None));
                        var spillFile = new MergeableFile<K, V>
                            (Direction.Forward,
                            spillFileName,
                            this.LazySorter,
                            keySerializer,
                            valueSerializer);
                        this.sortedFiles[spillFile.FileName] = spillFile;
                        this.InsertMergeEnumerator(spillFile);
                        logger.Info("Done Spilling {0} lazy queued objects to {1}", this.LazySorter.Count, spillFileName);
                        this.LazySorter.Clear();
                        logger.Info("Done clearing lazy queue", this.LazySorter.Count);
                    }
                }
                else
                {
                    // Otherwise,if we are looping, we need to use a sorter that is upto date in realtime.
                    // if the sortingQueue is not in the sort tree be sure to add it.
                    bool inMergeTree = sortingEnumerator.HasNext;
				    if (inMergeTree)
                    {
                        var prevKeyValue = sortingEnumerator.Next;
                        bool needReinsert = this.liveComparer.Compare(keyValuePair.Key, prevKeyValue.Key) < 0;
                        if (needReinsert)
                            this.RemoveMergeEnumerator(sortingEnumerator);
                        this.sortingQueue.Add(keyValuePair);
                        if (needReinsert)
                            this.InsertMergeEnumerator(sortingEnumerator);
                    }
                    else
                    {
                        if (this.sortingQueue.IsDisposed)
                        {
                            this.sortingQueue.IsDisposed = false;
                            this.sortingQueue.CountConsumed = 0;
                        }
                        this.sortingQueue.Add(keyValuePair);
                        this.InsertMergeEnumerable(this.sortingQueue);
                    }
                }
            }
            else
            {
                throw new Exception("unhandled SortOrder=" + inOrder);
            }

            // if we exceeded the max number of objects we
            // can keep in memory, then we need to serialize them all 
            // to a sorted file. If this file would take us over the max files,
            // then we also need to merge the file with fewest number of objects
            // remaining.
            if (this.LiveRowCount >= this.maxLiveObjects)
            {
                logger.Info("Too many objects [" + this.LiveRowCount + "], serialize to file");

                // Figure out what we want to serialize -- the reverse queue, the forward queue, or the sorting queue
                SortOrder direction = SortOrder.Forward;
                if (this.sortingQueue.Count > this.sortedQueue.Count)
                {
                    direction = SortOrder.None;
                }
                if (this.sortedQueueRev.Count > this.sortingQueue.Count)
                {
                    direction = SortOrder.Reverse;
                }

                // remove the live queue and tree
                if (direction == SortOrder.Forward)
                {
                    if (!this.RemoveMergeEnumerator(this.sortedQueue.GetMergeEnumerator()))
                    {
                        throw new Exception("Unexpected 9012325");
                    }

                    if (this.forwardFile == null)
                    {
                        string file = Path.Combine(this.tempDir, this.getTempFileName(SortOrder.Forward));
                        logger.Info("spilling {0} objects to {1}", this.sortedQueue.Count, file);
                        this.forwardFile = new MergeableFile<K, V>(Direction.Forward, file, this.sortedQueue, keySerializer, valueSerializer);
                        if (this.forwardFile.HasNext)
                        {
                            this.InsertMergeEnumerator(this.forwardFile);
                        }
                        else
                        {
                            throw new Exception("unexpected 923439");
                        }
                        // doing this because to avoid going in the else. AddLiveRows to the forward file causes the forward
                        // file to forget where it was. Need to circle back with metraconvert to see if they are relying on this
                        // For AMP it is not good, you can't forget your place. More likely this is a bug in MC too, but phil
                        // sizes the chunks so he never hits it.
                        this.sortedFiles[this.forwardFile.FileName] = this.forwardFile;
                        this.InsertMergeEnumerator(this.forwardFile);
                        this.forwardFile = null;
                    }
                    else
                    {
                        logger.Info("spilling adding {0} objects to {1}", this.sortedQueue.Count, this.forwardFile.FileName);
                        this.forwardFile.addLiveRows(this.sortedQueue);
                        if (this.forwardFile.HasNext)
                        {
                            this.InsertMergeEnumerator(this.forwardFile);
                        }
                        else
                        {
                            throw new Exception("unexpected 9234399831");
                        }
                    }
                }
                else if (direction == SortOrder.Reverse)
                {
                    if (!this.RemoveMergeEnumerator(this.sortedQueueRev.GetMergeEnumerator()))
                    {
                        throw new Exception("Unexpected 9012325");
                    }

                    // FIXME: once we consume any rows from a reverse file, we cannot append to it any longer; it should then become
                    // eligible to be merged (like the sorting files)
                    this.sortedQueueRev.outputDirection = SortOrder.Reverse;
                    if (this.reverseFile == null)
                    {
                        string file = Path.Combine(this.tempDir, this.getTempFileName(SortOrder.Reverse));
                        this.reverseFile = new MergeableFile<K, V>(Direction.Reverse, file, this.sortedQueueRev, keySerializer, valueSerializer);
                        if (this.reverseFile.HasNext)
                        {
                            this.InsertMergeEnumerator(this.reverseFile);
                        }
                        else
                        {
                            throw new Exception("unexpected 923439");
                        }
                    }
                    else
                    {
                        this.RemoveMergeEnumerator(this.reverseFile);
                        this.reverseFile.addLiveRows(this.sortedQueueRev);
                        this.InsertMergeEnumerator(this.reverseFile);
                    }
                    this.sortedQueueRev.outputDirection = SortOrder.Forward;
                }
                else if (direction == SortOrder.None)
                {
                    if (!this.RemoveMergeEnumerator(this.sortingQueue.GetMergeEnumerator()))
                    {
                        throw new Exception("Unexpected 12348957");
                    }

                    // If we are over the max files...
                    // Find the file with the fewest rows remaining and add that files
                    // current row to the sorted tree. We need to do this because the
                    // the current row has already been deserialized and we are going
                    // to create a raw reader directly on all the rows that are still
                    // in the file.
                    MergeableFile<K, V> minCountFile = null;
                    if (this.sortedFiles.Count >= this.maxOpenFiles)
                    {
                        List<MergeableFile<K, V>> fullyConsumedFiles = new List<MergeableFile<K, V>>();
                        int minCount = int.MaxValue;
                        foreach (var f in this.sortedFiles.Values)
                        {
                            if (!f.HasNext)
                            {
                                fullyConsumedFiles.Add(f);
                            }
                            if (f.Count < minCount)
                            {
                                minCount = f.Count;
                                minCountFile = f;
                            }
                        }

                        if (fullyConsumedFiles.Count > 0)
                        {
                            minCountFile = null;
                            foreach (var consumedFile in fullyConsumedFiles)
                            {
                                if (!this.sortedFiles.Remove(consumedFile.FileName))
                                {
                                    throw new Exception("unexpected cannot remove FULLY CONSUMED file [" + consumedFile.FileName + "] from sortedFiles");
                                }
                            }
                        }
                        else
                        {
                            logger.Info("Too many files, merge objects smallest file: " + minCountFile.FileName);
                            if (!this.sortedFiles.Remove(minCountFile.FileName))
                            {
                                throw new Exception("unexpected cannot remove file [" + minCountFile.FileName + "] from sortedFiles");
                            }
                            this.RemoveMergeEnumerator(minCountFile);
                        }
                    }

                    string mergeFileName = Path.Combine(this.tempDir, this.getTempFileName(SortOrder.None));

                    // need to merge smallest file with sortingQueue
                    if (minCountFile != null)
                    {
                        RawMergeSortReader<K, V> rawMergeSortReader = new RawMergeSortReader<K, V>(this.rawComparer);
                        rawMergeSortReader.InsertMergeEnumerator(minCountFile.GetRawKeyValuePairEnumerator());
                        rawMergeSortReader.InsertMergeEnumerator(new LiveToRawEnumerator<K, V>(this.sortingQueue.GetMergeEnumerator(), keySerializer, valueSerializer));
                        MergeableFile<K, V> mergeFile = new MergeableFile<K, V>(Direction.Forward, mergeFileName, rawMergeSortReader, keySerializer, valueSerializer);
                        if (mergeFile.HasNext)
                        {
                            this.sortedFiles[mergeFile.FileName] = mergeFile;
                            this.InsertMergeEnumerator(mergeFile);
                        }
                        else
                        {
                            throw new Exception("unexpected 923439");
                        }
                    }
                    // just dump sorting queue to file
                    else
                    {
                        MergeableFile<K, V> mergeFile = new MergeableFile<K, V>(Direction.Forward, mergeFileName, this.sortingQueue, keySerializer, valueSerializer);
                        if (mergeFile.HasNext)
                        {
                            this.sortedFiles[mergeFile.FileName] = mergeFile;
                            this.InsertMergeEnumerator(mergeFile);
                        }
                        else
                        {
                            throw new Exception("unexpected 923439");
                        }
                    }
                }

                // we know we just cleared out a lot of memory so do GC
                System.GC.Collect();
            }
        }
        
        /// <summary>
        /// Flushes all objects to a single sorted file on disk.
        /// </summary>
        /// <param name="fileName"></param>
        public void FlushToFile(string fileName)
        {
            //logger.Info("Too many objects [" + this.LiveRowCount + "], serialize to file");
            // remove the live queue and tree
            this.InstanciateEnumeratorHook();
            if (this.sortedQueue.Count > 0)
            {
                if (!this.RemoveMergeEnumerator(this.sortedQueue.GetMergeEnumerator()))
                {
                    throw new Exception("Unexpected 9012325");
                }
            }
            if (this.sortedQueueRev.Count > 0)
            {
                if (!this.RemoveMergeEnumerator(this.sortedQueueRev.GetMergeEnumerator()))
                {
                    throw new Exception("Unexpected 12348957");
                }
            }
            if (this.sortingQueue.Count > 0)
            {
                if (!this.RemoveMergeEnumerator(this.sortingQueue.GetMergeEnumerator()))
                {
                    throw new Exception("Unexpected 12348957");
                }
            }

            // Instanciate a raw merger which we'll use to serialize to disk.
            RawMergeSortReader<K, V> rawMergeSortReader = new RawMergeSortReader<K, V>(this.rawComparer);

            // Add all the files
            foreach (var f in this.sortedFiles.Values)
            {
                rawMergeSortReader.InsertMergeEnumerator(f.GetRawKeyValuePairEnumerator());
            }
            if (this.sortedQueue.Count > 0)
            {
                var mergeEnumerator = this.sortedQueue.GetMergeEnumerator();
                rawMergeSortReader.InsertMergeEnumerator(new LiveToRawEnumerator<K, V>(mergeEnumerator,keySerializer,valueSerializer));
            }
            if (this.sortedQueueRev.Count > 0)
            {
                var mergeEnumerator = this.sortedQueueRev.GetMergeEnumerator();
                rawMergeSortReader.InsertMergeEnumerator(new LiveToRawEnumerator<K, V>(mergeEnumerator, keySerializer, valueSerializer));
                throw new Exception("Not implemented yet: need to reverse the sorted stack.");
            }
            if (this.sortingQueue.Count > 0)
            {
                var mergeEnumerator = this.sortingQueue.GetMergeEnumerator();
                rawMergeSortReader.InsertMergeEnumerator(new LiveToRawEnumerator<K, V>(mergeEnumerator, keySerializer, valueSerializer));
            }
            MergeableFile<K, V> mergeFile = new MergeableFile<K, V>(Direction.Forward, fileName, rawMergeSortReader, keySerializer, valueSerializer);
            mergeFile.Close();
        }
    }
}
