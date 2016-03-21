using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI;

namespace WebAppl.sfts.UserControl
{
    public partial class PagerEn : System.Web.UI.UserControl
    {
        GridView gv_data = new GridView();
        private string _Ctrl_GV = "";
        /// <summary>
        /// GridView控制項的ID名稱
        /// </summary>
        public string Ctrl_GV
        {
            get { return this._Ctrl_GV; }
            set { this._Ctrl_GV = value; }
        }


        public DataTable Dtdata
        {
            get { return (DataTable)ViewState["_Dtdata"]; }
            set { ViewState["_Dtdata"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            gv_data = (GridView)this.NamingContainer.FindControl(Ctrl_GV);
            gv_data.AllowPaging = true;
            //gv_data.PageSize = 10;
            if (!IsPostBack)
            {

            }
        }

        public void Bind()
        {
            gv_data = (GridView)this.NamingContainer.FindControl(Ctrl_GV);
            gv_data.DataSource = Dtdata;
            gv_data.DataBind();
            gv_data.AllowPaging = true;

            if (Dtdata.Rows.Count > 0)
            {
                PagerBind(Dtdata);//分頁連接
            }
            if (Dtdata.Rows.Count < 10)
            {
                table_pager.Visible = false;
            }
            else
            {
                table_pager.Visible = true;
            }
        }

        #region 分頁
        #region 首頁 最末頁 前進十頁等等按鈕
        protected void Query_Click(object sender, EventArgs e)
        {
            int intpageindex = 0;
            LinkButton lkbtn = (LinkButton)sender;
            switch (lkbtn.CommandArgument.ToString())
            {
                case "last":
                    intpageindex = gv_data.PageCount - 1;
                    break;
                case "first":
                    intpageindex = 0;
                    break;
                case "back10page":
                    {
                        if (gv_data.PageIndex >= 10)
                        {
                            intpageindex = gv_data.PageIndex - 10;
                        }
                        else
                        {
                            intpageindex = 0;
                        }
                        break;
                    }
                case "backpage":
                    {
                        if (gv_data.PageIndex > 0)
                        {
                            intpageindex = gv_data.PageIndex - 1;
                        }
                        break;
                    }
                case "Advance10page":
                    {
                        if (gv_data.PageIndex + 10 <= gv_data.PageCount - 1)
                        {
                            intpageindex = gv_data.PageIndex + 10;
                        }
                        else
                        {
                            intpageindex = gv_data.PageCount - 1;
                        }
                        break;
                    }
                case "Advancepage":
                    {
                        if (gv_data.PageIndex + 1 < gv_data.PageCount - 1)
                        {
                            intpageindex = gv_data.PageIndex + 1;
                        }
                        else
                        {
                            intpageindex = gv_data.PageCount - 1;
                        }
                        break;
                    }

            }
            gv_data.PageIndex = intpageindex;
            Bind();
        }
        #endregion

        #region 跳至頁數
        protected void lbtn_page_Click(object sender, EventArgs e)
        {
            //Repeater rpter = ((Repeater)gv_data1.BottomPagerRow.FindControl("rep_page"));
            //LinkButton lkbtn = ((LinkButton)rpter.FindControl("lbtn_page"));
            //JavaScript.AlertMessage(this.Page, lkbtn.CommandArgument.ToString());
            //Response.Write(lkbtn.CommandArgument.ToString());
            if (gv_data.Rows.Count > 0)
            {
                gv_data.PageIndex = Convert.ToInt32(((LinkButton)sender).CommandArgument) - 1;
                Bind();
                //PagerBind(SelectPersonReturnDataTable());
            }

        }
        #endregion

        #region 分頁下拉選單
        protected void ddl_gopage_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DropDownList ddl_gopage = ((DropDownList)gv_data.BottomPagerRow.FindControl("ddl_gopage"));
            gv_data.PageIndex = Convert.ToInt32(ddl_gopage.SelectedValue.ToString()) - 1;
            Bind();
        }
        #endregion

        #region 分頁連接
        public void PagerBind(DataTable dt)
        {
            //Label lbl_pagecount = (Label)gv_data.BottomPagerRow.FindControl("lbl_pagecount");
            //Label lbl_datacount = (Label)gv_data.BottomPagerRow.FindControl("lbl_datacount");

            lbl_pagecount.Text = gv_data.PageCount.ToString();
            lbl_datacount.Text = dt.Rows.Count.ToString();

            int count = 0;

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows.Count % 10 == 0)
                {
                    count = dt.Rows.Count / 10;
                }
                else
                {
                    count = dt.Rows.Count / 10 + 1;

                }
            }
            //DropDownList ddlpage = (DropDownList)gv_data.BottomPagerRow.FindControl("ddl_gopage");
            DataTable dtPage = new DataTable();
            dtPage.Columns.Add("page");
            //產生清單頁碼數的DropDownList、DataTable
            ddl_gopage.Items.Clear();
            //if (count != 1)
            //{
            for (int i = 1; i < count + 1; i++)
            {
                ddl_gopage.Items.Add(new ListItem(i.ToString(), i.ToString()));
                DataRow myRow = dtPage.NewRow();
                myRow["page"] = i;
                dtPage.Rows.Add(myRow);
            }
            //}
            //else
            //{
            //    table_pager.Visible = false;
            //}

            ddl_gopage.SelectedIndex = gv_data.PageIndex;


            int LocalPageCount = Convert.ToInt32(gv_data.PageCount);
            int LocalPageIndex = Convert.ToInt32(gv_data.PageIndex);
            int ShowPageStart = 0;
            int ShowPageEnd = 0;

            //若頁碼已經是在10頁內的話，就不用判斷了
            if (LocalPageCount > 10)
            {
                if ((LocalPageIndex >= 0) && (LocalPageIndex <= 6))
                {
                    ShowPageStart = 1;
                }
                else
                {
                    ShowPageStart = LocalPageIndex - 5;
                }

                if ((ShowPageStart + 9) >= LocalPageCount) ShowPageEnd = LocalPageCount;
                else ShowPageEnd = ShowPageStart + 9;
                if (ShowPageEnd > LocalPageCount) ShowPageEnd = LocalPageCount;

                dtPage.DefaultView.RowFilter = string.Format("page >= {0} and page <= {1}", ShowPageStart, ShowPageEnd);
            }

            //產生頁碼清單Repeater
            //Repeater rep_page = (Repeater)gv_data.BottomPagerRow.FindControl("rep_page");
            rep_page.DataSource = dtPage;
            rep_page.DataBind();
        }
        #endregion

        #region 屬於目前的頁次的話，則不顯示底線，也將其功能關閉
        protected void rep_page_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemIndex < 0)
                return;

            LinkButton lbtn_page = (LinkButton)e.Item.FindControl("lbtn_page");
            //屬於目前的頁次的話，則不顯示底線，也將其功能關閉
            if (DataBinder.Eval(e.Item.DataItem, "page").ToString() == Convert.ToString(gv_data.PageIndex + 1))
            {
                lbtn_page.Attributes["style"] = "text-decoration: none;font-weight: 900;font-size:120%;";
                lbtn_page.Enabled = false;
            }
        }
        #endregion
        #endregion
    }
}