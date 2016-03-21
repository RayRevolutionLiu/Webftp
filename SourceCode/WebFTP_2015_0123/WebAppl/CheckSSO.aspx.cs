using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.IO;
using System.Data;
using System.Text;

namespace WebAppl
{
    public partial class CheckSSO : System.Web.UI.Page
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
                if (Request.QueryString["en"] != null)
                {
                    if (Request.QueryString["lang"] != null && Request.QueryString["lang"].ToString().Trim() == "en")
                    {
                        Response.Redirect("~/sfts/enVersion/VeryflyEn.aspx?en=" + Request.QueryString["en"].ToString());
                    }
                    else
                    {
                        Response.Redirect("~/sfts/Veryfly.aspx?en=" + Request.QueryString["en"].ToString());
                    }

                }
                else
                {
                    //Response.Write(Request.UrlReferrer.ToString());
                    if (Request.QueryString["lang"] != null && Request.QueryString["lang"].ToString().Trim() == "en")
                    {
                        Response.Redirect("~/sfts/enVersion/FileEditPageEn.aspx");
                    }
                    else
                    {
                        Response.Redirect("~/sfts/FileEditPage.aspx");
                    }
                }
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