using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Xml;

namespace WebAppl
{
    public class page_mantain_DB
    {
        public DataTable GetMainList(string main_infno, string main_isempno)
        {
            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(AppConfig.DSN);
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            try
            {
                sb.Append(@"SELECT * ");
                //sb.Append(@"");
                //修改篩選七天內資料 by 凱呈 1021004
                sb.Append(@"FROM sfts_main WHERE main_infno= @main_infno AND main_isempno= @main_isempno and main_createdate > @today ORDER BY main_createdate DESC ");
                //sb.Append(@"FROM sfts_main WHERE main_infno= @main_infno AND main_isempno= @main_isempno ORDER BY main_createdate DESC ");
                //sb.Append(@"");
                //sb.Append(@"");
                //sb.Append(@"");
                oCmd.Parameters.Add("@today", SqlDbType.DateTime).Value = System.DateTime.Now.AddDays(-7).ToShortDateString();
                oCmd.Parameters.Add("@main_infno", SqlDbType.NVarChar).Value = main_infno.ToString();
                oCmd.Parameters.Add("@main_isempno", SqlDbType.NVarChar).Value = main_isempno.ToString();
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


        public DataTable GetMainListAll(string main_infno, string main_isempno,string keyword ,string satrtdate, string enddate)
        {
            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(AppConfig.DSN);
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            try
            {
                sb.Append(@"SELECT * ");
                //sb.Append(@"");
                //修改篩選七天內資料 by 凱呈 1021004
                sb.Append(@"FROM sfts_main WHERE main_infno= @main_infno AND main_isempno= @main_isempno ");
                //sb.Append(@"FROM sfts_main WHERE main_infno= @main_infno AND main_isempno= @main_isempno ORDER BY main_createdate DESC ");
                //sb.Append(@"");
                //sb.Append(@"");
                if (keyword.Trim() != "")
                {
                    sb.Append(@" AND (main_title LIKE '%'+@keyword+'%' OR main_desc LIKE '%'+@keyword+'%') ");
                }
                if (satrtdate.ToString().Trim() != "" && enddate.ToString().Trim() != "")
                {
                    sb.Append(@" AND (main_createdate between @satrtdate AND @enddate) ");
                }
                sb.Append(@" ORDER BY main_createdate DESC ");
                oCmd.Parameters.Add("@keyword", SqlDbType.NVarChar).Value = keyword.ToString();

                if (satrtdate.ToString().Trim() != "")
                {
                    oCmd.Parameters.Add("@satrtdate", SqlDbType.DateTime).Value = Convert.ToDateTime(satrtdate.ToString());
                }
                if (enddate.ToString().Trim() != "")
                {
                    oCmd.Parameters.Add("@enddate", SqlDbType.DateTime).Value = Convert.ToDateTime(enddate.ToString() + " 23:59:59.999");
                }

                oCmd.Parameters.Add("@main_infno", SqlDbType.NVarChar).Value = main_infno.ToString();
                oCmd.Parameters.Add("@main_isempno", SqlDbType.NVarChar).Value = main_isempno.ToString();
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

        public DataTable GetfileList(string afile_parentid)
        {
            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(AppConfig.DSN);
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            try
            {
                sb.Append(@"SELECT *,case afile_origiFileName when '' then afile_encryptfileName ELSE afile_origiFileName END AS ShowFileName ");
                //sb.Append(@"");
                sb.Append(@"FROM sfts_afile INNER JOIN sfts_main ON afile_parentid=main_parentid WHERE afile_parentid= @afile_parentid AND afile_encrypt <> 'N' ");
                //sb.Append(@"");
                //sb.Append(@"");
                //sb.Append(@"");
                oCmd.Parameters.Add("@afile_parentid", SqlDbType.NVarChar).Value = afile_parentid.ToString();
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

        public DataTable GetsenderList(string sender_parentid)
        {
            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(AppConfig.DSN);
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            try
            {
                sb.Append(@"SELECT *,(SELECT COUNT(*) FROM sfts_download WHERE download_parentid=sender_parentid AND download_senderkey=sender_id GROUP BY download_parentid,download_senderkey) AS hitCount,");
                sb.Append(@"(SELECT top 1 convert(varchar(12), download_createdate,111) AS download_createdate FROM sfts_download WHERE download_parentid=sender_parentid ORDER BY download_createdate DESC) AS LastDownDate ");
                sb.Append(@" FROM sfts_sender INNER JOIN sfts_main ON sender_parentid=main_parentid WHERE sender_parentid= @sender_parentid ");
                //sb.Append(@"");
                //sb.Append(@"");
                //sb.Append(@"");
                oCmd.Parameters.Add("@sender_parentid", SqlDbType.NVarChar).Value = sender_parentid.ToString();
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

        public void GetchkboxStat(string id,string type,string value)
        {
            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(AppConfig.DSN);
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            try
            {
                if (type.ToString().Trim() == "NamefileOpen")
                {
                    sb.Append(@"UPDATE sfts_afile SET afile_stat = @afile_stat WHERE afile_id = @afile_id ");
                    oCmd.Parameters.Add("@afile_stat", SqlDbType.NVarChar).Value = value;
                    oCmd.Parameters.Add("@afile_id", SqlDbType.NVarChar).Value = id;
                }
                if (type.ToString().Trim() == "NamesenderOpen")
                {
                    sb.Append(@"UPDATE sfts_sender SET sender_queryenable = @sender_queryenable WHERE sender_id = @sender_id ");
                    oCmd.Parameters.Add("@sender_queryenable", SqlDbType.NVarChar).Value = value;
                    oCmd.Parameters.Add("@sender_id", SqlDbType.NVarChar).Value = id;
                }
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

        public void DisableMain(string id)
        {
            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(AppConfig.DSN);
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            try
            {

                sb.Append(@"UPDATE sfts_main SET main_stat = 'N',main_deletedate=GETDATE() WHERE main_id = @main_id ");
                oCmd.Parameters.Add("@main_id", SqlDbType.NVarChar).Value = id;
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

        public DataTable GetReSendMail(string senderid)
        {
            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(AppConfig.DSN);
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            try
            {
                sb.Append(@"SELECT *,CONVERT(varchar(12),main_createdate,111) AS Cmain_createdate ");
                sb.Append(@",case afile_origiFileName when '' then afile_encryptfileName ELSE afile_origiFileName END AS ShowFileName ");
                sb.Append(@" FROM sfts_sender INNER JOIN sfts_main ON sender_parentid=main_parentid INNER JOIN sfts_afile ON sender_parentid=afile_parentid ");
                sb.Append(@" WHERE sender_id=@sender_id AND afile_encrypt<>'N' AND afile_stat ='Y' ");
                oCmd.Parameters.Add("@sender_id", SqlDbType.NVarChar).Value = senderid.ToString();
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