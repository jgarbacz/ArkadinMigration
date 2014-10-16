using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Antlr.Runtime.Tree;

namespace MVM
{

    public class MvmExportAttribute : Attribute
    {
        public string exportedName;
        public MvmExportAttribute(string exportedName)
        {
            this.exportedName=exportedName;
        }
    }


    public class CallFunction : ISetupSyntax
    {
        public enum argType { moduleContext, named, named_writable, positional };
        private static Dictionary<string, List<MethodInfo>> reflected_functions = new Dictionary<string, List<MethodInfo>>();
        private static Dictionary<string, object> callable_functions = new Dictionary<string, object>();

        // inspects the assemble and loads MvmExport functions 
        public static void LoadedMvmExportedFunctions()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (Type type in assembly.GetTypes().Where(t => t.IsClass))
            {
                foreach (MethodInfo methodInfo in type.GetMethods())
                {
                    object[] attrs = new object[0];
                    try
                    {
                        // Ignore errors from GetCustomAttributes(), they generally mean an optional assembly isn't present
                        attrs = methodInfo.GetCustomAttributes(true);
                    }
                    catch
                    {
                    }
                    foreach (var attribute in attrs.Where(t => t is MvmExportAttribute))
                    {
                        MvmExportAttribute moduleAttribute = (MvmExportAttribute)attribute;
                        RegisterReflectedFunction(moduleAttribute.exportedName, methodInfo);
                    }
                }
            }
        }

        public static IEnumerable<KeyValuePair<string,MethodInfo>> GetMvmFunctionInfo()
        {
            foreach (var entry in reflected_functions)
            {
                foreach (var mi in entry.Value)
                {
                    yield return new KeyValuePair<string, MethodInfo>(entry.Key, mi);
                }
            }
        }

        public static bool IsMvmFunction(string name)
        {
            return reflected_functions.ContainsKey(name);
        }

        /// <summary>
        /// Resolves a function based on name and passed argument trees.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="positional_args"></param>
        /// <param name="named_args"></param>
        /// <returns></returns>
        public static MethodInfo ResolveReflectedFunction(string name, List<ITree> positional_args, Dictionary<string, ITree> named_args)
        {
            int passedParamCount = (positional_args != null ? positional_args.Count : 0) + (named_args != null ? named_args.Count : 0);
            return ResolveReflectedFunction(name, passedParamCount);
        }


        /// <summary>
        /// Resolves a function based on name and number of parameters. TODO: allow overrides based on param names.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="passedParamCount"></param>
        /// <returns></returns>
        public static MethodInfo ResolveReflectedFunction(string name, int passedParamCount)
        {
            if (reflected_functions.ContainsKey(name))
            {
                foreach (MethodInfo mi in reflected_functions[name])
                {
                    // get param count no including ModuleContext which is an implicit param. also, return value
                    // counts as a param so we need to subtract that out.
                    var Allparams = mi.GetParameters().ToList();
                    int paramCount = mi.GetParameters().Where(p => !p.ParameterType.Equals(typeof(ModuleContext))).Count();
                    //if (!mi.IsVoid())
                    //{
                    //    paramCount--;
                    //}
                    if (paramCount == passedParamCount) return mi;
                }
                return reflected_functions[name].First(); // just return the first match and let the caller sort it out.
            }
            throw new Exception("error, no function by the name of [" + name + "] in reflected_functions");
        }


        /// <summary>
        /// Registers an exported function
        /// </summary>
        /// <param name="exportedName"></param>
        /// <param name="methodInfo"></param>
        public static void RegisterReflectedFunction(string exportedName,MethodInfo methodInfo){
            if (!methodInfo.IsStatic)
            {
                throw new Exception("Error, only static method can be exported to mvm");
            }
            if (!reflected_functions.ContainsKey(exportedName))
            {
                //var existing=reflected_functions[exportedName];
                //throw new Exception("error, cannot register [" + exportedName + "] to [" + methodInfo.DeclaringType.ToString() + "][" + methodInfo.ToString() + "] because it is already registered to [" + existing.DeclaringType.ToString() + "][" + existing.ToString() + "]");
                reflected_functions[exportedName] = new List<MethodInfo>();
            }
            reflected_functions[exportedName].Add(methodInfo);
        }

