using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace blogging_app.Models
{
    public class HomeModel
    {
        cls_Response response = new cls_Response();
        public string getEncryptionKey()
        {
            try
            {
                MysqlHelper DAL = new MysqlHelper();
                string query = "SELECT `RedirectionCookieKey` FROM `tbl_encryptionkey`";
                string key = Convert.ToString(DAL.ExecuteScaler(query));

                return key;
            }
            catch (Exception ex)
            {
                cls_logger.LogError(ex.Message);
                throw ex;
            }
        }

        public cls_Response add_blog(string brief, string content, string uid)
        {
            try
            {
                MysqlHelper DAL = new MysqlHelper();
                string query = "sp_add_blog";
                List<MySqlParameter> Prm = new List<MySqlParameter>();
                Int64 LID = 0;

                Prm.Add(new MySqlParameter() { ParameterName = "_brief", Value = brief });
                Prm.Add(new MySqlParameter() { ParameterName = "_content", Value = content });
                Prm.Add(new MySqlParameter() { ParameterName = "_uid", Value = uid });
                Prm.Add(new MySqlParameter() { ParameterName = "LID", Value = LID, Direction = ParameterDirection.Output });

                LID = DAL.GetExecuteNonQuery(query,CommandType.StoredProcedure,Prm,out LID);
                if (LID > 0)
                {
                    response.status = response.Success;
                    response.data = "Blog added successfully";
                }
                else
                {
                    response.status = response.Failure;
                    response.data = "Already exists";
                }

            }
            catch(Exception ex)
            {
                response.status = response.Exception;
                response.data = ex.Message;
                throw ex;
            }
            return response;
        }

        //public cls_Response verify(string decryptedCookie)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        public cls_Response update_blog(string brief, string content, string uid)
        {
            try
            {
                MysqlHelper DAL = new MysqlHelper();
                string query = "sp_add_blog";
                List<MySqlParameter> Prm = new List<MySqlParameter>();

                Prm.Add(new MySqlParameter() { ParameterName = "_brief", Value = brief });
                Prm.Add(new MySqlParameter() { ParameterName = "_content", Value = content });
                Prm.Add(new MySqlParameter() { ParameterName = "_uid", Value = uid });

                DAL.ExecuteNonQuery(query,CommandType.StoredProcedure,Prm);
                response.status = response.Success;
                response.data = "Blog updated sucessfully";
            }
            catch (Exception ex)
            {
                response.status = response.Exception;
                response.data = ex.Message;
                throw ex;
            }
            return response;
        }

        public cls_Response delete_blog(string blog_id, string uid)
        {
            try
            {
                MysqlHelper DAL = new MysqlHelper();
                string query = "sp_delete_blog";
                List<MySqlParameter> Prm = new List<MySqlParameter>();

                Prm.Add(new MySqlParameter() { ParameterName = "_uid", Value = uid });

                DAL.ExecuteNonQuery(query, CommandType.StoredProcedure, Prm);
                response.status = response.Success;
                response.data = "Blog deleted sucessfully";
            }
            catch (Exception ex)
            {
                response.status = response.Exception;
                response.data = ex.Message;
                throw ex;
            }
            return response;
        }

        public cls_Response fetch_all_blogs(string uid)
        {
            try
            {
                MysqlHelper DAL = new MysqlHelper();
                string query = "sp_fetch_all_blogs";
                List<MySqlParameter> Prm = new List<MySqlParameter>();

                Prm.Add(new MySqlParameter() { ParameterName = "_uid", Value = uid });

                DataTable dt = new DataTable();

                dt = DAL.GetDataTable(query, CommandType.StoredProcedure, Prm);
                response.status = response.Success;
                response.data = JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                response.status = response.Exception;
                response.data = ex.Message;
                throw ex;
            }
            return response;
        }
    }
}
