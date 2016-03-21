using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace WebAppl
{
    public class getHistory_DB
    {
        public DataTable getHistoryList(string main_infno, string main_isempno, string FromYear, string FromMonth, string ToYear, string ToMonth)
        {
            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(AppConfig.DSN);
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            try
            {
                sb.Append(@"getHistorySp");
                //sb.Append(@"");
                oCmd.Parameters.Add("@main_infno", SqlDbType.NVarChar).Value = main_infno.ToString();
                oCmd.Parameters.Add("@main_isempno", SqlDbType.NVarChar).Value = main_isempno.ToString();
                oCmd.Parameters.Add("@FromYear", SqlDbType.NVarChar).Value = FromYear.ToString();
                oCmd.Parameters.Add("@FromMonth", SqlDbType.NVarChar).Value = FromMonth.ToString();
                oCmd.Parameters.Add("@ToYear", SqlDbType.NVarChar).Value = ToYear.ToString();
                oCmd.Parameters.Add("@ToMonth", SqlDbType.NVarChar).Value = ToMonth.ToString();
                oCmd.CommandText = sb.ToString();
                oCmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter oda = new SqlDataAdapter(oCmd);
                DataTable ds = new DataTable();
                oda.Fill(ds);
                return ds;

            }
            catch (Exception err)
            {
                throw new Exception(MessageUtil.DB_SelectFail + err.Message);
            }
        }
    }
}