using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MVM
{
    /// <summary>
    /// Utility class for easy access to common System.Reflection features
    /// http://mattberseth.com/blog/2007/04/net_reflection.html
    /// </summary>
    public static class Reflector
    {
        private const BindingFlags CommonFlags = BindingFlags.Public | BindingFlags.NonPublic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyFile"></param>
        /// <returns></returns>
        public static Assembly LoadAssemblyFrom(string assemblyFile)
        {
            return Assembly.LoadFrom(assemblyFile);
        }

        /// <summary>
        /// 
        /// </summary>
        public static object CreateInstance(Type type, params object[] args)
        {
            return Reflector.InvokeMember(
                type, null, null,
                Reflector.CommonFlags | BindingFlags.CreateInstance | BindingFlags.Instance, args);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void SetField(object target, string fieldName, object value)
        {
            Reflector.InvokeMember(
                target.GetType(), target, fieldName,
                Reflector.CommonFlags | BindingFlags.SetField | BindingFlags.Instance, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static object GetField(object target, string fieldName)
        {
            return Reflector.InvokeMember(
                target.GetType(), target, fieldName,
                Reflector.CommonFlags | BindingFlags.GetField | BindingFlags.Instance);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void SetProperty(object target, string propertyName, object value)
        {
            Reflector.InvokeMember(
                target.GetType(), target, propertyName,
                Reflector.CommonFlags | BindingFlags.SetProperty | BindingFlags.Instance, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static object GetProperty(object target, string propertyName)
        {
            return Reflector.InvokeMember(
                target.GetType(), target, propertyName,
                Reflector.CommonFlags | BindingFlags.GetProperty | BindingFlags.Instance);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void StaticSetField(Type type, string fieldName, object value)
        {
            Reflector.InvokeMember(
                type, null, fieldName,
                Reflector.CommonFlags | BindingFlags.SetField | BindingFlags.Static, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static object StaticGetField(Type type, string fieldName)
        {
            return Reflector.InvokeMember(
                type, null, fieldName,
                Reflector.CommonFlags | BindingFlags.GetField | BindingFlags.Static);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void StaticSetProperty(Type type, string propertyName, object value)
        {
            Reflector.InvokeMember(
                type, null, propertyName,
                Reflector.CommonFlags | BindingFlags.SetProperty | BindingFlags.Static, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static object StaticGetProperty(Type type, string propertyName)
        {
            return Reflector.InvokeMember(
                type, null, propertyName,
                Reflector.CommonFlags | BindingFlags.GetProperty | BindingFlags.Static);
        }

        /// <summary>
        /// 
        /// </summary>
        public static object CallMethod(object target, string methodName, params object[] args)
        {
            return Reflector.InvokeMember(
                target.GetType(), target, methodName,
                Reflector.CommonFlags | BindingFlags.InvokeMethod | BindingFlags.Instance, args);
        }

        /// <summary>
        /// 
        /// </summary>
        public static object StaticCallMethod(Type type, string memberName, params object[] args)
        {
            return Reflector.InvokeMember(
                type, null, null,
                Reflector.CommonFlags | BindingFlags.InvokeMethod | BindingFlags.Static, args);
        }

        /// <summary>
        /// 
        /// </summary>
        private static object InvokeMember(
            Type type, object target, string memberName, BindingFlags flags, params object[] args)
        {
            return type.InvokeMember(memberName, flags, null, target, args);
        }
    }

}
