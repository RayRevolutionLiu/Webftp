using System;
using System.Web;
using System.Text;
using System.Configuration;
using System.IO;
using System.Data;

namespace WebAppl.sfts.enVersion.handlerEn
{
    /// <summary>
    ///ReSendEMailEn 的摘要描述
    /// </summary>
    public class ReSendEMailEn : IHttpHandler
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
                        throw new Exception("paramater contains illegal characters");
                    }

                    Veryfly_DB myVery = new Veryfly_DB();
                    DataTable dt = myVery.checkimagetext(aid, afileID);

                    if (dt.Rows[0]["sender_stat"].ToString().Trim() == "N")
                    {
                        Email email = new Email();
                        //email.sendEmail(dt.Rows[0]["sender_mail"].ToString(), "[ITRI] 通知：您有來自工研院大檔案傳輸的信件", "親愛的朋友 您好：<br /><br />先生/小姐，在 2013/04/18寄送下列的檔案給您。<br />請您利用下面的解壓縮密碼來開啟檔案:<br /><img alt='認證碼' src='cid:attech01.jpg' /><br />請使用此密碼解壓縮檔案<br /> 謝謝", dt.Rows[0]["afile_encrypt"].ToString());
                        DataTable dtAccordingParentidToFindMember = Common.AccordingParentidToFindMember(dt.Rows[0]["sender_parentid"].ToString());
                        DataTable dtGetDetail = Common.GetDetail(dtAccordingParentidToFindMember.Rows[0]["main_infno"].ToString(), dtAccordingParentidToFindMember.Rows[0]["main_isempno"].ToString());
                        email.sendEmail(dt.Rows[0]["sender_mail"].ToString(), "[ITRI] Notification from ITRI WebFTP - unZip Mail ", "Dear: <br /><br />" + dtGetDetail.Rows[0]["cName"].ToString() + " &lt;" + dtGetDetail.Rows[0]["cEmail"].ToString() + "&gt; had send some files to you at " + dtAccordingParentidToFindMember.Rows[0]["Cmain_createdate"].ToString() + ".<br />Please use the following [Unzip Password] to open the file: (Please do not copy [] symbols)<br /><b>[</b><span style='color:Red'>" + dt.Rows[0]["afile_encrypt"].ToString() + "</span><b>]</b><br /><br /> Thank you.", dt.Rows[0]["afile_encrypt"].ToString(), "");
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