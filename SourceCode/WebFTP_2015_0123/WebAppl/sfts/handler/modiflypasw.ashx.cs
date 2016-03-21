using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppl.handler
{
    /// <summary>
    ///modiflypasw 的摘要描述
    /// </summary>
    public class modiflypasw : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string tbxpassw = context.Request.Form["tbxpassw"].ToString().Trim();
                string tbxconfirmpassw = context.Request.Form["tbxconfirmpassw"].ToString().Trim();
                string mem_id = context.Request.Form["mem_id"].ToString().Trim();
                modiflypasw_DB mymod = new modiflypasw_DB();

                if (tbxpassw != tbxconfirmpassw)
                {
                    context.Response.Write("確認密碼與密碼不符");
                }
                else
                {
                    mymod.UPDATEpassw(encode.sha1en(tbxpassw), mem_id);
                    context.Response.Write("success");
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