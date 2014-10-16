// Stephen Toub
// stoub@microsoft.com
//
// XmlComments.cs
// Retrieve the xml comments stored in the assembly's comments file
// for specific types or members of types.

#region Namespaces
using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Collections;
using System.Runtime.InteropServices;
using System.Linq;
using System.Collections.Generic;
#endregion

namespace MVM
{
    /// <summary>Used to retrieve the XML comments for MemberInfo objects.</summary>
    public class XmlComments
    {
        #region Static Variables
        /// <summary>Hashtable of all loaded XmlDocument comment files for assemblies.</summary>
        private static Hashtable _assemblyDocs = new Hashtable();
        /// <summary>
        /// Hashtable, indexed by Type, of all the accessors for a type.  Each entry is a Hashtable, 
        /// indexed by MethodInfo, that returns the MemberInfo for a given MethodInfo accessor.
        /// </summary>
        private static Hashtable _typeAccessors = new Hashtable();
        /// <summary>Binding flags to use for reflection operations.</summary>
        private static BindingFlags _bindingFlags =
            BindingFlags.Instance | BindingFlags.Static |
            BindingFlags.Public | BindingFlags.NonPublic;
        #endregion

        #region Member Variables
        /// <summary>The entire XML comment block for this member.</summary>
        private XmlNode _comments;
        /// <summary>The summary comment for this member.</summary>
        private XmlNode _summary;
        /// <summary>The remarks comment for this member.</summary>
        private XmlNode _remarks;
        /// <summary>The return comment for this member.</summary>
        private XmlNode _return;
        /// <summary>The value comment for this member.</summary>
        private XmlNode _value;
        /// <summary>The example comment for this member.</summary>
        private XmlNode _example;
        /// <summary>The includes comments for this member.</summary>
        private XmlNodeList _includes;
        /// <summary>The exceptions comments for this member.</summary>
        private XmlNodeList _exceptions;
        /// <summary>The paramrefs comments for this member.</summary>
        private XmlNodeList _paramrefs;
        /// <summary>The permissions comments for this member.</summary>
        private XmlNodeList _permissions;
        /// <summary>The params comments for this member.</summary>
        private XmlNodeList _params;
        #endregion

        #region Extracting Specific Comments
        /// <summary>Gets the entire XML comment block for this member.</summary>
        public XmlNode AllComments { get { return _comments; } }
        /// <summary>Gets the summary comment for this member.</summary>
        public XmlNode Summary { get { return _summary; } }
        /// <summary>Gets the remarks comment for this member.</summary>
        public XmlNode Remarks { get { return _remarks; } }
        /// <summary>Gets the return comment for this member.</summary>
        public XmlNode Return { get { return _return; } }
        /// <summary>Gets the value comment for this member.</summary>
        public XmlNode Value { get { return _value; } }
        /// <summary>Gets the example comment for this member.</summary>
        public XmlNode Example { get { return _example; } }
        /// <summary>Gets the includes comments for this member.</summary>
        public XmlNodeList Includes { get { return _includes; } }
        /// <summary>Gets the exceptions comments for this member.</summary>
        public XmlNodeList Exceptions { get { return _exceptions; } }
        /// <summary>Gets the paramrefs comments for this member.</summary>
        public XmlNodeList ParamRefs { get { return _paramrefs; } }
        /// <summary>Gets the permissions comments for this member.</summary>
        public XmlNodeList Permissions { get { return _permissions; } }
        /// <summary>Gets the params comments for this member.</summary>
        public XmlNodeList Params { get { return _params; } }
        /// <summary>Renders to a string the entire XML comment block for this member.</summary>
        public override string ToString() { return _comments.OuterXml; }
        #endregion

        #region Construction
        /// <summary>Gets the XML comments for the calling method.</summary>
        public static XmlComments Current
        {
            get { return new XmlComments(new StackTrace().GetFrame(1).GetMethod()); }
        }

        /// <summary>Initializes the XML comments for the specified member.</summary>
        /// <param name="mi">The member for which we want to retrieve the XML comments.</param>
        public XmlComments(MemberInfo mi)
        {
            if (mi == null) throw new ArgumentNullException("mi", "A MemberInfo must be supplied in order to retrieve the comments.");

            // If this is an accessor, get the owner property/event of the accessor.
            if (mi is MethodInfo)
            {
                MemberInfo owner = IsAccessor((MethodInfo)mi);
                if (owner != null) mi = owner;
            }

            // Get the comments.  If we got any, parse out the important stuff.
            _comments = GetComments(mi);
            if (_comments != null)
            {
                // Get single nodes (comments that can appear only once)
                _summary = _comments.SelectSingleNode("summary");
                _return = _comments.SelectSingleNode("return");
                _remarks = _comments.SelectSingleNode("remarks");
                _example = _comments.SelectSingleNode("example");
                _value = _comments.SelectSingleNode("value");

                // Get node lists (comments that can appear multiple times)
                _includes = _comments.SelectNodes("include");
                _exceptions = _comments.SelectNodes("exception");
                _paramrefs = _comments.SelectNodes("paramref");
                _permissions = _comments.SelectNodes("permission");
                _params = _comments.SelectNodes("param");
            }
            else
            {
                // Make it easier for people to use this class when no comments exist
                // by creating dummy nodes for all properties.
                _comments = new XmlDocument();
                _summary = _return = _remarks = _example = _value = _comments;
                _includes = _exceptions = _paramrefs = _permissions = _params = _comments.ChildNodes;
            }
        }
        #endregion

