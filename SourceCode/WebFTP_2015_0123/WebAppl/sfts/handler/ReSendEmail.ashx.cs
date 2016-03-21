using System;
using System.Web;
using System.Text;
using System.Configuration;
using System.IO;
using System.Data;

namespace WebAppl.handler
{
    /// <summary>
    ///ReSendEmail 的摘要描述
    /// </summary>
    public class ReSendEmail : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                if (context.Request["aid"] != null)
                {
                    string aid = context.Request.Form["aid"].ToString().Trim();
                    string afileID = context.Request.Form["afileID"].ToString().Trim();

                    if (encode.sqlInjection(aid) || encode.sqlInjection(afileID))
                    {
                        throw new Exception("參數包含不合法字元");
                    }

                    Veryfly_DB myVery = new Veryfly_DB();
                    DataTable dt = myVery.checkimagetext(aid, afileID);

                    if (dt.Rows[0]["sender_stat"].ToString().Trim() == "N")
                    {
                        Email email = new Email();
                        //email.sendEmail(dt.Rows[0]["sender_mail"].ToString(), "[ITRI] 通知：您有來自工研院大檔案傳輸的信件", "親愛的朋友 您好：<br /><br />先生/小姐，在 2013/04/18寄送下列的檔案給您。<br />請您利用下面的解壓縮密碼來開啟檔案:<br /><img alt='認證碼' src='cid:attech01.jpg' /><br />請使用此密碼解壓縮檔案<br /> 謝謝", dt.Rows[0]["afile_encrypt"].ToString());
                        DataTable dtAccordingParentidToFindMember = Common.AccordingParentidToFindMember(dt.Rows[0]["sender_parentid"].ToString());
                        DataTable dtGetDetail = Common.GetDetail(dtAccordingParentidToFindMember.Rows[0]["main_infno"].ToString(), dtAccordingParentidToFindMember.Rows[0]["main_isempno"].ToString());
                        email.sendEmail(dt.Rows[0]["sender_mail"].ToString(), "[ITRI] 通知：您有來自工研院大檔案傳輸的信件", "親愛的朋友 您好：<br /><br />" + dtGetDetail.Rows[0]["cName"].ToString() + " &lt;" + dtGetDetail.Rows[0]["cEmail"].ToString() + "&gt; 先生/小姐，在" + dtAccordingParentidToFindMember.Rows[0]["Cmain_createdate"].ToString() + "寄送下列的檔案給您。<br />請您利用下面的[解壓縮密碼]來開啟檔案:(請不要複製[]符號)<br /><b>[</b><span style='color:Red'>" + dt.Rows[0]["afile_encrypt"].ToString() + "</span><b>]</b><br />請使用此密碼解壓縮檔案<br /> 謝謝", dt.Rows[0]["afile_encrypt"].ToString(), "");
                        context.Response.Write("OK");
                    }
                    else
                    {
                        context.Response.Write("NO_downLoad");
                    }
                }
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message);
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