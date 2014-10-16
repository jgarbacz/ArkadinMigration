//using System;
//using System.Collections.Generic;

//using System.Text;

//namespace MVM
//{
//    public class SterileCursor: ICursor
//    {
//        private List<string> fieldNames;
//        public string oid;

//        public SterileCursor(ModuleContext mc, string inputCursorOid)
//        {
//            // get the object type of the input cursor
//            string inputCursorObjectType=mc.ReadObjectField(inputCursorOid, "object_type");
        
//            // TBD: NO NEED TO CHANGE OT (JUST FOR DEBUG)
//            string sterileCursorObjectType = "STERILE_" + inputCursorObjectType; 

//            // spawn a new object for the sterile cursor and save the id
//            this.oid = mc.Spawn(sterileCursorObjectType);
           
//            // inherit all the fields from the input cursor
//            mc.InheritObject(inputCursorOid, this.oid);

//            // Lookup the input cursor object so we can get the field names and copy them to the sterile cursor
//            ICursor cursor = (ICursor)mc.globalContext.GetNamedClassInst(inputCursorOid);
//            this.fieldNames = cursor.GetOrderedFieldNames();

//            // register the sterile cursor
//            mc.globalContext.SetNamedClassInst(this.oid, this);
//        }
//        public void Clear(ModuleContext mc)
//        {
//            //Console.WriteLine("clearing sterile cursor, oid="+this.oid+", ot="+mc.ReadObjectField(this.oid,"object_type"));
//        }

//        public bool Next(ModuleContext mc)
//        {
//            return false;
//        }

//        public List<string> GetOrderedFieldNames()
//        {
//            return this.fieldNames;
//        }
//    }
//}
