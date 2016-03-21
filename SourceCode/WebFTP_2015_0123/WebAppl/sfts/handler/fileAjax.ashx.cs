using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.SessionState;
using System.Data;
using System.Data.SqlClient;

namespace WebAppl.handler
{
    /// <summary>
    ///fileAjax 的摘要描述
    /// </summary>
    public class fileAjax : IHttpHandler,IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                //設定ScriptTimeout=5小時 by 凱呈 1021004
                HttpContext.Current.Server.ScriptTimeout=18000; //timeout in seconds
                //注意 要用context.Request該HTML元件必須要有NAME此屬性!!
                //fileAjax_DB myfile = new fileAjax_DB();
                //HttpFileCollection uploadFiles = context.Request.Files;//檔案集合
                string relativePath = AppConfig.source_path;

                string txtaResult = context.Request.Form["hiddenInputJson"];//收件者
                var json = new System.Web.Script.Serialization.JavaScriptSerializer();
                var jsonObjList = json.Deserialize<System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, string>>>(txtaResult);

                string radio = context.Request.Form["type"];//類型
                string tbxtitle = context.Request.Form["tbxtitle"];//主旨
                string tbxdesc = context.Request.Form["tbxdesc"];//留言內容
                //string hidGuid = Guid.NewGuid().ToString();//parentkey
                string hidGuid = context.Request.Form["hidGuid"];//parentkey

                string lang = "Ch";
                if (context.Request.UrlReferrer.OriginalString.ToString().IndexOf("enVersion") != -1)
                {
                    lang = "En";
                }

                string notifyflag = context.Request.Form["tbxnotifyflag"];//收件者下載檔案或刪除取檔網址時通知我
                string disableRCPTDELLINK = context.Request.Form["tbxdisableRCPTDELLINK"];//禁止收件者自己刪除自己的取檔網址

                //if (uploadFiles.Count == 1)
                //{
                //    if (uploadFiles[0].ContentLength == 0)
                //    {
                //        transResponse(context, "請選擇要上傳之檔案ashx");
                //        return;
                //    }
                //}
                //else
                //{
                //    for (int i = 0; i < uploadFiles.Count; i++)
                //    {
                //        HttpPostedFile aFile = uploadFiles[i];

                //        if (aFile.ContentLength == 0 || String.IsNullOrEmpty(System.IO.Path.GetFileName(aFile.FileName)))
                //        {
                //            continue;
                //        }

                //        //取得副檔名
                //        string extension = System.IO.Path.GetExtension(aFile.FileName);

