using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace WebAppl
{
    public class register_DB
    {
        public void INSERTmember(string mem_account, string mem_title, string mem_querystring)
        {
            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(AppConfig.DSN);
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            try
            {
                sb.Append(@"INSERT INTO sfts_member (mem_account,mem_title,mem_stat,mem_createdate,mem_lastlogdate,mem_querystring) VALUES (@mem_account,@mem_title,'0',GETDATE(),GETDATE(),@mem_querystring) ");
                oCmd.Parameters.Add("@mem_account", SqlDbType.NVarChar).Value = mem_account;
                oCmd.Parameters.Add("@mem_title", SqlDbType.NVarChar).Value = mem_title;
                oCmd.Parameters.Add("@mem_querystring", SqlDbType.NVarChar).Value = mem_querystring;
                oCmd.CommandText = sb.ToString();
                try
                {
                    oCmd.Connection.Open();
                    oCmd.ExecuteNonQuery();
                    oCmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (oCmd.Connection.State != ConnectionState.Closed)
                        oCmd.Connection.Close();
                }

            }
            catch (Exception err)
            {
                throw new Exception(MessageUtil.DB_SelectFail + err.Message);
            }
        }

        public DataTable CheckEmailExist(string email)
        {
            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(AppConfig.DSN);
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            try
            {
                sb.Append(@"SELECT mem_account FROM sfts_member WHERE UPPER(mem_account)=@mem_account ");
                //sb.Append(@"");
                oCmd.Parameters.Add("@mem_account", SqlDbType.NVarChar).Value = email.ToUpper().ToString();
                oCmd.CommandText = sb.ToString();
                SqlDataAdapter oda = new SqlDataAdapter(oCmd);
                DataTable ds = new DataTable();
                oda.Fill(ds);
                return ds;

            }
            catch (Exception err)
            {
                throw new Exception(MessageUtil.DB_SelectFail + err.HelpLink);
            }
        }
    }
}