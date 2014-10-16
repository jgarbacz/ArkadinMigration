using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Xml;
using System.IO;
using Oracle.DataAccess.Client;
using Antlr.Runtime.Tree;

namespace MVM
{
    class MGenerateMetraNetConfigOra 
    {
        public static Dictionary<string, List<string>> GetPiPtMap(ModuleContext mc, IDbLoginInfo dbInfo)
        {
            Dictionary<string, List<string>> piParamtables = new Dictionary<string, List<string>>();
            string db = dbInfo.GetDb(mc);
            string server = dbInfo.GetServer(mc);
            string user = dbInfo.GetUser(mc);
            string pw = dbInfo.GetPw(mc);
            string type = dbInfo.GetType(mc);
            if (type.Equals("oracle"))
            {
                string queryString =
                    @"select lower(b.nm_name) pi_name, lower(c.nm_instance_tablename) pt_name " +
                    @"from t_pi_rulesetdef_map a, t_base_props b,t_rulesetdefinition c where a.id_pi=b.id_prop and a.id_pt=c.id_paramtable and b.nm_name is not null " +
                    @"order by b.nm_name ";
                string connString = @"Pooling=true;server = " + db + ";uid = " + user + ";password = " + pw + ";";
                //Console.WriteLine("connecting with String: {0}", connString);
                using (OracleConnection conn = DbUtilsOra.GetOraConnection(db, user, pw))
                {
                    try
                    {
                        OracleCommand command = new OracleCommand(queryString, conn);
                        OracleDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            string piName = reader.GetString(0);
                            string ptName = reader.GetString(1);
                            if (!piParamtables.ContainsKey(piName)) piParamtables[piName] = new List<string>();
                            piParamtables[piName].Add(ptName);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Error, running query string=[" + queryString + "] using connString=[" + connString + "], msg=" + e.Message, e);
                    }
                }
                return piParamtables;
            }
            else
            {
                throw new Exception("expecting oracle");
            }
        }
        public static Dictionary<string, Dictionary<string, int>> GetEnumDictionary(ModuleContext mc, IDbLoginInfo dbInfo)
        {
            Dictionary<string, Dictionary<string, int>> enumDictionary = new Dictionary<string, Dictionary<string, int>>();
            string db = dbInfo.GetDb(mc);
            string server = dbInfo.GetServer(mc);
            string user = dbInfo.GetUser(mc);
            string pw = dbInfo.GetPw(mc);
            string type = dbInfo.GetType(mc);
            if (type.Equals("oracle"))
            {
                string queryString =
@"select " +
@"nvl(lower(substr(nm_enum_data,0,instr(nm_enum_data,'/',-1,1)-1)),'junk') enum_name, " +
@"substr(nm_enum_data,instr(nm_enum_data,'/',-1,1)+1) enum_value, " +
@"id_enum_data " +
@"from t_enum_data";
                using (OracleConnection conn = DbUtilsOra.GetOraConnection(db, user, pw))
                {
                    try
                    {
                        OracleCommand command = new OracleCommand(queryString, conn);
                        OracleDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            string enumName = reader.GetString(0).ToLower();
                            string enumValue = reader.GetString(1);
                            int idEnumData = reader.GetInt32(2);
                            if (!enumDictionary.ContainsKey(enumName)) enumDictionary[enumName] = new Dictionary<string, int>();
                            enumDictionary[enumName][enumValue] = idEnumData;
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Error, running query string=[" + queryString + "] using connString=[" + conn.ConnectionString + "], msg=" + e.Message, e);
                    }
                }
                return enumDictionary;
            }
            else
            {
                throw new Exception("expecting oracle");
            }
        }
    }
}
