<%@ Page Title="" Language="C#" MasterPageFile="~/sfts/enVersion/MasterPageEn.Master" AutoEventWireup="true" CodeBehind="uploadSuccessEn.aspx.cs" Inherits="WebAppl.sfts.enVersion.uploadSuccessEn" %>
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
                <h3>File Transfer Complete</h3>
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
                Dear<asp:Label ID="lbName" runat="server" ForeColor="#4bafe3"></asp:Label>
                :<br />
The file receivers you specified are listed below:<br />
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
                <h3>Your Message:</h3></td>
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
                <h3>The files you transferred are listed as follows:</h3></td>
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
                            <asp:BoundField DataField="file_origiFileNameALL" HeaderText="File Name" />
                            <asp:BoundField DataField="file_size" HeaderText="File Size" />
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
                Thank you for using WebFTP.
            </td>
            <td style="width: 10%">
                &nbsp;
            </td>
        </tr>
    </table>
</div>
</asp:Content>
