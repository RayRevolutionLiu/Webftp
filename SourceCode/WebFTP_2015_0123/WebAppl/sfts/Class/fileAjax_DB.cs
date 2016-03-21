using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace WebAppl
{
    public class fileAjax_DB
    {
        public void InsertFile(string file_parentid, string file_type,string file_comorsec, string file_encrypt, string file_origiFileName,string file_encryptfileName,int file_size,string file_exten, DateTime file_createdate, string file_stat)
        {
            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(AppConfig.DSN);
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            try
            {
                sb.Append(@"INSERT INTO sfts_file (file_parentid ,file_type, file_comorsec, file_encrypt,file_origiFileName,file_encryptfileName,file_size,file_exten,file_createdate,file_stat) ");
                sb.Append(@"VALUES (@file_parentid, @file_type, @file_comorsec, @file_encrypt, @file_origiFileName, @file_encryptfileName,@file_size,@file_exten, @file_createdate, @file_stat) ");
                oCmd.Parameters.Add("@file_parentid", SqlDbType.NVarChar).Value = file_parentid;
                oCmd.Parameters.Add("@file_type", SqlDbType.NVarChar).Value = file_type;
                oCmd.Parameters.Add("@file_comorsec", SqlDbType.NVarChar).Value = file_comorsec;
                oCmd.Parameters.Add("@file_encrypt", SqlDbType.NVarChar).Value = file_encrypt;
                oCmd.Parameters.Add("@file_origiFileName", SqlDbType.NVarChar).Value = file_origiFileName;
                oCmd.Parameters.Add("@file_encryptfileName", SqlDbType.NVarChar).Value = file_encryptfileName;
                oCmd.Parameters.Add("@file_size", SqlDbType.NVarChar).Value = file_size;
                oCmd.Parameters.Add("@file_exten", SqlDbType.NVarChar).Value = file_exten;
                oCmd.Parameters.Add("@file_createdate", SqlDbType.DateTime).Value = file_createdate;
                oCmd.Parameters.Add("@file_stat", SqlDbType.NVarChar).Value = file_stat;
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


        public void InsertSender(string sender_parentid, string sender_mail,string sender_querystring,
            string sender_notifyflag, string sender_disabled, string sender_logindate, string sender_downloaddate, string sender_stat, string sender_isempno)
        {
            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(AppConfig.DSN);
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            try
            {
                sb.Append(@"INSERT INTO sfts_sender (sender_parentid ,sender_mail,sender_isempno, sender_querystring,sender_notifyflag,sender_disabled,sender_logindate,sender_downloaddate,sender_stat,sender_queryenable) ");
                sb.Append(@"VALUES (@sender_parentid, @sender_mail,@sender_isempno, @sender_querystring, @sender_notifyflag, @sender_disabled, @sender_logindate,@sender_downloaddate,@sender_stat,'Y') ");
                oCmd.Parameters.Add("@sender_parentid", SqlDbType.NVarChar).Value = sender_parentid;
                oCmd.Parameters.Add("@sender_mail", SqlDbType.NVarChar).Value = sender_mail;
                oCmd.Parameters.Add("@sender_isempno", SqlDbType.NVarChar).Value = sender_isempno;
                oCmd.Parameters.Add("@sender_querystring", SqlDbType.NVarChar).Value = sender_querystring;
                oCmd.Parameters.Add("@sender_notifyflag", SqlDbType.NVarChar).Value = sender_notifyflag;
                oCmd.Parameters.Add("@sender_disabled", SqlDbType.NVarChar).Value = sender_disabled;
                if (sender_logindate == "")
                {
                    oCmd.Parameters.Add("@sender_logindate", SqlDbType.DateTime).Value = DBNull.Value;
                }
                else
                {
                    oCmd.Parameters.Add("@sender_logindate", SqlDbType.DateTime).Value = sender_logindate;
                }
                if (sender_downloaddate == "")
                {
                    oCmd.Parameters.Add("@sender_downloaddate", SqlDbType.DateTime).Value = DBNull.Value;
                }
                else
                {
                    oCmd.Parameters.Add("@sender_downloaddate", SqlDbType.DateTime).Value = sender_downloaddate;
                }
                oCmd.Parameters.Add("@sender_stat", SqlDbType.NVarChar).Value = sender_stat;
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


        public void InsertMain(string main_isempno, string main_infno, string main_parentid, string main_title, string main_desc, string main_stat, string main_secret, DateTime main_createdate)
        {
            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(AppConfig.DSN);
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            try
            {
                sb.Append(@"INSERT INTO sfts_main ");
                sb.Append(@"(main_isempno,main_infno,main_parentid,main_title,main_desc,main_stat,main_secret,main_createdate) ");
                sb.Append(@"VALUES ");
                sb.Append(@"(@main_isempno,@main_infno,@main_parentid,@main_title,@main_desc,@main_stat,@main_secret,@main_createdate) ");
                oCmd.Parameters.Add("@main_isempno", SqlDbType.NVarChar).Value = main_isempno;
                oCmd.Parameters.Add("@main_infno", SqlDbType.NVarChar).Value = main_infno;
                oCmd.Parameters.Add("@main_parentid", SqlDbType.NVarChar).Value = main_parentid;
                oCmd.Parameters.Add("@main_title", SqlDbType.NVarChar).Value = main_title;
                oCmd.Parameters.Add("@main_desc", SqlDbType.NVarChar).Value = main_desc;
                oCmd.Parameters.Add("@main_stat", SqlDbType.NVarChar).Value = main_stat;
                oCmd.Parameters.Add("@main_secret", SqlDbType.NVarChar).Value = main_secret;
                oCmd.Parameters.Add("@main_createdate", SqlDbType.DateTime).Value = main_createdate;
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