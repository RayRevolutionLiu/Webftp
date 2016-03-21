using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;

namespace WebAppl.handler
{
    /// <summary>
    ///ForGetPassW 的摘要描述
    /// </summary>
    public class ForGetPassW : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string tbxforgetAccount = context.Request.Form["tbxforgetAccount"].ToString().Trim();
            security sec = new security();
            Email myEmail = new Email();
            ForGetPassW_DB myForGet = new ForGetPassW_DB();

            if (tbxforgetAccount.ToString().Trim() == "")
            {
                context.Response.Write("請輸入E-mail");
                return;
            }
            else
            {
                //找出此EMAIL是否已註冊過
                DataTable dt = myForGet.SelectEmailExist(tbxforgetAccount.ToUpper());
                if (dt.Rows.Count == 1)
                {
                    //註冊過 寄信給此EMAIL更改連結 並變換QueryStr
                    /* 底下為本來密碼自動產生 不過規格取消 所以拿來用在querystring上 */
                    Random rdm = new Random();
                    string[] seeds = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
                    int strLen = (int)rdm.Next(5, 11);
                    string randStr = string.Empty;
                    for (int i = 0; i < strLen; i++)
                    {
                        randStr += seeds[rdm.Next(seeds.Length)].ToString().ToUpper();
                    }


                    StringBuilder sb = new StringBuilder();
                    sb.Append(@"您好：<br /><br />");
                    sb.Append(@"這封認證信函是由 工研院大檔案傳輸系統 所發出的。<br /><br />");
                    sb.Append(@"請點擊至下列網址修改您的密碼。<br /><br />");
                    sb.Append(@"<a href='" + AppConfig.MailUrl + "valid.aspx?vid=" + sec.encryptquerystring(randStr) + "'>" + AppConfig.MailUrl + "valid.aspx?vid=" + sec.encryptquerystring(randStr) + "</a><br /><br />");
                    myEmail.sendEmail(tbxforgetAccount, "[ITRI]工研院大檔案傳輸", sb.ToString(), "");
                    myForGet.UpdateMemberQueryStr(sec.encryptquerystring(randStr), dt.Rows[0]["mem_id"].ToString().Trim());
                    context.Response.Write("success");
                }
                else
                {
                    context.Response.Write("此帳號尚未註冊");
                    return;
                }
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