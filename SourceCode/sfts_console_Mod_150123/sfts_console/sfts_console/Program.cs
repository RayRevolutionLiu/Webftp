using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Xceed.Zip;
using Xceed.Zip.ReaderWriter;
using Xceed.Compression;
using Xceed.FileSystem;
using System.Security.Cryptography;

namespace sfts_console
{
    class Program
    {
        public static string pathUrl(string appkey)
        {
            return System.Configuration.ConfigurationManager.AppSettings[appkey].ToString();
        }

        static void Main(string[] args)
        {
            //Use this to license Xceed Real-Time Zip for .NET CF
            Xceed.Zip.ReaderWriter.Licenser.LicenseKey = "ZRT51-L1WSL-4KWJJ-GBEA";

            while (true)
            {
                StringBuilder sb = new StringBuilder();
                SqlConnection oConn = new SqlConnection(pathUrl("DSN"));
                SqlCommand oCmd = new SqlCommand();                
                oCmd.Connection = oConn;
                try
                {
                    //第五隻
                    //sb.Append(@"SELECT DISTINCT main_id,file_parentid,file_comorsec,main_lang FROM sfts_file INNER JOIN sfts_main ON main_parentid=file_parentid WHERE (file_type='0') OR (file_type='0.5' AND DATEDIFF(minute,file_consoledate,GETDATE()) > @consolehalf ) ");
                    //file_type 0為未處理,0.5為處理中,1為已處裡
                    sb.Append(@"SELECT DISTINCT main_id,file_parentid,file_comorsec,main_lang FROM sfts_file INNER JOIN sfts_main ON main_parentid=file_parentid WHERE file_type='0' AND main_id % @modCount = @lessCount ");
                    oCmd.CommandText = sb.ToString();
                    //oCmd.Parameters.AddWithValue("@consolehalf", Convert.ToInt32(pathUrl("consolehalf")));
                    oCmd.Parameters.AddWithValue("@modCount", Convert.ToInt32(pathUrl("modCount")));
                    oCmd.Parameters.AddWithValue("@lessCount", Convert.ToInt32(pathUrl("lessCount")));
                    SqlDataAdapter oda = new SqlDataAdapter(oCmd);
                    //延長sql連接timeout時間 600秒=10分鐘 by 凱呈
                    oda.SelectCommand.CommandTimeout = 600;
                    DataTable dt = new DataTable();
                    oda.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string file_parentid = dt.Rows[i]["file_parentid"].ToString();
                            string file_comorsec = dt.Rows[i]["file_comorsec"].ToString();
                            string main_lang = dt.Rows[i]["main_lang"].ToString();

                            if (file_comorsec == "common" || file_comorsec == "nonforward")
                            {
                                new Program().commonORnonforwardfun(file_parentid, file_comorsec);
                            }
                            else if (file_comorsec == "security")
                            {
                                new Program().securityfun(file_parentid, file_comorsec);
                            }

                            new Program().SendMail(file_parentid, file_comorsec, main_lang);
                        }
                    }
                    else
                    {
                        System.Console.WriteLine("No data Waiting!");
                    }
                }
                catch (Exception err)
                {
                    string url = pathUrl("Zippath") + "Error.txt";
                    StreamWriter sw = new StreamWriter(url, true, Encoding.UTF8);
                    if (err != null)
                    {
                        sw.WriteLine(err.GetType().FullName);
                        sw.WriteLine(err.Message);
                        sw.WriteLine(err.Source);
                        sw.WriteLine(err.StackTrace);
                        sw.WriteLine();
                        sw.WriteLine(err.GetBaseException().GetType().FullName);
                        sw.WriteLine(err.GetBaseException().Message);
                        sw.WriteLine(err.GetBaseException().Source);
                        sw.WriteLine(err.GetBaseException().StackTrace);
                        //sw.WriteLine(((HttpException) err).Source);
                    }
                    else
                    {
                        sw.WriteLine("UnKnown Error!");
                    }

                    sw.Flush();
                    sw.Close();

                    Email myEmail = new Email();
                    myEmail.sendEmail(pathUrl("DevEmail"), "第" + pathUrl("lessCount") + "console出問題", err.Message, "Error", "","WebFTP");

                    System.Console.WriteLine(err.Message);
                    System.Console.ReadKey();//有錯誤會HOLD住
                    if (oCmd.Connection.State != ConnectionState.Closed)
                        oCmd.Connection.Close();
                }
                finally
                {
                    if (oCmd.Connection.State != ConnectionState.Closed)
                        oCmd.Connection.Close();

                    System.Threading.Thread.Sleep(Convert.ToInt32(pathUrl("consoleSleep")));
                }
            }
        }


        public void commonORnonforwardfun(string file_parentid, string file_comorsec)
        {
            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(pathUrl("DSN"));
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            sb.Append(@"SELECT * FROM sfts_file WHERE file_parentid = @file_parentid AND file_comorsec= @file_comorsec ");
            oCmd.CommandText = sb.ToString();
            oCmd.Parameters.AddWithValue("@file_parentid", file_parentid);
            oCmd.Parameters.AddWithValue("@file_comorsec", file_comorsec);
            SqlDataAdapter oda = new SqlDataAdapter(oCmd);
            DataTable dt = new DataTable();
            oda.Fill(dt);

            Random ram = new Random();
            int numb = ram.Next(9999);
            string zipNmae = DateTime.Now.ToString("yyyyMMddHHmmss") + numb.ToString();

            //一般檔案 還需要壓縮成一個檔案 裡面放所有的檔案 但是不需要解壓縮密碼
            using (Stream zipFileStream = new FileStream(pathUrl("Zippath") + zipNmae + ".zip", FileMode.Create, FileAccess.Write))
            {
                int fileSize = 0;
                ZipWriter zipWriter = new ZipWriter(zipFileStream);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //把sfts_file的狀態更新(0.5處理中)
                    Update_sfts_fileOnGoing(dt.Rows[i]["file_id"].ToString(), "0.5");

                    ZipItemLocalHeader localHeader;
                    //一般
                    localHeader = new ZipItemLocalHeader(
                      dt.Rows[i]["file_origiFileName"].ToString() + dt.Rows[i]["file_exten"].ToString(),
                      CompressionMethod.Deflated64,
                      CompressionLevel.Highest
                    );

                    zipWriter.WriteItemLocalHeader(localHeader);

                    using (Stream sourceStream = new FileStream(pathUrl("path") + dt.Rows[i]["file_encryptfileName"].ToString() + dt.Rows[i]["file_exten"].ToString(), FileMode.Open, FileAccess.Read))
                    {
                        zipWriter.WriteItemData(sourceStream);
                    }
                    fileSize += Convert.ToInt32(dt.Rows[i]["file_size"].ToString());
                }
                zipWriter.CloseZipFile();
                zipWriter.Dispose();

                //把所有壓成一包的壓縮檔資料塞一筆到afile內 差別只在於此筆資料 file_origiFileName="" and afile_encrypt="N"(方便在撈的時候餵給GridView不要被撈到)
                INSERT_sfts_afile(dt.Rows[0]["file_parentid"].ToString(), dt.Rows[0]["file_comorsec"].ToString(), "N",
                    "", zipNmae, fileSize, dt.Rows[0]["file_stat"].ToString(), ".zip");
            }


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //不管怎樣 還是把所有資料二話不說複製一份到sfts_afile裡面 方便處裡
                INSERT_sfts_afile(dt.Rows[i]["file_parentid"].ToString(), dt.Rows[i]["file_comorsec"].ToString(), dt.Rows[i]["file_encrypt"].ToString(),
                    dt.Rows[i]["file_origiFileName"].ToString(), dt.Rows[i]["file_encryptfileName"].ToString(), Convert.ToInt32(dt.Rows[i]["file_size"].ToString()), dt.Rows[i]["file_stat"].ToString(), dt.Rows[i]["file_exten"].ToString());
            }

            //把sfts_file的狀態更新
            Update_sfts_file(file_parentid, file_comorsec, "1");


            oda.Dispose();
            oConn.Close();
            oConn.Dispose();
            oCmd.Dispose();
        }


        public void securityfun(string file_parentid, string file_comorsec)
        {
            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(pathUrl("DSN"));
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            sb.Append(@"SELECT * FROM sfts_file WHERE file_parentid = @file_parentid AND file_comorsec= @file_comorsec ");
            oCmd.CommandText = sb.ToString();
            oCmd.Parameters.AddWithValue("@file_parentid", file_parentid);
            oCmd.Parameters.AddWithValue("@file_comorsec", file_comorsec);
            SqlDataAdapter oda = new SqlDataAdapter(oCmd);
            DataTable dt = new DataTable();
            oda.Fill(dt);

            string pasw = GenAESCodePass(file_parentid);//解壓縮密碼
            int fileTotalSize = 0;
            using (Stream zipFileStream = new FileStream(pathUrl("Zippath") + dt.Rows[0]["file_encryptfileName"].ToString() + ".zip", FileMode.Create, FileAccess.Write))
            {
                ZipWriter zipWriter = new ZipWriter(zipFileStream);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //把sfts_file的狀態更新(0.5處理中)
                    Update_sfts_fileOnGoing(dt.Rows[i]["file_id"].ToString(), "0.5");

                    ZipItemLocalHeader localHeader;
                    //密件
                    localHeader = new ZipItemLocalHeader(
                       dt.Rows[i]["file_origiFileName"].ToString() + dt.Rows[i]["file_exten"].ToString(),
                       CompressionMethod.Deflated64,
                       CompressionLevel.Highest,
                       EncryptionMethod.WinZipAes,
                       pasw);

                    zipWriter.WriteItemLocalHeader(localHeader);
                    

                    using (Stream sourceStream = new FileStream(pathUrl("path") + dt.Rows[i]["file_encryptfileName"].ToString() + dt.Rows[i]["file_exten"].ToString(), FileMode.Open, FileAccess.Read))
                    {
                        zipWriter.WriteItemData(sourceStream);
                    }

                    fileTotalSize+=Convert.ToInt32(dt.Rows[i]["file_size"].ToString());

                }
                zipWriter.CloseZipFile();
                zipWriter.Dispose();
            }


            //把sft_afile新增
            INSERT_sfts_afile(dt.Rows[0]["file_parentid"].ToString(), dt.Rows[0]["file_comorsec"].ToString(), pasw,
                "", dt.Rows[0]["file_encryptfileName"].ToString(), fileTotalSize, dt.Rows[0]["file_stat"].ToString(),".zip");

            //把sfts_file的狀態更新
            Update_sfts_file(file_parentid, file_comorsec, "1");


            oda.Dispose();
            oConn.Close();
            oConn.Dispose();
            oCmd.Dispose();
        }

        public void SendMail(string file_parentid, string file_comorsec, string main_lang)
        {
            switch (main_lang)
            {
                case "Ch":
                    {
                        Encrypt_Class nyEn = new Encrypt_Class();
                        Email myEmail = new Email();
                        //string subject = "[ITRI] 通知：您有來自工研院大檔案傳輸的信件";
                        string to = "";

                        StringBuilder sb = new StringBuilder();
                        SqlConnection oConn = new SqlConnection(pathUrl("DSN"));
                        SqlCommand oCmd = new SqlCommand();
                        oCmd.Connection = oConn;
                        sb.Append(@"SELECT * FROM sfts_sender WHERE sender_parentid = @sender_parentid");
                        oCmd.CommandText = sb.ToString();
                        oCmd.Parameters.AddWithValue("@sender_parentid", file_parentid);
                        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
                        DataTable dt = new DataTable();
                        oda.Fill(dt);

                        DataTable dtMain = ReturnMain(file_parentid);
                        string subject = "[ITRI通知]：" + dtMain.Rows[0]["main_title"].ToString();
                        string ReturnSenderName = "";
                        string returnSenderMail = "";

                        if (dtMain.Rows[0]["main_isempno"].ToString().ToLower() == "true")//代表寄件者是院內員工 需要去common抓名字
                        {
                            ReturnSenderName = IsEmpnoTrue(dtMain.Rows[0]["main_infno"].ToString());
                            returnSenderMail = IsEmpnoTrueEmail(dtMain.Rows[0]["main_infno"].ToString());
                        }
                        else
                        {
                            ReturnSenderName = IsEmpnoFalse(dtMain.Rows[0]["main_infno"].ToString());
                            returnSenderMail = IsEmpnoFalseEmail(dtMain.Rows[0]["main_infno"].ToString());
                        }

                        DataTable dtFile = ReturnFileList(file_parentid);
                        string fileList = "";
                        for (int i = 0; i < dtFile.Rows.Count; i++)
                        {
                            fileList += dtFile.Rows[i]["fileNameShow"].ToString() + dtFile.Rows[i]["afile_exten"].ToString() + "<br />";
                        }

                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                Random ram = new Random();
                                int numb = ram.Next(9999);

                                to = dt.Rows[i]["sender_mail"].ToString().Trim();
                                StringBuilder sbBody = new StringBuilder();
                                sbBody.Append(@"<html><head><title></title><style type='text/css'>DIV.PlainText {FONT-FAMILY: monospace; FONT-SIZE: 120%}</style></head>");
                                sbBody.Append(@"<body><font size='2'>");
                                sbBody.Append(@"<div class='PlainText'>");
                                sbBody.Append(@"親愛的朋友 您好：<br />");
                                sbBody.Append(@"<br />");
                                sbBody.AppendFormat(@"{0} &lt;{2}&gt; 先生/小姐，在 {1}寄送下列的檔案給您。", ReturnSenderName, DateTime.Now.ToString("yyyy/MM/dd"), returnSenderMail);
                                sbBody.Append(@"請您利用下面的網址來下載檔案，謝謝您！<br /><br />※ 所有的檔案將在 7 天後刪除※");
                                sbBody.Append(@"<br /><br />訊息留言：<br />");
                                sbBody.AppendFormat(@"{0}", dtMain.Rows[0]["main_desc"].ToString());
                                sbBody.Append(@"<br />");
                                sbBody.Append(@"檔案列表：<br />");
                                sbBody.Append(@"" + fileList + "");

                                if (file_comorsec == "security")
                                {
                                    sbBody.Append(@"取檔認證碼：<br />如為密件檔案，請在下載時輸入圖片內之數字，即會再寄一封解壓縮密碼給您。<br />");
                                    sbBody.Append(@"<img alt='認證碼' src='cid:attech01.jpg' /><br />");
                                }
                                sbBody.Append(@"取檔網址：<br />");
                                sbBody.Append(@"※ 1. 如果有「安全性警示」的視窗跳出，請按「是」接受以繼續接收檔案<br />");
                                sbBody.Append(@"※ 2. 此加密取檔網址為您所獨有，若需要轉寄信件時，請務必留意：系統將所有透過此網址取檔者均視同已取得您本人同意。<br />");
                                sbBody.Append(@"<a href='" + pathUrl("MailUrl") + "Veryfly.aspx?en=" + dt.Rows[i]["sender_querystring"].ToString() + "' target='_blank'>" + pathUrl("MailUrl") + "Veryfly.aspx?en=" + dt.Rows[i]["sender_querystring"].ToString() + "</a><br />");//pathUrl("MailUrl") + "Veryfly.aspx?en=" + dt.Rows[i]["sender_querystring"].ToString(), pathUrl("MailUrl") + "Veryfly.aspx?en=" + dt.Rows[i]["sender_querystring"].ToString()
                                sbBody.Append(@"====================================================================<br />");
                                sbBody.Append(@"本信件可能包含工研院機密資訊，非指定之收件者，請勿使用或揭露本信件內容，並請銷毀此信件。<br />");
                                sbBody.Append(@"This email may contain confidential information. Please do not use or disclose ");
                                sbBody.Append(@"it in any way and delete it if you are not the intended recipient.</div>");
                                sbBody.Append(@"</font></body></html>");
                                //sbBody.AppendFormat(@"");

                                myEmail.sendEmail(to, subject, sbBody.ToString(), numb.ToString(), returnSenderMail,ReturnSenderName);

                                if (file_comorsec == "security")
                                {
                                    UpdateImageText(numb.ToString(), dt.Rows[i]["sender_id"].ToString());
                                }

                            }
                        }
                        else
                        {
                            System.Console.WriteLine("找不到收件者");
                        }
                        oda.Dispose();
                        oConn.Close();
                        oConn.Dispose();
                        oCmd.Dispose();
                        break;
                    }
                case "En":
                    {
                        Encrypt_Class nyEn = new Encrypt_Class();
                        Email myEmail = new Email();
                        //string subject = "[ITRI] Notification from ITRI WebFTP - You have a file send by your partner";
                        string to = "";

                        StringBuilder sb = new StringBuilder();
                        SqlConnection oConn = new SqlConnection(pathUrl("DSN"));
                        SqlCommand oCmd = new SqlCommand();
                        oCmd.Connection = oConn;
                        sb.Append(@"SELECT * FROM sfts_sender WHERE sender_parentid = @sender_parentid");
                        oCmd.CommandText = sb.ToString();
                        oCmd.Parameters.AddWithValue("@sender_parentid", file_parentid);
                        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
                        DataTable dt = new DataTable();
                        oda.Fill(dt);

                        DataTable dtMain = ReturnMain(file_parentid);
                        string subject ="[ITRI] Notification from WebFTP - "+dtMain.Rows[0]["main_title"].ToString();
                        string ReturnSenderName = "";
                        string returnSenderMail = "";

                        if (dtMain.Rows[0]["main_isempno"].ToString().ToLower() == "true")//代表寄件者是院內員工 需要去common抓名字
                        {
                            ReturnSenderName = IsEmpnoTrue(dtMain.Rows[0]["main_infno"].ToString());
                            returnSenderMail = IsEmpnoTrueEmail(dtMain.Rows[0]["main_infno"].ToString());
                        }
                        else
                        {
                            ReturnSenderName = IsEmpnoFalse(dtMain.Rows[0]["main_infno"].ToString());
                            returnSenderMail = IsEmpnoFalseEmail(dtMain.Rows[0]["main_infno"].ToString());
                        }

                        DataTable dtFile = ReturnFileList(file_parentid);
                        string fileList = "";
                        for (int i = 0; i < dtFile.Rows.Count; i++)
                        {
                            fileList += dtFile.Rows[i]["fileNameShow"].ToString() + dtFile.Rows[i]["afile_exten"].ToString() + "<br />";
                        }

                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                Random ram = new Random();
                                int numb = ram.Next(9999);

                                to = dt.Rows[i]["sender_mail"].ToString().Trim();
                                StringBuilder sbBody = new StringBuilder();
                                sbBody.Append(@"<html><head><title></title><style type='text/css'>DIV.PlainText {FONT-FAMILY: monospace; FONT-SIZE: 120%}</style></head>");
                                sbBody.Append(@"<body><font size='2'>");
                                sbBody.Append(@"<div class='PlainText'>");
                                sbBody.Append(@"Dear:<br />");
                                sbBody.Append(@"<br />");
                                sbBody.AppendFormat(@"{0} &lt;{2}&gt; had send some files to you at  {1}.", ReturnSenderName, DateTime.Now.ToString("yyyy/MM/dd"), returnSenderMail);
                                sbBody.Append(@"Please download your files as soon as possible.<br /><br />※ All files will be deleted in 7 Days ※");
                                sbBody.Append(@"<br /><br />Messages:<br />");
                                sbBody.AppendFormat(@"{0}", dtMain.Rows[0]["main_desc"].ToString());
                                sbBody.Append(@"<br />");
                                sbBody.Append(@"File List:<br />");
                                sbBody.Append(@"" + fileList + "");

                                if (file_comorsec == "security")
                                {
                                    sbBody.Append(@"authentication code:<br />If it is security file,Please enter the number in the CAPTCHA when you download. We will send you another mail with a password for you to unzip the file(s).<br />");
                                    sbBody.Append(@"<img alt='認證碼' src='cid:attech01.jpg' /><br />");
                                }
                                sbBody.Append(@"Download URL:<br />");
                                sbBody.Append(@"* 1. If any 'Security Alert' windows pop up, please proceed by clicking 'Yes'. <br />");
                                sbBody.Append(@"* 2. This encoded URL is for YOU ONLY.<br />");
                                sbBody.Append(@"We will consider everyone accessing this URL having your approval.<br />Please confirm before you forward this e-mail.<br />");
                                sbBody.Append(@"<a href='" + pathUrl("MailUrl") + "enVersion/VeryflyEn.aspx?en=" + dt.Rows[i]["sender_querystring"].ToString() + "' target='_blank'>" + pathUrl("MailUrl") + "enVersion/VeryflyEn.aspx?en=" + dt.Rows[i]["sender_querystring"].ToString() + "</a><br />");//pathUrl("MailUrl") + "Veryfly.aspx?en=" + dt.Rows[i]["sender_querystring"].ToString(), pathUrl("MailUrl") + "Veryfly.aspx?en=" + dt.Rows[i]["sender_querystring"].ToString()
                                sbBody.Append(@"====================================================================<br />");
                                sbBody.Append(@"This email may contain confidential information. Please do not use or disclose ");
                                sbBody.Append(@"it in any way and delete it if you are not the intended recipient.</div>");
                                sbBody.Append(@"</font></body></html>");
                                //sbBody.AppendFormat(@"");

                                myEmail.sendEmail(to, subject, sbBody.ToString(), numb.ToString(), returnSenderMail,ReturnSenderName);

                                if (file_comorsec == "security")
                                {
                                    UpdateImageText(numb.ToString(), dt.Rows[i]["sender_id"].ToString());
                                }

                            }
                        }
                        else
                        {
                            System.Console.WriteLine("Can't find a recipient");
                        }
                        oda.Dispose();
                        oConn.Close();
                        oConn.Dispose();
                        oCmd.Dispose();
                        break;
                    }

                default:
                    {
                        Encrypt_Class nyEn = new Encrypt_Class();
                        Email myEmail = new Email();
                        //string subject = "[ITRI] 通知：您有來自工研院大檔案傳輸的信件";
                        string to = "";

                        StringBuilder sb = new StringBuilder();
                        SqlConnection oConn = new SqlConnection(pathUrl("DSN"));
                        SqlCommand oCmd = new SqlCommand();
                        oCmd.Connection = oConn;
                        sb.Append(@"SELECT * FROM sfts_sender WHERE sender_parentid = @sender_parentid");
                        oCmd.CommandText = sb.ToString();
                        oCmd.Parameters.AddWithValue("@sender_parentid", file_parentid);
                        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
                        DataTable dt = new DataTable();
                        oda.Fill(dt);

                        DataTable dtMain = ReturnMain(file_parentid);
                        string subject = "[ITRI通知]：" + dtMain.Rows[0]["main_title"].ToString();
                        string ReturnSenderName = "";
                        string returnSenderMail = "";

                        if (dtMain.Rows[0]["main_isempno"].ToString().ToLower() == "true")//代表寄件者是院內員工 需要去common抓名字
                        {
                            ReturnSenderName = IsEmpnoTrue(dtMain.Rows[0]["main_infno"].ToString());
                            returnSenderMail = IsEmpnoTrueEmail(dtMain.Rows[0]["main_infno"].ToString());
                        }
                        else
                        {
                            ReturnSenderName = IsEmpnoFalse(dtMain.Rows[0]["main_infno"].ToString());
                            returnSenderMail = IsEmpnoFalseEmail(dtMain.Rows[0]["main_infno"].ToString());
                        }

                        DataTable dtFile = ReturnFileList(file_parentid);
                        string fileList = "";
                        for (int i = 0; i < dtFile.Rows.Count; i++)
                        {
                            fileList += dtFile.Rows[i]["fileNameShow"].ToString() + dtFile.Rows[i]["afile_exten"].ToString() + "<br />";
                        }

                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                Random ram = new Random();
                                int numb = ram.Next(9999);

                                to = dt.Rows[i]["sender_mail"].ToString().Trim();
                                StringBuilder sbBody = new StringBuilder();
                                sbBody.Append(@"<html><head><title></title><style type='text/css'>DIV.PlainText {FONT-FAMILY: monospace; FONT-SIZE: 120%}</style></head>");
                                sbBody.Append(@"<body><font size='2'>");
                                sbBody.Append(@"<div class='PlainText'>");
                                sbBody.Append(@"親愛的朋友 您好：<br />");
                                sbBody.Append(@"<br />");
                                sbBody.AppendFormat(@"{0} &lt;{2}&gt; 先生/小姐，在 {1}寄送下列的檔案給您。", ReturnSenderName, DateTime.Now.ToString("yyyy/MM/dd"), returnSenderMail);
                                sbBody.Append(@"請您利用下面的網址來下載檔案，謝謝您！<br /><br />※ 所有的檔案將在 7 天後刪除※");
                                sbBody.Append(@"<br /><br />訊息留言：<br />");
                                sbBody.AppendFormat(@"{0}", dtMain.Rows[0]["main_desc"].ToString());
                                sbBody.Append(@"<br />");
                                sbBody.Append(@"檔案列表：<br />");
                                sbBody.Append(@"" + fileList + "");

                                if (file_comorsec == "security")
                                {
                                    sbBody.Append(@"取檔認證碼：<br />如為密件檔案，請在下載時輸入圖片內之數字，即會再寄一封解壓縮密碼給您。<br />");
                                    sbBody.Append(@"<img alt='認證碼' src='cid:attech01.jpg' /><br />");
                                }
                                sbBody.Append(@"取檔網址：<br />");
                                sbBody.Append(@"※ 1. 如果有「安全性警示」的視窗跳出，請按「是」接受以繼續接收檔案<br />");
                                sbBody.Append(@"※ 2. 此加密取檔網址為您所獨有，若需要轉寄信件時，請務必留意：系統將所有透過此網址取檔者均視同已取得您本人同意。<br />");
                                sbBody.Append(@"<a href='" + pathUrl("MailUrl") + "Veryfly.aspx?en=" + dt.Rows[i]["sender_querystring"].ToString() + "' target='_blank'>" + pathUrl("MailUrl") + "Veryfly.aspx?en=" + dt.Rows[i]["sender_querystring"].ToString() + "</a><br />");//pathUrl("MailUrl") + "Veryfly.aspx?en=" + dt.Rows[i]["sender_querystring"].ToString(), pathUrl("MailUrl") + "Veryfly.aspx?en=" + dt.Rows[i]["sender_querystring"].ToString()
                                sbBody.Append(@"====================================================================<br />");
                                sbBody.Append(@"本信件可能包含工研院機密資訊，非指定之收件者，請勿使用或揭露本信件內容，並請銷毀此信件。<br />");
                                sbBody.Append(@"This email may contain confidential information. Please do not use or disclose ");
                                sbBody.Append(@"it in any way and delete it if you are not the intended recipient.</div>");
                                sbBody.Append(@"</font></body></html>");
                                //sbBody.AppendFormat(@"");

                                myEmail.sendEmail(to, subject, sbBody.ToString(), numb.ToString(), returnSenderMail,ReturnSenderName);

                                if (file_comorsec == "security")
                                {
                                    UpdateImageText(numb.ToString(), dt.Rows[i]["sender_id"].ToString());
                                }

                            }
                        }
                        else
                        {
                            System.Console.WriteLine("找不到收件者");
                        }
                        oda.Dispose();
                        oConn.Close();
                        oConn.Dispose();
                        oCmd.Dispose();
                        break;
                    }
            
            }
        }

        #region 公用函式

        #region 更新sfts_file
        public void Update_sfts_file(string file_parentid, string file_comorsec,string type)
        {
            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(pathUrl("DSN"));
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            sb.Append(@"UPDATE sfts_file SET file_type = @type WHERE file_parentid = @file_parentid AND file_comorsec = @file_comorsec ");
            oCmd.Parameters.Add("@file_parentid", SqlDbType.NVarChar).Value = file_parentid;
            oCmd.Parameters.Add("@file_comorsec", SqlDbType.NVarChar).Value = file_comorsec;
            oCmd.Parameters.Add("@type", SqlDbType.NVarChar).Value = type;
            oCmd.CommandText = sb.ToString();
            try
            {
                oCmd.Connection.Open();
                oCmd.ExecuteNonQuery();
                oCmd.Connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (oCmd.Connection.State != ConnectionState.Closed)
                    oCmd.Connection.Close();
            }

        }

        public void Update_sfts_fileOnGoing(string file_id, string type)
        {
            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(pathUrl("DSN"));
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            sb.Append(@"UPDATE sfts_file SET file_type = @type,file_consoledate=GETDATE() WHERE file_id = @file_id ");
            oCmd.Parameters.Add("@file_id", SqlDbType.NVarChar).Value = file_id;
            oCmd.Parameters.Add("@type", SqlDbType.NVarChar).Value = type;
            oCmd.CommandText = sb.ToString();
            try
            {
                oCmd.Connection.Open();
                oCmd.ExecuteNonQuery();
                oCmd.Connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (oCmd.Connection.State != ConnectionState.Closed)
                    oCmd.Connection.Close();
            }

        }
        #endregion

        #region 新增sfts_afile
        public void INSERT_sfts_afile(string file_parentid, string file_comorsec, string file_encrypt, string file_origiFileName, string file_encryptfileName,int file_size, string file_stat,string exten)
        {
            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(pathUrl("DSN"));
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            sb.Append(@"INSERT INTO sfts_afile (afile_parentid,afile_comorsec,afile_encrypt,afile_origiFileName,afile_encryptfileName,afile_size,afile_exten,afile_createdate,afile_stat) ");
            sb.Append(@" VALUES (@file_parentid,@file_comorsec,@file_encrypt,@file_origiFileName,@file_encryptfileName,@file_size,@file_exten,GETDATE(),@file_stat) ");

            oCmd.Parameters.Add("@file_parentid", SqlDbType.NVarChar).Value = file_parentid;
            oCmd.Parameters.Add("@file_comorsec", SqlDbType.NVarChar).Value = file_comorsec;
            oCmd.Parameters.Add("@file_encrypt", SqlDbType.NVarChar).Value = file_encrypt;
            oCmd.Parameters.Add("@file_origiFileName", SqlDbType.NVarChar).Value = file_origiFileName;
            oCmd.Parameters.Add("@file_encryptfileName", SqlDbType.NVarChar).Value = file_encryptfileName;
            oCmd.Parameters.Add("@file_size", SqlDbType.NVarChar).Value = file_size;
            oCmd.Parameters.Add("@file_exten", SqlDbType.NVarChar).Value = exten;
            oCmd.Parameters.Add("@file_stat", SqlDbType.NVarChar).Value = file_stat;
            oCmd.CommandText = sb.ToString();
            try
            {
                oCmd.Connection.Open();
                oCmd.ExecuteNonQuery();
                oCmd.Connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (oCmd.Connection.State != ConnectionState.Closed)
                    oCmd.Connection.Close();
            }

        }
        #endregion

        #region 根據main內的資訊做出雜湊值當作解壓縮密碼
        public string GenAESCodePass(string main_parentid)
        {
            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(pathUrl("DSN"));
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            sb.Append(@"SELECT * FROM sfts_main WHERE main_parentid = @main_parentid ");
            oCmd.CommandText = sb.ToString();
            oCmd.Parameters.AddWithValue("@main_parentid", main_parentid);
            SqlDataAdapter oda = new SqlDataAdapter(oCmd);
            DataTable dt = new DataTable();
            oda.Fill(dt);

            string ReturnValue = dt.Rows[0]["main_title"].ToString().Trim() +
                dt.Rows[0]["main_desc"].ToString().Trim() + DateTime.Now.ToString("yyyy/MM/dd") +
                dt.Rows[0]["main_infno"].ToString().Trim();//寄件者信箱不好抓 用寄件者帳號取代

            oda.Dispose();
            oConn.Close();
            oConn.Dispose();
            oCmd.Dispose();

            return sha1en(ReturnValue);
        }
        #endregion

        #region 雜湊
        public static string sha1en(string unCodeString)
        {
            string enCodeString;
            SHA1CryptoServiceProvider sha1en = new SHA1CryptoServiceProvider();
            enCodeString = BitConverter.ToString(sha1en.ComputeHash(UTF8Encoding.Default.GetBytes(unCodeString)), 4, 8);
            enCodeString = enCodeString.Replace("-", "");
            return enCodeString;
        }
        #endregion

        #region 把認證碼存回資料庫
        public void UpdateImageText(string ramd, string sender_id)
        {
            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(pathUrl("DSN"));
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            sb.Append(@"UPDATE sfts_sender SET sender_imagetext = @sender_imagetext WHERE sender_id = @sender_id ");
            oCmd.Parameters.Add("@sender_imagetext", SqlDbType.NVarChar).Value = ramd;
            oCmd.Parameters.Add("@sender_id", SqlDbType.NVarChar).Value = sender_id;
            oCmd.CommandText = sb.ToString();
            try
            {
                oCmd.Connection.Open();
                oCmd.ExecuteNonQuery();
                oCmd.Connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (oCmd.Connection.State != ConnectionState.Closed)
                    oCmd.Connection.Close();
            }

        }
        #endregion

        public DataTable ReturnMain(string main_parentid)
        {

            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(pathUrl("DSN"));
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            sb.Append(@"SELECT * FROM sfts_main WHERE main_parentid = @main_parentid");
            oCmd.CommandText = sb.ToString();
            oCmd.Parameters.AddWithValue("@main_parentid", main_parentid);
            SqlDataAdapter oda = new SqlDataAdapter(oCmd);
            DataTable dt = new DataTable();
            oda.Fill(dt);

            oda.Dispose();
            oConn.Close();
            oConn.Dispose();
            oCmd.Dispose();
            return dt;
        }

        public DataTable ReturnFileList(string afile_parentid)
        {

            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(pathUrl("DSN"));
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            sb.Append(@"SELECT *,case RTRIM(LTRIM(afile_origiFileName)) when '' then afile_encryptfileName ELSE RTRIM(LTRIM(afile_origiFileName)) END AS fileNameShow FROM sfts_afile WHERE afile_parentid = @afile_parentid AND afile_encrypt<>'N' ");
            oCmd.CommandText = sb.ToString();
            oCmd.Parameters.AddWithValue("@afile_parentid", afile_parentid);
            SqlDataAdapter oda = new SqlDataAdapter(oCmd);
            DataTable dt = new DataTable();
            oda.Fill(dt);

            oda.Dispose();
            oConn.Close();
            oConn.Dispose();
            oCmd.Dispose();
            return dt;
        }


        public string IsEmpnoTrue(string com_empno)
        {
            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(pathUrl("Common"));
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            sb.Append(@"SELECT com_empno,com_cname FROM sfts_com WHERE com_empno = @com_empno");
            oCmd.CommandText = sb.ToString();
            oCmd.Parameters.AddWithValue("@com_empno", com_empno);
            SqlDataAdapter oda = new SqlDataAdapter(oCmd);
            DataTable dt = new DataTable();
            oda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                oda.Dispose();
                oConn.Close();
                oConn.Dispose();
                oCmd.Dispose();
                return dt.Rows[0]["com_cname"].ToString().Trim();
            }
            else
            {
                return "";
                //System.Console.WriteLine("抓不到common內的員工資料");
                //System.Console.ReadKey();//有錯誤會HOLD住
            }
        }


        public string IsEmpnoFalse(string mem_account)
        {
            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(pathUrl("DSN"));
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            sb.Append(@"SELECT mem_account,mem_title FROM sfts_member WHERE mem_account = @mem_account");
            oCmd.CommandText = sb.ToString();
            oCmd.Parameters.AddWithValue("@mem_account", mem_account);
            SqlDataAdapter oda = new SqlDataAdapter(oCmd);
            DataTable dt = new DataTable();
            oda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                oda.Dispose();
                oConn.Close();
                oConn.Dispose();
                oCmd.Dispose();
                return dt.Rows[0]["mem_title"].ToString().Trim();
            }
            else
            {
                return "";
                //System.Console.WriteLine("抓不到member內的員工資料");
                //System.Console.ReadKey();//有錯誤會HOLD住
            }
        }


        public string IsEmpnoTrueEmail(string com_empno)
        {
            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(pathUrl("Common"));
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            sb.Append(@"SELECT com_mailadd FROM sfts_com WHERE com_empno = @com_empno");
            oCmd.CommandText = sb.ToString();
            oCmd.Parameters.AddWithValue("@com_empno", com_empno);
            SqlDataAdapter oda = new SqlDataAdapter(oCmd);
            DataTable dt = new DataTable();
            oda.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                oda.Dispose();
                oConn.Close();
                oConn.Dispose();
                oCmd.Dispose();
                return dt.Rows[0]["com_mailadd"].ToString().Trim();
            }
            else
            {
                return "";
                //System.Console.WriteLine("抓不到common內的員工資料");
                //System.Console.ReadKey();//有錯誤會HOLD住
            }
        }


        public string IsEmpnoFalseEmail(string mem_account)
        {
            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(pathUrl("DSN"));
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            sb.Append(@"SELECT mem_account FROM sfts_member WHERE mem_account = @mem_account");
            oCmd.CommandText = sb.ToString();
            oCmd.Parameters.AddWithValue("@mem_account", mem_account);
            SqlDataAdapter oda = new SqlDataAdapter(oCmd);
            DataTable dt = new DataTable();
            oda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                oda.Dispose();
                oConn.Close();
                oConn.Dispose();
                oCmd.Dispose();
                return dt.Rows[0]["mem_account"].ToString().Trim();
            }
            else
            {
                return "";
                //System.Console.WriteLine("抓不到member內的員工資料");
                //System.Console.ReadKey();//有錯誤會HOLD住
            }
        }

        #endregion
    }
}
