using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace WebAppl
{
    public class valid_DB
    {
        public DataTable GETdataByQueryString(string querystring)
        {
            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(AppConfig.DSN);
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            try
            {
                sb.Append(@"SELECT * FROM sfts_member WHERE mem_querystring= @mem_querystring ");
                oCmd.Parameters.Add("@mem_querystring", SqlDbType.NVarChar).Value = querystring.ToString();
                //oCmd.Parameters.Add("@actGuid", SqlDbType.UniqueIdentifier).Value = System.Guid.NewGuid();
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

        public void UPDATEpassw(string passw,string mem_id)
        {
            SqlConnection oConn = new SqlConnection(AppConfig.DSN);
            oConn.Open();
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            SqlTransaction myTrans = oConn.BeginTransaction();
            oCmd.Transaction = myTrans;
            try
            {
                oCmd.CommandText = "UPDATE sfts_member SET mem_passw = @mem_passw WHERE mem_id = @mem_id ";
                oCmd.Parameters.AddWithValue(@"mem_passw", passw);
                oCmd.Parameters.AddWithValue(@"mem_id", mem_id);
                oCmd.ExecuteNonQuery();
                myTrans.Commit();
            }
            catch (Exception ex)
            {
                myTrans.Rollback();
            }
            finally
            {
                oCmd.Connection.Close();
                oConn.Close();
            }
        }
    }
}