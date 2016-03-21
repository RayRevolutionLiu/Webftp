<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="header.ascx.cs" Inherits="WebAppl.UserControl.header" %>

<div class="header">
    <a href="../sfts/Default.aspx" target="_self">
        <div class="logo">
            <h1>
                工研院檔案傳輸系統</h1>
        </div>
    </a>
    <div class="menu">
        <div class="btncurrent" id="hylCreate" runat="server" visible="false">
        <a href="javascript:void(0);" target="_self">申請帳號</a></div>
        <div class="btncurrent" id="hyValid" runat="server" visible="false">
        <a href="javascript:void(0);" target="_self">驗證</a></div>
        <div class="btncurrent" id="hyl1" runat="server">
        <a href="../sfts/FileEditPage.aspx" target="_self">檔案傳送</a></div>
        <div class="btn" id="hyl2" runat="server">
        <a href="../sfts/page_mantain.aspx" target="_self">檔案接收狀況</a></div>
        <div class="btn" id="hyl3" runat="server">
        <a href="../sfts/page_mantainall.aspx" target="_self">寄送紀錄</a></div>
        <div class="btn" id="hyl4" runat="server">
        <a href="../sfts/page_about.aspx" target="_self">系統說明</a>
        </div>
    </div>
</div>
<!--{* header *}-->
