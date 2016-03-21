using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppl.handler
{
    /// <summary>
    ///cbxChangeImg 的摘要描述
    /// </summary>
    public class cbxChangeImg : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string nameType = string.IsNullOrEmpty(context.Request.Form["nameType"]) ? "" : context.Request.Form["nameType"].ToString().Trim();
                string id = string.IsNullOrEmpty(context.Request.Form["id"]) ? "" : context.Request.Form["id"].ToString().Trim();
                string value = string.IsNullOrEmpty(context.Request.Form["value"]) ? "" : context.Request.Form["value"].ToString().Trim();

                if (encode.sqlInjection(id) || encode.sqlInjection(value) || encode.sqlInjection(nameType))
                {
                    throw new Exception("參數包含不合法字元");
                }

                page_mantain_DB mypage = new page_mantain_DB();
                mypage.GetchkboxStat(id, nameType, value);

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