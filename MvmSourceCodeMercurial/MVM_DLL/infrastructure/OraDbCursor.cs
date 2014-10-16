using System;
using System.Collections.Generic;
using System.Text;
using Oracle.DataAccess.Client;

namespace MVM
{
    public class OraDbCursor : CursorCommonLinqEnabled, ICursor
    {
        // set by constructor
        private OracleDataReader reader;
        private OracleConnection conn;
        private OracleCommand command;
        private List<string> fromTables;

        // constructor
        public OraDbCursor(ModuleContext mc, CursorSetupCommon cursorSetup, List<string> orderedFieldNames, OracleDataReader reader, OracleCommand command, OracleConnection conn, List<string> fromTables)
            : base(mc, cursorSetup)
        {
            this.orderedFieldNames = orderedFieldNames;
            this.reader = reader;
            this.conn = conn;
            this.command = command;
            this.fromTables = fromTables;
        }

        public override IObjectData CursorNext()
        {
            if (reader.Read())
            {
                using (var csrObj = this.CreateNewObject())
                {
                    ObjectDataFormattedDelta deltaObj = csrObj as ObjectDataFormattedDelta;
                    if (deltaObj != null)
                    {
                        deltaObj.deltaState = ObjectDataFormattedDelta.DeltaState.SettingPersisted;
                    }

                    for (int i = 0; i < this.orderedFieldNames.Count; i++)
                    {
                        string fieldName = this.orderedFieldNames[i];
                        string paramValue = DbUtilsOra.ReadStringValue(reader, fieldName, i, "");
                        csrObj[fieldName] = paramValue;
                    }
                    // if this is an erd object, load the from tables
                    if (deltaObj != null)
                    {
                        if (this.fromTables != null)
                        {
                            foreach (string fromTable in this.fromTables)
                            {
                                deltaObj.AddPersistFromTable(fromTable);
                            }
                        }
                        deltaObj.deltaState = ObjectDataFormattedDelta.DeltaState.SettingNew;
                    }
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
            this.reader.Close();
            this.conn.Close();
        }
    }
}
