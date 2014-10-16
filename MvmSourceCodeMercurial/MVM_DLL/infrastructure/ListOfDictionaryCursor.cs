using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace MVM
{
    public class ListOfDictionaryCursor : CursorCommonLinqEnabled, ICursor
    {
        public List<IDictionary<string,string>> dic;
        //public List<string> orderedFieldNames=new List<string>();
        public int listIdx = -1;
        public ListOfDictionaryCursor(ModuleContext mc, CursorSetupCommon cursorSetup, List<IDictionary<string, string>> dic)
            : base(mc, cursorSetup)
        {           
            this.dic = dic;
            if(this.dic.Count > 0){
                this.orderedFieldNames=this.dic[0].Keys.ToList();
            }
        }

        #region ICursor Members
        public override void CursorClear()
        {
            // no resources to free
        }
        public override IObjectData CursorNext()
        {
            listIdx++;
            if (listIdx < this.dic.Count)
            {
                using (var csrObj = this.CreateNewObject())
                {
                    foreach (var kv in this.dic[listIdx])
                    {
                        csrObj[kv.Key]=kv.Value;
                    }
                    return csrObj;
                }
            }
            else
            {
                return null;
            }
        }

       

       
        //public List<string> GetOrderedFieldNames()
        //{
        //    return this.orderedFieldNames;
        //}

        #endregion
    }
}
