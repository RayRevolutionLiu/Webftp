using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppl
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lkEnVersion_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/sfts/enVersion/Default.aspx");
        }
    }
}