        static CallFunction()
        {
            // search the assembly for mvm exported functions
            LoadedMvmExportedFunctions();
        }

        #region ISetupSyntax Members
        public IReadString SetupRead(SyntaxMaster syntaxMaster, ITree tree)
        {
            string funcName = tree.GetChild(0).Text;

            // Determine our positional and named arguments
            List<ITree> positional_args = new List<ITree>();
            Dictionary<string, ITree> named_args = new Dictionary<string, ITree>();
            for (int i = 1; i < tree.ChildCount; i++)
            {
                ITree child = tree.GetChild(i);
                if (child.Text.Equals("ARGUMENT"))
                {
                    positional_args.Add(child.GetChild(0));
                }
                else if (child.Text.Equals("NAMED_ARGUMENT"))
                {
                    named_args[child.GetChild(0).Text] = child.GetChild(1);
                }
            }

            if (callable_functions.ContainsKey(funcName))
            {
                return SetupCallableRead(syntaxMaster, funcName, positional_args, named_args);
            }
            else if (reflected_functions.ContainsKey(funcName))
            {
                return SetupReflectedRead(syntaxMaster, funcName, positional_args, named_args);
            }
            else
            {
                throw new Exception("Unknown function: [" + funcName + "]");
            }
        }

        public IReadString SetupCallableRead(SyntaxMaster syntaxMaster, string funcName, List<ITree> positional_args, Dictionary<string, ITree> named_args)
        {
            throw new Exception("Implement callable functions");
        }

        public IReadString SetupReflectedRead(SyntaxMaster syntaxMaster, string funcName, List<ITree> positional_args, Dictionary<string, ITree> named_args)
        {
            List<IReadable> readableParameters = new List<IReadable>();
            List<OutParamInfo> outputParameters = new List<OutParamInfo>();
            List<int> specialParameters = new List<int>();
            bool result = false;

            MethodInfo method = ResolveReflectedFunction(funcName,positional_args,named_args);

            // this is for getting input params
            int positional_ctr = 0;
            foreach (var param in method.GetParameters())
            {
                int special = -1;
                ITree node = null;

                //if (param.Name.Equals("_module_context"))
                if (param.ParameterType.Equals(typeof(ModuleContext)))
                {
                    special = (int)argType.moduleContext;
                }
                else if (param.Name.Equals("_named_arguments"))
                {
                    special = (int)argType.named;
                }
                else if (param.Name.Equals("_named_arguments_w"))
                {
                    special = (int)argType.named_writable;
                }
                else if (param.Name.Equals("_positional_arguments"))
                {
                    special = (int)argType.positional;
                }
                else if (named_args.ContainsKey(param.Name))
                {
                    // It matches a named parameter
                    node = named_args[param.Name];
                }
                else if (positional_args.Count > positional_ctr)
                {
                    // There are positional args remaining
                    node = positional_args[positional_ctr++];
                }
                else
                {
                    throw new Exception("Invalid arguments for function " + funcName + ": [" + param.Name + "]");
                }

                specialParameters.Add(special);
                // special arguments
                if (node == null)
                {
                    readableParameters.Add(null);
                }
                // out parameter
                else if (param.IsOut)
                {
                    var readable = new ReadNull();
                    readableParameters.Add(readable);
                    var parameterType = param.ParameterType.GetElementType();
                    var writable = syntaxMaster.SetupWritable(node);
                    var writeTo = TypeConverter.ConvertWritable(writable, parameterType);
                    outputParameters.Add(new OutParamInfo(writeTo, readableParameters.Count - 1));
                }
                // ref parameter
                else if (param.ParameterType.IsByRef)
                {
                    // Disallow true pass by reference. Copy field to tempContext space the read back.
                    var parameterType = param.ParameterType.GetElementType();
                    var readable = syntaxMaster.SetupRead(node);
                    var readAsObject = TypeConverter.ConvertReadable(parameterType, readable);
                    readableParameters.Add(readable);
                    var writable = syntaxMaster.SetupWritable(node);
                    var writeTo = TypeConverter.ConvertWritable(writable, parameterType);
                    outputParameters.Add(new OutParamInfo(writeTo, readableParameters.Count - 1));
                }
                // assume normal input parameter...
                else
                {
                    var readable = syntaxMaster.SetupRead(node);
                    var readAsObject = TypeConverter.ConvertReadable(param.ParameterType, readable);
                    readableParameters.Add(readAsObject);
                }
            }
            if (!method.ReturnType.Equals(typeof(void)))
            {
                
                result = true;
            }

            return new Function(method, syntaxMaster, readableParameters, outputParameters, specialParameters, result, positional_args, named_args);
        }

