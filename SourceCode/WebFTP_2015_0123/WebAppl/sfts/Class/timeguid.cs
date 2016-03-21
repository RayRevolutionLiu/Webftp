using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// timeguid 的摘要描述
/// </summary>
public class timeguidclass
{
	public static string timeguid()
	{
        string GUID = Guid.NewGuid().ToString();
        GUID = GUID.Replace("-", "");
        string timeguid = _8timestampclass._8timestamp() + GUID;
        return timeguid;

	}
}