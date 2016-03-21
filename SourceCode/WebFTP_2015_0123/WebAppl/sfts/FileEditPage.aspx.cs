using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppl.sfts
{
    public partial class FileEditPage : System.Web.UI.Page
    {
        GenJson_DB myGen = new GenJson_DB();

        protected void Page_Load(object sender, EventArgs e)
        {
            lbName.Text = sAccount.GetAccInfo().Title;
            lbEmail.Text = sAccount.GetAccInfo().Email;
            tbxGenGuid.Text = Guid.NewGuid().ToString();

            if (sAccount.GetAccInfo().Com_Isempno)
            {
                hiddenUser.Visible = true;
            }
            else
            {
                hiddenUser.Visible = false;
            }
        }
    }
}