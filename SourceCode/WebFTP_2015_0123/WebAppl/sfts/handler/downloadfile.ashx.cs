using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.IO;

namespace WebAppl.handler
{
    /// <summary>
    ///downloadfile 的摘要描述
    /// </summary>
    public class downloadfile : IHttpHandler
    {
        public class TooL
        {
            public string afile_id { get; set; }
            public string afile_parentid { get; set; }
            public string afile_comorsec { get; set; }
            public string afile_encrypt { get; set; }
            public string afile_origiFileName { get; set; }
            public string afile_encryptfileName { get; set; }
            public string afile_size { get; set; }
            public string afile_exten { get; set; }
            public string ShowFileName { get; set; }
            public string main_stat { get; set; }
        }


        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string afile_id = context.Request.Form["afile_id"];
                string afile_parentid = context.Request.Form["afile_parentid"];
                string afile_comorsec = context.Request.Form["afile_comorsec"];

                Veryfly_DB myVery = new Veryfly_DB();

                DataTable dt = myVery.downloadfileList(afile_id, afile_parentid, afile_comorsec);

                if (dt.Rows.Count > 0)
                {
                    List<TooL> eList = new List<TooL>();
                    TooL e = new TooL();
                    e.afile_id = dt.Rows[0]["afile_id"].ToString();
                    e.afile_parentid = dt.Rows[0]["afile_parentid"].ToString().Trim();
                    e.afile_comorsec = dt.Rows[0]["afile_comorsec"].ToString().Trim();
                    e.afile_encrypt = dt.Rows[0]["afile_encrypt"].ToString().Trim();
                    e.afile_origiFileName = dt.Rows[0]["afile_origiFileName"].ToString().Trim();
                    e.afile_encryptfileName = dt.Rows[0]["afile_encryptfileName"].ToString().Trim();
                    e.afile_size = dt.Rows[0]["afile_size"].ToString().Trim();
                    e.afile_exten = dt.Rows[0]["afile_exten"].ToString().Trim();
                    e.ShowFileName = dt.Rows[0]["ShowFileName"].ToString().Trim();
                    e.main_stat = dt.Rows[0]["main_stat"].ToString().Trim();
                    eList.Add(e);

                    System.Web.Script.Serialization.JavaScriptSerializer objSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    string ans = objSerializer.Serialize(eList);  //new
                    context.Response.ContentType = "application/json";
                    context.Response.Write(ans);
                }
                else
                {
                    context.Response.Write("false");
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