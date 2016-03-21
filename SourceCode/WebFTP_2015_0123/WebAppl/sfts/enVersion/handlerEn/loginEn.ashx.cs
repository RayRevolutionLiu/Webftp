using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Text;

namespace WebAppl.sfts.enVersion.handlerEn
{
    /// <summary>
    ///loginEn 的摘要描述
    /// </summary>
    public class loginEn : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string tbxUserName = context.Request.Form["tbxUserName"];
                string tbxPwd = context.Request.Form["tbxPwd"];

                string returnValue = string.Empty;
                security sec = new security();
                Email myEmail = new Email();
                //底下為登入後之判斷
                AccountInfo accInfo = new sAccount().ExecLogonOutCompany(tbxUserName, encode.sha1en(tbxPwd.ToString()));
                //如果accinfo不等於空值
                if (accInfo != null)
                {
                    //將該物件accinfo傳給Session["AccountInfo"]保存
                    context.Session["pwerRowData"] = accInfo;
                    DateTime lastlogdate = DateTime.Parse(accInfo.LastLogDate);
                    DateTime nowdate = DateTime.Now;
                    TimeSpan Total = nowdate.Subtract(lastlogdate);
                    int daysub = Total.Days;
                    login_DB mylog = new login_DB();
                    if (daysub > 90)
                    {
                        returnValue = "This account has not logged more than 90 days, the system will re-send password change link to your mailbox";

                        //寄送密碼變更連結
                        StringBuilder sb = new StringBuilder();
                        sb.Append(@"Dear " + accInfo.Title + " :<br /><br />");
                        sb.Append(@"Since you have more than 90 days are not logged in,<br /> the system requires you to change your password and then try to re-login again <br /><br />");
                        sb.Append(@"Please click on the link below to change the password back to the system<br /><br />");
                        sb.Append(@"<a href='" + AppConfig.MailUrl + "?vid=" + accInfo.QueryStr + "'>" + AppConfig.MailUrl + "?vid=" + accInfo.QueryStr + "</a><br /><br />");
                        sb.Append(@"Thank you！");
                        myEmail.sendEmail(accInfo.Account, "[ITRI]Notification from ITRI WebFTP - Change password", sb.ToString(), "");
                    }
                    else
                    {
                        returnValue = "success";
                        //更新最後登入日期
                        mylog.UPDATElastlogdate(accInfo.IDmem);
                    }
                }
                else
                {
                    returnValue = "login Fail, Please check that you enter the E-mail and password are correct";

                }

                context.Response.Write(returnValue);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageUtil.loginlogError + ex.HelpLink);
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