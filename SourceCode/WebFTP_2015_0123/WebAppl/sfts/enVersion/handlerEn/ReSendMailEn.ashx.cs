using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;

namespace WebAppl.sfts.enVersion.handlerEn
{
    /// <summary>
    ///ReSendMailEn 的摘要描述
    /// </summary>
    public class ReSendMailEn : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string senderid = context.Request.Form["senderid"];
                page_mantain_DB mypage = new page_mantain_DB();
                Email myEmail = new Email();

                DataTable dt = mypage.GetReSendMail(senderid);
                string subject = "[ITRI] Notification from ITRI WebFTP - You have a file send by your partner";
                string to = dt.Rows[0]["sender_mail"].ToString().Trim();
                DataTable ReturnSenderNameDT = Common.GetDetail(dt.Rows[0]["main_infno"].ToString().Trim(), dt.Rows[0]["main_isempno"].ToString().Trim());
                string ReturnSenderName = ReturnSenderNameDT.Rows[0]["cName"].ToString().Trim();
                string returnsendercEmail = ReturnSenderNameDT.Rows[0]["cEmail"].ToString().Trim();

                string fileList = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    fileList += dt.Rows[i]["ShowFileName"].ToString() + dt.Rows[i]["afile_exten"].ToString() + "<br />";
                }

                StringBuilder sbBody = new StringBuilder();
                sbBody.Append(@"<html><head><title></title><style type='text/css'>DIV.PlainText {FONT-FAMILY: monospace; FONT-SIZE: 120%}</style></head>");
                sbBody.Append(@"<body><font size='2'>");
                sbBody.Append(@"<div class='PlainText'>");
                sbBody.Append(@"Dear:<br />");
                sbBody.Append(@"<br />");
                sbBody.AppendFormat(@"{0} &lt;{2}&gt;  had send some files to you at  {1}.", ReturnSenderName, dt.Rows[0]["cmain_createdate"].ToString().Trim(), returnsendercEmail);
                sbBody.Append(@"Please download your files as soon as possible.<br /><br />※ All files will be deleted in 7 Days ※");
                sbBody.Append(@"<br /><br />Messages:<br />");
                sbBody.AppendFormat(@"{0}", dt.Rows[0]["main_desc"].ToString());
                sbBody.Append(@"<br />");
                sbBody.Append(@"File List:<br />");
                sbBody.Append(@"" + fileList + "");

                if (dt.Rows[0]["main_secret"].ToString() == "security")
                {
                    sbBody.Append(@"authentication code:<br />If it is security file,Please enter the number in the CAPTCHA when you download. We will send you another mail with a password for you to unzip the file(s).<br /><br />");
                    sbBody.Append(@"<img alt='認證碼' src='cid:attech01.jpg' /><br />");
                }
                sbBody.Append(@"Download URL:<br />");
                sbBody.Append(@"* 1. If any 'Security Alert' windows pop up, please proceed by clicking 'Yes'.<br />");
                sbBody.Append(@"* 2. This encoded URL is for YOU ONLY.<br />");
                sbBody.Append(@"We will consider everyone accessing this URL having your approval.<br />Please confirm before you forward this e-mail.<br />");
                sbBody.Append(@"<a href='" + AppConfig.MailUrl + "enVersion/VeryflyEn.aspx?en=" + dt.Rows[0]["sender_querystring"].ToString() + "' target='_blank'>" + AppConfig.MailUrl + "enVersion/VeryflyEn.aspx?en=" + dt.Rows[0]["sender_querystring"].ToString() + "</a><br />");//pathUrl("MailUrl") + "Veryfly.aspx?en=" + dt.Rows[i]["sender_querystring"].ToString(), pathUrl("MailUrl") + "Veryfly.aspx?en=" + dt.Rows[i]["sender_querystring"].ToString()
                sbBody.Append(@"====================================================================<br />");
                sbBody.Append(@"This email may contain confidential information. Please do not use or disclose ");
                sbBody.Append(@"it in any way and delete it if you are not the intended recipient.</div>");
                sbBody.Append(@"</font></body></html>");
                //sbBody.AppendFormat(@"");

                myEmail.sendEmail(to, "", subject, sbBody.ToString(), dt.Rows[0]["sender_imagetext"].ToString(), returnsendercEmail);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}