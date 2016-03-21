using System;
using System.Net.Mail;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Mime;
using System.Drawing;
using System.Drawing.Imaging;

/// <summary>
/// Email 的摘要描述
/// </summary>
public class Email
{
	public Email()
	{
		//
		// TODO: 在此加入建構函式的程式碼
		//
	}

    public static string pathUrl(string appkey)
    {
        return System.Configuration.ConfigurationManager.AppSettings[appkey].ToString();
    }
    
    public void sendEmail(string to, string subject, string body,string path,string FromMail,string FromName)
    {
        try
        {
            string EmailAccount = pathUrl("EmailAccount");// 帳號不需要帶 AD Domain 的名稱
            string EmailPassword = pathUrl("EmailPassword");
            //string FromAddress = pathUrl("EmailFromAddress"); // 一定要正確, 不可以亂填
            SmtpClient smtp = new SmtpClient(pathUrl("EmailSmtpClient"));

            MailMessage msg = new MailMessage();
            //msg.From = new MailAddress(FromAddress, "");
            msg.From = new MailAddress(FromMail,FromName);
            if (body.Contains("cid:"))
            {
                AlternateView avHtml = AlternateView.CreateAlternateViewFromString(body, Encoding.UTF8, MediaTypeNames.Text.Html);
                //取得圖片物件
                System.Drawing.Image image = this.CreateCheckCodeImage(path);

                //20150123 小賴改寫信件遷入圖片寫法
                MemoryStream logo = new MemoryStream();
                image.Save(logo, ImageFormat.Jpeg);
                image.Dispose();
                logo.Position = 0;

                /* 20150123 凱呈修改密件傳送的密碼嵌入圖寫法
                Bitmap b = new Bitmap(image);
                ImageConverter ic = new ImageConverter();
                Byte[] ba = (Byte[])ic.ConvertTo(b, typeof(Byte[]));
                MemoryStream logo = new MemoryStream(ba);
                */


                avHtml.LinkedResources.Add(new LinkedResource(logo, MediaTypeNames.Image.Jpeg));
                avHtml.LinkedResources[0].ContentId = "attech01.jpg";
                avHtml.LinkedResources[0].TransferEncoding = TransferEncoding.Base64;
                msg.AlternateViews.Add(avHtml);
            }
            //TO
            msg.To.Add(new MailAddress(to));
            //string[] tos=to.Split(',');
            //for (int i = 0; i < tos.Length; i++)
            //{
            //    if (!string.IsNullOrEmpty(tos[i]))
            //    {
            //        msg.To.Add(new MailAddress(tos[i]));
            //    }
            //}

            //CC
            //string[] ccs = cc.Split(',');
            //for (int i = 0; i < ccs.Length; i++)
            //{
            //    if (!string.IsNullOrEmpty(ccs[i]))
            //    {
            //        msg.CC.Add(new MailAddress(ccs[i]));
            //    }
            //}

            //msg.Bcc.Add(pathUrl("DevEmail"));

            msg.SubjectEncoding = Encoding.UTF8;
            msg.BodyEncoding = Encoding.UTF8;
            //msg.Subject = "測試 System.Net.Mail 在 Exchange 下可不可以使用!";
            msg.Subject = subject;
            //msg.Body = "測試 System.Net.Mail 在 Exchange 下可不可以使用!<br/>測試 <font style=\'color:red;\'>System.Net.Mail 在 Exchange </font>下可不可以使用!<script>alert(\'aaaaaaaa\');</script><br/>測試 System.Net.Mail 在 Exchange 下可不可以使用!";
            msg.Body = body;
            msg.IsBodyHtml = true;
            msg.DeliveryNotificationOptions = DeliveryNotificationOptions.Never; // 請自行調整
            msg.ReplyToList.Add(FromMail);
            //Attachments
            //string[] AttachmentsNames = AttachmentsName.Split(',');
            //for (int i = 0; i < AttachmentsNames.Length; i++)
            //{
            //    if (!string.IsNullOrEmpty(AttachmentsNames[i]))
            //    {                   
            //        //若有任一個附件不存在,則寄發取消
            //        if (!System.IO.File.Exists(AppConfig.UploadPath + AttachmentsNames[i]))
            //        {
            //            throw new Exception(MessageUtil.Msg_NotFoundAttachment);
            //        }
            //        msg.Attachments.Add(new Attachment(AppConfig.UploadPath + AttachmentsNames[i]));
            //    }
            //}

            smtp.Credentials = new NetworkCredential(EmailAccount, EmailPassword); 
            smtp.Send(msg);
        }
        catch (Exception err) {
            throw new Exception(err.Message);
        }
    }

    #region 產生數字亂數
    private string GetRandomNumberString(int int_NumberLength)
    {
        System.Text.StringBuilder str_Number = new System.Text.StringBuilder();//字串儲存器
        Random rand = new Random(Guid.NewGuid().GetHashCode());//亂數物件

        for (int i = 1; i <= int_NumberLength; i++)
        {
            str_Number.Append(rand.Next(0, 10).ToString());//產生0~9的亂數
        }

        return str_Number.ToString();
    }
    #endregion

    #region 產生圖片
    private System.Drawing.Image CreateCheckCodeImage(string checkCode)
    {

        System.Drawing.Bitmap image = new System.Drawing.Bitmap((checkCode.Length * 20), 40);//產生圖片，寬20*位數，高40像素
        System.Drawing.Graphics g = Graphics.FromImage(image);


        //生成隨機生成器
        Random random = new Random(Guid.NewGuid().GetHashCode());
        int int_Red = 0;
        int int_Green = 0;
        int int_Blue = 0;
        int_Red = random.Next(256);//產生0~255
        int_Green = random.Next(256);//產生0~255
        int_Blue = (int_Red + int_Green > 400 ? 0 : 400 - int_Red - int_Green);
        int_Blue = (int_Blue > 255 ? 255 : int_Blue);

        //清空圖片背景色
        //g.Clear(Color.FromArgb(int_Red, int_Green, int_Blue));
        g.Clear(Color.FromKnownColor(KnownColor.Orange));
        //畫圖片的背景噪音線
        for (int i = 0; i <= 24; i++)
        {


            int x1 = random.Next(image.Width);
            int x2 = random.Next(image.Width);
            int y1 = random.Next(image.Height);
            int y2 = random.Next(image.Height);

            g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);

            g.DrawEllipse(new Pen(Color.DarkViolet), new System.Drawing.Rectangle(x1, y1, x2, y2));
        }

        Font font = new System.Drawing.Font("Arial", 20, (System.Drawing.FontStyle.Bold));
        System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2F, true);

        g.DrawString(checkCode, font, brush, 2, 2);
        for (int i = 0; i <= 99; i++)
        {

            //畫圖片的前景噪音點
            int x = random.Next(image.Width);
            int y = random.Next(image.Height);

            image.SetPixel(x, y, Color.FromArgb(random.Next()));
        }

        //畫圖片的邊框線
        g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);


        return image;

    }
    #endregion

}
