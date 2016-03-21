using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.IO;
using System.Web.Management;

namespace WebAppl
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started
            //System.Web.UI.Page page = System.Web.HttpContext.Current.Handler as System.Web.UI.Page;
            //string url = Request.Url.ToString();
            //if (url.IndexOf("Default.aspx") < 0 && System.IO.Path.GetExtension(url).ToString().ToLower().IndexOf(".ashx") < 0)
            //{

            //    Response.Redirect("~/Web/luv_login.aspx?url=" + SECURITY.Encrypt(url));
            //}

        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            //System.Web.UI.Page page = System.Web.HttpContext.Current.Handler as System.Web.UI.Page;
            //string url = Request.Url.ToString();
            //if (url.IndexOf("FileEditPage") > 0 || url.IndexOf("page_mantain") > 0)
            //{

            //    if (HttpContext.Current.Session["pwerRowData"] == null)
            //    {
            //        Response.Redirect("~/sfts/Default.aspx");
            //    }
            //}
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            bool IsExecAjax = false;
            Regex regexFormat = new Regex(@"exec\S*\.aspx", RegexOptions.IgnoreCase);
            IsExecAjax = (regexFormat.Match(Request.Path).Success == true) ? true : false;

            Exception ex = Server.GetLastError().GetBaseException();
            string Message = string.Format("訊息：{0}", ex.GetBaseException().Message);

            

            /*===========explain：show message*/
            Server.ClearError();

            if (IsExecAjax)
            {
                Response.Write(Message);
            }
            else
            {
                string url = Request.Url.ToString();
                if (url.IndexOf("FileEditPage") > 0 || url.IndexOf("page_mantain") > 0)
                {
                    if (HttpContext.Current.Session["pwerRowData"] == null)
                    {
                        Application["ErrorMsg"] = "您尚未登入";
                    }
                }
                else
                {
                    HttpContext ctx = HttpContext.Current;
                    int httpCode = ctx.Response.StatusCode;

                    if (httpCode == 404)
                    {
                        Application["ErrorMsg"] = "系統出現錯誤，請聯絡管理者";
                    }
                    else
                    {
                        Application["ErrorMsg"] = Message;
                    }
                }

                //Email myEmal = new Email();
                //myEmal.sendEmail(AppConfig.DevEmail, "大檔案傳輸系統錯誤", Message, "");

                Response.Redirect("~/sfts/errorPage.aspx");
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {
            Session["pwerRowData"] = null;
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}