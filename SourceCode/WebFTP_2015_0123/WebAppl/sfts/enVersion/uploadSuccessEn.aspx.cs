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
    public partial class uploadSuccessEn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["pn"] == null)
                {
                    throw new Exception("QueryString Error");
                }

                string pn = Request.QueryString["pn"].ToString().Trim();
                if (encode.sqlInjection(pn))
                {
                    throw new Exception("validate QueryString Error");
                }

                Guid query = new Guid(pn);
                uploadSuccess_DB myupload = new uploadSuccess_DB();

                DataTable dtMain = myupload.GetSuccessResult("main", query.ToString());
                DataTable dtSender = myupload.GetSuccessResult("sender", query.ToString());
                DataTable dtFile = myupload.GetSuccessResult("file", query.ToString());

                lbName.Text = sAccount.GetAccInfo().Title;

                HtmlGenericControl ul = new HtmlGenericControl("ul");

                for (int i = 0; i < dtSender.Rows.Count; i++)
                {
                    HtmlGenericControl li = new HtmlGenericControl("li");
                    li.InnerText = dtSender.Rows[i]["sender_mail"].ToString();
                    ul.Controls.Add(li);
                }

                panelsender.Controls.Add(ul);

                lbdesc.Text = dtMain.Rows[0]["main_desc"].ToString().Trim();

                gvfilelist.DataSource = dtFile;
                gvfilelist.DataBind();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void gvfilelist_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Text = Convert.ToInt64(e.Row.Cells[1].Text) / 1024 + "KB";
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
            }
        }
    }
}