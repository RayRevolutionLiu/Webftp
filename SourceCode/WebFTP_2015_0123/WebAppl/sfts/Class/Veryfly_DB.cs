using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace WebAppl
{
    public class Veryfly_DB
    {
        public DataTable CheckQueryExist(string querystring)
        {
            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(AppConfig.DSN);
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            try
            {
                sb.Append(@"select * ,convert(varchar(15),main_createdate,111) AS main_createdateCon, convert(varchar(15), DATEADD(day, 7, main_createdate) ,111) AS main_deldate ");
                sb.Append(@",case afile_origiFileName when '' then afile_encryptfileName ELSE afile_origiFileName END  AS ShowFileName ");
                sb.Append(@" from sfts_sender inner join sfts_afile ON sender_parentid=afile_parentid inner join sfts_main ON main_parentid=sender_parentid ");
                sb.Append(@" where sender_querystring= @sender_querystring AND DATEDIFF(day,main_createdate,GETDATE()) <= 7");
                oCmd.Parameters.Add("@sender_querystring", SqlDbType.NVarChar).Value = querystring.ToString();
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


        public DataTable downloadfileList(string afile_id, string afile_parentid, string afile_comorsec)
        {
            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(AppConfig.DSN);
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            try
            {
                sb.Append(@"SELECT *,case afile_origiFileName when '' then afile_encryptfileName ELSE afile_origiFileName END AS ShowFileName ");
                sb.Append(@"FROM sfts_afile INNER JOIN sfts_main ON afile_parentid=main_parentid WHERE afile_id=@afile_id AND afile_parentid=@afile_parentid AND afile_comorsec=@afile_comorsec ");
                oCmd.Parameters.Add("@afile_id", SqlDbType.NVarChar).Value = afile_id.ToString();
                oCmd.Parameters.Add("@afile_parentid", SqlDbType.NVarChar).Value = afile_parentid.ToString();
                oCmd.Parameters.Add("@afile_comorsec", SqlDbType.NVarChar).Value = afile_comorsec.ToString();
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

        public DataTable checkimagetext(string sender_id,string afile_id)
        {
            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(AppConfig.DSN);
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            try
            {
                sb.Append(@"SELECT *,case afile_origiFileName when '' then afile_encryptfileName ELSE afile_origiFileName END AS ShowFileName FROM sfts_sender INNER JOIN sfts_afile ON sender_parentid=afile_parentid INNER JOIN sfts_main ON sender_parentid=main_parentid WHERE sender_id= @sender_id AND afile_id= @afile_id");
                oCmd.Parameters.Add("@sender_id", SqlDbType.NVarChar).Value = sender_id.ToString();
                oCmd.Parameters.Add("@afile_id", SqlDbType.NVarChar).Value = afile_id.ToString();
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

        public void UpdateSender_Stat(string sender_id)
        {
            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(AppConfig.DSN);
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            try
            {
                sb.Append(@"UPDATE sfts_sender SET sender_stat='N' WHERE sender_id=@sender_id ");
                oCmd.Parameters.Add("@sender_id", SqlDbType.NVarChar).Value = sender_id;
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


        public void UpdateSender_QueryEnable(string sender_id)
        {
            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(AppConfig.DSN);
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            try
            {
                sb.Append(@"UPDATE sfts_sender SET sender_queryenable='N' WHERE sender_id=@sender_id ");
                oCmd.Parameters.Add("@sender_id", SqlDbType.NVarChar).Value = sender_id;
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


        public void InsertDownloadLog(string download_fgkey, string download_parentid, string download_senderkey, DateTime download_createdate)
        {
            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(AppConfig.DSN);
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            try
            {
                sb.Append(@"INSERT INTO sfts_download (download_fgkey,download_parentid,download_senderkey,download_createdate) VALUES (@download_fgkey,@download_parentid,@download_senderkey,@download_createdate)");
                oCmd.Parameters.Add("@download_fgkey", SqlDbType.BigInt).Value = Convert.ToInt64(download_fgkey);
                oCmd.Parameters.Add("@download_parentid", SqlDbType.NVarChar).Value = download_parentid;
                oCmd.Parameters.Add("@download_senderkey", SqlDbType.BigInt).Value = Convert.ToInt64(download_senderkey);
                oCmd.Parameters.Add("@download_createdate", SqlDbType.DateTime).Value = download_createdate;
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

        #region 把認證碼存回資料庫
        public void UpdateImageText(string ramd, string sender_id)
        {
            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(AppConfig.DSN);
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            sb.Append(@"UPDATE sfts_sender SET sender_imagetext = @sender_imagetext,sender_passValiddate=GETDATE() WHERE sender_id = @sender_id ");
            oCmd.Parameters.Add("@sender_imagetext", SqlDbType.NVarChar).Value = ramd;
            oCmd.Parameters.Add("@sender_id", SqlDbType.NVarChar).Value = sender_id;
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
        #endregion
    }
}