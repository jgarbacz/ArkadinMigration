using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
namespace MVM
{
    public class TempContext
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();

        // we are not using this now and certainly we won't serialize it here.
        // we might want the ref here so we can scan up the hierarcy of scopes looking for 
        // existence of a value.
        public ProcContext procContext;
       
        // stores the symbols that have been added to a scope level
        // scopedFields[i]=(fieldName1,fieldName2)
        private Stack<Dictionary<string,bool>> scopedFields;

        // stores the value of the field. right now we don't support redefining values, only undefining them.
        // fields[fieldName1]=value
        public Dictionary<string, string> fields;

        // This is the number of pushes we need to do to catchup. We start out by needing to catchup 1
        // since the starting scope is 1 level deep. Elsewhere in the code I am pushing scope before
        // i ever write to it. This is unnecessary now. Try to find out where I call the constructor
        // and immediately push scope and stop doing that.
        private int catchupPushes = 1;

        // constructor
        public TempContext(ProcContext procContext)
        {
            this.procContext = procContext;
            //logger.Info("NEW_SCOPE, scopeFields.Count=" + (this.scopedFields != null ? this.scopedFields.Count : 0) + ",catchupPushes=" + this.catchupPushes + ", total=" + ((this.scopedFields != null ? this.scopedFields.Count : 0) + this.catchupPushes));
           
        }
        public TempContext() { 
        }

        // Forking the context does a deep copy. It also flattens the context b/c it is 
        // assumed that you'll never need to pop up higher than when you forked.
        public TempContext Fork()
        {
            TempContext tc = new TempContext();
            if (this.fields == null) return tc;
            foreach (string field in this.fields.Keys)
            {
                    tc[field] = this[field];
            }
            return tc;
        }

        // maintains lexical scope
        public void PushScope()
        {
            this.catchupPushes++;
            //logger.Info("PUSH_SCOPE, scopeFields.Count=" + (this.scopedFields != null ? this.scopedFields.Count : 0) + ",catchupPushes=" + this.catchupPushes + ", total=" + ((this.scopedFields != null ? this.scopedFields.Count : 0) + this.catchupPushes));
           
        }

        // maintains lexical scope
        public void PopScope()
        {
            if (this.catchupPushes > 0)
            {
                //logger.Info("Just dcr");
                this.catchupPushes--;
            }
            else
            {
                //logger.Info("pop before:" +this.scopedFields.Count);
                var fieldNameList = this.scopedFields.Pop();
                if (fieldNameList != null)
                {
                    foreach (string field in fieldNameList.Keys)
                    {
                        //logger.Info("out of scope:" + field);
                        fields.Remove(field);
                    }
                }
                //logger.Info("pop after:" + this.scopedFields.Count);
            }
            //logger.Info("POP_SCOPE, scopeFields.Count=" + (this.scopedFields != null ? this.scopedFields.Count : 0) + ",catchupPushes=" + this.catchupPushes + ", total=" + ((this.scopedFields != null ? this.scopedFields.Count : 0) + this.catchupPushes));
        }



        public int ScopeDepth
        {
            get
            {
                return (this.scopedFields!=null?this.scopedFields.Count:0) + this.catchupPushes;
            }
            set
            {
                while (this.ScopeDepth > value)
                {
                    this.PopScope();
                }
            }
        }
        
        


        // gets or set a tempContext field
        public string this[string fieldName]
        {
            get
            {
                if (this.fields == null) return "";
                string output;
                if (this.fields.TryGetValue(fieldName, out output)) return output;
                return "";
            }
            set
            {
                try
                {
                    if (this.fields == null)
                    {
                        scopedFields = new Stack<Dictionary<string, bool>>();
                        fields = new Dictionary<string, string>();
                        //logger.Info("[temp]first write, scopeFields.Count=" + (this.scopedFields != null ? this.scopedFields.Count : 0) + ",catchupPushes=" + this.catchupPushes + ", total=" + ((this.scopedFields != null ? this.scopedFields.Count : 0) + this.catchupPushes));
                    }
                   
                    // if this is the first time we've written to this field added it to the scopedFields so we know when to take it out of scope
                    if (!this.fields.ContainsKey(fieldName))
                    {
                        if (this.catchupPushes > 0)
                        {
                            for (; this.catchupPushes > 1; this.catchupPushes--)
                            {
                                this.scopedFields.Push(null);
                                //logger.Info("[temp]catchup nulls, scopeFields.Count=" + (this.scopedFields != null ? this.scopedFields.Count : 0) + ",catchupPushes=" + this.catchupPushes + ", total=" + ((this.scopedFields != null ? this.scopedFields.Count : 0) + this.catchupPushes));
                            }
                            this.catchupPushes--;
                            this.scopedFields.Push(new Dictionary<string, bool>());
                            //logger.Info("[temp]catchup live, scopeFields.Count=" + (this.scopedFields != null ? this.scopedFields.Count : 0) + ",catchupPushes=" + this.catchupPushes + ", total=" + ((this.scopedFields != null ? this.scopedFields.Count : 0) + this.catchupPushes));
                        }
                        var peekDic = this.scopedFields.Peek();
                        if (peekDic == null)
                        {
                            /*
                               This case used to cause a problem here, but now we
                               push pop off the null and add a Dictionary when needed.
                              <block>
                                <block>
                                    <do>TEMP.a='a'</do>
                                </block>
                                <do>TEMP.b='b'</do>
                              </block>
                             
                             This solution is correct as it still allows us to only instanciate a dictionary 
                             when we have fields at a certain level.
                             */
                            peekDic = new Dictionary<string, bool>();
                            this.scopedFields.Pop();
                            this.scopedFields.Push(peekDic);
                            //throw new Exception("[temp]temp scope error");
                        }
                        peekDic[fieldName] = true; 
                    }
                    this.fields[fieldName]=value; 
                }
                catch (Exception e)
                {
                    throw new Exception("[temp]Error setting TEMP." + fieldName, e);
                }
            }
        }
    }
}
