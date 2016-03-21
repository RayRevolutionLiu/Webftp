using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace WebAppl.handler
{
    /// <summary>
    ///GenJson 的摘要描述
    /// </summary>
    public class GenJson : IHttpHandler
    {
        public class TooL
        {
            public string key { get; set; }
            public string title { get; set; }
            public string empno { get; set; }
            public bool isFolder { get; set; }
            public bool noLink { get; set; }
            public bool hideCheckbox { get; set; }
            public bool isLazy { get; set; }
            public string deptcd3 { get; set; }
            public string mailadd { get; set; }
        }

        public void ProcessRequest(HttpContext context)
        {
            string orgcd = string.IsNullOrEmpty(context.Request.Form["orgcd"]) ? "" : context.Request.Form["orgcd"].ToString().Trim();
            string deptcd = string.IsNullOrEmpty(context.Request.Form["deptcd3"]) ? "" : context.Request.Form["deptcd3"].ToString().Trim();
            GenJson_DB myGen = new GenJson_DB();
            DataTable dt = myGen.GenJsonDB(orgcd,deptcd);
            List<TooL> eList = new List<TooL>();
             /* 
             json格式
             title 要顯示出來的文字
             isFolder 資料夾的圖片要不要出來 TRUE 出來
             noLink 可以讓該選項的文字不可以點 true
             hideCheckbox true可以把ROOT的CHECKBOX隱藏 
             */
            try
            {
                if (orgcd != "")//不等於空值,就代表是點擊單位之後下一偕
                {
                    if (deptcd != "")//單位跟部門都有值,跑到人的選單
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            TooL e = new TooL();
                            e.key = dt.Rows[i]["com_orgcd"].ToString();
                            e.empno = dt.Rows[i]["com_empno"].ToString();
                            e.title = dt.Rows[i]["com_cname"].ToString().Trim() + "< " + dt.Rows[i]["com_mailadd"].ToString() + " > ";
                            e.noLink = false;
                            e.hideCheckbox = false;
                            e.isFolder = false;
                            e.isLazy = false;
                            e.deptcd3 = "";
                            e.mailadd = dt.Rows[i]["com_mailadd"].ToString();
                            eList.Add(e);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            TooL e = new TooL();
                            e.key = dt.Rows[i]["dep_orgcd"].ToString();
                            e.title = dt.Rows[i]["dep_deptcd"].ToString() + "&nbsp;" + dt.Rows[i]["dep_abbrnm"].ToString();
                            e.noLink = false;
                            e.hideCheckbox = true;
                            e.isFolder = true;
                            e.isLazy = true;
                            e.deptcd3 = dt.Rows[i]["dep_deptcd"].ToString();
                            eList.Add(e);
                        }
                    }
                }
                else//預設一進入之後撈出所有的單位清單(不包括00)
                {
                    
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TooL e = new TooL();
                        e.key = dt.Rows[i]["org_orgcd"].ToString();
                        e.title = dt.Rows[i]["org_abbr_chnm1"].ToString();
                        e.noLink = false;
                        e.hideCheckbox = true;
                        e.isFolder = true;
                        e.isLazy = true;
                        e.deptcd3 = "";
                        eList.Add(e);
                    }
                }

                System.Web.Script.Serialization.JavaScriptSerializer objSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                string ans = objSerializer.Serialize(eList);  //new
                context.Response.ContentType = "application/json";
                context.Response.Write(ans);
            }
            catch (Exception err)
            {
                throw new Exception(MessageUtil.DB_SelectFail + err.HelpLink);
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