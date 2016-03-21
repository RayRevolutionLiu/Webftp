using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace WebAppl.sfts.enVersion.handlerEn
{
    /// <summary>
    ///VaildNoforwardEn 的摘要描述
    /// </summary>
    public class VaildNoforwardEn : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.Form["aid"] != null)
            {
                string aid = context.Request.Form["aid"].ToString().Trim();
                string afileID = context.Request.Form["afileID"].ToString().Trim();
                if (encode.sqlInjection(aid) || encode.sqlInjection(afileID))
                {
                    throw new Exception("illegal paramater value");
                }


                Veryfly_DB myVery = new Veryfly_DB();
                DataTable dt = myVery.checkimagetext(aid, afileID);
                List<TooL> eList = new List<TooL>();
                TooL e = new TooL();

                if (dt.Rows[0]["sender_imagetext"].ToString().Trim() == context.Request.Form["vaild"].ToString().Trim())
                {
                    DataView dv = dt.DefaultView;
                    dv.RowFilter = "main_stat<>'N'";
                    if (dv.Count == 0)
                    {
                        e.err_msg = "This data has been deleted,download fail.";
                        e.afile_encryptfileName = "";
                        e.afile_origiFileName = "";
                        e.afile_id = "";
                        e.sender_id = "";
                        e.afile_exten = "";
                        e.sender_notifyflag = "";
                    }
                    else
                    {
                        e.err_msg = "";
                        e.afile_encryptfileName = dv[0]["afile_encryptfileName"].ToString().Trim();
                        e.afile_origiFileName = dv[0]["afile_origiFileName"].ToString().Trim();
                        e.afile_id = dv[0]["afile_id"].ToString().Trim();
                        e.sender_id = dv[0]["sender_id"].ToString().Trim();
                        e.afile_exten = dv[0]["afile_exten"].ToString().Trim();
                        e.sender_notifyflag = dv[0]["sender_notifyflag"].ToString().Trim();
                    }
                }
                else
                {
                    e.err_msg = "Verification fails, make sure you type the Verification code is correct.";
                    e.afile_encryptfileName = "";
                    e.afile_origiFileName = "";
                    e.afile_id = "";
                    e.sender_id = "";
                    e.afile_exten = "";
                    e.sender_notifyflag = "";
                }
                eList.Add(e);

                System.Web.Script.Serialization.JavaScriptSerializer objSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                string ans = objSerializer.Serialize(eList);  //new
                context.Response.ContentType = "application/json";
                context.Response.Write(ans);
            }
        }

        public class TooL
        {
            public string afile_encryptfileName { get; set; }
            public string afile_origiFileName { get; set; }
            public string afile_id { get; set; }
            public string sender_id { get; set; }
            public string afile_exten { get; set; }
            public string sender_notifyflag { get; set; }
            public string err_msg { get; set; }// 在success來alert error message
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