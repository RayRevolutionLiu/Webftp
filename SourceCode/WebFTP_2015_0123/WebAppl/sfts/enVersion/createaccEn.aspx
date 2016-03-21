<%@ Page Title="" Language="C#" MasterPageFile="~/sfts/enVersion/MasterPageEn.Master" AutoEventWireup="true" CodeBehind="createaccEn.aspx.cs" Inherits="WebAppl.sfts.enVersion.createaccEn" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#btnclear").click(function () {
                $("#tbxName").val("");
                $("#tbxAccount").val("");
                $("input[name='type']").get(0).checked = true;
            });

            $("#btnsubmit").click(function () {
                var tbxName = $("#tbxName").val();
                var tbxAccount = $("#tbxAccount").val();
                var type = $("input[name='type']:checked").val();

                if (type == "agree") {
                    $.ajax({
                        url: "handlerEn/registerAccEn.ashx",
                        type: 'POST',
                        data: {
                            tbxNameQ: tbxName,
                            tbxAccountQ: tbxAccount,
                            typeQ: type
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            return;
                        },
                        success: function (data, textStatus) {
                            if (data == "success") {
                                $("#resultName").text(tbxName);
                                $("#resultNameList").text(tbxName);
                                $("#resultEmail").text(tbxAccount);
                                $("#resultEmailList").text(tbxAccount);
                                $("#divSuccess").dialog("open");

                            }
                            else {
                                alert(data);
                            }
                        }
                    });
                }
                else {
                    alert("You have not agreed \"User agreement\" yet");
                }
            });

            $("#divSuccess").dialog({
                autoOpen: false,
                height: 240,
                width: 570,
                modal: true,
                draggable: false, //可拖動 
                closeOnEscape: true, //讓Esc可以關掉視窗
                close: function (event, ui) { window.location.href = "Default.aspx"; }
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
                <h3>Apply New User Account</h3>
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
                        <input id="tbxName" type="text" maxlength="35" /><span class="font-red">*</span></td>
                </tr>
                <tr>
                    <td style="width: 40%" align="right">E-mail：</td>
                    <td align="left">
                        <input id="tbxAccount" type="text" maxlength="60" /><span class="font-red">*</span></td>
                </tr>
                <tr>
                <td colspan="2">
                    <br />
                    <div  style="width:98%">
                        <p align="left" style="color: #cc3333">
                            User Rules & Privacy：
                        </p>
                        <div align="left">
                            <ul>
                                <li>The WebFTP service can only be used to deliver business related large file(s). It is not allowed to send personal file(s).</li>
                                <li>Users are not permitted to send unauthorized software via WebFTP.</li>
                                <li>Users attempting to hack/destroy the system will be suspended from using it when found.</li>
                                <li>Personal Information & Privacy Protection Policy and Statement	: This service is provided by ITRI. When applying an account, this system will collect your name and email address just for file transfer function. If you do not permit to provide your information, this system will not function. If you want to stop using this system and clear all your information in this system, please contact 03-5918899.</li>
                            </ul>
                        </div>
                    </div>
                </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <input type="radio" name="type" value="agree"/>&nbsp;Agree&nbsp;&nbsp;<input
                            type="radio" name="type" value="disagree" checked="checked" />&nbsp;Disagree
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="right">
                        <input id="btnsubmit" type="button" value="Submit" class="btn_mouseout" onmouseover="this.className='btn_mouseover'"
                            onmouseout="this.className='btn_mouseout'" />
                        <input id="btnclear" type="button" value="Reset" class="btn_mouseout" onmouseover="this.className='btn_mouseover'"
                            onmouseout="this.className='btn_mouseout'" />
                    </td>
                </tr>
                <tr>
                <td colspan="2">
                    <br />
                    <div class="DelDiv" style="width:98%">
                        <p align="left" style="color: #cc3333">
                            Note：
                        </p>
                        <div align="left">
                            <ul>
                                <li>(<span class="font-red">*</span>) fields are required. You must fill them.</li>
                                <li>If your E-mail address is not ITRI email, file receiver(s) can only be ITRI employee(s).</li>
                                <li>Your E-mail address will be your user account when login the system.</li>
                                <li>Your account and password will be send to you via the E-mail address that you entered.</li>
                            </ul>
                        </div>
                    </div>
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

    <div id="divSuccess" title="Success" class="font-gray block font-size2">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td colspan="2">
                    Dear<span style="color:#4bafe3" id="resultName"></span>：
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 10%">
                </td>
                <td>
                    Your account information：
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width:14%">
                                Name：
                            </td>
                            <td>
                                <span style="color:#4bafe3" id="resultNameList"></span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                E-mail：
                            </td>
                            <td>
                                <span style="color:#4bafe3" id="resultEmail"></span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                            &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                An E-mail has been sent to<span style="color:#4bafe3" id="resultEmailList"></span> Please check your e-mail box and contact us if you have any questions.
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                &nbsp;
                </td>
            </tr>            
            <tr>
                <td colspan="2" align="right">
                    <input id="btnback" type="button" value="Back" class="btn_mouseout" onmouseover="this.className='btn_mouseover'"
                        onmouseout="this.className='btn_mouseout'" onclick="javascript:location.href='https://webftp.itri.org.tw/sfts/Default.aspx'" />
                </td>
            </tr>
        </table>
    </div>

</div>
</asp:Content>
