using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace WebAppl
{
    public class ForGetPassW_DB
    {
        public DataTable SelectEmailExist(string email)
        {
            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(AppConfig.DSN);
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            try
            {
                sb.Append(@"SELECT * FROM sfts_member WHERE UPPER(mem_account) = @mem_account ");
                oCmd.Parameters.Add("@mem_account", SqlDbType.NVarChar).Value = email.ToString();
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

        public void UpdateMemberQueryStr(string mem_querystring, string mem_id)
        {
            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(AppConfig.DSN);
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            try
            {
                sb.Append(@"UPDATE sfts_member SET mem_querystring = @mem_querystring WHERE mem_id = @mem_id ");
                oCmd.Parameters.Add("@mem_querystring", SqlDbType.NVarChar).Value = mem_querystring;
                oCmd.Parameters.Add("@mem_id", SqlDbType.NVarChar).Value = mem_id;
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
    }
}