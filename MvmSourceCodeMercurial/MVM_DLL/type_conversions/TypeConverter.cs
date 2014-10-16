using System;
using System.Collections.Generic;
using System.Text;

namespace MVM
{
    class TypeConverter
    {
        // So when i parse something it gives me something back and i don't know what that something is,
        // i need to type type to figure that out.
        // TEMP.int=OBJECT.string
        // 
        // we need implicitly defined types. mean left is implicity defined by right
        // optionally you want right to give left back the correct type of the left.
        // 
        // TEMP.f = "99" how you first define it is how it will be stored.
        // do i really care about generate time having the right type?? 
        // OBJECT.field=99 (99 is going to be an int) 
        // 
        // IReadable readable=parseRead(readSyntax)
        // IWritable writable=parseWrite(writeSyntax,readable)
        // 
        // writeable.write(readable);
        // 
        // Need to assume not everything is the STRING!!! So when we parse we need to get back something 
        // that tells us the type of the thing we just parsed.
        //
        // IReadable
        //      System.Type GetReadType();
        //      object Read(mc)
        // IReadInt:IReadable
        //      int Read(mc)
        // IReadString:IReadable
        //      string Read(mc)
        // IWriteInt:IWritable
        //      void Write(mc,object) // does cast interal
        // IWriteString:IWritable
        //      string Write(mc,string)
        // IWritable
        //       System.Type GetWriteType();
        // 
        //
        // parse needs to return type object which i examine to 

        /**
         * IReadAsObject GetReadAsObject(Type objectType, Readable readableString) 
         * if(ReadAsObjectTypes.ContainsKey(objectType)) return new 
         * 
         */

        // 
        // Some data reader X can be converted to type Y at runtime via object O
        // readAsObject[X][Y]=O
        // 
        // example 1:
        // readAsObject[iReadString][int]=new ReadStringToInt()     // registered at startup
        // iReadAsObject=readIntAsObject.Setup(iReadString)         // called during newModule setup
        // iReadAsObject.ReadAsObject(mc) will returns int object   // called during newModule run
        // 
        // example 2:
        // readAsObject[iReadInt][string]=new ReadIntToString()     // registered at startup
        // iReadAsObject=readIntAsObject.Setup(iReadInt)            // called during newModule setup
        // iReadAsObject.ReadAsObject(mc) returns a string object   // called during newModule run
        //
        // So if you have a IReadString and need an int object you call
        // iReadObject=GetReadAsObject(IReadString,int);            // newModule setup
        // int value=(int) iReadObject.Read(mc)                     // newModule run
        // 
        // nowone is going to ever ask for a reader, just a type the need. The
        // IRead* stuff represents our internal type. The hard type is what ppl need.
        // newModules need to say the type the return.
        // 
        // 

        // do any casting at setup time NOT runtime.
        static Dictionary<Type, Dictionary<Type, IConvertReadable>> convertReadableTypes = new Dictionary<Type, Dictionary<Type, IConvertReadable>>();
        static Dictionary<Type, Dictionary<Type, IConvertWritable>> convertWritableTypes = new Dictionary<Type, Dictionary<Type, IConvertWritable>>();
      
        static TypeConverter()
        {
            RegisterConvertReadable(typeof(string), typeof(int), new ReadStringAsInt());
            RegisterConvertWritable(typeof(string), typeof(int), new WriteStringWithInt());
            RegisterConvertReadable(typeof(bool), typeof(string), new ReadBoolAsString());
            RegisterConvertReadable(typeof(string),typeof(bool), new ReadStringAsBool());
            RegisterConvertWritable(typeof(string), typeof(bool), new WriteStringWithBool());
        }

        // regisers a readable type converter
        public static void RegisterConvertReadable(Type fromType, Type toType, IConvertReadable converter)
        {
            if (!convertReadableTypes.ContainsKey(fromType)) convertReadableTypes[fromType] = new Dictionary<Type, IConvertReadable>();
            if (convertReadableTypes[fromType].ContainsKey(toType)) throw new Exception("Error, dupe type converters");
            convertReadableTypes[fromType][toType] = converter;
        }

        // regisers a writable type converter
        public static void RegisterConvertWritable(Type fromType, Type toType, IConvertWritable converter)
        {
            if (!convertWritableTypes.ContainsKey(fromType)) convertWritableTypes[fromType] = new Dictionary<Type, IConvertWritable>();
            if (convertWritableTypes[fromType].ContainsKey(toType)) throw new Exception("Error, dupe type converters");
            convertWritableTypes[fromType][toType] = converter;
        }


        // should be able to use extension methods: 
        // IReadAsInt MyReadableString.AsInt() 
        // IReadAsObject MyReadableString.AsIntObject() 
        // object MyReadableString.As(System.Type), would require a cast. Don't know if this is useful
        // IReadAsObject MyReadableString.AsObject(System.Type)
        // MyWritableString.Writer
        //
        public static IReadable ConvertReadable(Type toType, IReadable readable)
        {
            Type fromType = readable.GetReadType();
            if(fromType.Equals(toType)) return readable;
            if (convertReadableTypes.ContainsKey(fromType)&&convertReadableTypes[fromType].ContainsKey(toType))
                return convertReadableTypes[fromType][toType].ConvertReadable(readable);
            // tbd: do shortest path search for a->b->c type conversions. maybe resolve upstream
            throw new Exception("IReadable error, cannot convert fromType=[" + fromType.ToString() + "] to toType=[" + toType.ToString() + "]");
        }

        // returns something that will create a new object of the type each time it is run
        public static IReadable GetReadableConsructor(System.Type type)
        {
            return new ReadableConstructor(type);
        }

        // converts are writable type to accept a potentially different type.
        public static IWritable ConvertWritable(IWritable writable, System.Type toType)
        {
            Type fromType = writable.GetWriteType();
            if (toType.Equals(fromType)) return writable;
            if (convertWritableTypes.ContainsKey(fromType) && convertReadableTypes[writable.GetWriteType()].ContainsKey(toType))
                return convertWritableTypes[fromType][toType].ConvertWritable(writable);
            // tbd: do shortest path search for a->b->c type conversions. maybe resolve upstream
            throw new Exception("IWritable error, cannot convert fromType=[" + fromType.ToString() + "] to toType=[" + toType.ToString() + "]");
       }

      


    }
   
}
