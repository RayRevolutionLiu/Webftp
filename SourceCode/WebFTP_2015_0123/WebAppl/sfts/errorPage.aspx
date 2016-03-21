<%@ Page Title="" Language="C#" MasterPageFile="~/sfts/MasterPage.Master" AutoEventWireup="true" CodeBehind="errorPage.aspx.cs" Inherits="WebAppl.errorPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <fieldset>
            <legend class="font_black font_size15">錯誤訊息</legend>
            <table width="95%">
                <tr>
                    <td class="font_black font_size15">
                        <br />
                        <%
                            string ErrorMsg = (Application["ErrorMsg"] == null) ? "您不可以進入本網頁!" : (string)Application["ErrorMsg"];
                            Response.Write(ErrorMsg);
                        %>                        
                    </td>
                </tr>
                <tr>
                    <td class="font_size13">
                    <br />
                    <a href="<%=ResolveUrl("~/sfts/Default.aspx")%>" target="_self">返回首頁</a>
                    </td>
                </tr>
             </table>
        </fieldset>
    </div>
</asp:Content>
