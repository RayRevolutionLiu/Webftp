using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// 產生8碼時間戳記
/// </summary>
public class _8timestampclass
{
    public static string _8timestamp()
	{
        DateTime dt = DateTime.Now;
    
        dt.Year.ToString();

        string _8timestamp,month, day;

        if (dt.Month < 10)
        {
            month = "0" + dt.Month.ToString();
        }
        else
        {
            month = dt.Month.ToString();
        }


        if (dt.Day < 10)
        {
            day = "0" + dt.Day.ToString();
        }
        else
        {
            day = dt.Day.ToString();
        }


        _8timestamp = dt.Year.ToString() + month + day;

        return _8timestamp;
       
	}
}