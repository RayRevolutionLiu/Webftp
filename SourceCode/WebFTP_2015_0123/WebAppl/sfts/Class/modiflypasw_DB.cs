using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace WebAppl
{
    public class modiflypasw_DB
    {
        public void UPDATEpassw(string mem_passw, string mem_id)
        {
            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(AppConfig.DSN);
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            try
            {
                sb.Append(@"UPDATE sfts_member SET mem_passw = @mem_passw,mem_lastlogdate = GETDATE() WHERE mem_id = @mem_id");
                oCmd.Parameters.Add("@mem_passw", SqlDbType.NVarChar).Value = mem_passw;
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
                throw new Exception(MessageUtil.DB_SelectFail + err.HelpLink);
            }
        }
    }
}