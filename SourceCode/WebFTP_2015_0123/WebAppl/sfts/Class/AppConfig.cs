using System;
using System.Configuration;
using System.Web;

/// <summary>
/// AppConfig 的摘要描述
/// </summary>
public class AppConfig
{

    static string _DSN;
    static string _Common;
    static bool _SSODebug;
    static string _SSODebug_USER_ACCOUNT;
    static string _source_path;
    static string _source_zip_path;
    static string _EmailAccount;
    static string _EmailPassword;
    static string _EmailFromAddress;
    static string _EmailSmtpClient;
    static string _DevEmail;
    static string _SysWhere;
    static string _MailUrl;

	static AppConfig()
	{
        _DSN = ReadConfigString("DSN.charity").ToString();
        _Common = ReadConfigString("Common").ToString();
        _SSODebug = ReadConfigString("SSODebug") == "true";
        _SSODebug_USER_ACCOUNT = ReadConfigString("SSODebug.USER.ACCOUNT").ToString();
        _source_path = ReadConfigString("source_path").ToString();
        _source_zip_path = ReadConfigString("source_zip_path").ToString();
        _EmailAccount = ReadConfigString("EmailAccount").ToString();
        _EmailPassword = ReadConfigString("EmailPassword").ToString();
        _EmailFromAddress = ReadConfigString("EmailFromAddress").ToString();
        _EmailSmtpClient = ReadConfigString("EmailSmtpClient").ToString();
        _DevEmail = ReadConfigString("DevEmail").ToString();
        _SysWhere = ReadConfigString("SysWhere").ToString();
        _MailUrl = ReadConfigString("MailUrl").ToString();
	}

    private static string ReadConfigString(string keyName)
    {
        string retStr = ConfigurationManager.AppSettings[keyName];
        if (retStr == null)
        {
            throw new Exception("Unable to read app settings.Please check appsetting section in Web.config file.(" + keyName + ")");
        }
        return retStr;
    }


    public static string DSN
    {
        get { return AppConfig._DSN; }
    }
    public static bool SSODebug
    {
        get { return AppConfig._SSODebug; }
    }
    public static string SSODebug_USER_ACCOUNT
    {
        get { return AppConfig._SSODebug_USER_ACCOUNT; }
    }
    public static string source_path
    {
        get { return AppConfig._source_path; }
    }
    public static string Common
    {
        get { return AppConfig._Common; }
    }
    public static string Source_zip_path
    {
        get { return AppConfig._source_zip_path; }
    }
    public static string EmailAccount
    {
        get { return AppConfig._EmailAccount; }
    }
    public static string EmailPassword
    {
        get { return AppConfig._EmailPassword; }
    }
    public static string EmailFromAddress
    {
        get { return AppConfig._EmailFromAddress; }
    }
    public static string EmailSmtpClient
    {
        get { return AppConfig._EmailSmtpClient; }
    }
    public static string DevEmail
    {
        get { return AppConfig._DevEmail; }
    }
    public static string SysWhere
    {
        get { return AppConfig._SysWhere; }
    }
    public static string MailUrl
    {
        get { return AppConfig._MailUrl; }
    }
}