        public IReadString SetupWrite(SyntaxMaster syntaxMaster, ITree leftTree, ITree rightTree)
        {
            string v = leftTree.GetChild(0).Text;
            throw new Exception("Cannot write to function: [" + v + "]");
        }

        public IReadString SetupWrite(SyntaxMaster syntaxMaster, ITree leftTree, IReadString right)
        {
            string v = leftTree.GetChild(0).Text;
            throw new Exception("Cannot write to function: [" + v + "]");
        }

        #endregion

        class Function : ReadStringBase
        {
            MethodInfo method = null;
            List<IReadable> readableParameters = null;
            List<OutParamInfo> outputParameters = null;
            List<int> specialParameters = null;
            List<IReadable> positional_arguments = new List<IReadable>();
            Dictionary<string, IReadable> named_arguments = new Dictionary<string, IReadable>();
            Dictionary<string, IWritable> named_arguments_w = new Dictionary<string, IWritable>();
            bool returnResult = false;

            public Function(MethodInfo m, SyntaxMaster sm, List<IReadable> rp, List<OutParamInfo> op, List<int> sp, bool result, List<ITree> positional_args, Dictionary<string, ITree> named_args)
            {
                this.method = m;
                this.readableParameters = rp;
                this.outputParameters = op;
                this.specialParameters = sp;
                this.returnResult = result;
                foreach (var parg in positional_args)
                {
                    positional_arguments.Add(TypeConverter.ConvertReadable(typeof(string), sm.SetupRead(parg)));
                }
                foreach (var narg in named_args)
                {
                    named_arguments[narg.Key] = TypeConverter.ConvertReadable(typeof(string), sm.SetupRead(narg.Value));
                    try
                    {
                        named_arguments_w[narg.Key] = TypeConverter.ConvertWritable(sm.SetupWritable(narg.Value), typeof(string));
                    }
                    catch
                    {
                        // This means it wasn't a writable expression, so ignore it
                    }
                }
            }
            public override string Read(ModuleContext mc)
            {
                // Here we call the reflected function and return the result
                object[] parameters = null;
                if (readableParameters.Count > 0)
                {
                    parameters = new object[readableParameters.Count];
                    for (int i = 0; i < readableParameters.Count; i++)
                    {
                        var readAsObject = readableParameters[i];
                        if (specialParameters[i] == (int)argType.moduleContext)
                        {
                            parameters[i] = mc;
                        }
                        else if (specialParameters[i] == (int)argType.named)
                        {
                            parameters[i] = named_arguments;
                        }
                        else if (specialParameters[i] == (int)argType.named_writable)
                        {
                            parameters[i] = named_arguments_w;
                        }
                        else if (specialParameters[i] == (int)argType.positional)
                        {
                            parameters[i] = positional_arguments;
                        }
                        else
                        {
                            var paramValue = readAsObject.ReadObject(mc);
                            parameters[i] = paramValue;
                        }
                    }
                }
                // call the method
                object result = method.Invoke(null, parameters);
                // get any out/ref parameters
                foreach (var outParamInfo in outputParameters)
                {
                    outParamInfo.writer.WriteObject(mc, parameters[outParamInfo.parameterIdx]);
                }
                // get return value if any
                if (returnResult)
                {
                    // need to convert the result to string.
                    if (result is bool)
                    {
                        return ((bool)result) ? "1" : "0";
                    }
                    else
                    {
                        return result.ToString();
                    }
                }
                return "";
            }
        }




    }
}
