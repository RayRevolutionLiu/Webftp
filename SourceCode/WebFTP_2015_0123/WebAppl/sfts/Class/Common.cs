using System;
using System.Security.Cryptography;
using System.Web;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Net.Mail;


/// <summary>
/// Common 
/// </summary>
public class Common
{
    #region 檢查身份證字號格式
    public static bool CheckIdentificationId(string Input_ID)
    {
        bool IsTrue = false;
        if (Input_ID.Length == 10)
        {
            Input_ID = Input_ID.ToUpper();
            if (Input_ID[0] >= 0x41 && Input_ID[0] <= 0x5A)
            {
                int[] Location_No = new int[] { 10, 11, 12, 13, 14, 15, 16, 17, 34, 18, 19, 20, 21, 22, 35, 23, 24, 25, 26, 27, 28, 29, 32, 30, 31, 33 };
                int[] Temp = new int[11];
                Temp[1] = Location_No[(Input_ID[0]) - 65] % 10;
                int Sum = Temp[0] = Location_No[(Input_ID[0]) - 65] / 10;
                for (int i = 1; i <= 9; i++)
                {
                    Temp[i + 1] = Input_ID[i] - 48;
                    Sum += Temp[i] * (10 - i);
                }
                if (((Sum % 10) + Temp[10]) % 10 == 0)
                {
                    IsTrue = true;
                }
            }
        }
        return IsTrue;
    }
    #endregion

