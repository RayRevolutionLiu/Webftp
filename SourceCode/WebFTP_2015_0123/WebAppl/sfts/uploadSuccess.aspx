<%@ Page Title="" Language="C#" MasterPageFile="~/sfts/MasterPage.Master" AutoEventWireup="true" CodeBehind="uploadSuccess.aspx.cs" Inherits="WebAppl.uploadSuccess" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="lineheight03">
        <br />
    <table width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td style="width: 10%">
                &nbsp;
            </td>
            <td>
                <h3>檔案傳送完成</h3>
            </td>
            <td style="width: 10%">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="width: 10%">
                &nbsp;
            </td>
            <td>
                <br />
                <asp:Label ID="lbName" runat="server" ForeColor="#4bafe3"></asp:Label>
                &nbsp;君您好：<br />
您方才傳送的檔案，其收件人名單如下：<br />
</td>
            <td style="width: 10%">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="width: 10%">
                &nbsp;</td>
            <td>
                <asp:Panel ID="panelsender" runat="server">
                </asp:Panel>
            </td>
            <td style="width: 10%">
                &nbsp;</td>
        </tr>
                <tr>
            <td style="width: 10%">
                &nbsp;
            </td>
            <td>
                <h3>您的信件主旨如下：</h3></td>
            <td style="width: 10%">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="width: 10%">
                &nbsp;
            </td>
            <td>
                <asp:Label ID="lbTitle" runat="server"></asp:Label>
            </td>
            <td style="width: 10%">
                &nbsp;
            </td>
        </tr>
                <tr>
            <td style="width: 10%">
                &nbsp;
            </td>
            <td>
                <h3>您的信件留言如下：</h3></td>
            <td style="width: 10%">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="width: 10%">
                &nbsp;
            </td>
            <td>
                <asp:Label ID="lbdesc" runat="server"></asp:Label>
            </td>
            <td style="width: 10%">
                &nbsp;
            </td>
        </tr>

        <tr>
            <td style="width: 10%">
                &nbsp;
            </td>
            <td>
                <h3>您傳送的檔案列表如下：</h3></td>
            <td style="width: 10%">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="width: 10%">
                &nbsp;
            </td>
            <td>
                <span class="stripeMe">
                    <asp:GridView ID="gvfilelist" runat="server" AutoGenerateColumns="false" onrowdatabound="gvfilelist_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="file_origiFileNameALL" HeaderText="檔案名稱" />
                            <asp:BoundField DataField="file_size" HeaderText="檔案大小" />
                        </Columns>
                    </asp:GridView>
                </span>
                <br />
            </td>
            <td style="width: 10%">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="width: 10%">
                &nbsp;
            </td>
            <td>
                我們會儘快將上述的相關檔案訊息，通知您所指定的收件人。謝謝您！
            </td>
            <td style="width: 10%">
                &nbsp;
            </td>
        </tr>
    </table>
</div>
</asp:Content>
