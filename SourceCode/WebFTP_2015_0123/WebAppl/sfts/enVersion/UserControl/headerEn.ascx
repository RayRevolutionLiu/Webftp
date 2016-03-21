<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="headerEn.ascx.cs" Inherits="WebAppl.sfts.enVersion.UserControl.headerEn" %>
<div class="header">
    <a href="Default.aspx" target="_self">
        <div class="logo">
            <h1>
                ITRI Secured File Transfer System</h1>
        </div>
    </a>
    <div class="menu">
        <div class="btncurrent" id="hylCreate" runat="server" visible="false">
        <a href="javascript:void(0);" target="_self">Create Account</a></div>
        <div class="btncurrent" id="hyValid" runat="server" visible="false">
        <a href="javascript:void(0);" target="_self">Verification</a></div>
        <div class="btncurrent" id="hyl1" runat="server">
        <a href="FileEditPageEn.aspx" target="_self">Trasfered Files</a></div>
        <div class="btn" id="hyl2" runat="server">
        <a href="page_mantainEn.aspx" target="_self">Manage Files</a></div>
        <div class="btn" id="hyl3" runat="server">
        <a href="page_mantainallEn.aspx" target="_self">Files logs</a></div>
        <div class="btn" id="hyl4" runat="server">
        <a href="page_aboutEn.aspx" target="_self">Overview</a>
        </div>
    </div>
</div>