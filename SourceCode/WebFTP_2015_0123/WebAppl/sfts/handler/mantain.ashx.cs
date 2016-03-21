using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace WebAppl.handler
{
    /// <summary>
    ///mantain 的摘要描述
    /// </summary>
    public class mantain : IHttpHandler
    {
        public class TooLFile
        {
            public string id { get; set; }
            public string fileName { get; set; }
            public string stat { get; set; }
            public string main_stat { get; set; }
        }

        public class TooLSender
        {
            public string id { get; set; }
            public string email { get; set; }
            public string trytimes { get; set; }
            public string lasttrydate { get; set; }
            public string queryenable { get; set; }
            public string main_stat { get; set; }
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string parentid = string.IsNullOrEmpty(context.Request.Form["parentid"]) ? "" : context.Request.Form["parentid"].ToString().Trim();
                string imgid = string.IsNullOrEmpty(context.Request.Form["imgid"]) ? "" : context.Request.Form["imgid"].ToString().Trim();
                Guid query = new Guid(parentid);
                page_mantain_DB mypage = new page_mantain_DB();

                if (imgid.ToString().Trim() == "fileOpen")
                {
                    DataTable dt = mypage.GetfileList(parentid);
                    List<TooLFile> eList = new List<TooLFile>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TooLFile e = new TooLFile();
                        e.id = dt.Rows[i]["afile_id"].ToString().Trim();
                        e.fileName = dt.Rows[i]["ShowFileName"].ToString() + dt.Rows[i]["afile_exten"].ToString();
                        e.stat = dt.Rows[i]["afile_stat"].ToString().Trim();
                        e.main_stat = dt.Rows[i]["main_stat"].ToString().Trim();
                        eList.Add(e);
                    }

                    System.Web.Script.Serialization.JavaScriptSerializer objSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    string ans = objSerializer.Serialize(eList);  //new
                    context.Response.ContentType = "application/json";
                    context.Response.Write(ans);
                }

                if (imgid.ToString().Trim() == "senderOpen")
                {
                    DataTable dt = mypage.GetsenderList(parentid);
                    List<TooLSender> eList = new List<TooLSender>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TooLSender e = new TooLSender();
                        e.id = dt.Rows[i]["sender_id"].ToString().Trim();
                        e.email = dt.Rows[i]["sender_mail"].ToString().Trim();
                        e.trytimes = dt.Rows[i]["hitCount"].ToString().Trim() == "" ? "0" : dt.Rows[i]["hitCount"].ToString().Trim();
                        e.lasttrydate = dt.Rows[i]["LastDownDate"].ToString().Trim();
                        e.queryenable = dt.Rows[i]["sender_queryenable"].ToString().Trim();
                        e.main_stat = dt.Rows[i]["main_stat"].ToString().Trim();
                        eList.Add(e);
                    }

                    System.Web.Script.Serialization.JavaScriptSerializer objSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    string ans = objSerializer.Serialize(eList);  //new
                    context.Response.ContentType = "application/json";
                    context.Response.Write(ans);
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