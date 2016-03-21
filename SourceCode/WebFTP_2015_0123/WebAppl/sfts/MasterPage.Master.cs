using System;
using System.Web.UI.HtmlControls;

namespace WebAppl
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //string thePageFileNameWithExtention = Request.Url.Segments[Request.Url.Segments.Length - 1].ToString();  //透過Url的Segments，取得網址分析最後一個頁面的名稱
            //if (thePageFileNameWithExtention.ToLower().IndexOf("default") > -1)
            //{
            //    header1.Visible = false;
            //    footer1.Visible = false;
            //}
        }
    }
}