using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;

namespace WebAppl.handler
{
    /// <summary>
    ///ReSendMail 的摘要描述
    /// </summary>
    public class ReSendMail : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string senderid = context.Request.Form["senderid"];
                page_mantain_DB mypage = new page_mantain_DB();
                Email myEmail = new Email();

                DataTable dt = mypage.GetReSendMail(senderid);
                string subject = "[ITRI] 通知：您有來自工研院大檔案傳輸的信件";
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
                sbBody.Append(@"親愛的朋友 您好：<br />");
                sbBody.Append(@"<br />");
                sbBody.AppendFormat(@"{0} &lt;{2}&gt; 先生/小姐，在 {1}寄送下列的檔案給您。", ReturnSenderName, dt.Rows[0]["cmain_createdate"].ToString().Trim(), returnsendercEmail);
                sbBody.Append(@"請您利用下面的網址來下載檔案，謝謝您！<br /><br />※ 所有的檔案將在 7 天後刪除※");
                sbBody.Append(@"<br /><br />訊息留言：<br />");
                sbBody.AppendFormat(@"{0}", dt.Rows[0]["main_desc"].ToString());
                sbBody.Append(@"<br />");
                sbBody.Append(@"檔案列表：<br />");
                sbBody.Append(@"" + fileList + "");

                if (dt.Rows[0]["main_secret"].ToString() == "security")
                {
                    sbBody.Append(@"取檔認證碼：<br />如為密件檔案，請在下載時輸入圖片內之數字，即會再寄一封解壓縮密碼給您。<br />");
                    sbBody.Append(@"<img alt='認證碼' src='cid:attech01.jpg' /><br />");
                }
                sbBody.Append(@"取檔網址：<br />");
                sbBody.Append(@"※ 1. 如果有「安全性警示」的視窗跳出，請按「是」接受以繼續接收檔案<br />");
                sbBody.Append(@"※ 2. 此加密取檔網址為您所獨有，若需要轉寄信件時，請務必留意：系統將所有透過此網址取檔者均視同已取得您本人同意。<br />");
                sbBody.Append(@"<a href='" + AppConfig.MailUrl + "Veryfly.aspx?en=" + dt.Rows[0]["sender_querystring"].ToString() + "' target='_blank'>" + AppConfig.MailUrl + "Veryfly.aspx?en=" + dt.Rows[0]["sender_querystring"].ToString() + "</a><br />");//pathUrl("MailUrl") + "Veryfly.aspx?en=" + dt.Rows[i]["sender_querystring"].ToString(), pathUrl("MailUrl") + "Veryfly.aspx?en=" + dt.Rows[i]["sender_querystring"].ToString()
                sbBody.Append(@"====================================================================<br />");
                sbBody.Append(@"本信件可能包含工研院機密資訊，非指定之收件者，請勿使用或揭露本信件內容，並請銷毀此信件。<br />");
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