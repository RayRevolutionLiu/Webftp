using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

namespace WebAppl.sfts.enVersion
{
    public partial class page_mantainallEn : System.Web.UI.Page
    {
        page_mantain_DB mypage = new page_mantain_DB();

        protected void Page_Load(object sender, EventArgs e)
        {
            PagerEn1.Ctrl_GV = "gvmainlist";
            if (!IsPostBack)
            {
                DataTable dt = mypage.GetMainListAll(sAccount.GetAccInfo().Account.ToString().Trim(), sAccount.GetAccInfo().Com_Isempno.ToString().Trim(), tbxkeyword.Text.Trim(), tbxstartdate.Text.Trim(), tbxenddate.Text.Trim());
                PagerEn1.Dtdata = dt;
                PagerEn1.Bind();
                //gvmainlist.DataSource = dt;
                //gvmainlist.DataBind();
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

                page_mantain_DB mypage = new page_mantain_DB();
                //檔案列表清單
                Panel panelFile = (Panel)e.Row.Cells[7].FindControl("panelfile");
                DataTable dtFile = mypage.GetfileList(e.Row.Cells[2].Text);
                HtmlGenericControl span = new HtmlGenericControl("span");
                span.Attributes.Add("class", "font-size2 underlineClass");
                HtmlGenericControl table = new HtmlGenericControl("table");
                table.Attributes.Add("width", "100%");
                table.Attributes.Add("cellpadding", "0");
                table.Attributes.Add("cellspacing", "0");
                table.Attributes.Add("class", "OpenUpTable");

                if (dtFile.Rows.Count > 0)
                {
                    for (int i = 0; i < dtFile.Rows.Count; i++)
                    {
                        HtmlGenericControl tr = new HtmlGenericControl("tr");
                        HtmlGenericControl td = new HtmlGenericControl("td");
                        td.Attributes.Add("width", "20px");
                        td.Attributes.Add("style", "font-weight:bold");
                        td.InnerText = (i + 1).ToString();
                        HtmlGenericControl td2 = new HtmlGenericControl("td");
                        td2.InnerText = dtFile.Rows[i]["ShowFileName"].ToString().Trim() + dtFile.Rows[i]["afile_exten"].ToString().Trim();
                        tr.Controls.Add(td);
                        tr.Controls.Add(td2);
                        table.Controls.Add(tr);
                    }
                }

                span.Controls.Add(table);
                panelFile.Controls.Add(span);


                //收件人清單
                Panel panelsender = (Panel)e.Row.Cells[7].FindControl("panelsender");
                DataTable dtsender = mypage.GetsenderList(e.Row.Cells[2].Text);
                HtmlGenericControl spansender = new HtmlGenericControl("span");
                spansender.Attributes.Add("class", "font-size2 underlineClass");
                HtmlGenericControl tablesender = new HtmlGenericControl("table");
                tablesender.Attributes.Add("width", "100%");
                tablesender.Attributes.Add("cellpadding", "0");
                tablesender.Attributes.Add("cellspacing", "0");
                tablesender.Attributes.Add("class", "OpenUpTable");

                if (dtsender.Rows.Count > 0)
                {
                    for (int i = 0; i < dtsender.Rows.Count; i++)
                    {
                        HtmlGenericControl tr = new HtmlGenericControl("tr");
                        HtmlGenericControl td = new HtmlGenericControl("td");
                        td.Attributes.Add("width", "40%");
                        td.InnerText = dtsender.Rows[i]["sender_mail"].ToString().Trim();
                        HtmlGenericControl td2 = new HtmlGenericControl("td");
                        td2.Attributes.Add("width", "20%");
                        string hitcount = dtsender.Rows[i]["hitCount"].ToString().Trim() == "" ? "0" : dtsender.Rows[i]["hitCount"].ToString().Trim();
                        td2.InnerText = "Tried " + hitcount + " Times ";
                        HtmlGenericControl td3 = new HtmlGenericControl("td");
                        td3.Attributes.Add("width", "39%");
                        td3.InnerText = "Last time " + dtsender.Rows[i]["LastDownDate"].ToString().Trim();

                        tr.Controls.Add(td);
                        tr.Controls.Add(td2);
                        tr.Controls.Add(td3);
                        tablesender.Controls.Add(tr);
                    }
                }

                spansender.Controls.Add(tablesender);
                panelsender.Controls.Add(spansender);
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

        protected void Button1_Click(object sender, EventArgs e)
        {
            DataTable dt = mypage.GetMainListAll(sAccount.GetAccInfo().Account.ToString().Trim(), sAccount.GetAccInfo().Com_Isempno.ToString().Trim(), tbxkeyword.Text.Trim(), tbxstartdate.Text.Trim(), tbxenddate.Text.Trim());
            PagerEn1.Dtdata = dt;
            PagerEn1.Bind();
        }
    }
}