using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace MVM
{
     [Module(@"
        <module_config>
            <name>object_delta_persist_cursor_changes</name>
            <xsd xmlns:xs='http://www.w3.org/2001/XMLSchema'>
                <xs:complexType>
                    <xs:sequence>
                        <xs:element name='persisted_cursor' type='xs:string' datatype='object_id' mode='in' description='cursor of object as they appear in the database'/>
                        <xs:element name='current_cursor' type='xs:string' datatype='object_id' mode='in' description='cursor of objects as you would like them to appear'/>
                        <xs:element name='primary_key' maxOccurs='unbounded' datatype='object_id' mode='in' description='cursor of objects as you would like them to appear'>
                            <xs:complexType>
                              <xs:simpleContent>
                                <xs:extension base='xs:string'>
                                  <xs:attribute name='direction' use='optional'>
                                      <xs:simpleType>
                                        <xs:restriction base='xs:string'>
                                          <xs:enumeration value='asc'/>
                                          <xs:enumeration value='desc'/>
                                        </xs:restriction>
                                      </xs:simpleType>
                                    </xs:attribute>
                                    <xs:attribute name='type' use='optional'>
                                      <xs:simpleType>
                                        <xs:restriction base='xs:string'>
                                          <xs:enumeration value='string'/>
                                          <xs:enumeration value='numeric'/>
                                        </xs:restriction>
                                      </xs:simpleType>
                                    </xs:attribute>
                                </xs:extension>
                              </xs:simpleContent>
                            </xs:complexType>
                        </xs:element>
                    </xs:sequence>
                </xs:complexType>
            </xsd>
            <doc>
                <category>Delta Objects</category>
                <description>Makes the persisted_cursor in the database looks like the current_cursor</description>
            </doc>
        </module_config>
    ")]
    class MObjectDeltaPersistCursorChanges: BaseModuleSetup,IModuleRun
    {
        private string persistedCursorSyntax;
        private IReadString persistedObjectIdParsed;
        private string currentCursorSyntax;
        private IReadString currentObjectIdParsed;
        private List<string> pkSyntax;
        private List<IReadString> pkParsed;
        private ObjectComparer objectComparer;
        
        public override void Setup(XmlElement me, ModuleContext mc,List<IModuleRun> run)
        {
            MObjectDeltaPersistCursorChanges m = new MObjectDeltaPersistCursorChanges();
            this.SetupReadString(me, mc, "persisted_cursor", out m.persistedCursorSyntax, out m.persistedObjectIdParsed);
            this.SetupReadString(me, mc, "current_cursor", out m.currentCursorSyntax, out m.currentObjectIdParsed);
            this.SetupReadString(me, mc, "primary_key", out m.pkSyntax, out m.pkParsed);


            List<ObjectFieldCompareInfo> primaryKey=new List<ObjectFieldCompareInfo>();
            foreach (XmlElement elem in me.SelectNodes("./primary_key"))
            {
                string fieldName = mc.SyntaxReadString(elem.InnerText);
                int ufn=mc.mvmCluster.GetUfn(fieldName);
                string direction = elem.GetAttributeDefault("direction", "asc");
                bool isDesc=direction.EqualsIgnoreCase("desc");
                bool isNumeric=direction.EqualsIgnoreCase("numeric");
                string type = elem.GetAttributeDefault("type", "string");
                primaryKey.Add(new ObjectFieldCompareInfo(fieldName,ufn,isNumeric,isDesc));
            }
            m.objectComparer = new ObjectComparer(primaryKey);
            run.Add(m);
        }
        public void Run(ModuleContext mc)
        {
            CursorDiffer cursorDiffer;

            // if we don't have cursor differ in play, create one now.
            if (mc.tempContext[CursorDiffer.tempFieldName].Equals(""))
            {
                mc.mvm.Log("Instanciating a cursorDiffer");
                string persistedOid = this.persistedObjectIdParsed.Read(mc);
                string currentOid = this.currentObjectIdParsed.Read(mc);
                List<string> pk = new List<string>();
                foreach (var primaryParsed in this.pkParsed)
                {
                    pk.Add(primaryParsed.Read(mc));
                }

                ICursor persistedCursor;
                if (!mc.LookupCursorViaOid(persistedOid, out persistedCursor))
                {
                    persistedCursor = null;
                }

                ICursor currentCursor;
                if (!mc.LookupCursorViaOid(currentOid, out currentCursor))
                {
                    currentCursor = null;
                }

                cursorDiffer = new CursorDiffer(mc, persistedCursor,currentCursor,objectComparer);
                mc.globalContext.SetNamedClassInst(cursorDiffer.globalName, cursorDiffer);
                mc.tempContext[CursorDiffer.tempFieldName] = cursorDiffer.globalName;
            }
            else
            {
                cursorDiffer = mc.globalContext.GetNamedClassInst(mc.tempContext[CursorDiffer.tempFieldName]) as CursorDiffer;
            }

            // pulse the cursor differ
            cursorDiffer.Run(mc);
        }
    }

     public class CursorDiffer : IModuleRun
     {
         public static readonly string tempFieldName = "cursor_differ";
         public readonly string globalName;
         readonly ICursor currentCursor;
         readonly ICursor persistedCursor;
         readonly ObjectComparer objectComparer;
         ObjectDataFormattedDelta currentObj;
         ObjectDataFormattedDelta persistedObj;
         CursorStatus persistedStatus=CursorStatus.YIELD;
         CursorStatus currentStatus=CursorStatus.YIELD;

         public CursorDiffer(ModuleContext mc, ICursor persistedCursor, ICursor currentCursor,ObjectComparer objectComparer)
         {
             this.globalName = mc.GetGenSym("cursorDiffer");
             this.persistedCursor = persistedCursor;
             this.currentCursor = currentCursor;
             this.objectComparer = objectComparer;
             this.MyCursorNext(mc, this.persistedCursor, out this.persistedStatus, out this.persistedObj);
             this.MyCursorNext(mc, this.currentCursor, out this.currentStatus, out this.currentObj);
         }

         public bool MyCursorNext(ModuleContext mc,ICursor cursor, out CursorStatus cursorStatus, out ObjectDataFormattedDelta cursorObj)
         {
             cursorObj = null;
             IObjectData myObj;
             cursorStatus = cursor.Next(mc, out myObj);
             switch (cursorStatus)
             {
                 case CursorStatus.HAS_ROW:
                     {
                         cursorObj = myObj as ObjectDataFormattedDelta;
                         return true;
                     }
                 case CursorStatus.EOF:
                     {
                         return false;
                     }
                 case CursorStatus.PARENT_NEXT:
                     {
                         return false;
                     }
                 case CursorStatus.YIELD:
                     {
                         return false;
                     }
                 default: throw new Exception("unexpected cursorStatus=" + cursorStatus);
             }
         }

        
         /// <summary>
         /// Diffs the persisted cursor to the current cursor and persists the changes.
         /// This expects the cursors to already be on their first row or NULL.
         /// </summary>
         /// <param name="persistedCsr"></param>
         /// <param name="currentCsr"></param>
         public void Run(ModuleContext mc)
         {

             for (; ; )
             {
                 // if last time we didnt get a row try again
                 if (persistedStatus == CursorStatus.YIELD)
                 {
                     if (!this.MyCursorNext(mc, this.persistedCursor, out this.persistedStatus, out this.persistedObj))
                     {
                         mc.YieldAndCallback();
                         return;
                     }
                 }

                 // if last time we didnt get a row try again
                 if (currentStatus == CursorStatus.YIELD)
                 {
                     if (!this.MyCursorNext(mc, this.currentCursor, out this.currentStatus, out this.currentObj))
                     {
                         mc.YieldAndCallback();
                         return;
                     }
                 }

                 // if current and persisted at EOF, we're done
                 if (persistedCursor.Eof && currentCursor.Eof)
                 {
                     return;
                 }

                 // if persisted cursor is null, insert the rest of current
                 if (persistedCursor.Eof)
                 {
                     do
                     {
                         currentObj.PersistObjectChanges(null, this.currentObj);
                     } while (this.MyCursorNext(mc, this.currentCursor, out this.currentStatus, out this.currentObj));
                     if (!this.currentCursor.Eof)
                     {
                         mc.YieldAndCallback();
                     }
                     return;
                 }

                 // if current cursor is null, deleted the rest of persisted
                 if (this.currentCursor.Eof)
                 {
                     do
                     {
                         persistedObj.PersistObjectChanges(this.persistedObj, null);
                     } while (this.MyCursorNext(mc, this.persistedCursor, out this.persistedStatus, out this.persistedObj));
                     if (!this.persistedCursor.Eof)
                     {
                         mc.YieldAndCallback();
                     }
                     return;
                 }

                 int cmp = this.objectComparer.Compare(persistedObj, currentObj);
                 if (cmp == 0)
                 {
                     persistedObj.PersistObjectChanges(this.persistedObj, this.currentObj);
                     bool p = this.MyCursorNext(mc, this.persistedCursor, out this.persistedStatus, out this.persistedObj);
                     bool c = this.MyCursorNext(mc, this.currentCursor, out this.currentStatus, out this.currentObj);
                     if (!p || !c)
                     {
                         mc.YieldAndCallback();
                         return;
                     }
                 }
                 else if (cmp > 0)
                 {
                     bool c = this.MyCursorNext(mc, this.currentCursor, out this.currentStatus, out this.currentObj);
                     if (!c)
                     {
                         mc.YieldAndCallback();
                         return;
                     }
                 }
                 else if (cmp < 0)
                 {
                     bool p = this.MyCursorNext(mc, this.persistedCursor, out this.persistedStatus, out this.persistedObj);
                     if (!p)
                     {
                         mc.YieldAndCallback();
                         return;
                     }
                 }
                 else
                 {
                     throw new Exception("unreacheable 12312344987");
                 }
             }
         }
     }

     public class ObjectFieldCompareInfo
     {
         public string fieldName;
         public int ufn;
         public bool isNumeric;
         public bool isDesc;
         public ObjectFieldCompareInfo(string fieldName,int ufn,bool isNumeric,bool isDesc){
             this.fieldName=fieldName;
             this.ufn=ufn;
             this.isNumeric=isNumeric;
             this.isDesc=isDesc;
         }
     }

     public class ObjectComparer : IComparer<IObjectData>
     {
         #region IComparer<IObjectData> Members
         // fieldname, fieldUfn, isNumeric, isDesc
         List<ObjectFieldCompareInfo> primaryKey;
         public ObjectComparer(List<ObjectFieldCompareInfo> primaryKey)
         {
             this.primaryKey = primaryKey;
         }

         public int Compare(IObjectData x, IObjectData y)
         {
             foreach (var pk in primaryKey)
             {
                 string xVal=x[pk.ufn];
                 string yVal=x[pk.ufn];
                 if (pk.isNumeric)
                 {
                     decimal xDec = Decimal.Parse(xVal);
                     decimal yDec = Decimal.Parse(yVal);
                     int cmp = xDec.CompareTo(yDec);
                     if (cmp != 0)
                     {
                         return pk.isDesc ? 0 - cmp : cmp;
                     }
                 }
                 else
                 {
                     int cmp = xVal.CompareTo(yVal);
                     if (cmp != 0)
                     {
                         return pk.isDesc ? 0 - cmp : cmp;
                     }
                 }
             }
             return 0;
         }

         #endregion
     }
}
