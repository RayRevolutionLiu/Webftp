using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace WebAppl.sfts.enVersion
{
    public partial class page_mantainEn : System.Web.UI.Page
    {
        page_mantain_DB mypage = new page_mantain_DB();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lbName.Text = sAccount.GetAccInfo().Title;
                DataTable dt = mypage.GetMainList(sAccount.GetAccInfo().Account.ToString().Trim(), sAccount.GetAccInfo().Com_Isempno.ToString().Trim());
                gvmainlist.DataSource = dt;
                gvmainlist.DataBind();
            }
        }

        protected void gvmainlist_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[4].Visible = false;

                Button btnDel = (Button)e.Row.Cells[8].FindControl("btnDel");
                Panel Panel1 = (Panel)e.Row.Cells[8].FindControl("Panel1");
                if (e.Row.Cells[4].Text.Trim() == "N")
                {
                    btnDel.Visible = false;
                    Panel1.Visible = true;
                }
                else
                {
                    btnDel.Visible = true;
                    Panel1.Visible = false;
                }
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[4].Visible = false;
            }
        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            GridViewRow thisRow = ((Button)sender).Parent.Parent as GridViewRow;  //此段取到該Row
            string id = thisRow.Cells[0].Text.Trim();
            mypage.DisableMain(id);
            Button btnDel = (Button)thisRow.Cells[8].FindControl("btnDel");
            Panel Panel1 = (Panel)thisRow.Cells[8].FindControl("Panel1");
            Panel1.Visible = true;
            btnDel.Visible = false;
            JavaScript.AlertMessage(this.Page, "Delete Success!");
            DataTable dt = mypage.GetMainList(sAccount.GetAccInfo().Account.ToString().Trim(), sAccount.GetAccInfo().Com_Isempno.ToString().Trim());
            gvmainlist.DataSource = dt;
            gvmainlist.DataBind();
        }
    }
}