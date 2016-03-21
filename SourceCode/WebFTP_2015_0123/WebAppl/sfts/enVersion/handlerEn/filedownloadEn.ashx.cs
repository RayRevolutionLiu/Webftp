using System;
using System.Web;
using System.Text;
using System.Configuration;
using System.IO;
using System.Data;

namespace WebAppl.sfts.enVersion.handlerEn
{
    /// <summary>
    ///filedownloadEn 的摘要描述
    /// </summary>
    public class filedownloadEn : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request["aid"] != null)
            {
                string aid = context.Request.QueryString["aid"].ToString().Trim();
                string afileID = context.Request.QueryString["afileID"].ToString().Trim();

                if (encode.sqlInjection(aid) || encode.sqlInjection(afileID))
                {
                    throw new Exception("illegal paramater value");
                }
                else
                {
                    Veryfly_DB myVery = new Veryfly_DB();
                    DataTable dt = myVery.checkimagetext(aid, afileID);
                    if (dt.Rows.Count == 0)
                    {
                        throw new Exception("Can't find the sender.");
                    }
                    else
                    {
                        string vailde = dt.Rows[0]["sender_imagetext"].ToString().Trim();
                        if (vailde == context.Request["vailde"].ToString().Trim())
                        {
                            string strpath;
                            //底下的IF為 該筆是一般 而且是單筆的不是整包的壓縮檔(!="N")
                            if (dt.Rows[0]["afile_comorsec"].ToString().Trim() == "common" && dt.Rows[0]["afile_encrypt"].ToString().Trim() != "N")
                            {
                                strpath = string.Format("{0}{1}", AppConfig.source_path, context.Request["filename"] + context.Request["exten"]);
                            }
                            else if (dt.Rows[0]["afile_comorsec"].ToString().Trim() == "nonforward" && dt.Rows[0]["afile_encrypt"].ToString().Trim() != "N")
                            {
                                strpath = string.Format("{0}{1}", AppConfig.source_path, context.Request["filename"] + context.Request["exten"]);
                            }
                            else
                            {
                                strpath = string.Format("{0}{1}", AppConfig.Source_zip_path, context.Request["filename"] + context.Request["exten"]);
                            }

                            FileInfo file = new FileInfo(strpath);

                            string strContentType = string.Empty;
                            switch (file.Extension)
                            {
                                case ".asf":
                                    strContentType = "video/x-ms-asf";
                                    break;
                                case ".avi":
                                    strContentType = "video/avi";
                                    break;
                                case ".doc":
                                    strContentType = "application/msword";
                                    break;
                                case ".zip":
                                    strContentType = "application/zip";
                                    break;
                                case ".xls":
                                    strContentType = "application/vnd.ms-excel";
                                    break;
                                case ".csv":
                                    strContentType = "application/vnd.ms-excel";
                                    break;
                                case ".gif":
                                    strContentType = "image/gif";
                                    break;
                                case ".jpg":
                                    strContentType = "image/jpeg";
                                    break;
                                case "jpeg":
                                    strContentType = "image/jpeg";
                                    break;
                                case ".wav":
                                    strContentType = "audio/wav";
                                    break;
                                case ".mp3":
                                    strContentType = "audio/mpeg3";
                                    break;
                                case ".mpg":
                                    strContentType = "video/mpeg";
                                    break;
                                case "mpeg":
                                    strContentType = "video/mpeg";
                                    break;
                                case ".htm":
                                    strContentType = "text/html";
                                    break;
                                case ".html":
                                    strContentType = "text/html";
                                    break;
                                case ".asp":
                                    strContentType = "text/asp";
                                    break;

                                default:
                                    strContentType = "application/octet-stream";
                                    break;

                            }
                            if (file.Exists)
                            {
                                FileStream oF = new FileStream(strpath, FileMode.Open, FileAccess.Read);
                                context.Response.ClearHeaders();
                                context.Response.Clear();
                                context.Response.BufferOutput = false;
                                context.Response.CacheControl = "Private";
                                context.Response.ContentType = strContentType;
                                context.Response.AppendHeader("Content-Length", oF.Length.ToString()); /*指定文件大小，可讓瀏覽器能夠顯示下載進度，並可優化、加快下載速度*/
                                string strDownloadName = string.Empty;

                                if (context.Request.Browser.Browser == "IE")
                                {
                                    context.Response.HeaderEncoding = System.Text.Encoding.GetEncoding("big5");
                                    strDownloadName = context.Request["source"];
                                }
                                else
                                {
                                    strDownloadName = System.Web.HttpUtility.UrlEncode(context.Request["source"]);
                                }

                                context.Response.AddHeader("content-disposition", "attachment;filename=" + strDownloadName + context.Request["exten"]);

                                //context.Response.WriteFile(strpath);
                                //context.Response.Flush();
                                //context.Response.End();
                                if (PassStream(context.Response.OutputStream, oF, context))
                                {
                                    if (dt.Rows[0]["sender_stat"].ToString().Trim() == "Y" && dt.Rows[0]["afile_comorsec"].ToString().Trim() == "security")//寄解壓縮密碼 只有在第一次下載才需要
                                    {
                                        Email email = new Email();
                                        //email.sendEmail(dt.Rows[0]["sender_mail"].ToString(), "[ITRI] 通知：您有來自工研院大檔案傳輸的信件", "親愛的朋友 您好：<br /><br />先生/小姐，在 2013/04/18寄送下列的檔案給您。<br />請您利用下面的解壓縮密碼來開啟檔案:<br /><img alt='認證碼' src='cid:attech01.jpg' /><br />請使用此密碼解壓縮檔案<br /> 謝謝", dt.Rows[0]["afile_encrypt"].ToString());
                                        DataTable dtAccordingParentidToFindMember = Common.AccordingParentidToFindMember(dt.Rows[0]["sender_parentid"].ToString());
                                        DataTable dtGetDetail = Common.GetDetail(dtAccordingParentidToFindMember.Rows[0]["main_infno"].ToString(), dtAccordingParentidToFindMember.Rows[0]["main_isempno"].ToString());
                                        email.sendEmail(dt.Rows[0]["sender_mail"].ToString(), "[ITRI] Notification from ITRI WebFTP - unZip Mail", "Dear: <br /><br />" + dtGetDetail.Rows[0]["cName"].ToString() + " &lt;" + dtGetDetail.Rows[0]["cEmail"].ToString() + "&gt;  had send some files to you at " + dtAccordingParentidToFindMember.Rows[0]["Cmain_createdate"].ToString() + "<br />Please use the following [Unzip Password] to open the file: (Please do not copy [] symbols)<br /><b>[</b><span style='color:Red'>" + dt.Rows[0]["afile_encrypt"].ToString() + "</span><b>]</b><br /><br />Thank you.", dt.Rows[0]["afile_encrypt"].ToString(), "");
                                        //把狀態更新 就不會再寄信了
                                        myVery.UpdateSender_Stat(dt.Rows[0]["sender_id"].ToString());
                                    }

                                    //下載次數跟時間LOG
                                    myVery.InsertDownloadLog(afileID, dt.Rows[0]["sender_parentid"].ToString(), aid, DateTime.Now);
                                }

                                //判斷[收件者下載檔案或刪除取檔網址時通知我]
                                if (context.Request.QueryString["notifyflag"] != null && context.Request.QueryString["notifyflag"].ToString().Trim() == "Y")
                                {
                                    //如果有勾選要寄信給寄件人
                                    Email email = new Email();
                                    StringBuilder sb = new StringBuilder();
                                    sb.Append(@"Dear:  <br /> *Notices*<br />");
                                    sb.Append(@"The file " + dt.Rows[0]["ShowFileName"].ToString() + dt.Rows[0]["afile_exten"].ToString() + " you sent at " + Convert.ToDateTime(dt.Rows[0]["afile_createdate"].ToString()).ToString("yyy/MM/dd") + "");
                                    sb.Append(@"is downloading by receiver ");
                                    sb.Append(@" &lt; " + dt.Rows[0]["sender_mail"].ToString() + " &gt; ");
                                    sb.Append(@"now.<br />");
                                    //sb.Append(@"");
                                    DataTable dtAccordingParentidToFindMember = Common.AccordingParentidToFindMember(dt.Rows[0]["sender_parentid"].ToString());
                                    DataTable dtGetDetail = Common.GetDetail(dtAccordingParentidToFindMember.Rows[0]["main_infno"].ToString(), dtAccordingParentidToFindMember.Rows[0]["main_isempno"].ToString());
                                    email.sendEmail(dtGetDetail.Rows[0]["cEmail"].ToString(), "[ITRI] Notification from ITRI WebFTP - your file has been downloaded", sb.ToString(), "");
                                }
                            }
                        }
                    }
                }


            }
            else
            {
                throw new Exception("File does not exist");
            }
        }

        /// <summary>
        /// 以分批的方式傳送串流間的資料, 以減少對記憶體的需求
        /// 預設緩衝區(buffer)=8192(byte)，可經由設定大小，來控制下載速度及對記憶體的需求
        /// </summary>
        /// <param name="outStream">資料輸出串流</param>
        /// <param name="inStream">資料輸入串流</param>
        bool PassStream(Stream outStream, Stream inStream, HttpContext context)
        {
            int bufflen = 8192;
            int len = 0;
            byte[] buff = new byte[bufflen];
            len = inStream.Read(buff, 0, bufflen);
            while (len > 0)
            {
                //檢查用戶是否能在連線,  如果有才繼續傳送封包
                if (context.Response.IsClientConnected)
                {

                    outStream.Write(buff, 0, len);
                    len = inStream.Read(buff, 0, bufflen);

                }
                else
                {
                    len = 0;
                }

            }
            outStream.Flush();
            return true;
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