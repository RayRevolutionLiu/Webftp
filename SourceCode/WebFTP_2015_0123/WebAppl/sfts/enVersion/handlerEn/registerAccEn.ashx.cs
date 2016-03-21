using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;

namespace WebAppl.sfts.enVersion.handlerEn
{
    /// <summary>
    ///registerAccEn 的摘要描述
    /// </summary>
    public class registerAccEn : IHttpHandler
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
                    context.Response.Write("Please enter your Name");
                    return;
                }

                if (tbxAccountQ.ToString().Trim() == "")
                {
                    context.Response.Write("Please enter your E-mail Address");
                    return;
                }

                if (!Common.IsVaildEmail(tbxAccountQ))
                {
                    context.Response.Write("Email format is not vaild");
                    return;
                }

                if (encode.sqlInjection(tbxNameQ))
                {
                    context.Response.Write("Name contains illegal characters");
                    return;
                }

                DataTable dtEmail = Common.AccordEmailIsitFromITRI(tbxAccountQ, "");
                if (dtEmail.Rows.Count > 0)
                {
                    context.Response.Write("Please enter E-mail without ITRI employee");
                    return;
                }

                if (typeQ != "agree")
                {
                    context.Response.Write("You have not agreed 'User agreement' yet");
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
                        context.Response.Write("This Email has been registered");
                        return;
                    }

                    /* 底下為本來密碼自動產生 不過規格取消 所以拿來用在querystring上 */
                    string[] seeds = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
                    int strLen = (int)rdm.Next(5, 11);
                    string randStr = string.Empty;
                    for (int i = 0; i < strLen; i++)
                    {
                        randStr += seeds[rdm.Next(seeds.Length)].ToString().ToUpper();
                    }

                    StringBuilder sb = new StringBuilder();
                    sb.Append(@"Dear" + tbxNameQ + " :<br /><br />");
                    sb.Append(@"This letter was sent by ITRIWebFTP system. You received this email because you've registered a new account.<br /><br />");
                    sb.Append(@" Please go to the url listed below to get your member verification.<br /><br />");
                    sb.Append(@"<a href='" + AppConfig.MailUrl + "enVersion/validEn.aspx?vid=" + sec.encryptquerystring(randStr) + "'>" + AppConfig.MailUrl + "valid.aspx?vid=" + sec.encryptquerystring(randStr) + "</a><br /><br />");
                    //sb.Append(@"");
                    //sb.Append(@"");
                    //sb.Append(@"");
                    //sb.Append(@"");
                    myEmail.sendEmail(tbxAccountQ, "[ITRI] WebFTP Account Notification ", sb.ToString(), "");
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