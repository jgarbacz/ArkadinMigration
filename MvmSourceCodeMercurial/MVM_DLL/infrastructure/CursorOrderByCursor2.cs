using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MVM
{
    public class CursorOrderByCursor2 : CursorCommonOp
    {
        public IReadString inputCursorInstIdParsed;
        private List<IReadString> orderBysParsed;
        private int orderBysCount;

        public MergeSortReaderWriter<DynamicKey, IObjectData> rows;

        private IEnumerator<KeyValuePair<DynamicKey, IObjectData>> _myReaderEnumerator = null;
        private IEnumerator<KeyValuePair<DynamicKey, IObjectData>> myReaderEnumerator
        {
            get
            {
                if (this._myReaderEnumerator == null)
                {
                    this._myReaderEnumerator = rows.GetEnumerator();
                }
                return this._myReaderEnumerator;
            }

        }

        public IMergeableComparer<DynamicKey> dynamicKeyComparer;
        public ISerializer<DynamicKey> dynamicKeySerializer;
        public IDynamicParser[] dynamicParsers;
        public int maxOpenFiles;
        public int maxLiveObjects;
        public string tempPath;
        /// <summary>
        /// Sets up the cursor, including how rows should be sorted.
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="orderedFieldNames"></param>
        /// <param name="cursorOid"></param>
        /// <param name="objectArrayComparer"></param>
        public CursorOrderByCursor2(
            ModuleContext mc,
            CursorSetupCommon cursorSetup,
            List<IReadString> orderBysParsed,
            int maxOpenFiles,
            int maxLiveObjects,
            string tempPath,
            IMergeableComparer<DynamicKey> dynamicKeyComparer,
            ISerializer<DynamicKey> dynamicKeySerializer,
            IDynamicParser[] dynamicParsers
            )
            : base(mc, cursorSetup)
        {
            //logger.Info("CURSOR ORDER BY HAS OUTPUT CURSOR INST ID=" + this.CursorInstId);
            this.orderBysParsed = orderBysParsed;
            this.orderBysCount = this.orderBysParsed.Count;
            this.dynamicKeyComparer = dynamicKeyComparer;
            this.dynamicKeySerializer = dynamicKeySerializer;
            this.dynamicParsers = dynamicParsers;
            this.maxOpenFiles = maxOpenFiles;
            this.maxLiveObjects = maxLiveObjects;
            this.tempPath = tempPath;
            this.rows = new MergeSortReaderWriter<DynamicKey, IObjectData>(
                       this.maxOpenFiles,
                       this.maxLiveObjects,
                       this.tempPath,
                       "",
                       this.dynamicKeyComparer,
                       this.dynamicKeySerializer,
                       mc.mvm.objectDataSerializer
                       );
        }

        /// <summary>
        /// Adds a new row 
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="vals"></param>
        public void Add(string[] stringKeys, IObjectData obj)
        {
            // need to convert the keys
            object[] parsedKeys = new object[stringKeys.Length];
            for (int i = 0; i < dynamicParsers.Length; i++)
            {
                parsedKeys[i] = dynamicParsers[i].Parse(stringKeys[i]);
            }

            var kv = new KeyValuePair<DynamicKey, IObjectData>(new DynamicKey(parsedKeys), obj);
            this.rows.Add(kv, SortOrder.None);
            //logger.Info("CoC.Add(" + prevKeyValuePair.Value.Key.ToString() + "," + prevKeyValuePair.Value.Value.objectId + ") refCnt=" + prevKeyValuePair.Value.Value.RefCount);
        }      


        // cursor order by states.
        // 0: first time it is called it always returns PARENT_NEXT
        // 1: next N times it is called with a non null input object, buffer it and return PARENT NEXT
        // 2: if it is called with null input object sort the buffer and goto 3
        // 3: if i have a row left in the buffer give it out and return HAS ROW otherwise return EOF
        public enum State { First, BufferInputObjects, SortInputObjects, ReturnNextRow, ReturnEof };
        public State state;
        public override CursorStatus CursorNext(ModuleContext mc, IObjectData inputObj, out IObjectData outputObj)
        {
            outputObj = null;
            for (; ; )
            {
            NEXT_STATE:
                switch (state)
                {
                    case State.First:
                        {
                            state = State.BufferInputObjects;
                            return CursorStatus.PARENT_NEXT;
                        }
                    case State.BufferInputObjects:
                        {
                            if (inputObj == null)
                            {
                                state = State.SortInputObjects;
                                goto NEXT_STATE;
                            }
                            else
                            {
                                // temporarily make the current object the input object
                                var currentObject = mc.objectData;
                                mc.objectData = inputObj;

                                // extract the order by fields
                                inputObj.CursorInstId = this.CursorInstId;
                                string[] orderByFields = new string[this.orderBysCount];
                                for (int i = 0; i < this.orderBysCount; i++)
                                {
                                    orderByFields[i] = orderBysParsed[i].Read(mc);
                                }
                                this.Add(orderByFields, inputObj);

                                // restore the current object
                                mc.objectData = currentObject;

                                // try to get more rows from the parent cursor
                                return CursorStatus.PARENT_NEXT;
                            }
                        }
                    case State.SortInputObjects:
                        {
                            //this.AddPrevious();
                            state = State.ReturnNextRow;
                            goto NEXT_STATE;
                        }
                    case State.ReturnNextRow:
                        {
                            // return rows while we have them
                            if (!this.myReaderEnumerator.MoveNext())
                            {
                                return CursorStatus.EOF;
                            }
                            outputObj = this.myReaderEnumerator.Current.Value;
                            // now that we got our object back we need to put it back in
                            // the object cache.
                            this.mvm.objectCache.AddOrMergeObject(outputObj);
                            //logger.Info("CoC.ReturnNextRow:" + outputObj.objectId + "," + outputObj.RefCount);
                            return CursorStatus.HAS_ROW;
                        }
                }
            }
        }

        protected override void CursorClear(ModuleContext mc)
        {
            // remove refs to leftover buffered objects.
            while (this.myReaderEnumerator.MoveNext())
            {
                var outputObj = this.myReaderEnumerator.Current.Value;
                outputObj.RefRemove();
            }
            // dispose the enumerator
            this.myReaderEnumerator.Dispose();
            this._myReaderEnumerator = null;
        }
    }
}