                //        if (extension.ToString().Trim() == "")
                //        {
                //            if (context.Request.UrlReferrer.OriginalString.ToString().IndexOf("enVersion") != -1)
                //            {
                //                transResponse(context, "You must enter the file extensions");
                //            }
                //            else
                //            {
                //                transResponse(context, "檔案必須輸入副檔名");
                //            }
                //            return;
                //        }
                //    }
                //}
                if (txtaResult.ToString().Trim() == "")
                {
                    transResponse(context, "請選擇收件人ashx");
                    return;
                }
                else
                {
                    //處理是否收件人中有任一的院內員工
                    //先轉成json格式

                    foreach (var jsonObj in jsonObjList)
                    {
                        jsonObj["account"].ToString();
                        jsonObj["email"].ToString();
                    }

                    bool judge = false;
                    //再用迴圈找裡面的值
                    for (int i = 0; i < jsonObjList.Count; i++)
                    {
                        DataTable dt = Common.AccordEmailIsitFromITRI(jsonObjList[i]["email"].ToString(), jsonObjList[i]["account"].ToString());

                        if (dt.Rows.Count > 0)
                        {
                            judge = true;
                            //表示其中之一人是院內人士
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    //2013/06/27 只有院外廠商寄送時需要判斷收件人必須至少有一位是院內員工
                    if (sAccount.GetAccInfo().Com_Isempno)
                    {
                        judge = true;
                    }

                    if (judge == false)
                    {
                        if (context.Request.UrlReferrer.OriginalString.ToString().IndexOf("enVersion") != -1)
                        {
                            transResponse(context, "Recipients must have at least one ITRI employee");
                        }
                        else
                        {
                            transResponse(context, "收件人必須有至少一位院內人士");
                        }
                        return;
                    }
                }
                if (radio.ToString() == "")
                {
                    transResponse(context, "請選擇類型ashx");
                    return;
                }
                if (tbxtitle.ToString().Trim() == "")
                {
                    transResponse(context, "請輸入主旨ashx");
                    return;
                }

                if (radio == "security")
                {
                    notifyflag = "Y";//如果是密件 無條件[收件者下載檔案或刪除取檔網址時通知我]
                }

                //2013/06/03 由於檔案跟收件人以及主旨是分開來新增的 若是檔案已經上去了 使用者突然關閉瀏覽器 會導致檔案已上傳
                //但是收件人跟主檔卻沒新增成功,故此處須改用Transaction
                SqlConnection oConn = new SqlConnection(AppConfig.DSN);
                oConn.Open();
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn;
                SqlTransaction myTrans = oConn.BeginTransaction();                
                oCmd.Transaction = myTrans;
                try
                {
                    //oCmd.Parameters.Add("@file_parentid", SqlDbType.NVarChar);
                    //oCmd.Parameters.Add("@file_type", SqlDbType.NVarChar);
                    //oCmd.Parameters.Add("@file_comorsec", SqlDbType.NVarChar);
                    //oCmd.Parameters.Add("@file_encrypt", SqlDbType.NVarChar);
                    //oCmd.Parameters.Add("@file_origiFileName", SqlDbType.NVarChar);
                    //oCmd.Parameters.Add("@file_encryptfileName", SqlDbType.NVarChar);
                    //oCmd.Parameters.Add("@file_size", SqlDbType.NVarChar);
                    //oCmd.Parameters.Add("@file_exten", SqlDbType.NVarChar);
                    //oCmd.Parameters.Add("@file_createdate", SqlDbType.DateTime);
                    //oCmd.Parameters.Add("@file_stat", SqlDbType.NVarChar);
                    ////----------------------------------------------------
                    //string fileName;

                    //for (int i = 0; i < uploadFiles.Count; i++)
                    //{
                    //    HttpPostedFile aFile = uploadFiles[i];
                      
                    //    if (aFile.ContentLength == 0 || String.IsNullOrEmpty(System.IO.Path.GetFileName(aFile.FileName)))
                    //    {
                    //        continue;
                    //    }

                    //    //取得副檔名
                    //    string extension = System.IO.Path.GetExtension(aFile.FileName);
                    //    //取得TIME與GUID
                    //    string timeguid = timeguidclass.timeguid();
                    //    //儲存的名稱
                    //    string realFileName = relativePath + timeguid + extension;

                    //    aFile.SaveAs(realFileName);

                    //    //檔案原本檔名
                    //    fileName = System.IO.Path.GetFileName(aFile.FileName).Replace(extension, "");

                    //    //進資料庫前, 儲存名稱要去除路徑
                    //    realFileName = realFileName.Replace(relativePath, "");
                    //    realFileName = realFileName.Replace(extension, "");
                    //    int file_size = aFile.ContentLength;
                    //    //寫入原本檔案紀錄表
                    //    oCmd.CommandText = @"INSERT INTO sfts_file (file_parentid ,file_type, file_comorsec, file_encrypt,file_origiFileName,file_encryptfileName,file_size,file_exten,file_createdate,file_stat) VALUES (@file_parentid, @file_type, @file_comorsec, @file_encrypt, @file_origiFileName, @file_encryptfileName,@file_size,@file_exten, @file_createdate, @file_stat);  ";
                    //    oCmd.Parameters["@file_parentid"].Value = hidGuid;
                    //    oCmd.Parameters["@file_type"].Value = "0";
                    //    oCmd.Parameters["@file_comorsec"].Value = radio;
                    //    oCmd.Parameters["@file_encrypt"].Value = "";
                    //    oCmd.Parameters["@file_origiFileName"].Value = fileName;
                    //    oCmd.Parameters["@file_encryptfileName"].Value = realFileName;
                    //    oCmd.Parameters["@file_size"].Value = file_size;
                    //    oCmd.Parameters["@file_exten"].Value = extension;
                    //    oCmd.Parameters["@file_createdate"].Value = DateTime.Now;
                    //    oCmd.Parameters["@file_stat"].Value = "Y";
                    //    oCmd.ExecuteNonQuery();

                    //    //myfile.InsertFile(hidGuid, "0", radio, "", fileName, realFileName, file_size, extension, DateTime.Now, "Y");
                    //}

                    //收件者新增
                    string query = string.Empty;
                    Random ram = new Random();
                    int numb = 0;

                    oCmd.Parameters.Add("@sender_parentid", SqlDbType.NVarChar);
                    oCmd.Parameters.Add("@sender_mail", SqlDbType.NVarChar);
                    oCmd.Parameters.Add("@sender_isempno", SqlDbType.NVarChar);
                    oCmd.Parameters.Add("@sender_querystring", SqlDbType.NVarChar);
                    oCmd.Parameters.Add("@sender_notifyflag", SqlDbType.NVarChar);
                    oCmd.Parameters.Add("@sender_disabled", SqlDbType.NVarChar);
                    oCmd.Parameters.Add("@sender_logindate", SqlDbType.DateTime);
                    oCmd.Parameters.Add("@sender_downloaddate", SqlDbType.DateTime);
                    oCmd.Parameters.Add("@sender_stat", SqlDbType.NVarChar);


                    for (int i = 0; i < jsonObjList.Count; i++)
                    {
                        string email = jsonObjList[i]["email"].ToString().Trim();
                        string account = jsonObjList[i]["account"].ToString().Trim();
                        numb = ram.Next(9999);
                        //                              主旨     內容        時間                 收件者EMAIL   (同樣的EMAIL 由於時間會一樣 所以值也會一樣 最後面再加個亂數)     
                        query = encode.sha1en(tbxtitle + tbxdesc + DateTime.Now.ToString() + email + numb.ToString());
                        //                              主旨     內容        時間(時間格式差別)        收件者EMAIL
                        oCmd.Parameters["@sender_parentid"].Value = hidGuid;
                        oCmd.Parameters["@sender_mail"].Value = email;
                        oCmd.Parameters["@sender_querystring"].Value = query;
                        oCmd.Parameters["@sender_notifyflag"].Value = notifyflag;
                        oCmd.Parameters["@sender_disabled"].Value = disableRCPTDELLINK;
                        oCmd.Parameters["@sender_logindate"].Value = DBNull.Value;
                        oCmd.Parameters["@sender_downloaddate"].Value = DBNull.Value;
                        oCmd.Parameters["@sender_stat"].Value = "Y";
                        oCmd.Parameters["@sender_isempno"].Value = account;
                        oCmd.CommandText = @"INSERT INTO sfts_sender (sender_parentid ,sender_mail,sender_isempno, sender_querystring,sender_notifyflag,sender_disabled,sender_logindate,sender_downloaddate,sender_stat,sender_queryenable) VALUES (@sender_parentid, @sender_mail,@sender_isempno, @sender_querystring, @sender_notifyflag, @sender_disabled, @sender_logindate,@sender_downloaddate,@sender_stat,'Y') ";
                        oCmd.ExecuteNonQuery();
                        //myfile.InsertSender(hidGuid, email, query, notifyflag, disableRCPTDELLINK, "", "", "Y", account);
                    }

                    //主檔新增
                    //myfile.InsertMain(sAccount.GetAccInfo().Com_Isempno.ToString(), sAccount.GetAccInfo().Account, hidGuid, tbxtitle, tbxdesc,"Y", radio, DateTime.Now);
                    oCmd.Parameters.Add("@main_isempno", SqlDbType.NVarChar);
                    oCmd.Parameters.Add("@main_infno", SqlDbType.NVarChar);
                    oCmd.Parameters.Add("@main_parentid", SqlDbType.NVarChar);
                    oCmd.Parameters.Add("@main_title", SqlDbType.NVarChar);
                    oCmd.Parameters.Add("@main_desc", SqlDbType.NVarChar);
                    oCmd.Parameters.Add("@main_stat", SqlDbType.NVarChar);
                    oCmd.Parameters.Add("@main_secret", SqlDbType.NVarChar);
                    oCmd.Parameters.Add("@main_createdate", SqlDbType.DateTime);
                    //2013/07/04 新增語系 目的為console發信時判斷該筆是從何處新增 就要發送何種語系的通知信
                    oCmd.Parameters.Add("@main_lang", SqlDbType.NVarChar);

                    oCmd.Parameters["@main_isempno"].Value = sAccount.GetAccInfo().Com_Isempno.ToString();
                    oCmd.Parameters["@main_infno"].Value = sAccount.GetAccInfo().Account;
                    oCmd.Parameters["@main_parentid"].Value = hidGuid;
                    oCmd.Parameters["@main_title"].Value = tbxtitle;
                    oCmd.Parameters["@main_desc"].Value = tbxdesc;
                    oCmd.Parameters["@main_stat"].Value = "Y";
                    oCmd.Parameters["@main_secret"].Value = radio;
                    oCmd.Parameters["@main_createdate"].Value = DateTime.Now;
                    oCmd.Parameters["@main_lang"].Value = lang;

                    oCmd.CommandText = @"INSERT INTO sfts_main (main_isempno,main_infno,main_parentid,main_title,main_desc,main_stat,main_secret,main_createdate,main_lang) VALUES (@main_isempno,@main_infno,@main_parentid,@main_title,@main_desc,@main_stat,@main_secret,@main_createdate,@main_lang)";
                    oCmd.ExecuteNonQuery();

                    myTrans.Commit();
                }
                catch (Exception ex)
                {
                    myTrans.Rollback();
                }
                finally
                {
                    oCmd.Connection.Close();
                    oConn.Close();
                    context.Response.ContentType = "text/html";
                    context.Response.Write("<script type='text/JavaScript'>parent.feedbackFun('" + hidGuid + "');</script>");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void transResponse(HttpContext context,string mess)
        {
            context.Response.ContentType = "text/html";
            context.Response.Write("<script type='text/JavaScript'>parent.alertMessageASHX('" + mess + "');</script>");
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