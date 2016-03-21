using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.SessionState;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace WebAppl.sfts.enVersion.handlerEn
{
    /// <summary>
    ///nonforwardEn 的摘要描述
    /// </summary>
    public class nonforwardEn : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string aid = context.Request.Form["aid"].ToString().Trim();

            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(AppConfig.DSN);
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            sb.Append(@"SELECT * FROM sfts_sender WHERE sender_id = @sender_id ");
            oCmd.Parameters.Add("@sender_id", SqlDbType.NVarChar).Value = aid;
            DataTable dt = new DataTable();
            oCmd.CommandText = sb.ToString();
            try
            {
                oCmd.Connection.Open();
                SqlDataAdapter oda = new SqlDataAdapter(oCmd);
                oda.Fill(dt);

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


            if (dt.Rows.Count > 0)
            {
                Email myEmail = new Email();
                StringBuilder sbContent = new StringBuilder();

                Random ram = new Random();
                int numb = ram.Next(9999);

                //noforward 要去給一組新的imagetext 再UPDATE 回stts_sender 的sender_imagetext
                if (dt.Rows[0]["sender_passValiddate"].ToString().Trim() == "")
                {
                    UpdateImageText(numb.ToString(), dt.Rows[0]["sender_id"].ToString());
                    context.Response.Write("0");
                    sbContent.Append(@"<html><head><title></title><style type='text/css'>DIV.PlainText {FONT-FAMILY: monospace; FONT-SIZE: 120%}</style></head>");
                    sbContent.Append(@"<body><font size='2'>");
                    sbContent.Append(@"<div class='PlainText'>");
                    sbContent.Append(@"Dear:<br />");
                    sbContent.Append(@"<br />");
                    sbContent.Append(@"The authentication code below:");
                    sbContent.Append(@"<img alt='認證碼' src='cid:attech01.jpg' /><br />");
                    sbContent.Append(@"====================================================================<br />");
                    sbContent.Append(@"This email may contain confidential information. Please do not use or disclose ");
                    sbContent.Append(@"it in any way and delete it if you are not the intended recipient.</div>");
                    sbContent.Append(@"</font></body></html>");
                    myEmail.sendEmail(dt.Rows[0]["sender_mail"].ToString(), "[ITRI] Notification of WebFTP authentication code", sbContent.ToString(), numb.ToString(), "");
                }
                else
                {
                    DateTime validTime = DateTime.Parse(dt.Rows[0]["sender_passValiddate"].ToString().Trim());
                    DateTime nowdate = DateTime.Now;
                    TimeSpan Total = nowdate.Subtract(validTime);
                    double daysub = Total.TotalMinutes;

                    if (daysub > 30)
                    {
                        UpdateImageText(numb.ToString(), dt.Rows[0]["sender_id"].ToString());
                        context.Response.Write("1");
                        sbContent.Append(@"<html><head><title></title><style type='text/css'>DIV.PlainText {FONT-FAMILY: monospace; FONT-SIZE: 120%}</style></head>");
                        sbContent.Append(@"<body><font size='2'>");
                        sbContent.Append(@"<div class='PlainText'>");
                        sbContent.Append(@"Dear:<br />");
                        sbContent.Append(@"<br />");
                        sbContent.Append(@"The authentication code below:");
                        sbContent.Append(@"<img alt='認證碼' src='cid:attech01.jpg' /><br />");
                        sbContent.Append(@"====================================================================<br />");
                        sbContent.Append(@"This email may contain confidential information. Please do not use or disclose ");
                        sbContent.Append(@"it in any way and delete it if you are not the intended recipient.</div>");
                        sbContent.Append(@"</font></body></html>");
                        myEmail.sendEmail(dt.Rows[0]["sender_mail"].ToString(), "[ITRI] Notification of Resending WebFTP authentication code", sbContent.ToString(), numb.ToString(), "");
                    }
                }

            }
            else
            {
                throw new Exception("Can't find a recipient");
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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}