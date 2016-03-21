using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppl.sfts.enVersion.UserControl
{
    public partial class headerEn : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string thePageFileNameWithExtention = Request.Url.Segments[Request.Url.Segments.Length - 1].ToString();  //透過Url的Segments，取得網址分析最後一個頁面的名稱
            if (thePageFileNameWithExtention.ToLower().IndexOf("fileeditpage") > -1)
            {
                hyl1.Attributes.Add("class", "btncurrent");
                hyl2.Attributes.Add("class", "btn");
                hyl3.Attributes.Add("class", "btn");
                hyl4.Attributes.Add("class", "btn");
            }
            else if (thePageFileNameWithExtention.ToLower().IndexOf("page_mantainall") > -1)
            {
                hyl1.Attributes.Add("class", "btn");
                hyl2.Attributes.Add("class", "btn");
                hyl3.Attributes.Add("class", "btncurrent");
                hyl4.Attributes.Add("class", "btn");
            }
            else if (thePageFileNameWithExtention.ToLower().IndexOf("page_mantain") > -1)
            {
                hyl1.Attributes.Add("class", "btn");
                hyl2.Attributes.Add("class", "btncurrent");
                hyl3.Attributes.Add("class", "btn");
                hyl4.Attributes.Add("class", "btn");
            }
            else if (thePageFileNameWithExtention.ToLower().IndexOf("page_how") > -1)
            {
                hyl1.Attributes.Add("class", "btn");
                hyl2.Attributes.Add("class", "btn");
                hyl3.Attributes.Add("class", "btncurrent");
                hyl4.Attributes.Add("class", "btn");
            }
            else if (thePageFileNameWithExtention.ToLower().IndexOf("page_about") > -1)
            {
                hyl1.Attributes.Add("class", "btn");
                hyl2.Attributes.Add("class", "btn");
                hyl3.Attributes.Add("class", "btn");
                hyl4.Attributes.Add("class", "btncurrent");
            }
            else if (thePageFileNameWithExtention.ToLower().IndexOf("createacc") > -1)
            {
                hyl1.Visible = false;
                hyl2.Attributes.Add("class", "btn");
                hyl3.Attributes.Add("class", "btn");
                hyl4.Attributes.Add("class", "btn");
                hylCreate.Visible = true;
            }
            else if (thePageFileNameWithExtention.ToLower().IndexOf("valid") > -1)
            {
                hyl1.Visible = false;
                hyl2.Attributes.Add("class", "btn");
                hyl3.Attributes.Add("class", "btn");
                hyl4.Attributes.Add("class", "btn");
                hyValid.Visible = true;
            }
            else if (thePageFileNameWithExtention.ToLower().IndexOf("uploadsuccess") > -1)
            {
                hyl1.Attributes.Add("class", "btn");
                hyl2.Attributes.Add("class", "btn");
                hyl3.Attributes.Add("class", "btncurrent");
                hyl4.Attributes.Add("class", "btn");
            }
        }
    }
}