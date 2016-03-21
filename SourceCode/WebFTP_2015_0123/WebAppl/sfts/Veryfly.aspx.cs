using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Text;


namespace WebAppl
{
    public partial class Veryfly : System.Web.UI.Page
    {
        Veryfly_DB myVery = new Veryfly_DB();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["en"] == null)
                {
                    throw new Exception("參數錯誤");
                }

                string en = Request.QueryString["en"].ToString().Trim();
                if (encode.sqlInjection(en))
                {
                    throw new Exception("參數包含不合法字元");
                }

                if (!IsPostBack)
                {
                    string querystr = Request.QueryString["en"].ToString();
                    DataTable dt = myVery.CheckQueryExist(querystr);
                    DataView dvGv = dt.DefaultView;
                    DataView dvJudge = dt.Copy().DefaultView;//此DataView是用來跟上面比較筆數是否相同
                    //sender_queryenable 是用來判斷使用者此URL是否被刪除
                    //afile_stat是此檔案是否被啟用(可再寄件者後管理介面修改)
                    dvGv.RowFilter = " afile_encrypt<>'N' AND ISNULL(sender_queryenable,'') <> 'N' AND main_stat<>'N'";
                    dvJudge.RowFilter = "afile_encrypt<>'N' AND ISNULL(sender_queryenable,'') <> 'N' AND afile_stat ='Y' AND main_stat<>'N'";

                    if (dvGv.Count == 0)
                    {
                        throw new Exception("連結已刪除或此連結已過七天");
                    }
                    else 
                    {
                        //2013/06/03 若該封郵件為[檔案加密]郵件，強迫需要收件者登入才能存取
                        if (dvGv[0]["main_secret"].ToString().Trim() == "security")
                        {
                            if (Session["pwerRowData"] == null)
                            {
                                //判斷"收件者"是否有登入
                                JavaScript.AlertMessageRedirect(this.Page, "此連結為加密檔案的URL，需登入方可取檔。", "Default.aspx?en=" + querystr);
                                //Response.Redirect("~/sfts/Default.aspx?en=" + querystr);
                                //return;
                            }
                            else
                            {
                                //就算登入了 還要判斷是否登入者就是此連結的收件人
                                //修改廠商登入帳號e-mail不分大小寫，一律用小寫比對 by 凱呈 20130905
                                if (sAccount.GetAccInfo().Email.ToLower() != dvGv[0]["sender_mail"].ToString().ToLower())                                
                                {
                                    throw new Exception("此不為您的連結，禁止登入");
                                }
                            }
                        }
                        DataTable dtName = Common.GetDetail(dvGv[0]["main_infno"].ToString(), dvGv[0]["main_isempno"].ToString());
                        lbName.Text = dtName.Rows[0]["cName"].ToString();
                        lbemail.Text = dtName.Rows[0]["cEmail"].ToString();

                        lbCreatedate.Text = dvGv[0]["main_createdateCon"].ToString();
                        lbdeldate.Text = dvGv[0]["main_deldate"].ToString();
                        lbTitle.Text = dvGv[0]["main_title"].ToString();
                        lbdesc.Text = dvGv[0]["main_desc"].ToString();

                        gvfilelist.DataSource = dvGv;
                        gvfilelist.DataBind();

                        if (dvGv[0]["afile_comorsec"].ToString().Trim() == "security")
                        {
                            gvfilelist.Columns[8].Visible = true;
                        }
                        else
                        {
                            gvfilelist.Columns[8].Visible = false;
                        }
                        //20150309 新增nonforward 屬性的檔案藥可以重寄認證碼
                        if (dvGv[0]["afile_comorsec"].ToString().Trim() == "nonforward")
                        {
                            btnReSendValidCode.Visible = true;
                        }
                        else
                        {
                            btnReSendValidCode.Visible = false;
                        }


                        //因為所有都會是同樣的性質(一般或密件 所以只要抓第一筆就夠了)
                        if (dvGv[0]["afile_comorsec"].ToString().Trim() == "common" || dvGv[0]["afile_comorsec"].ToString().Trim() == "nonforward")
                        {
                            if (dvGv.Count != dvJudge.Count)
                            {
                                //兩方數字不同 代表有些檔案被寄件者取消了 一般檔案就必須把"下載所有附件"隱藏
                                PaneldownLoadAll.Visible = false;
                            }
                            else
                            {
                                HtmlGenericControl btn = new HtmlGenericControl("input");
                                DataView dvN = dt.DefaultView;
                                dvN.RowFilter = " afile_encrypt='N' ";
                                if (dvN.Count == 1)
                                {
                                    btn.Attributes.Add("id", "btnDownLoadAll");
                                    btn.Attributes.Add("type", "button");
                                    btn.Attributes.Add("value", "下載所有檔案");
                                    btn.Attributes.Add("class", "btn_mouseout");
                                    btn.Attributes.Add("onmouseover", "this.className='btn_mouseover'");
                                    btn.Attributes.Add("onmouseout", "this.className='btn_mouseout'");
                                    btn.Attributes.Add("filename", dvN[0]["afile_encryptfileName"].ToString());
                                    btn.Attributes.Add("source", dvN[0]["afile_encryptfileName"].ToString());
                                    btn.Attributes.Add("exten", dvN[0]["afile_exten"].ToString());
                                    btn.Attributes.Add("aid", dvN[0]["sender_id"].ToString());
                                    btn.Attributes.Add("afileID", dvN[0]["afile_id"].ToString());
                                    btn.Attributes.Add("comorsec", dvN[0]["afile_comorsec"].ToString());
                                    //btn.Attributes.Add("notifyflag", dvN[0]["sender_notifyflag"].ToString());
                                }
                                PaneldownLoadAll.Controls.Add(btn);
                                PaneldownLoadAll.Visible = true;
                            }
                        }
                        else
                        {
                            PaneldownLoadAll.Visible = false;
                        }


                        if (dvGv[0]["sender_disabled"].ToString().Trim() == "Y")//Y表示禁止使用者刪除 所以要把PANEL隱藏
                        {
                            PanelDelUrl.Visible = false;
                            btnDelUrl.Visible = false;
                        }
                        else
                        {
                            PanelDelUrl.Visible = true;
                            btnDelUrl.Visible = true;
                        }
                    }
                }

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
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[5].Text = e.Row.Cells[5].Text + e.Row.Cells[2].Text;
                e.Row.Cells[6].Text = Convert.ToInt64(e.Row.Cells[6].Text) / 1024 + "KB";
                if (e.Row.Cells[4].Text.Trim() != "Y")
                {
                    ((Panel)e.Row.Cells[7].FindControl("PanelGetFile")).Visible = false;
                    ((Panel)e.Row.Cells[7].FindControl("PanelShowFileFail")).Visible = true;
                }
                else
                {
                    ((Panel)e.Row.Cells[7].FindControl("PanelGetFile")).Visible = true;
                    ((Panel)e.Row.Cells[7].FindControl("PanelShowFileFail")).Visible = false;
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

        protected void btnDelUrl_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["en"] == null)
            {
                throw new Exception("參數錯誤");
            }

