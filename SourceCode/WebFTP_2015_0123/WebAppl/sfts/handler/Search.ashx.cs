using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace WebAppl.handler
{
    /// <summary>
    ///Search 的摘要描述
    /// </summary>
    public class Search : IHttpHandler
    {
        public class TooL
        {
            public string com_cname { get; set; }
            public string com_mailadd { get; set; }
            public string org_abbr_chnm1 { get; set; }
            public string com_deptcd { get; set; }
            public string dep_deptname { get; set; }
            public string com_empno { get; set; }
        }

        GenJson_DB myGen = new GenJson_DB();

        public void ProcessRequest(HttpContext context)
        {
            string keyword = context.Request.Form["key_word"];
            try
            {
                DataTable dt = myGen.SearchAllEmpno(keyword.ToUpper());
                if (dt.Rows.Count > 0)
                {
                    List<TooL> eList = new List<TooL>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TooL e = new TooL();
                        e.com_cname = dt.Rows[i]["com_cname"].ToString().Trim();
                        e.com_mailadd = dt.Rows[i]["com_mailadd"].ToString().Trim();
                        e.org_abbr_chnm1 = dt.Rows[i]["org_abbr_chnm1"].ToString().Trim();
                        e.com_deptcd = dt.Rows[i]["com_deptcd"].ToString().Trim();
                        e.dep_deptname = dt.Rows[i]["dep_abbrnm"].ToString().Trim();
                        e.com_empno = dt.Rows[i]["com_empno"].ToString().Trim();
                        eList.Add(e);
                    }

                    System.Web.Script.Serialization.JavaScriptSerializer objSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    string ans = objSerializer.Serialize(eList);  //new
                    context.Response.ContentType = "application/json";
                    context.Response.Write(ans);
                }
                else
                {
                    context.Response.Write("empty");
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