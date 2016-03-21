<%@ Page Title="" Language="C#" MasterPageFile="~/sfts/MasterPage.Master" AutoEventWireup="true" CodeBehind="valid.aspx.cs" Inherits="WebAppl.valid" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    $(document).ready(function () {
        $("#btnsubmit").click(function () {
            if ($("#tbxpassw").val() != $("#tbxconfirmpassw").val()) {
                alert("確認密碼與密碼不合");
                return;
            }
            else {
                $.ajax({
                    type: "POST",
                    data: {
                        tbxpassw: $("#tbxpassw").val(),
                        tbxconfirmpassw: $("#tbxconfirmpassw").val(),
                        mem_id: $("#<% =hiddenID.ClientID %>").val()
                    },
                    url: "handler/modiflypasw.ashx",
                    success: function (data, textStatus) {
                        if (data == "success") {
                            alert("密碼變更成功");
                            location.href = "Default.aspx";
                        }
                        else {
                            alert(data);
                            return;
                        }
                    }
                })
            }
        });
    });
</script>
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
                <h3>新增/修改密碼</h3>
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
            <span class="sidebarcreateacc">
            <table cellpadding="0" cellspacing="0" border="0" width="100%" >
            <tr>
            <td>&nbsp;</td>
            </tr>
                <tr>
                    <td style="width: 40%" align="right">中文姓名：</td>
                    <td align="left">
                        <asp:Label ID="lbName" runat="server" ForeColor="#4bafe3"></asp:Label>
                        <input id="hiddenID" runat="server" style="display:none" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 40%" align="right">
                        電子郵件：
                    </td>
                    <td align="left">
                        <asp:Label ID="lbEmail" runat="server" ForeColor="#4bafe3"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 40%" align="right">
                        密碼：
                    </td>
                    <td align="left">
                        <input id="tbxpassw" type="password" maxlength="40" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 40%" align="right">
                        確認密碼：
                    </td>
                    <td align="left">
                        <input id="tbxconfirmpassw" type="password" maxlength="40" />
                    </td>
                </tr>            
                <tr>
                    <td colspan="2" align="right">
                        <input id="btnsubmit" type="button" value="送出" class="btn_mouseout" onmouseover="this.className='btn_mouseover'"
                            onmouseout="this.className='btn_mouseout'" />
                    </td>
                </tr>
            </table>
            </span>
            </td>
            <td style="width: 10%">
                &nbsp;
            </td>
        </tr>
    </table>
    </div>
</asp:Content>
