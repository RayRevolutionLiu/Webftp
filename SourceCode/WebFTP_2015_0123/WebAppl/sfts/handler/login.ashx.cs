using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Text;

namespace WebAppl.handler
{
    /// <summary>
    ///login 的摘要描述
    /// </summary>
    public class login : IHttpHandler, IRequiresSessionState
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
                        returnValue = "此帳號已超過90天未登入，系統將會重新寄送密碼變更連結，請至您的信箱收取信件並由信內之連結登入";

                        //寄送密碼變更連結
                        StringBuilder sb = new StringBuilder();
                        sb.Append(@"親愛的" + accInfo.Title + " 您好：<br /><br />");
                        sb.Append(@"由於您已超過90天未登入，系統需要您重新變更密碼再嘗試重新登入<br /><br />");
                        sb.Append(@"請點擊下方連結回系統變更密碼<br /><br />");
                        sb.Append(@"<a href='" + AppConfig.MailUrl + "valid.aspx?vid=" + accInfo.QueryStr + "'>" + AppConfig.MailUrl + "valid.aspx?vid=" + accInfo.QueryStr + "</a><br /><br />");
                        sb.Append(@"謝謝您！");
                        myEmail.sendEmail(accInfo.Account, "[ITRI]工研院大檔案傳輸密碼通知", sb.ToString(), "");
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
                    returnValue = "登入失敗 請檢查您輸入之E-mail與密碼是否正確";

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