using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Xml;

namespace WebAppl.WebServer
{
    /// <summary>
    /// WebService1 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://microsoft.com/webservices/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下一行。
    // [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {

        [WebMethod]
        public void INSERTitriToSfts(string xmlStr)
        {
            SqlConnection oConn = new SqlConnection(AppConfig.DSN);
            oConn.Open();
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            SqlTransaction myTrans = oConn.BeginTransaction();
            oCmd.Transaction = myTrans;
            try
            {
                oCmd.CommandText = "DELETE FROM sfts_com";
                oCmd.ExecuteNonQuery();

                oCmd.CommandText = "DELETE FROM sfts_dep";
                oCmd.ExecuteNonQuery();

                oCmd.CommandText = "DELETE FROM sfts_org";
                oCmd.ExecuteNonQuery();

                XmlDocument xmd = new XmlDocument();

                if (xmlStr.ToString().Trim() != "")
                {
                    xmd.LoadXml(xmlStr);
                    XmlNodeList recList = xmd.SelectNodes("//ex_current_account/rec");

                    oCmd.Parameters.Add("@com_orgcd", SqlDbType.NVarChar);
                    oCmd.Parameters.Add("@com_cname", SqlDbType.NVarChar);
                    oCmd.Parameters.Add("@com_empno", SqlDbType.NVarChar);
                    oCmd.Parameters.Add("@com_mailadd", SqlDbType.NVarChar);
                    oCmd.Parameters.Add("@com_deptcd", SqlDbType.NVarChar);
                    oCmd.Parameters.Add("@com_telext", SqlDbType.NVarChar);

                    for (int i = 0; i < recList.Count; i++)
                    {
                        string com_orgcd = recList[i].Attributes["com_orgcd"].Value;
                        string com_cname = recList[i].Attributes["DisplayName"].Value;
                        string com_empno = recList[i].Attributes["SamAccountName"].Value;
                        string com_mailadd = recList[i].Attributes["PrimarySmtpAddress"].Value;
                        string com_deptcd = recList[i].Attributes["com_deptcd"].Value;
                        string com_telext = recList[i].Attributes["com_telext"].Value;

                        oCmd.Parameters["@com_orgcd"].Value = com_orgcd;
                        oCmd.Parameters["@com_cname"].Value = com_cname;
                        oCmd.Parameters["@com_empno"].Value = com_empno;
                        oCmd.Parameters["@com_mailadd"].Value = com_mailadd;
                        oCmd.Parameters["@com_deptcd"].Value = com_deptcd;
                        oCmd.Parameters["@com_telext"].Value = com_telext;

                        oCmd.CommandText = "INSERT INTO sfts_com (com_orgcd,com_cname,com_empno,com_mailadd,com_deptcd,com_telext) VALUES (@com_orgcd,@com_cname,@com_empno,@com_mailadd,@com_deptcd,@com_telext) ";
                        oCmd.ExecuteNonQuery();
                    }


                    XmlNodeList recListorgcod = xmd.SelectNodes("//orgcod/rec");

                    oCmd.Parameters.Add("@org_orgcd", SqlDbType.NVarChar);
                    oCmd.Parameters.Add("@org_abbr_chnm1", SqlDbType.NVarChar);
                    oCmd.Parameters.Add("@org_abbr_egnm", SqlDbType.NVarChar);

                    for (int i = 0; i < recListorgcod.Count; i++)
                    {
                        string org_orgcd = recListorgcod[i].Attributes["org_orgcd"].Value;
                        string org_abbr_chnm1 = recListorgcod[i].Attributes["org_abbr_chnm1"].Value;
                        string org_abbr_egnm = recListorgcod[i].Attributes["org_abbr_egnm"].Value;

                        oCmd.Parameters["@org_orgcd"].Value = org_orgcd;
                        oCmd.Parameters["@org_abbr_chnm1"].Value = org_abbr_chnm1;
                        oCmd.Parameters["@org_abbr_egnm"].Value = org_abbr_egnm;

                        oCmd.CommandText = "INSERT INTO sfts_org (org_orgcd,org_abbr_chnm1,org_abbr_egnm) VALUES (@org_orgcd,@org_abbr_chnm1,@org_abbr_egnm) ";
                        oCmd.ExecuteNonQuery();
                    }


                    XmlNodeList recListdepcod = xmd.SelectNodes("//depcod/rec");

                    oCmd.Parameters.Add("@dep_orgcd", SqlDbType.NVarChar);
                    oCmd.Parameters.Add("@dep_deptcd", SqlDbType.NVarChar);
                    oCmd.Parameters.Add("@dep_abbrnm", SqlDbType.NVarChar);

                    for (int i = 0; i < recListdepcod.Count; i++)
                    {
                        string dep_orgcd = recListdepcod[i].Attributes["dep_orgcd"].Value;
                        string dep_deptcd = recListdepcod[i].Attributes["dep_deptcd"].Value;
                        string dep_abbrnm = recListdepcod[i].Attributes["dep_abbrnm"].Value;

                        oCmd.Parameters["@dep_orgcd"].Value = dep_orgcd;
                        oCmd.Parameters["@dep_deptcd"].Value = dep_deptcd;
                        oCmd.Parameters["@dep_abbrnm"].Value = dep_abbrnm;

                        oCmd.CommandText = "INSERT INTO sfts_dep (dep_orgcd,dep_deptcd,dep_abbrnm) VALUES (@dep_orgcd,@dep_deptcd,@dep_abbrnm) ";
                        oCmd.ExecuteNonQuery();
                    }

                    myTrans.Commit();
                }

            }
            catch (Exception ex)
            {
                myTrans.Rollback();
                Email myEmail = new Email();
                myEmail.sendEmail(AppConfig.DevEmail, "itriListError", ex.Message, "");
            }
            finally
            {
                oCmd.Connection.Close();
                oConn.Close();
            }
        }
    }
}
