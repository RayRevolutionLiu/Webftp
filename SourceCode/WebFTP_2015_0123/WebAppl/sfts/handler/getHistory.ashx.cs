using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data;
using System.Collections;


namespace WebAppl.handler
{
    /// <summary>
    ///getHistory 的摘要描述
    /// </summary>
    public class getHistory : IHttpHandler, IRequiresSessionState
    {
        public class TooL
        {
            public string key { get; set; }
            public string title { get; set; }
            public bool isFolder { get; set; }
            public bool noLink { get; set; }
            public bool isLazy { get; set; }
            public List<Children> children { get; set; }
        }

        public class Children
        {
            public string title { get; set; }
            public string key { get; set; }
            public string isempno { get; set; }
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string FromYear = context.Request.Form["FromYear"].ToString().Trim();
                string FromMonth = context.Request.Form["FromMonth"].ToString().Trim();
                string ToYear = context.Request.Form["ToYear"].ToString().Trim();
                string ToMonth = context.Request.Form["ToMonth"].ToString().Trim();
                string languageType = context.Request.Form["languageType"] == null ? "" : context.Request.Form["languageType"].ToString().Trim();

                string main_infno = sAccount.GetAccInfo().Account.ToString().Trim();
                string main_isempno = sAccount.GetAccInfo().Com_Isempno.ToString();
                string ans = string.Empty;
                List<TooL> eList = new List<TooL>();

                getHistory_DB myget = new getHistory_DB();
                DataTable dt = myget.getHistoryList(main_infno, main_isempno, FromYear, FromMonth, ToYear, ToMonth);


                string createdate = string.Empty;
                TooL e = new TooL();
                List<Children> eChild = new List<Children>();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["main_createdate"].ToString().Trim() == createdate)
                        {
                            Children ex = new Children();
                            ex.key = dt.Rows[i]["sender_mail"].ToString().Trim();
                            ex.title = dt.Rows[i]["sender_mail"].ToString().Trim();
                            ex.isempno = dt.Rows[i]["sender_isempno"].ToString().Trim();
                            eChild.Add(ex);

                            if (i == dt.Rows.Count - 1)//最後一筆
                            {
                                e.children = eChild;
                                eList.Add(e);
                            }
                        }
                        else
                        {
                            if (i > 0)
                            {
                                e.children = eChild;
                                eList.Add(e);

                                e = new TooL();
                                eChild = new List<Children>();

                                e.key = "replaceStr";
                                if (languageType.ToString().Trim() != "")
                                {
                                    e.title = Convert.ToDateTime(dt.Rows[i]["main_createdate"].ToString().Trim()).GetDateTimeFormats('r')[0].ToString();
                                }
                                else
                                {
                                    e.title = Convert.ToDateTime(dt.Rows[i]["main_createdate"].ToString().Trim()).GetDateTimeFormats('f')[0].ToString();
                                }
                                e.isFolder = true;
                                e.isLazy = true;
                                e.noLink = false;

                                Children ex = new Children();
                                ex.key = dt.Rows[i]["sender_mail"].ToString().Trim();
                                ex.title = dt.Rows[i]["sender_mail"].ToString().Trim();
                                ex.isempno = dt.Rows[i]["sender_isempno"].ToString().Trim();
                                eChild.Add(ex);
                                if (i == dt.Rows.Count - 1)//最後一筆
                                {
                                    e.children = eChild;
                                    eList.Add(e);
                                }
                            }
                            else
                            {
                                e.key = "replaceStr";
                                if (languageType.ToString().Trim() != "")
                                {
                                    e.title = Convert.ToDateTime(dt.Rows[i]["main_createdate"].ToString().Trim()).GetDateTimeFormats('r')[0].ToString();
                                }
                                else
                                {
                                    e.title = Convert.ToDateTime(dt.Rows[i]["main_createdate"].ToString().Trim()).GetDateTimeFormats('f')[0].ToString();
                                }
                                e.isFolder = true;
                                e.isLazy = true;
                                e.noLink = false;
                                //因為不等於 所以是新的GROUP

                                Children ex = new Children();
                                ex.key = dt.Rows[i]["sender_mail"].ToString().Trim();
                                ex.title = dt.Rows[i]["sender_mail"].ToString().Trim();
                                ex.isempno = dt.Rows[i]["sender_isempno"].ToString().Trim();
                                eChild.Add(ex);

                                if (dt.Rows.Count == 1)
                                {
                                    e.children = eChild;
                                    eList.Add(e);
                                }
                            }

                        }
                        createdate = dt.Rows[i]["main_createdate"].ToString().Trim();
                    }

                    System.Web.Script.Serialization.JavaScriptSerializer objSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    ans = objSerializer.Serialize(eList);  //new
                }
                else
                {
                    if (languageType.ToString().Trim() != "")
                    {
                        ans = "[ { \"title\": \"No Records\", \"isFolder\": false, \"key\": \"replaceStr\", \"noLink\": true, \"hideCheckbox\": true} ]";
                    }
                    else
                    {
                        ans = "[ { \"title\": \"查無紀錄\", \"isFolder\": false, \"key\": \"replaceStr\", \"noLink\": true, \"hideCheckbox\": true} ]";
                    }
                    
                }

                context.Response.ContentType = "application/json";
                context.Response.Write(ans);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //public void GenJsonTool(string createdate, List<TooL> eList)
        //{
        //    TooL e = new TooL();
        //    e.key = "replaceStr";
        //    e.title = Convert.ToDateTime(createdate).GetDateTimeFormats('f')[0].ToString();
        //    e.isFolder = true;
        //    e.isLazy = true;
        //    e.noLink = false;

        //    List<Children> eChild = new List<Children>();

        //    for (int j = 0; j < mArray.Count; j++)
        //    {
        //        Children ex = new Children();
        //        ex.key = mArray[j].ToString();
        //        ex.title = mArray[j].ToString();
        //        ex.isempno = mArrayIsEmpno[j].ToString();
        //        eChild.Add(ex);
        //    }

        //    e.children = eChild;
        //    eList.Add(e);
        //    eChild = null;
        //}

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}