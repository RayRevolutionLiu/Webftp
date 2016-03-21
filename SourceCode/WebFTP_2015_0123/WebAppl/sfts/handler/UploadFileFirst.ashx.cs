using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.SessionState;
using System.Data;
using System.Data.SqlClient;

namespace WebAppl.sfts.handler
{
    /// <summary>
    ///UploadFileFirst 的摘要描述
    /// </summary>
    public class UploadFileFirst : IHttpHandler
    {
        //public class TooL
        //{
        //    public string contentLeng { get; set; }
        //    public string fedbkSTR { get; set; }
        //}

        public void ProcessRequest(HttpContext context)
        {
            string relativePath = AppConfig.source_path;
            string hidGuid = context.Request.Form["guid"].ToString().Trim();
            string radio = context.Request.Form["type"];//類型

            //List<TooL> eList = new List<TooL>();
            if (context.Request.Files.Count > 0)
            {
                SqlConnection oConn = new SqlConnection(AppConfig.DSN);
                oConn.Open();
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn;

                try
                {
                    oCmd.Parameters.Add("@file_parentid", SqlDbType.NVarChar);
                    oCmd.Parameters.Add("@file_type", SqlDbType.NVarChar);
                    oCmd.Parameters.Add("@file_comorsec", SqlDbType.NVarChar);
                    oCmd.Parameters.Add("@file_encrypt", SqlDbType.NVarChar);
                    oCmd.Parameters.Add("@file_origiFileName", SqlDbType.NVarChar);
                    oCmd.Parameters.Add("@file_encryptfileName", SqlDbType.NVarChar);
                    oCmd.Parameters.Add("@file_size", SqlDbType.NVarChar);
                    oCmd.Parameters.Add("@file_exten", SqlDbType.NVarChar);
                    oCmd.Parameters.Add("@file_createdate", SqlDbType.DateTime);
                    oCmd.Parameters.Add("@file_stat", SqlDbType.NVarChar);

                    HttpFileCollection files = context.Request.Files;
                    HttpPostedFile file = files[0];
                    //取得副檔名
                    string extension = System.IO.Path.GetExtension(file.FileName);
                    //取得TIME與GUID
                    string timeguid = timeguidclass.timeguid();
                    //儲存的名稱
                    string realFileName = relativePath + timeguid + extension;

                    file.SaveAs(realFileName);

                    //檔案原本檔名
                    string fileName = System.IO.Path.GetFileName(file.FileName).Replace(extension, "");
                    //進資料庫前, 儲存名稱要去除路徑
                    realFileName = realFileName.Replace(relativePath, "");
                    realFileName = realFileName.Replace(extension, "");
                    int file_size = file.ContentLength;
                    //寫入原本檔案紀錄表
                    oCmd.CommandText = @"INSERT INTO sfts_file (file_parentid ,file_type, file_comorsec, file_encrypt,file_origiFileName,file_encryptfileName,file_size,file_exten,file_createdate,file_stat) VALUES (@file_parentid, @file_type, @file_comorsec, @file_encrypt, @file_origiFileName, @file_encryptfileName,@file_size,@file_exten, @file_createdate, @file_stat);  ";
                    oCmd.Parameters["@file_parentid"].Value = hidGuid;
                    oCmd.Parameters["@file_type"].Value = "0";
                    oCmd.Parameters["@file_comorsec"].Value = radio;
                    oCmd.Parameters["@file_encrypt"].Value = "";
                    oCmd.Parameters["@file_origiFileName"].Value = fileName;
                    oCmd.Parameters["@file_encryptfileName"].Value = realFileName;
                    oCmd.Parameters["@file_size"].Value = file_size;
                    oCmd.Parameters["@file_exten"].Value = extension;
                    oCmd.Parameters["@file_createdate"].Value = DateTime.Now;
                    oCmd.Parameters["@file_stat"].Value = "Y";
                    oCmd.ExecuteNonQuery();


                    //TooL e = new TooL();
                    //e.contentLeng = file.ContentLength.ToString();
                    //e.fedbkSTR = file.FileName;
                    //eList.Add(e);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    oConn.Close();
                    oCmd.Connection.Close();
                }
            }

            //System.Web.Script.Serialization.JavaScriptSerializer objSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            //string ans = objSerializer.Serialize(eList);  //new
            //context.Response.ContentType = "text/plain";//為了IE而改 勿動
            //context.Response.Write("Success");
            //context.Response.Write(ans);//一定要回傳json格式,或是把前台dataType拿掉才可回傳字串
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