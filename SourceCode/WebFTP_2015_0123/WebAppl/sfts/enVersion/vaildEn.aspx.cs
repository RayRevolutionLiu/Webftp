using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace WebAppl.sfts.enVersion
{
    public partial class vaildEn : System.Web.UI.Page
    {
        security sec = new security();
        valid_DB myvaild = new valid_DB();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    string qureystr = Request.QueryString["vid"].ToString().Trim();
                    string strde = sec.decryptquerystring(qureystr);//用來判斷字串是否是符合規則的 否則會被catch

                    DataTable dt = myvaild.GETdataByQueryString(qureystr);

                    if (dt.Rows.Count > 0)
                    {
                        lbName.Text = dt.Rows[0]["mem_title"].ToString().Trim();
                        lbEmail.Text = dt.Rows[0]["mem_account"].ToString().Trim();
                        hiddenID.Value = dt.Rows[0]["mem_id"].ToString().Trim();
                    }
                    else
                    {
                        throw new Exception("Link fail ,please use the latest link to (Add/Modification) your Password");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}