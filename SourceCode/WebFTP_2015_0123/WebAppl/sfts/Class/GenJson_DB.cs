using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace WebAppl
{
    public class GenJson_DB
    {
        public DataTable GenJsonDB(string orgcd,string deptcd)
        {
            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(AppConfig.Common);
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            try
            {
                if (orgcd.ToString().Trim() != "")//不等於空值,就代表是點擊單位之後下一偕
                {
                    if (deptcd.ToString().Trim() != "")//單位跟部門都有值,跑到人的選單
                    {
                        sb.Append(@"SELECT com_orgcd,com_empno,com_cname,com_mailadd FROM sfts_com WHERE com_orgcd=@com_orgcd AND com_deptcd=@com_deptcd ");
                        oCmd.Parameters.Add("@com_orgcd", SqlDbType.NVarChar).Value = orgcd.ToString();
                        oCmd.Parameters.Add("@com_deptcd", SqlDbType.NVarChar).Value = deptcd.ToString();
                    }
                    else
                    {
                        sb.Append(@"SELECT dep_orgcd,dep_deptcd,dep_abbrnm from sfts_dep where dep_orgcd=@dep_orgcd ");
                        oCmd.Parameters.Add("@dep_orgcd", SqlDbType.NVarChar).Value = orgcd.ToString();
                    }
                }
                else
                {

                    sb.Append(@"SELECT org_orgcd ,");
                    sb.Append(@"org_abbr_chnm1 ");
                    sb.Append(@" FROM sfts_org WHERE org_orgcd <> '00' ");
                }
                //oCmd.Parameters.Add("@actGuid", SqlDbType.UniqueIdentifier).Value = System.Guid.NewGuid();
                oCmd.CommandText = sb.ToString();
                SqlDataAdapter oda = new SqlDataAdapter(oCmd);
                DataTable ds = new DataTable();
                oda.Fill(ds);
                return ds;

            }
            catch (Exception err)
            {
                throw new Exception(MessageUtil.DB_SelectFail + err.HelpLink);
            }
        }


        public DataTable SearchAllEmpno(string keyword)
        {
            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(AppConfig.Common);
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            try
            {
                sb.Append(@"select com_empno,com_cname,RTRIM(LTRIM(org_abbr_chnm1)) AS org_abbr_chnm1,com_mailadd,dep_abbrnm,com_deptcd FROM (sfts_com INNER JOIN sfts_org ON com_orgcd=org_orgcd ) ");
                sb.Append(@"INNER JOIN sfts_dep ON dep_orgcd = com_orgcd AND dep_deptcd = com_deptcd ");
                sb.Append(@" where (UPPER(LTRIM(RTRIM(com_cname))) like '%'+@keyword+'%' or  UPPER(LTRIM(RTRIM(com_empno))) like '%'+ @keyword+'%' or  UPPER(LTRIM(RTRIM(com_mailadd))) like '%'+ @keyword+'%') ");
                sb.Append(@" and com_cname !='' and com_cname is not null ");
                oCmd.Parameters.Add("@keyword", SqlDbType.NVarChar).Value = keyword;
                oCmd.CommandText = sb.ToString();
                SqlDataAdapter oda = new SqlDataAdapter(oCmd);
                DataTable ds = new DataTable();
                oda.Fill(ds);
                return ds;

            }
            catch (Exception err)
            {
                throw new Exception(MessageUtil.DB_SelectFail + err.Message);
            }
        }

        //新增範例
        public void managerInsEXEC(string empno1, string startDate, string com_orgcd)
        {
            StringBuilder sb = new StringBuilder();
            SqlConnection oConn = new SqlConnection(AppConfig.Common);
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn;
            try
            {
                sb.Append(@"
                INSERT INTO[sys_manager]
                           ([mana_empno]
                           ,[mana_orgcd]
                           ,[mana_creator]
                           ,[mana_usedate]
                           ,[mana_createdate]
                           ,[mana_delete])
                     VALUES
                           (@empno1
                           ,@orgcd
                           ,@creator
                           ,@usedate
                           ,getdate()
                           ,'0')
                ");
                //oCmd.Parameters.Add("@empno1", SqlDbType.NVarChar).Value = empno1;
                //oCmd.Parameters.Add("@orgcd", SqlDbType.NVarChar).Value = com_orgcd;
                //oCmd.Parameters.Add("@creator", SqlDbType.NVarChar).Value = BasePage.GetCurrentUser().工號;
                //oCmd.Parameters.Add("@usedate", SqlDbType.DateTime).Value = startDate;
                try
                {
                    oCmd.Connection.Open();
                    oCmd.ExecuteNonQuery();
                    oCmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (oCmd.Connection.State != ConnectionState.Closed)
                        oCmd.Connection.Close();
                }

            }
            catch (Exception err)
            {
                throw new Exception(MessageUtil.DB_InsertFail + "(managerInsEXEC)" + err.HelpLink);
            }
        }
    }
}