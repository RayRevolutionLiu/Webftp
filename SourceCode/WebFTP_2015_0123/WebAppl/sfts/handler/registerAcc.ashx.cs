using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Data;
using System.Text;

namespace WebAppl.handler
{
    /// <summary>
    ///registerAcc 的摘要描述
    /// </summary>
    public class registerAcc : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string tbxNameQ = context.Request.Form["tbxNameQ"].ToString().Trim();
                string tbxAccountQ = context.Request.Form["tbxAccountQ"].ToString().Trim();
                string typeQ = context.Request.Form["typeQ"].ToString().Trim();

                if (tbxNameQ.ToString().Trim() == "")
                {
                    context.Response.Write("請輸入中文姓名");
                    return;
                }

                if (tbxAccountQ.ToString().Trim() == "")
                {
                    context.Response.Write("請輸入電子郵件");
                    return;
                }

                if (!Common.IsVaildEmail(tbxAccountQ))
                {
                    context.Response.Write("Email格式錯誤");
                    return;
                }

                if (encode.sqlInjection(tbxNameQ))
                {
                    context.Response.Write("中文姓名包含不合法字元");
                    return;
                }

                DataTable dtEmail = Common.AccordEmailIsitFromITRI(tbxAccountQ, "");
                if (dtEmail.Rows.Count > 0)
                {
                    context.Response.Write("請輸入院外信箱");
                    return;
                }

                if (typeQ != "agree")
                {
                    context.Response.Write("您並未同意使用規約 無法接受您的申請");
                    return; 
                }
                else
                {
                    Random rdm = new Random();
                    register_DB myreg = new register_DB();
                    security sec = new security();
                    Email myEmail = new Email();

                    DataTable dt = myreg.CheckEmailExist(tbxAccountQ);
                    if (dt.Rows.Count > 0)
                    {
                        context.Response.Write("此Email已註冊過帳號");
                        return;
                    }

                    /* 底下為本來密碼自動產生 不過規格取消 所以拿來用在querystring上 */
                    string[] seeds = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
                    int strLen = (int)rdm.Next(5,11);
                    string randStr = string.Empty;
                    for (int i = 0; i < strLen; i++)
                    {
                        randStr += seeds[rdm.Next(seeds.Length)].ToString().ToUpper();
                    }
                  
                    StringBuilder sb = new StringBuilder();
                    sb.Append(@"親愛的" + tbxNameQ + " 您好：<br /><br />");
                    sb.Append(@"這封認證信函是由 工研院大檔案傳輸系統 所發出的，您收到這封電子郵件可能是因為您註冊了新的帳號。<br /><br />");
                    sb.Append(@"請點擊至下列網址，即可通過會員認證並新增您的密碼。<br /><br />");
                    sb.Append(@"<a href='" + AppConfig.MailUrl + "valid.aspx?vid=" + sec.encryptquerystring(randStr) + "'>" + AppConfig.MailUrl + "valid.aspx?vid=" + sec.encryptquerystring(randStr) + "</a><br /><br />");
                    //sb.Append(@"");
                    //sb.Append(@"");
                    //sb.Append(@"");
                    //sb.Append(@"");
                    myEmail.sendEmail(tbxAccountQ, "[ITRI]工研院大檔案傳輸會員驗證", sb.ToString(), "");
                    myreg.INSERTmember(tbxAccountQ, tbxNameQ, sec.encryptquerystring(randStr));
                    context.Response.Write("success");
                }
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