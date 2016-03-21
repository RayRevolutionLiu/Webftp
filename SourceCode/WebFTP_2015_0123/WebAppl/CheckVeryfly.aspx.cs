using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppl
{
    public partial class CheckVeryfly : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //把single sign on 之後的工號抓到
            string getAD = GetSSOAttribute("SM_USER");

            string AllHttpAttrs = Request.ServerVariables["ALL_HTTP"];

            AccountInfo accInfo = new sAccount().ExecLogon(getAD);
            //如果accinfo不等於空值
            if (accInfo != null)
            {
                //將該物件accinfo傳給Session["AccountInfo"]保存
                Session["pwerRowData"] = accInfo;
                string querystr = Request.QueryString["en"].ToString();
                if (Request.UrlReferrer.OriginalString.ToString().IndexOf("enVersion") != -1)
                {
                    Response.Redirect("~/sfts/enVersion/VeryflyEn.aspx?en=" + querystr);
                }
                else
                {
                    Response.Redirect("~/sfts/Veryfly.aspx?en=" + querystr);
                }
            }
            else
            {
                Application["ErrorMsg"] = "您沒有權限";
                Response.Redirect("~/errorPage.aspx");
            }
        }

        public string GetSSOAttribute(string AttrName)
        {

            if (AppConfig.SSODebug.ToString().ToUpper() == "TRUE")
            {
                return AppConfig.SSODebug_USER_ACCOUNT;
            }

            string AllHttpAttrs, FullAttrName, Result;
            int AttrLocation;
            AllHttpAttrs = Request.ServerVariables["ALL_HTTP"];
            FullAttrName = "HTTP_" + AttrName.ToUpper();
            AttrLocation = AllHttpAttrs.IndexOf(FullAttrName + ":");
            if (AttrLocation > 0)
            {
                Result = AllHttpAttrs.Substring(AttrLocation + FullAttrName.Length + 1);
                AttrLocation = Result.IndexOf("\n");
                if (AttrLocation <= 0) AttrLocation = Result.Length + 1;
                //return Result.Substring(0, AttrLocation - 1); 
                return HttpContext.Current.Request.ServerVariables["HTTP_SM_USER"];
            }

            return "";
        }
    }
}