    #region 取得詳細資料[輸入參數 第一為工號,第二為是否為院內人士]
    public static DataTable GetDetail(string empno, string isempno)
    {
        StringBuilder sb = new StringBuilder();
        SqlConnection oConn = new SqlConnection();
        oConn.ConnectionString = AppConfig.DSN;
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = oConn;
        try
        {
            if (isempno == "True")
            {
                sb.Append(@"SELECT com_mailadd AS cEmail,com_cname AS cName FROM sfts_com WHERE com_empno=@com_empno ");
            }
            else
            {
                sb.Append(@"SELECT mem_account AS cEmail,mem_title AS cName FROM sfts_member WHERE mem_account=@com_empno ");
            }

            oCmd.Parameters.Add("@com_empno", SqlDbType.NVarChar).Value = empno.ToString();
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
    #endregion

    #region 根據parentid抓到main的資料
    public static DataTable AccordingParentidToFindMember(string parentid)
    {
        StringBuilder sb = new StringBuilder();
        SqlConnection oConn = new SqlConnection();
        oConn.ConnectionString = AppConfig.DSN;
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = oConn;
        try
        {
            sb.Append(@"SELECT *,convert(varchar(20),main_createdate,111) AS Cmain_createdate FROM sfts_main WHERE main_parentid=@main_parentid ");
            oCmd.Parameters.Add("@main_parentid", SqlDbType.NVarChar).Value = parentid.ToString();
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
    #endregion

    #region 去判斷輸入的Email跟每天跑Web Server更新的itrimail是否相同
    public static DataTable AccordEmailIsitFromITRI(string email,string empno)
    {
        StringBuilder sb = new StringBuilder();
        SqlConnection oConn = new SqlConnection();
        oConn.ConnectionString = AppConfig.DSN;
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = oConn;
        try
        {
            sb.Append(@"SELECT * FROM sfts_com WHERE UPPER(com_mailadd)=@email ");
            if (empno.Trim() != "")
            {
                sb.Append(@" AND com_empno=@empno ");
                oCmd.Parameters.Add("@empno", SqlDbType.NVarChar).Value = empno.ToString();
            }
            oCmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = email.ToString().ToUpper();
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
    #endregion

    #region Email格式驗證
    //2013/06/28 小賴哥建議EMAIL格式檢查使用using System.Net.Mail;

    public static bool IsVaildEmail(string email)
    {     
        try
        {
            MailAddress MailAdd = new MailAddress(email);
            return true;
        }
        catch(Exception ex)
        {
            return false;
        }
        //bool retuVal = false;
        //Regex regexFormat = new Regex(@"^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$");
        //Regex regexFormat = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        //if (regexFormat.IsMatch(email))
        //{
        //    retuVal = true;
        //}
        //return retuVal;
    }
    #endregion

} 

#region SSO登入之後 可以直接抓到使用者的基本資料並且讓之後寫程式能夠輕鬆抓取
    /// <summary>
    /// 範例:Account.GetAccInfo().com_cname.ToString().Trim() 抓取SSO之後該工號的本名
    /// </summary>
public class sAccount
{
    /// <summary>
    /// GetAccInfo
    /// </summary>
    /// <returns></returns>
    public static AccountInfo GetAccInfo()
    {
        return (AccountInfo)HttpContext.Current.Session["pwerRowData"];
    }

   
    /// <summary>
    /// 進行帳號登入檢查，通過後會取得登入者資料並放入session:ds_CurrentUser
    /// </summary>
    /// <param name="inputID"></param>
    /// <returns></returns>
    public AccountInfo ExecLogon(string getAD)
    {
        string conn = AppConfig.Common;
        DataSet ds = new DataSet();
        SqlDataAdapter oda = new SqlDataAdapter();
        SqlConnection thisConnection = new SqlConnection(conn);
        SqlCommand thisCommand = thisConnection.CreateCommand();
        StringBuilder show_value = new StringBuilder();
        try
        {
            thisConnection.Open();
            show_value.Append(@"select com_empno AS Account,'' AS  PassWord,");
            show_value.Append(@"com_cname AS Title,com_deptcd,");
            show_value.Append(@"com_telext,com_mailadd AS Email,'0' AS Stat,'' AS LastLogDate,'' AS IDmem,'' AS mem_querystring ");
            show_value.Append(@"from sfts_com where com_empno=@getAD");
            //show_value.Append(@"com_telext,itri_PrimarySmtpAddress AS Email,'0' AS Stat,'' AS LastLogDate,'' AS IDmem,'' AS mem_querystring ");
            //show_value.Append(@"from common..comper LEFT JOIN sfts_itri ON itri_SamAccountName=com_empno where com_empno=@getAD");
            thisCommand.Parameters.AddWithValue("@getAD", getAD);
            thisCommand.CommandType = CommandType.Text;
            thisCommand.CommandText = show_value.ToString();
            oda.SelectCommand = thisCommand;
            oda.Fill(ds);
        }
        finally
        {
            oda.Dispose();
            thisConnection.Close();
            thisConnection.Dispose();
            thisCommand.Dispose();

        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            return new AccountInfo(ds, true);
        }
        else
        {
            return null;
        }
    }

    public AccountInfo ExecLogonOutCompany(string getAD,string getPaw)
    {
        string conn = AppConfig.DSN;
        DataSet ds = new DataSet();
        SqlDataAdapter oda = new SqlDataAdapter();
        SqlConnection thisConnection = new SqlConnection(conn);
        SqlCommand thisCommand = thisConnection.CreateCommand();
        StringBuilder show_value = new StringBuilder();
        try
        {
            thisConnection.Open();
            show_value.Append(@"select mem_account AS Account,mem_passw AS PassWord,");
            show_value.Append(@"mem_title AS Title,'' AS com_deptcd,");
            show_value.Append(@"'' AS com_telext,mem_account AS Email,mem_stat AS Stat,CONVERT(varchar(12),mem_lastlogdate,111) AS LastLogDate,");
            show_value.Append(@"mem_id AS IDmem, mem_querystring ");
            //修改廠商登入帳號e-mail不分大小寫，一律用小寫比對 by 凱呈 20130905
            show_value.Append(@"from sfts_member where Lower(mem_account)=@getAD AND mem_passw=@getPaw");
            thisCommand.Parameters.AddWithValue("@getAD", getAD.ToLower());            
            thisCommand.Parameters.AddWithValue("@getPaw", getPaw);
            thisCommand.CommandType = CommandType.Text;
            thisCommand.CommandText = show_value.ToString();
            oda.SelectCommand = thisCommand;
            oda.Fill(ds);
        }
        finally
        {
            oda.Dispose();
            thisConnection.Close();
            thisConnection.Dispose();
            thisCommand.Dispose();

        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            return new AccountInfo(ds, false);
        }
        else
        {
            return null;
        }
    }

}

/*-------------------------------------------------------------------------------------------------------------------------*/

    /// <summary>
    /// MbrInfo 的摘要描述。
    /// </summary>
public class AccountInfo
{
    /// <summary>
    /// 工號/帳號
    /// </summary>
    public readonly string Account = "";
    /// <summary>
    /// 空值/密碼
    /// </summary>
    public readonly string PassWord = "";
    /// <summary>
    /// 姓名/抬頭
    /// </summary>
    public readonly string Title = "";
    /// <summary>
    /// 部門例如(SF200)
    /// </summary>
    public readonly string com_deptcd = "";
    /// <summary>
    /// 分機
    /// </summary>
    public readonly string com_telext = "";
    /// <summary>
    /// E-Mail
    /// </summary>
    public readonly string Email = "";
    /// <summary>
    /// 廠商帳號是否正常(0正常,1註銷)
    /// </summary>
    public readonly string Stat = "0";
    /// <summary>
    /// 用來判斷是否為院內成員登入
    /// </summary>
    public readonly bool Com_Isempno = false;
    /// <summary>
    /// 最後登入時間(廠商用)
    /// </summary>
    public readonly string LastLogDate = "";
    /// <summary>
    /// 廠商primary Key(廠商用)
    /// </summary>
    public readonly string IDmem = "";
    /// <summary>
    /// 廠商queryString(廠商用)
    /// </summary>
    public readonly string QueryStr = "";

    public AccountInfo(DataSet ds,bool isempno)
    {
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            Account = dr["Account"].ToString();
            PassWord = dr["PassWord"].ToString();
            Title = dr["Title"].ToString();
            com_deptcd = dr["com_deptcd"].ToString();
            com_telext = dr["com_telext"].ToString();
            Email = dr["Email"].ToString();
            Stat = dr["Stat"].ToString();
            Com_Isempno = isempno;
            LastLogDate = dr["LastLogDate"].ToString();
            IDmem = dr["IDmem"].ToString();
            QueryStr = dr["mem_querystring"].ToString();
        }
    }
}

#endregion

#region JavaScript 後台Alert之後可以直接使用裡面的FUNCTION 不用再落落長的寫一大段了
/// <summary>
/// JavaScript 的摘要描述。
/// </summary>
public class JavaScript
{
    /// <summary>
    /// AlertMessage
    /// </summary>
    public static void AlertMessage(System.Web.UI.Page objPage, string strMessage)
    {
        strMessage = strMessage.Replace("\r\n", "\\r");
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat(@"<Script language=""javascript"" type=""text/javascript"">");
        sb.AppendFormat(@"alert(""{0}"");", strMessage);
        sb.AppendFormat(@"</Script>");

        //objPage.ClientScript.RegisterClientScriptBlock(objPage.GetType(), "", strJS, false);
        objPage.ClientScript.RegisterStartupScript(objPage.GetType(), "", sb.ToString(), false);
    }

    public static void PopUp(System.Web.UI.Page objPage, string url)
    {
        url = url.Replace("\r\n", "\\r");
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat(@"<Script language=""javascript"" type=""text/javascript"">");
        sb.AppendFormat(@"window.open('" + url + "');");
        sb.AppendFormat(@"</Script>");

        //objPage.ClientScript.RegisterClientScriptBlock(objPage.GetType(), "", strJS, false);
        objPage.ClientScript.RegisterStartupScript(objPage.GetType(), "", sb.ToString(), false);
    }

    /// <summary>
    /// AlertMessageClose
    /// </summary>
    public static void AlertMessageClose(System.Web.UI.Page objPage, string strMessage)
    {
        string strJS = "";
        strMessage = strMessage.Replace("\r\n", "\\r");
        strJS = @"<Script language='javascript' type='text/javascript' >";
        strJS += "alert('" + strMessage + "');";
        strJS += "window.close();";
        strJS += "</Script>";
        //objPage.ClientScript.RegisterClientScriptBlock(objPage.GetType(), "", strJS, false);
        objPage.ClientScript.RegisterStartupScript(objPage.GetType(), "", strJS, false);
    }

    /// <summary>
    /// AlertMessageRedirect
    /// </summary>
    public static void AlertMessageRedirect(System.Web.UI.Page objPage, string strMessage, string strRedirectPage)
    {
        AlertMessageRedirect(objPage, strMessage, strRedirectPage, false);
    }

    public static void AlertMessageRedirect(System.Web.UI.Page objPage, string strMessage, string strRedirectPage, bool IsDisplayData)
    {
        string strJS = "";
        strMessage = strMessage.Replace("\r\n", "\\r");
        strJS = @"<Script language='javascript' type='text/javascript'>";
        strJS += "alert('" + strMessage + "');";
        strJS += "window.location ='" + strRedirectPage + "'; ";
        strJS += "</Script>";

        if (IsDisplayData)
            objPage.ClientScript.RegisterStartupScript(objPage.GetType(), "", strJS, false);
        else
            objPage.ClientScript.RegisterClientScriptBlock(objPage.GetType(), "", strJS, false);
    }

}
    #endregion

#region 加解密
/// <summary>
/// encryptquerystring 加密QueryString decryptquerystring 解密
/// encryptpassword 加密密碼 decryptpassword 解密
/// 用法 string aaa = encryptquerystring(要加密的字串);
/// </summary>
public class security
{
    string _querystringkey = "sftsqury"; //url传输参数加密key
    string _passwordkey = "sftspasw"; //password加密key

    public security()
    {
        //
        // todo: 在此处添加构造函数逻辑
        //
    }

    /// 
    /// 加密url传输的字符串
    /// 
    /// 
    /// 
    public string encryptquerystring(string querystring)
    {
        return Encrypt(querystring, _querystringkey);
    }

    /// 
    /// 解密url传输的字符串
    /// 
    /// 
    /// 
    public string decryptquerystring(string querystring)
    {
        return decrypt(querystring, _querystringkey);
    }

    /// 
    /// 加密帐号口令
    /// 
    /// 
    /// 
    public string encryptpassword(string password)
    {
        return Encrypt(password, _passwordkey);
    }

    /// 
    /// 解密帐号口令
    /// 
    /// 
    /// 
    public string decryptpassword(string password)
    {
        return decrypt(password, _passwordkey);
    }

    /// 
    /// dec 加密过程
    /// 
    /// 
    /// 
    /// 
    public string Encrypt(string pToEncrypt, string sKey)
    {
        DESCryptoServiceProvider des = new DESCryptoServiceProvider(); //把字符串放到byte數組中 

        byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
        //byte[] inputByteArray=Encoding.Unicode.GetBytes(pToEncrypt); 

        des.Key = ASCIIEncoding.ASCII.GetBytes(sKey); //建立加密對象的密鑰和偏移量 
        des.IV = ASCIIEncoding.ASCII.GetBytes(sKey); //原文使用ASCIIEncoding.ASCII方法的GetBytes方法 
        MemoryStream ms = new MemoryStream(); //使得輸入密碼必須輸入英文文本 
        CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

        cs.Write(inputByteArray, 0, inputByteArray.Length);
        cs.FlushFinalBlock();

        StringBuilder ret = new StringBuilder();
        foreach (byte b in ms.ToArray())
        {
            ret.AppendFormat("{0:X2}", b);
        }
        ret.ToString();
        return ret.ToString();
    }

    /// 
    /// dec 解密过程
    /// 
    /// 
    /// 
    /// 
    public string decrypt(string ptodecrypt, string skey)
    {
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();

        byte[] inputbytearray = new byte[ptodecrypt.Length / 2];
        for (int x = 0; x < ptodecrypt.Length / 2; x++)
        {
            int i = (Convert.ToInt32(ptodecrypt.Substring(x * 2, 2), 16));
            inputbytearray[x] = (byte)i;
        }

        des.Key = ASCIIEncoding.ASCII.GetBytes(skey); //建立加密对象的密钥和偏移量，此值重要，不能修改 
        des.IV = ASCIIEncoding.ASCII.GetBytes(skey);
        MemoryStream ms = new MemoryStream();
        CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);

        cs.Write(inputbytearray, 0, inputbytearray.Length);
        cs.FlushFinalBlock();

        StringBuilder ret = new StringBuilder(); //建立stringbuild对象，createdecrypt使用的是流对象，必须把解密后的文本变成流对象 

        return System.Text.Encoding.Default.GetString(ms.ToArray());
    }

    /// 
    /// 检查己加密的字符串是否和原文相同
    /// 
    /// 
    /// 
    /// 
    /// 
    public bool validatestring(string enstring, string fostring, int mode)
    {
        switch (mode)
        {
            default:
            case 1:
                if (decrypt(enstring, _querystringkey) == fostring.ToString())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 2:
                if (decrypt(enstring, _passwordkey) == fostring.ToString())
                {
                    return true;
                }
                else
                {
                    return false;
                }
        }
    }
}
    #endregion

public class encode
{
    public static string sha1en(string unCodeString)
    {
        string enCodeString;
        SHA512CryptoServiceProvider sha1en = new SHA512CryptoServiceProvider();
        enCodeString = BitConverter.ToString(sha1en.ComputeHash(UTF8Encoding.Default.GetBytes(unCodeString)), 4, 8);
        enCodeString = enCodeString.Replace("-", "");
        return enCodeString;
    }

    public static bool sqlInjection(string CheckString)
    {
        string SQL_injdata =@":|;|>|<|--|sp_|xp_|\|dir|cmd|^|(|)|+|$|'|copy|format|and|exec|insert|select|delete|update|count|*|%|chr|mid|master|truncate|char|declare";
        string[] SQL_inj = SQL_injdata.Split('|');
        bool returnBool = false;

        for (int i = 0; i < SQL_inj.Length; i++)
        {
            if (CheckString.IndexOf(SQL_inj[i]) != -1)
            {
                returnBool = true;
                break;
            }
            else
            {
                returnBool = false;
            }
        }

        return returnBool;
    }
}

