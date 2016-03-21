using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;


namespace WebAppl.sfts.enVersion.handlerEn
{
    /// <summary>
    ///VaildEn 的摘要描述
    /// </summary>
    public class VaildEn : IHttpHandler
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
                if (dt.Rows[0]["sender_imagetext"].ToString().Trim() == context.Request.Form["vaild"].ToString().Trim())
                {
                    DataView dv = dt.DefaultView;
                    dv.RowFilter = "main_stat<>'N'";
                    if (dv.Count == 0)
                    {
                        context.Response.Write("This data has been deleted,download fail.");
                    }
                    else
                    {
                        context.Response.Write("Authentication is successful! Unzip password will be sent to your mailbox when the download is complete.");
                    }
                }
                else
                {
                    context.Response.Write("Verification fails, make sure you type the Verification code is correct.");
                }
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