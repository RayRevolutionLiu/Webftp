using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Net.Mail;

namespace WebAppl.sfts.enVersion.handlerEn
{
    /// <summary>
    ///AddCompEn 的摘要描述
    /// </summary>
    public class AddCompEn : IHttpHandler
    {
        public class TooL
        {
            public string account { get; set; }
            public string email { get; set; }
        }


        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string tbxOutComp = string.IsNullOrEmpty(context.Request.Form["tbxOutComp"]) ? "" : context.Request.Form["tbxOutComp"].ToString().Trim();

                if (tbxOutComp.Trim() == "")
                {
                    context.Response.Write("Please enter E-mail");
                    return;
                }
                else
                {
                    List<TooL> eList = new List<TooL>();
                    string[] split = tbxOutComp.Split(new Char[] { ';', ',' });
                    //20130813新增，檢查email格式部分 by 凱呈
                    MailAddress[] addrs = new MailAddress[split.Length];
                    //foreach (string str in split)
                    for (int i = 0; i < split.Length; i++)
                    {
                        //if (!Common.IsVaildEmail(str))
                        //{
                        //    context.Response.Write("Email format Error");
                        //    return;
                        //}
                        try
                        {
                            addrs[i] = new MailAddress(split[i]);
                        }
                        catch (Exception)
                        {
                            context.Response.Write("Email format Error");
                            return;
                        }

                        //DataTable dt = Common.AccordEmailIsitFromITRI(str, "");
                        DataTable dt = Common.AccordEmailIsitFromITRI(addrs[i].Address, "");

                        if (dt.Rows.Count > 0)
                        {
                            //表示輸入的人是院內員工
                            TooL e = new TooL();
                            e.account = dt.Rows[0]["com_empno"].ToString();
                            e.email = dt.Rows[0]["com_mailadd"].ToString();
                            eList.Add(e);
                        }
                        else
                        {
                            TooL e = new TooL();
                            e.account = addrs[i].Address;
                            e.email = addrs[i].Address;
                            eList.Add(e);
                        }
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