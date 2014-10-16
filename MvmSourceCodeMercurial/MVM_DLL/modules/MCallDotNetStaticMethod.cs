using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;

using Antlr.Runtime.Tree;
using System.Reflection;
namespace MVM
{

    /*
     
    TODO: does not procInst with overloaded methods. 
    TODO: Allow user to specify <field type='int'/> for overloads
    TODO: Use best guess based on number of args passed.
    TODO: If call by name=value, we pick overload based on param name.
    TODO: figure out how to deal with params keyword
    TODO: we should have a dll search path that searches dotnet and user configured spots.
     
<call_dotnet_static_method>
<assembly>'OptionalAssembly.dll'</assembly> //defaults to ADE.exe assembly
<type>'System.Text'</type>
<method>'Substring'</method>
<field name='inputValue'>'abc'</field>
<field name='offset'>0</field>
<field name='length'>3</field>
<result>TEMP.output</field>
</call_dotnet_static_method>

      */
    public struct OutParamInfo
    {
        public int parameterIdx;
        public IWritable writer;
        public OutParamInfo(IWritable writer, int parameterIdx)
        {
            this.parameterIdx = parameterIdx;
            this.writer = writer;
        }
    }

    class MCallDotNetStaticMethod : IModuleSetup, IModuleRun
    {
        // from xml
        private string assemblySyntax;
        private string methodSyntax;
        private string typeSyntax;
        private string resultSyntax;

        // from setup
        private IReadString assemblyParsed;
        private IReadString typeParsed;
        private IReadString methodParsed;
        private IWriteString resultParsed;
        private MethodInfo method;
        private List<IReadable> readableParameters = new List<IReadable>();
        private List<OutParamInfo> outputParameters = new List<OutParamInfo>();
        public IWritable resultParam;

        public void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            MCallDotNetStaticMethod m = new MCallDotNetStaticMethod();
            m.assemblySyntax = me.SelectNodeInnerText("./assembly");
            m.typeSyntax = me.SelectNodeInnerText("./type");
            m.methodSyntax = me.SelectNodeInnerText("./method");
            m.resultSyntax = me.SelectNodeInnerText("./result");
            m.assemblyParsed = mc.ParseSyntax(m.assemblySyntax);
            m.typeParsed = mc.ParseSyntax(m.typeSyntax);
            m.methodParsed = mc.ParseSyntax(m.methodSyntax);
            m.resultParsed = mc.ParseWritableSyntax(m.resultSyntax);

            //do reflection on the assembly to find the method.
            string assemblyName = m.assemblyParsed.Read(mc);
            string typeName = m.typeParsed.Read(mc);
            string methodName = m.methodParsed.Read(mc);

            Assembly assembly;
            if (assemblyName.Equals(""))
            {
                assembly = Assembly.GetExecutingAssembly();
            }
            else
            {
                assembly = Assembly.LoadFrom(assemblyName);
            }

            var type = assembly.GetType(typeName);
            m.method = type.GetMethod(methodName); //TBD pass types to pick best overload.

            // this is for getting input params
            Overrider overrider = new Overrider(me);
            foreach (var param in m.method.GetParameters())
            {
                // out parameter
                if (param.IsOut)
                {
                    var syntax = overrider.GetSyntax(param.Name);
                    var readable = new ReadNull();
                    m.readableParameters.Add(readable);
                    var parameterType = param.ParameterType.GetElementType();
                    var writable = mc.ParseWritableSyntax(syntax);
                    var writeTo = TypeConverter.ConvertWritable(writable, parameterType);
                    m.outputParameters.Add(new OutParamInfo(writeTo, m.readableParameters.Count - 1));
                }
                // ref parameter
                else if (param.ParameterType.IsByRef)
                {
                    // Disallow true pass by reference. Copy field to tempContext space the read back.
                    var parameterType = param.ParameterType.GetElementType();
                    var syntax = overrider.GetSyntax(param.Name);
                    var readable = mc.ParseSyntax(syntax);
                    var readAsObject = TypeConverter.ConvertReadable(parameterType, readable);
                    m.readableParameters.Add(readable);
                    var writable = mc.ParseWritableSyntax(syntax);
                    var writeTo = TypeConverter.ConvertWritable(writable, parameterType);
                    m.outputParameters.Add(new OutParamInfo(writeTo, m.readableParameters.Count - 1));
                }
                // assume normal input parameter...
                else
                {
                    var syntax = overrider.GetSyntax(param.Name);
                    var readable = mc.ParseSyntax(syntax);
                    var readAsObject = TypeConverter.ConvertReadable(param.ParameterType, readable);
                    m.readableParameters.Add(readAsObject);
                }
            }
            if (!m.method.ReturnType.Equals(typeof(void)))
            {
                var writableString = mc.ParseWritableSyntax(m.resultSyntax);
                m.resultParam = TypeConverter.ConvertWritable(writableString, m.method.ReturnType);
            }
            run.Add(m);
        }


        public void Run(ModuleContext mc)
        {
            // setup the input parameters
            object[] parameters = null;
            if (this.readableParameters.Count > 0)
            {
                parameters = new object[this.readableParameters.Count];
                for (int i = 0; i < readableParameters.Count; i++)
                {
                    var readAsObject = this.readableParameters[i];
                    var paramValue = readAsObject.ReadObject(mc);
                    parameters[i] = paramValue;
                }
            }
            // call the method
            var result = this.method.Invoke(null, parameters);
            // get return value if any
            if (this.resultParam != null)
            {
                resultParam.WriteObject(mc, result);
            }
            // get any out/ref parameters
            foreach (var outParamInfo in this.outputParameters)
            {
                outParamInfo.writer.WriteObject(mc, parameters[outParamInfo.parameterIdx]);
            }
        }

        public void Log(ModuleContext mc, ILogger log)
        {
            log.LogInfo("call_dotnet_static_method:");
        }
    }
}
