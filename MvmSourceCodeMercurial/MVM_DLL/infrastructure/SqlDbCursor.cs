using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace MVM
{
    public class SqlDbCursor : CursorCommonLinqEnabled, ICursor
    {
        // set by constructor
        private SqlDataReader reader;
        private SqlConnection conn;
        private SqlCommand command;
        private List<string> fromTables;
        // constructor
        public SqlDbCursor(ModuleContext mc, CursorSetupCommon cursorSetup, List<string> orderedFieldNames, SqlDataReader reader, SqlCommand command, SqlConnection conn, List<string> fromTables)
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
                        string paramValue = DbUtilsSql.ReadStringValue(reader, fieldName, i, "");
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
            this.command.Dispose();
            this.reader.Close();
            this.conn.Close();
        }
    }
}
