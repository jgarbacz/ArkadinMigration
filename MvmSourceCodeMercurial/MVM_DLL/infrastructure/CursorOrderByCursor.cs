using System;
using System.Collections.Generic;

using System.Text;

namespace MVM
{
    public class CursorOrderByRow
    {
        public object[] keys;
        public IObjectData obj;
        public CursorOrderByRow(object[] keys, IObjectData obj)
        {
            this.keys=keys;
            this.obj = obj;
        }
    }

    public class CursorOrderByRowComparer :IComparer<CursorOrderByRow>
    {
        private IComparer<object[]> objectArrayComparer;
        public CursorOrderByRowComparer(IComparer<object[]> objectArrayComparer)
        {
            this.objectArrayComparer = objectArrayComparer;
        }
        public int Compare(CursorOrderByRow x, CursorOrderByRow y)
        {
            return objectArrayComparer.Compare(x.keys, y.keys);
        }
    }

    public class CursorOrderByCursor : CursorCommonOp
    {
        public List<CursorOrderByRow> rows = new List<CursorOrderByRow>();
        public CursorOrderByRowComparer rowComparer;
        private int rowIdx;
        public IReadString inputCursorInstIdParsed;
        private List<IReadString> orderBysParsed;
        private int orderBysCount;

        /// <summary>
        /// Sets up the cursor, including how rows should be sorted.
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="orderedFieldNames"></param>
        /// <param name="cursorOid"></param>
        /// <param name="objectArrayComparer"></param>
        public CursorOrderByCursor(ModuleContext mc, CursorSetupCommon cursorSetup, List<IReadString> orderBysParsed, IComparer<object[]> objectArrayComparer)
            : base(mc, cursorSetup)
        {
            //logger.Info("CURSOR ORDER BY HAS OUTPUT CURSOR INST ID=" + this.CursorInstId);
            this.orderBysParsed = orderBysParsed;
            this.orderBysCount = this.orderBysParsed.Count;
            this.rowIdx = -1;
            this.rowComparer = new CursorOrderByRowComparer(objectArrayComparer);
        }
            
        /// <summary>
        /// Adds a new row 
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="vals"></param>
        public void Add(object[] keys, IObjectData obj)
        {
            // when we store the object, we need to hold a reference to it otherwise
            // it could get GCed.
            obj.RefGet();
            rows.Add(new CursorOrderByRow(keys, obj));
        }

        /// <summary>
        /// Sorts the rows structure using comparers from constructor.
        /// </summary>
        public void Sort(ModuleContext mc)
        {
            this.rows.Sort(rowComparer);
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
                                object[] orderByFields = new object[this.orderBysCount];
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
                            this.Sort(mc);
                            state = State.ReturnNextRow;
                            goto NEXT_STATE;
                        }
                    case State.ReturnNextRow:
                        {
                            // return rows while we have them
                            if (++rowIdx >= rows.Count)
                            {
                                return CursorStatus.EOF;
                            }
                            outputObj = this.rows[rowIdx].obj;
                            // remove the buffer ref, but do it without 
                            // any chance of refCtr hitting zero because we
                            // know for certain that the cursor is going to
                            // do refCtr++ right after this. This make me think
                            // that subclasses like this should be responsible
                            // for updating the cursor object.
                            outputObj.UnsafeRefRemoveNoDelete();
                            return CursorStatus.HAS_ROW;
                        }
                }
            }
        }

        protected override void CursorClear(ModuleContext mc)
        {
            // remove refs to leftover buffered objects.
            if (this.rows != null)
            {
                while(++rowIdx < rows.Count)
                {
                    this.rows[rowIdx].obj.RefRemove();
                }
                this.rows = null; // not necessary but good for gc
            }
}
        }
}
