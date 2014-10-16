using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace MVM
{
    public class DictionaryCursor<K, V> : CursorCommonLinqEnabled, ICursor
    {
        public string csrOid;
        public string cursorKeyName;
        public string cursorValueName;
        public IDictionary<K,V> dic;
        public List<K> keyList;
        //public List<string> orderedFieldNames=new List<string>();
        public int keyListIdx = -1;
        public DictionaryCursor(ModuleContext mc, CursorSetupCommon cursorSetup, string cursorKeyName, string cursorValueName, IDictionary<K, V> dic)
            : base(mc, cursorSetup)
        {
            this.orderedFieldNames=new List<string>{cursorKeyName,cursorValueName};
            this.dic = dic;
            this.keyList = dic.Keys.ToList();
        }

            
        public override IObjectData CursorNext()
        {
            
                keyListIdx++;
                if (keyListIdx < this.keyList.Count)
                {
                    K key = this.keyList[keyListIdx];
                    string keyString = key.ToString();
                    string valString = dic[key].ToString_Safe();
                    using (var csrObj = this.CreateNewObject())
                    {
                        csrObj[this.orderedFieldNames[0]] = keyString;
                        csrObj[this.orderedFieldNames[1]] = valString;
                        return csrObj;
                    }
                }
                else
                {
                    return null;
                }
            
        }

        public override void CursorClear()
        {
// nothing to clear
        }

    }
}
