using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace WebAppl.handler
{
    /// <summary>
    ///Vaild 的摘要描述
    /// </summary>
    public class Vaild : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.Form["aid"] != null)
            {
                string aid = context.Request.Form["aid"].ToString().Trim();
                string afileID = context.Request.Form["afileID"].ToString().Trim();
                if (encode.sqlInjection(aid) || encode.sqlInjection(afileID))
                {
                    throw new Exception("參數包含不合法字元");
                }


                Veryfly_DB myVery = new Veryfly_DB();
                DataTable dt = myVery.checkimagetext(aid, afileID);
                if (dt.Rows[0]["sender_imagetext"].ToString().Trim() == context.Request.Form["vaild"].ToString().Trim())
                {
                    DataView dv = dt.DefaultView;
                    dv.RowFilter = "main_stat<>'N'";
                    if (dv.Count == 0)
                    {
                        context.Response.Write("此筆資料寄件者已刪除，檔案下載失敗");
                    }
                    else
                    {
                        context.Response.Write("驗證成功!確認下載完成之後，解壓縮密碼會寄到您的信箱內");
                    }
                }
                else
                {
                    context.Response.Write("驗證失敗，請確認輸入之驗證碼是否正確");
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