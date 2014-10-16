using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{
    class IndexFunctions
    {

       
        //index_count(index => 'INDEX_NAME', key1=>value1 ...)

        /// <summary>
        /// Returns the number of items in the passed INDEX after plugging in the passed keys
        /// </summary>
        /// <param name="_module_context"></param>
        /// <param name="_named_arguments"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <category>Indexes</category>
        [MvmExport("index_count")]
        public static int index_count(ModuleContext _module_context, Dictionary<string, IReadable> _named_arguments, string index)
        {
            IIndex iindex = (IIndex)_module_context.globalContext.GetNamedClassInst(index);
            List<string> readkeys = new List<string>();
            foreach (string fieldName in iindex.GetOrderedKeyFields())
            {
                if (_named_arguments.ContainsKey(fieldName))
                {
                    readkeys.Add(_named_arguments[fieldName].ReadObject(_module_context).ToString());
                }
                else
                {
                    if (!(iindex.NestedKeys() || readkeys.Count == 0))
                    {
                        throw new Exception("Function missing field " + fieldName);
                    }
                    break;
                }
            }
            return iindex.GetCount(readkeys);
        }

        // index_get(index => 'INDEX_NAME', key1=>value1 ...)
        /// <summary>
        /// Returns the first item in the passed INDEX for the passed keys.
        /// </summary>
        /// <param name="_module_context"></param>
        /// <param name="_named_arguments"></param>
        /// <param name="_named_arguments_w"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <category>Indexes</category>
        [MvmExport("index_get")]
        public static string index_get(ModuleContext _module_context, Dictionary<string, IReadable> _named_arguments, Dictionary<string, IWritable> _named_arguments_w, string index)
        {
            IIndex iindex = (IIndex)_module_context.globalContext.GetNamedClassInst(index);
            Dictionary<string, string> keys = new Dictionary<string, string>();
            List<string> orderedKeyValues = new List<string>();
            Dictionary<string, string> values = new Dictionary<string, string>();
            foreach (string fieldName in iindex.GetOrderedKeyFields())
            {
                if (_named_arguments.ContainsKey(fieldName))
                {
                    keys[fieldName] = _named_arguments[fieldName].ReadObject(_module_context).ToString();
                }
                else
                {
                    throw new Exception("Function missing field " + fieldName);
                }
                orderedKeyValues.Add(keys[fieldName]);
            }
            foreach (string fieldName in iindex.GetOrderedValueFields())
            {
                if (_named_arguments_w.ContainsKey(fieldName) && !keys.ContainsKey(fieldName))
                {
                    values[fieldName] = "";
                }
            }
            string retval = iindex.IndexGet(_module_context, orderedKeyValues, values);
            foreach (var entry in values)
            {
                _named_arguments_w[entry.Key].WriteObject(_module_context, entry.Value);
            }
            return retval;
        }

        // index_replace('INDEX_NAME', key1=>value1 ...)
        /// <summary>
        /// Replaces the first item in the passed index for the PASSED keys.
        /// </summary>
        /// <param name="_module_context"></param>
        /// <param name="_named_arguments"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <category>Indexes</category>
        [MvmExport("index_replace")]
        public static string index_replace(ModuleContext _module_context, Dictionary<string, IReadable> _named_arguments, string index)
        {
            IIndex iindex = (IIndex)_module_context.globalContext.GetNamedClassInst(index);
            Dictionary<string, string> args = new Dictionary<string, string>();

            List<string> orderedFieldValues = new List<string>();
            List<string> orderedKeyValues = new List<string>();
            foreach (string fieldName in iindex.GetOrderedValueFields())
            {
                if (_named_arguments.ContainsKey(fieldName))
                {
                    args[fieldName] = _named_arguments[fieldName].ReadObject(_module_context).ToString();
                }
                else
                {
                    args[fieldName] = "";
                }
                orderedFieldValues.Add(args[fieldName]);
            }
            foreach (string fieldName in iindex.GetOrderedKeyFields())
            {
                orderedKeyValues.Add(args[fieldName]);
            }
            iindex.IndexRemove(_module_context, orderedKeyValues, IndexRemovalOption.All);
            iindex.IndexInsert(_module_context, orderedFieldValues);
            return "";
        }


        // index_insert('INDEX_NAME', key1=>value1 ...)
        /// <summary>
        /// Insert in the passed index for the PASSED keys.
        /// </summary>
        /// <param name="_module_context"></param>
        /// <param name="_named_arguments"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <category>Indexes</category>
        [MvmExport("index_insert")]
        public static string index_insert(ModuleContext _module_context, Dictionary<string, IReadable> _named_arguments, string index)
        {
            IIndex iindex = (IIndex)_module_context.globalContext.GetNamedClassInst(index);
            Dictionary<string, string> args = new Dictionary<string, string>();

            List<string> orderedFieldValues = new List<string>();
            List<string> orderedKeyValues = new List<string>();
            foreach (string fieldName in iindex.GetOrderedValueFields())
            {
                if (_named_arguments.ContainsKey(fieldName))
                {
                    args[fieldName] = _named_arguments[fieldName].ReadObject(_module_context).ToString();
                }
                else
                {
                    args[fieldName] = "";
                }
                orderedFieldValues.Add(args[fieldName]);
            }
            foreach (string fieldName in iindex.GetOrderedKeyFields())
            {
                orderedKeyValues.Add(args[fieldName]);
            }
            iindex.IndexInsert(_module_context, orderedFieldValues);
            return "";
        }
        /// <summary>
        /// Clears the entire index
        /// index_clear('INDEX_NAME')
        /// </summary>
        /// <param name="_module_context"></param>
        /// <param name="_named_arguments"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <category>Indexes</category>
        [MvmExport("index_clear")]
        public static string index_clear(ModuleContext _module_context, Dictionary<string, IReadable> _named_arguments, string index)
        {
            IIndex iindex = (IIndex)_module_context.globalContext.GetNamedClassInst(index);
            return iindex.IndexClear(_module_context);
        }

        /// <summary>
        /// Updates all the items in the passed INDEX for the passed keys.
        /// index_update('INDEX_NAME', key1=>value1 ...)
        /// </summary>
        /// <param name="_module_context"></param>
        /// <param name="_named_arguments"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <category>Indexes</category>
        [MvmExport("index_update")]
        public static string index_update(ModuleContext _module_context, Dictionary<string, IReadable> _named_arguments, string index)
        {
            IIndex iindex = (IIndex)_module_context.globalContext.GetNamedClassInst(index);
            Dictionary<string, string> keys = new Dictionary<string, string>();
            Dictionary<string, string> values = new Dictionary<string, string>();
            List<string> orderedKeyValues = new List<string>();
            foreach (string fieldName in iindex.GetOrderedKeyFields())
            {
                if (_named_arguments.ContainsKey(fieldName))
                {
                    keys[fieldName] = _named_arguments[fieldName].ReadObject(_module_context).ToString();
                }
                else
                {
                    throw new Exception("Function missing field " + fieldName);
                }
                orderedKeyValues.Add(keys[fieldName]);
            }
            foreach (string fieldName in iindex.GetOrderedValueFields())
            {
                if (_named_arguments.ContainsKey(fieldName) && !keys.ContainsKey(fieldName))
                {
                    values[fieldName] = _named_arguments[fieldName].ReadObject(_module_context).ToString();
                }
            }
            return iindex.IndexUpdate(_module_context, orderedKeyValues, values);
        }


    }
}