            string en = Request.QueryString["en"].ToString().Trim();
            if (encode.sqlInjection(en))
            {
                throw new Exception("參數包含不合法字元");
            }

            string querystr = Request.QueryString["en"].ToString();
            DataTable dt = myVery.CheckQueryExist(querystr);
            DataView dvGv = dt.DefaultView;
            dvGv.RowFilter = " afile_encrypt<>'N' ";

            if (dvGv.Count == 0)
            {
                throw new Exception("檔案已遺失或已過七天刪除");
            }
            else
            {
                myVery.UpdateSender_QueryEnable(dvGv[0]["sender_id"].ToString());

                if (dvGv[0]["sender_notifyflag"].ToString().Trim() == "Y")//有勾要收件者下載檔案或刪除取檔網址時通知我。 
                {
                    Email myEmail = new Email();
                    //[ITRI] 通知：您透過大檔案傳輸寄出的取檔網址之一已被收件人刪除
                    DataTable dtAccordingParentidToFindMember = Common.AccordingParentidToFindMember(dvGv[0]["sender_parentid"].ToString());
                    DataTable dtGetDetail = Common.GetDetail(dtAccordingParentidToFindMember.Rows[0]["main_infno"].ToString(), dtAccordingParentidToFindMember.Rows[0]["main_isempno"].ToString());

                    StringBuilder sb = new StringBuilder();
                    sb.Append(@"親愛的朋友&nbsp;&nbsp;您好：<br /><br />" + dvGv[0]["sender_mail"].ToString() + "&nbsp;");
                    sb.Append(@"已將您在&nbsp;" + Convert.ToDateTime(dvGv[0]["afile_createdate"].ToString()).ToString("yyy/MM/dd") + "&nbsp;寄送過去的加密取檔網址刪除，任何人將無法由該取檔網址進行下載。");
                    sb.Append(@"<br />但您的檔案仍存在，若有其他收件人，仍可以由自己專屬的取檔網址下載檔案。<br /><br />");
                    sb.Append(@"  ※ 被刪除的取檔網址相關資料如下 ※<br /><br />");
                    sb.Append(@"訊息留言：<br /><br />");
                    sb.Append(@"" + dvGv[0]["main_desc"].ToString() + "<br /><br />");
                    sb.Append(@"檔案列表：<br /><br />");
                    for (int i = 0; i < dvGv.Count; i++)
                    {
                        sb.Append(@"" + dvGv[i]["ShowFileName"].ToString() + dvGv[i]["afile_exten"].ToString() + "<br />");
                    }
                    //sb.Append(@"");
                    myEmail.sendEmail(dtGetDetail.Rows[0]["cEmail"].ToString(), "[ITRI] 通知：您透過大檔案傳輸寄出的取檔網址之一已被收件人刪除", sb.ToString(), "");
                }

                JavaScript.AlertMessageRedirect(this.Page, "連結已刪除", "Veryfly.aspx?en=" + querystr);
            }
        }

        protected void btnReSendValidCode_Click(object sender, EventArgs e)
        {
            string querystr = Request.QueryString["en"].ToString();
            DataTable dt = myVery.CheckQueryExist(querystr);

            if (dt.Rows[0]["sender_passValiddate"].ToString().Trim() == "")
            {
                //表示從沒寄過認證信
                JavaScript.AlertMessage(this.Page, "您尚未取檔");
                return;
            }
            else
            {
                Email myEmail = new Email();
                StringBuilder sbContent = new StringBuilder();

                Random ram = new Random();
                int numb = ram.Next(9999);

                myVery.UpdateImageText(numb.ToString(), dt.Rows[0]["sender_id"].ToString());

                sbContent.Append(@"<html><head><title></title><style type='text/css'>DIV.PlainText {FONT-FAMILY: monospace; FONT-SIZE: 120%}</style></head>");
                sbContent.Append(@"<body><font size='2'>");
                sbContent.Append(@"<div class='PlainText'>");
                sbContent.Append(@"親愛的朋友 您好：<br />");
                sbContent.Append(@"<br />");
                sbContent.Append(@"取檔驗證碼如下圖所示：");
                sbContent.Append(@"<img alt='認證碼' src='cid:attech01.jpg' /><br />");
                sbContent.Append(@"====================================================================<br />");
                sbContent.Append(@"本信件可能包含工研院機密資訊，非指定之收件者，請勿使用或揭露本信件內容，並請銷毀此信件。<br />");
                sbContent.Append(@"This email may contain confidential information. Please do not use or disclose ");
                sbContent.Append(@"it in any way and delete it if you are not the intended recipient.</div>");
                sbContent.Append(@"</font></body></html>");
                myEmail.sendEmail(dt.Rows[0]["sender_mail"].ToString(), "[ITRI通知]：驗證碼寄送通知", sbContent.ToString(), numb.ToString(), "");

                JavaScript.AlertMessage(this.Page, "認證碼已寄送至您的信箱");
            }

            #region 因為POSTBACK之後 下載所有檔案這個按鈕會消失 所以在此再註冊一次

            DataView dvGv = dt.DefaultView;
            DataView dvJudge = dt.Copy().DefaultView;//此DataView是用來跟上面比較筆數是否相同
            //sender_queryenable 是用來判斷使用者此URL是否被刪除
            //afile_stat是此檔案是否被啟用(可再寄件者後管理介面修改)
            dvGv.RowFilter = " afile_encrypt<>'N' AND ISNULL(sender_queryenable,'') <> 'N' AND main_stat<>'N'";
            dvJudge.RowFilter = "afile_encrypt<>'N' AND ISNULL(sender_queryenable,'') <> 'N' AND afile_stat ='Y' AND main_stat<>'N'";

            //因為所有都會是同樣的性質(一般或密件 所以只要抓第一筆就夠了)
            if (dvGv[0]["afile_comorsec"].ToString().Trim() == "common" || dvGv[0]["afile_comorsec"].ToString().Trim() == "nonforward")
            {
                if (dvGv.Count != dvJudge.Count)
                {
                    //兩方數字不同 代表有些檔案被寄件者取消了 一般檔案就必須把"下載所有附件"隱藏
                    PaneldownLoadAll.Visible = false;
                }
                else
                {
                    HtmlGenericControl btn = new HtmlGenericControl("input");
                    DataView dvN = dt.DefaultView;
                    dvN.RowFilter = " afile_encrypt='N' ";
                    if (dvN.Count == 1)
                    {
                        btn.Attributes.Add("id", "btnDownLoadAll");
                        btn.Attributes.Add("type", "button");
                        btn.Attributes.Add("value", "下載所有檔案");
                        btn.Attributes.Add("class", "btn_mouseout");
                        btn.Attributes.Add("onmouseover", "this.className='btn_mouseover'");
                        btn.Attributes.Add("onmouseout", "this.className='btn_mouseout'");
                        btn.Attributes.Add("filename", dvN[0]["afile_encryptfileName"].ToString());
                        btn.Attributes.Add("source", dvN[0]["afile_encryptfileName"].ToString());
                        btn.Attributes.Add("exten", dvN[0]["afile_exten"].ToString());
                        btn.Attributes.Add("aid", dvN[0]["sender_id"].ToString());
                        btn.Attributes.Add("afileID", dvN[0]["afile_id"].ToString());
                        btn.Attributes.Add("comorsec", dvN[0]["afile_comorsec"].ToString());
                        //btn.Attributes.Add("notifyflag", dvN[0]["sender_notifyflag"].ToString());
                    }
                    PaneldownLoadAll.Controls.Add(btn);
                    PaneldownLoadAll.Visible = true;
                }
            }
            else
            {
                PaneldownLoadAll.Visible = false;
            }
            #endregion

        }

    }
}