<%@ Page Title="" Language="C#" MasterPageFile="~/sfts/enVersion/MasterPageEn.Master" AutoEventWireup="true" CodeBehind="vaildEn.aspx.cs" Inherits="WebAppl.sfts.enVersion.vaildEn" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    $(document).ready(function () {
        $("#btnsubmit").click(function () {
            if ($("#tbxpassw").val() != $("#tbxconfirmpassw").val()) {
                alert("Password and confirm password is not equal");
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
                    url: "handlerEn/modiflypaswEn.ashx",
                    success: function (data, textStatus) {
                        if (data == "success") {
                            alert("Change password Success");
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
                <h3>Add/Modification Password</h3>
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
                    <td style="width: 40%" align="right">Name：</td>
                    <td align="left">
                        <asp:Label ID="lbName" runat="server" ForeColor="#4bafe3"></asp:Label>
                        <input id="hiddenID" runat="server" style="display:none" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 40%" align="right">
                        E-mail：
                    </td>
                    <td align="left">
                        <asp:Label ID="lbEmail" runat="server" ForeColor="#4bafe3"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 40%" align="right">
                        Password：
                    </td>
                    <td align="left">
                        <input id="tbxpassw" type="password" maxlength="40" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 40%" align="right">
                        Confirm Password：
                    </td>
                    <td align="left">
                        <input id="tbxconfirmpassw" type="password" maxlength="40" />
                    </td>
                </tr>            
                <tr>
                    <td colspan="2" align="right">
                        <input id="btnsubmit" type="button" value="Submit" class="btn_mouseout" onmouseover="this.className='btn_mouseover'"
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