        #region Parsing the XML Document for an Assembly
        /// <summary>Retrieve the XML comments for a type or a member of a type.</summary>
        /// <param name="mi">The type or member for which comments should be retrieved.</param>
        /// <returns>A string of xml containing the xml comments of the selected type or member.</returns>
        private static XmlNode GetComments(MemberInfo mi)
        {
            Type declType = (mi is Type) ? ((Type)mi) : mi.DeclaringType;
            XmlDocument doc = LoadAssemblyComments(declType.Assembly);
            if (doc == null) return null;
            string xpath;

            // The fullname uses plus signs to separate nested types from their declaring
            // types.  The xml documentation uses dotted-notation.  We need to change
            // from one to the other.
            string typeName = declType.FullName.Replace("+", ".");

            // Based on the member type, get the correct xpath query to lookup the 
            // member's comments in the assembly's documentation.
            switch (mi.MemberType)
            {
                case MemberTypes.NestedType:
                case MemberTypes.TypeInfo:
                    xpath = "//member[@name='T:" + typeName + "']";
                    break;

                case MemberTypes.Constructor:
                    xpath = "//member[@name='M:" + typeName + "." +
                        "#ctor" + CreateParamsDescription(((ConstructorInfo)mi).GetParameters()) + "']";
                    break;

                case MemberTypes.Method:
                    xpath = "//member[@name='M:" + typeName + "." +
                        mi.Name + CreateParamsDescription(((MethodInfo)mi).GetParameters());
                    if (mi.Name == "op_Implicit" || mi.Name == "op_Explicit")
                    {
                        xpath += "~{" + ((MethodInfo)mi).ReturnType.FullName + "}";
                    }
                    xpath += "']";
                    break;

                case MemberTypes.Property:
                    xpath = "//member[@name='P:" + typeName + "." +
                        mi.Name + CreateParamsDescription(((PropertyInfo)mi).GetIndexParameters()) + "']"; // have args when indexers
                    break;

                case MemberTypes.Field:
                    xpath = "//member[@name='F:" + typeName + "." + mi.Name + "']";
                    break;

                case MemberTypes.Event:
                    xpath = "//member[@name='E:" + typeName + "." + mi.Name + "']";
                    break;

                // Unknown type, nothing to do
                default:
                    return null;
            }

            // Get the appropriate node from the document
            return doc.SelectSingleNode(xpath);
        }

        /// <summary>Determines if a MethodInfo represents an accessor.</summary>
        /// <param name="mi">The MethodInfo to check.</param>
        /// <returns>The MemberInfo that represents the property or event if this is an accessor; null, otherwise.</returns>
        private static MemberInfo IsAccessor(MethodInfo mi)
        {
            // Must be a special name in order to be an accessor
            if (!mi.IsSpecialName) return null;

            Hashtable accessors;
            lock (_typeAccessors.SyncRoot)
            {
                // We cache accessor information to speed things up, so check to see if the array
                // of accessors for this type has already been computed.
                accessors = (Hashtable)_typeAccessors[mi.DeclaringType];
                if (accessors == null)
                {
                    // Retrieve the accessors for the declaring type
                    _typeAccessors[mi.DeclaringType] = accessors = RetrieveAccessors(mi.DeclaringType);
                }
            }

            // Return the owning property or event for the accessor
            return (MemberInfo)accessors[mi];
        }

        /// <summary>Retrieve all property and event accessors on a given type.</summary>
        /// <param name="t">The type from which the accessors should be retrieved.</param>
        /// <returns>A dictionary of all accessors.</returns>
        private static Hashtable RetrieveAccessors(Type t)
        {
            // Build up list of accessors to exclude from method list
            Hashtable ht = new Hashtable();

            // Get all property accessors
            foreach (PropertyInfo pi in t.GetProperties(_bindingFlags))
            {
                foreach (MethodInfo mi in pi.GetAccessors(true)) ht[mi] = pi;
            }

            // Get all event accessors
            foreach (EventInfo ei in t.GetEvents(_bindingFlags))
            {
                MethodInfo addMethod = ei.GetAddMethod(true);
                MethodInfo removeMethod = ei.GetRemoveMethod(true);
                MethodInfo raiseMethod = ei.GetRaiseMethod(true);

                if (addMethod != null) ht[addMethod] = ei;
                if (removeMethod != null) ht[removeMethod] = ei;
                if (raiseMethod != null) ht[raiseMethod] = ei;
            }

            // Return the whole list
            return ht;
        }

