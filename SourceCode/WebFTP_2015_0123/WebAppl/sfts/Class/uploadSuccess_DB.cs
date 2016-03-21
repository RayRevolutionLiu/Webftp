using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace WebAppl
{
    public class uploadSuccess_DB
    {
        public DataTable GetSuccessResult(string flag, string parentid)
        {
            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(AppConfig.DSN);
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            try
            {
                if (flag == "main")
                {
                    sb.Append(@"SELECT * FROM sfts_main WHERE main_parentid=@parentid ");
                }
                else if (flag == "sender")
                {
                    sb.Append(@"SELECT * FROM sfts_sender WHERE sender_parentid=@parentid ");
                }
                else
                {
                    sb.Append(@"SELECT *,file_origiFileName+file_exten AS file_origiFileNameALL FROM sfts_file WHERE file_parentid=@parentid ");
                }

                oCmd.Parameters.Add("@parentid", SqlDbType.NVarChar).Value = parentid.ToString();
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