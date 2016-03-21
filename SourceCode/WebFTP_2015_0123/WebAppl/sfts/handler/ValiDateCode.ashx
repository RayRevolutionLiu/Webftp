<%@ WebHandler Language="C#" Class="WebAppl.handler.ValiDateCode" %>

using System;
using System.Web;
using System.Drawing;
using System.Web.SessionState;

/* 此功能目前Web Form無用 僅保留用*/
public class ValiDateCode : IHttpHandler, IRequiresSessionState
{   
    public void ProcessRequest (HttpContext context) {
        try
        {
            security sec = new security();
            string str_ValidateCode = sec.decryptquerystring(context.Request.QueryString["num"].ToString()); //this.GetRandomNumberString(NumCount);
            //取得圖片物件
            System.Drawing.Image image = this.CreateCheckCodeImage(context, str_ValidateCode);
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            /*輸出圖片*/
            context.Response.Clear();
            context.Response.ContentType = "image/jpeg";
            context.Response.BinaryWrite(ms.ToArray());
            ms.Close();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
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
    private System.Drawing.Image CreateCheckCodeImage(HttpContext context, string checkCode)
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
        g.Clear(Color.FromArgb(int_Red, int_Green, int_Blue));

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

    /*實作 IHttpHandler介面的方法，不可刪除*/
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }


}
