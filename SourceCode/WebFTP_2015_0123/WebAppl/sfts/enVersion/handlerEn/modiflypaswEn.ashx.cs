using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppl.sfts.enVersion.handlerEn
{
    /// <summary>
    ///modiflypaswEn 的摘要描述
    /// </summary>
    public class modiflypaswEn : IHttpHandler
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
                    context.Response.Write("Password and confirm password is not equal");
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