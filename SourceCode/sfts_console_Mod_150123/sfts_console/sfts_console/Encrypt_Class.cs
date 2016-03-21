using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

public class Encrypt_Class
{

    string _querystringkey = "sftsqury"; //url传输参数加密key
    string _passwordkey = "sftspasw"; //password加密key

    public Encrypt_Class()
    {

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