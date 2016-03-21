
/// <summary>
/// MessageUtil 的摘要描述
/// </summary>
public class MessageUtil
{
	public MessageUtil()
	{
		// 
		// TODO: 在此加入建構函式的程式碼
		//
    }
    public static readonly string basePageError = "<div style=color:#ff0000;text-align:center>網頁不存在 or 資料庫連線失敗，請聯絡系統管理者，謝謝！</div>";

    public static readonly string Msg_Empno = "取得帳號失敗，請聯絡系統管理者，謝謝！";
    public static readonly string Msg_EmpData = "取得帳號基本資料失敗，請聯絡系統管理者，謝謝！";

    public static readonly string Msg_SendEmail = "E-mail寄發失敗";
    public static readonly string Msg_NotFoundAttachment = "E-mail寄發失敗(找不到 Email 附件)";
    public static readonly string Msg_NotFoundEmailTemplate = "E-mail寄發失敗(找不到 Email Content Template)";

    public static readonly string Msg_ConnDB = "連結資料庫失敗，請聯絡系統管理者，謝謝！";
    public static readonly string DB_InsertFail = "DB資料新增時發生錯誤";
    public static readonly string DB_DeleteFail = "DB資料刪除時發生錯誤";
    public static readonly string DB_UpdateFail = "DB資料修改時發生錯誤";
    public static readonly string DB_SelectFail = "DB資料查詢時發生錯誤";

    public static readonly string saveFiles = "儲存上傳檔案的相關資料時發生錯誤";

    public static readonly string loginlogError = "登入錯誤";





    

}