        /// <summary>Generates a parameter string used when searching xml comment files.</summary>
        /// <param name="parameters">List of parameters to a member.</param>
        /// <returns>A parameter string used when searching xml comment files.</returns>
        private static string CreateParamsDescription(ParameterInfo[] parameters)
        {
            StringBuilder paramDesc = new StringBuilder();

            // If there are parameters then we need to construct a list
            if (parameters.Length > 0)
            {
                // Start the list
                paramDesc.Append("(");

                // For each parameter, append the type of the parameter.
                // Separate all items with commas.
                for (int i = 0; i < parameters.Length; i++)
                {
                    Type paramType = parameters[i].ParameterType;

                    if (!paramType.IsGenericType)
                    {

                        string paramName = paramType.FullName;

                        // Handle special case where ref parameter ends in & but xml docs use @.
                        // Pointer parameters end in * in both type representation and xml comments representation.
                        if (paramName.EndsWith("&")) paramName = paramName.Substring(0, paramName.Length - 1) + "@";

                        // Handle multidimensional arrays
                        if (paramType.IsArray && paramType.GetArrayRank() > 1)
                        {
                            paramName = paramName.Replace(",", "0:,").Replace("]", "0:]");
                        }
                        // Append the fixed up parameter name
                        paramDesc.Append(paramName);
                    }
                    else
                    {
                        // ROB Handle generics as these must have changed for c#4.0
                        // start with something like this:
                        // System.Collections.Generic.Dictionary`2[
                        //      [System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],
                        //      [MVM.IReadable, mvm_lib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]
                        //  ]
                        // end with something like this:
                        // System.Collections.Generic.Dictionary{System.String,MVM.IReadable}
                        string pName = paramType.FullName;
                        int gstart = pName.IndexOf('`');
                        pName = pName.Substring(0, gstart);
                        List<string> gParams = new List<string>();
                        foreach (Type myType in paramType.GetGenericArguments())
                        {
                            var g = myType;
                            gParams.Add(myType.FullName);
                        }

                        string docString = pName + "{" + gParams.JoinStrings(",") + "}";
                        paramDesc.Append(docString);
                    }

                    
                    if (i != parameters.Length - 1) paramDesc.Append(",");
                }

                // End the list
                paramDesc.Append(")");
            }

            // Return the parameter list description
            return paramDesc.ToString();
        }
        #endregion

        #region Loading the Assembly's XML Comments
        /// <summary>Get the xml documentation for an assembly.</summary>
        /// <param name="a">The assembly whose documentation is to be loaded.</param>
        /// <returns>The xml documentation for an assembly; null if none found.</returns>
        public static XmlDocument LoadAssemblyComments(Assembly a)
        {
            // Get xml dom for the assembly's documentation
            XmlDocument doc;
            lock (_assemblyDocs.SyncRoot)
            {
                if (!_assemblyDocs.ContainsKey(a.FullName))
                {
                    string xmlPath = DetermineXmlPath(a);
                    if (xmlPath == null) return null;

                    // Load it and store it
                    doc = new XmlDocument();
                    doc.Load(xmlPath);
                    _assemblyDocs[a.FullName] = doc;
                }
                // Get the docs from the cache
                else doc = (XmlDocument)_assemblyDocs[a.FullName];
            }

            // Return the docs
            return doc;
        }

        /// <summary>Gets the path to a valid xml comments file for the assembly.</summary>
        /// <param name="asm">The assembly whose documentation is to be found.</param>
        /// <returns>The path to documentation for an assembly; null if none found.</returns>
        private static string DetermineXmlPath(Assembly asm)
        {
            // Get a list of locations to examine for the xml
            // 1. The location of the assembly.
            // 2. The runtime directory of the framework.
            string[] locations = new string[]
				{   
					asm.Location,
					RuntimeEnvironment.GetRuntimeDirectory() + Path.GetFileName(asm.CodeBase)
				};

            // Checks each path to see if the xml file exists there; if it does, return it.
            foreach (string location in locations)
            {
                string newPath = Path.ChangeExtension(location, ".xml");
                if (File.Exists(newPath)) return newPath;
            }

            // No xml found; return null.
            return null;
        }
        #endregion
    }
}

// End of XmlComments.cs