using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace blogging_app
{
    public sealed class MysqlHelper
    {
        private string DbConnection = blogging_app.Startup.getDefaultConnectionString();
        private MySqlConnection DBCon = null;
        public MysqlHelper()
        {
            DBCon = new MySqlConnection(DbConnection);
        }

        public String TitleCaseString(String s)
        {
            if (s == null) return s;

            String[] words = s.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length == 0) continue;

                Char firstChar = Char.ToUpper(words[i][0]);
                String rest = "";
                if (words[i].Length > 1)
                {
                    rest = words[i].Substring(1).ToLower();
                }
                words[i] = firstChar + rest;
            }
            return String.Join(" ", words);
        }
        private void OPenConnection()
        {
            if (DBCon.State != ConnectionState.Open)
                DBCon.Open();
        }

        private void CloseConnection()
        {
            if (DBCon.State != ConnectionState.Closed)
            {
                DBCon.Close();
                DBCon.Dispose();
            }
        }
        private Int32 GetExecuteNonQuery(string commendText, CommandType cmdType, List<MySqlParameter> Prms)
        {
            Int32 RowUpdate = 0;
            try
            {
                OPenConnection();
                using (MySqlCommand mycmd = new MySqlCommand(commendText, DBCon))
                {
                    mycmd.CommandType = cmdType;
                    if (Prms != null)
                    {
                        foreach (MySqlParameter prm in Prms)
                        {
                            mycmd.Parameters.Add(prm);
                        }
                    }
                    RowUpdate = mycmd.ExecuteNonQuery();
                }
                CloseConnection();
                return RowUpdate;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public Int32 GetExecuteNonQuery(string commendText, CommandType cmdType, List<MySqlParameter> Prms, out Int64 LID)
        {
            Int32 RowUpdate = 0;
            Int64 LastRecordid = 0;
            try
            {
                OPenConnection();
                using (MySqlCommand mycmd = new MySqlCommand(commendText, DBCon))
                {
                    mycmd.CommandType = cmdType;
                    if (Prms != null)
                    {
                        foreach (MySqlParameter prm in Prms)
                        {
                            mycmd.Parameters.Add(prm);
                        }
                        mycmd.Parameters.Add(new MySqlParameter() { ParameterName = "_Recordid", Direction = ParameterDirection.Output });
                    }
                    RowUpdate = mycmd.ExecuteNonQuery();
                    LastRecordid = Convert.ToInt64(mycmd.Parameters["LID"].Value);
                }
                CloseConnection();
                LID = LastRecordid;
                return RowUpdate;
            }
            catch (Exception ex)
            {
                LID = 0;
                return 0;
            }
        }

        public Int32 ExecuteNonQuery(string commendText, CommandType cmdType, List<MySqlParameter> Prms, out Int64 Recordid)
        {
            return GetExecuteNonQuery(commendText, cmdType, Prms, out Recordid);
        }
        public Int32 ExecuteNonQuery(string commendText, CommandType cmdType, out Int64 Recordid)
        {
            return GetExecuteNonQuery(commendText, cmdType, null, out Recordid);
        }
        public Int32 ExecuteNonQuery(string commendText, out Int64 Recordid)
        {
            return GetExecuteNonQuery(commendText, CommandType.Text, null, out Recordid);
        }
        public Int32 ExecuteNonQuery(string commendText, CommandType cmdType, List<MySqlParameter> Prms)
        {
            return GetExecuteNonQuery(commendText, cmdType, Prms);
        }
        public Int32 ExecuteNonQuery(string commendText, CommandType cmdType)
        {
            return GetExecuteNonQuery(commendText, cmdType, null);
        }
        public Int32 ExecuteNonQuery(string commendText)
        {
            return GetExecuteNonQuery(commendText, CommandType.Text, null);
        }
        public object ExecuteScaler(string commendText, CommandType cmdType, List<MySqlParameter> Prms)
        {
            return GetExecuteScaler(commendText, cmdType, Prms);
        }
        public object ExecuteScaler(string commendText, CommandType cmdType)
        {
            return GetExecuteScaler(commendText, cmdType, null);
        }
        public object ExecuteScaler(string commendText)
        {
            return GetExecuteScaler(commendText, CommandType.Text, null);
        }

        private Int32 GetExecuteScaler(string commendText, CommandType cmdType, List<MySqlParameter> Prms, out Int64 Recordid)
        {
            Int32 RowUpdate = 0;
            Int32 LastRecordid = 0;
            try
            {
                OPenConnection();
                using (MySqlCommand mycmd = new MySqlCommand(commendText, DBCon))
                {
                    mycmd.CommandType = cmdType;
                    if (Prms != null)
                    {
                        foreach (MySqlParameter prm in Prms)
                        {
                            mycmd.Parameters.Add(prm);
                        }
                    }
                    RowUpdate = mycmd.ExecuteNonQuery();
                    List<MySqlParameter> lstout = Prms.Where(p => p.Direction == ParameterDirection.Output).ToList<MySqlParameter>();
                    LastRecordid = Convert.ToInt32(mycmd.Parameters[lstout[0].ParameterName].Value);
                }
                CloseConnection();
                Recordid = LastRecordid;
                return LastRecordid;
            }
            catch (Exception ex)
            {
                Recordid = 0;
                return 0;
            }
        }

        private object GetExecuteScaler(string commendText, CommandType cmdType, List<MySqlParameter> Prms)
        {
            object data = null;
            try
            {
                OPenConnection();
                using (MySqlCommand mycmd = new MySqlCommand(commendText, DBCon))
                {
                    mycmd.CommandType = cmdType;
                    if (Prms != null)
                    {

                        foreach (MySqlParameter prm in Prms)
                        {
                            mycmd.Parameters.Add(prm);
                        }
                    }
                    data = mycmd.ExecuteScalar();
                    var id = mycmd.LastInsertedId;
                }
                CloseConnection();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                data = null;
            }
        }
        #region GetData Table
        public DataTable GetDataTable(string commendText, CommandType cmdType, List<MySqlParameter> Prms)
        {
            return DataSet(commendText, cmdType, Prms).Tables[0];
        }
        public DataTable GetDataTable(string commendText)
        {
            return DataSet(commendText, CommandType.Text, null).Tables[0];
        }
        public DataTable GetDataTable(string commendText, CommandType cmdType)
        {
            return DataSet(commendText, cmdType, null).Tables[0];
        }
        #endregion
        #region Get Data Set
        public DataSet GetDataSet(string commendText)
        {
            return DataSet(commendText, CommandType.Text, null);
        }
        public DataSet GetDataSet(string commendText, CommandType cmdType)
        {
            return DataSet(commendText, cmdType, null);
        }
        public DataSet GetDataSet(string commendText, CommandType cmdType, List<MySqlParameter> Prms)
        {
            return DataSet(commendText, cmdType, Prms);
        }
        #endregion
        private DataSet DataSet(string commendText, CommandType cmdType, List<MySqlParameter> Prms)
        {
            DataSet dsData = null;
            try
            {
                OPenConnection();
                dsData = new DataSet();
                using (MySqlCommand mycmd = new MySqlCommand(commendText, DBCon))
                {
                    mycmd.CommandType = cmdType;
                    if (Prms != null)
                    {
                        foreach (MySqlParameter prm in Prms)
                        {
                            mycmd.Parameters.Add(prm);
                        }
                    }
                    using (MySqlDataAdapter myadp = new MySqlDataAdapter(mycmd))
                    {
                        myadp.Fill(dsData);
                    }
                }
                CloseConnection();
                return dsData;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dsData.Dispose();
            }
        }

        public string GetLastInsertedID(string commendText)
        {
            string lastInsertedID = string.Empty;
            try
            {
                OPenConnection();

                using (MySqlCommand mycmd = new MySqlCommand(commendText, DBCon))
                {
                    mycmd.ExecuteNonQuery();
                    lastInsertedID = Convert.ToString(mycmd.LastInsertedId);
                }
                CloseConnection();
                return lastInsertedID;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
