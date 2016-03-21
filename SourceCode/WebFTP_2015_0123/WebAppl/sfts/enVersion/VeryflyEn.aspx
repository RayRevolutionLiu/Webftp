<%@ Page Title="" Language="C#" MasterPageFile="~/sfts/enVersion/MasterPageEn.Master" AutoEventWireup="true" CodeBehind="VeryflyEn.aspx.cs" Inherits="WebAppl.sfts.enVersion.VeryflyEn" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var vailde, encryN, showN, exten, aid, afileID, notifyflag;
            $(document).on("click", "input[name='ButtonGetFile']", function (event) {
                aid = $(this).attr("sendid");
                notifyflag = $(this).attr("notifyflag");


                $.ajax({
                    type: "POST",
                    data: {
                        afile_id: $(this).attr("afile_id"),
                        afile_parentid: $(this).attr("afile_parentid"),
                        afile_comorsec: $(this).attr("afile_comorsec")
                    },
                    async: false,
                    url: "../handler/downloadfile.ashx",
                    success: function (response) {
                        encryN = response[0].afile_encryptfileName;
                        showN = response[0].ShowFileName;
                        exten = response[0].afile_exten;
                        afileID = response[0].afile_id;

                        if (response[0].afile_comorsec == "common") {
                            if (response[0].main_stat != "N") {
                                $.fileDownload("handler/filedownload.ashx?filename=" + encodeURIComponent(response[0].afile_encryptfileName) + "&source=" + encodeURIComponent(response[0].ShowFileName) + "&afileID=" + response[0].afile_id + "&exten=" + response[0].afile_exten + "&aid=" + aid + "&vailde=&notifyflag=" + notifyflag);
                            }
                            else {
                                alert("This data has been deleted,download fail.");
                                return;
                            }
                        }
                        else if (response[0].afile_comorsec == "security") {
                            $("#divEnterValide").dialog("open");
                            $("#loginBtn").focus();
                        }
                        else if (response[0].afile_comorsec == "nonforward") {
                            $.ajax({
                                type: "POST",
                                data: {
                                    aid: aid
                                },
                                url: "handlerEn/nonforwardEn.ashx",
                                success: function (response) {
                                    if (response == "0") {
                                        //表示還未取過檔 
                                        alert("This is your first time to receive files, the CAPTCHA will be sent to your mailbox.\r\n\r\nPlease enter the authentication code after you receive a mail which contains CAPTCHA.");
                                    }
                                    else if (response == "1") {
                                        alert("This CAPTCHA can be used in 30 minutes,Please enter the new CAPTCHA which just sent to your mailbox.");
                                    }

                                    $("#divEnterValideNoforward").dialog("open");
                                    $("#loginBtnNoforward").focus();
                                },
                                error: function (xhr, ajaxOptions, thrownError) {
                                    alert(thrownError);
                                    return;
                                }
                            });
                        }
                    },
                    dataType: "json",
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(thrownError);
                        return;
                    }
                });
            });

            $(document).on("click", "input[name='ButtonReSend']", function (event) {
                $.ajax({
                    type: "POST",
                    data: {
                        aid: $(this).attr("sendid"),
                        afileID: $(this).attr("afile_id")
                    },
                    url: "handlerEn/ReSendEmailEn.ashx",
                    success: function (response) {
                        if (response == "OK") {
                            alert("Unzip password has been re-sent to your mailbox.");
                        }
                        else if (response == "NO_downLoad") {
                            alert("You are not downloading files yet");
                            return;
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr);
                        return;
                    }
                });
            });

            $(document).on("click", "#loginBtn", function (event) {
                $.ajax({
                    type: "POST",
                    data: {
                        aid: aid,
                        vaild: $("#tbxValide").val(),
                        afileID: afileID
                    },
                    url: "handlerEn/VaildEn.ashx",
                    success: function (response) {
                        alert(response);
                        if (response == "Authentication is successful! Unzip password will be sent to your mailbox when the download is complete.") {
                            $("#divEnterValide").dialog("close");
                            $.fileDownload("handler/filedownload.ashx?filename=" + encryN + "&source=" + showN + "&afileID=" + afileID + "&exten=" + exten + "&aid=" + aid + "&vailde=" + $("#tbxValide").val() + "&notifyflag=" + notifyflag);
                        }
                        else {
                            return;
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(thrownError);
                        return;
                    }
                });
            });


            $(document).on("click", "#loginBtnNoforward", function (event) {
                $.ajax({
                    type: "POST",
                    data: {
                        aid: aid,
                        vaild: $("#tbxValideNoforward").val(),
                        afileID: afileID
                    },
                    url: "handlerEn/VaildNoforwardEn.ashx",
                    success: function (response) {
                        if (response[0].err_msg == "") {
                            alert("success!");
                            $("#divEnterValideNoforward").dialog("close");

                            var sourceN = response[0].afile_origiFileName == "" ? response[0].afile_encryptfileName : response[0].afile_origiFileName;

                            $.fileDownload("../handler/filedownload.ashx?filename=" + response[0].afile_encryptfileName + "&source=" +
                            sourceN + "&afileID=" + response[0].afile_id + "&exten=" + response[0].afile_exten + "&aid=" + response[0].sender_id +
                            "&vailde=" + $("#tbxValideNoforward").val() + "&notifyflag=" + response[0].sender_notifyflag);
                        }
                        else {
                            alert(response[0].err_msg);
                            return;
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(thrownError);
                        return;
                    }
                });
            });

            $(document).on("click", "#btnDownLoadAll", function (event) {
                if ($(this).attr("comorsec") == "common") {
                    //一般下載
                    $.fileDownload("handlerEn/filedownloadEn.ashx?filename=" + $(this).attr("filename") + "&source=" + $(this).attr("source") + "&afileID=" + $(this).attr("afileID") + "&exten=" + $(this).attr("exten") + "&aid=" + $(this).attr("aid") + "&vailde=&notifyflag=");
                }
                else if ($(this).attr("comorsec") == "nonforward") {
                    aid = $(this).attr("aid");
                    afileID = $(this).attr("afileID");
                    //不可轉寄
                    $.ajax({
                        type: "POST",
                        data: {
                            aid: $(this).attr("aid")
                        },
                        url: "handlerEn/nonforwardEn.ashx",
                        success: function (response) {
                            if (response == "0") {
                                //表示還未取過檔 
                                alert("This is your first time to receive files, the CAPTCHA will be sent to your mailbox.\r\n\r\nPlease enter the authentication code after you receive a mail which contains CAPTCHA.");
                            }
                            else if (response == "1") {
                                alert("This CAPTCHA can be used in 30 minutes,Please enter the new CAPTCHA which just sent to your mailbox.");
                            }

                            $("#divEnterValideNoforward").dialog("open");
                            $("#loginBtnNoforward").focus();
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(thrownError);
                            return;
                        }
                    });
                }

            });
        });
    </script>
<script type="text/javascript">
    $(function () {
        $("#divEnterValide,#divEnterValideNoforward").dialog({
            modal: true,
            position: ["center", 100],
            width: 280,
            autoOpen: false,
            show: {
                duration: 300
            },
            hide: {
                duration: 300
            },
            closeOnEscape: false //讓Esc可以關掉視窗
        });
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="lineheight03">
    <table width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td style="width:10%">&nbsp;</td>
            <td>
                <br />
                Dear: <br />
                The files listed below are sent by <asp:Label ID="lbName" runat="server" ForeColor="#4bafe3"></asp:Label>&lt;<asp:Label ID="lbemail" runat="server" ForeColor="#4bafe3"></asp:Label>
                &gt;
                at
                <asp:Label ID="lbCreatedate" runat="server" ForeColor="#4bafe3"></asp:Label>
                .These files will be deleted at 
                <asp:Label ID="lbdeldate" runat="server" ForeColor="#4bafe3"></asp:Label>
                .<br />
                Please download your files by clicking on the 'Retrieve' button below.
                <br />Contact us if you have any problem.
            </td>
             <td style="width:10%">&nbsp;</td>
        </tr>
    </table>
    <table width="100%" cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td style="width: 10%">
            &nbsp;
        </td>
        <td>
        <h3>Subject &amp; Description:</h3>
        </td>
        <td style="width: 10%">
            &nbsp;
        </td>
    </tr>
        <tr>
            <td style="width: 10%">
                &nbsp;
            </td>
            <td align="center">
                <span class="stripeMe">
                <table width="50%" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <th style="width:40%">
                        Subject:
                        </th>
                        <td>
                            <asp:Label ID="lbTitle" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th>
                        Description:
                        </th>
                        <td>
                            <asp:Label ID="lbdesc" runat="server"></asp:Label>
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
    <table width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td style="width: 10%">
                &nbsp;
            </td>
            <td>
                <h3>
                    File List:</h3>
            </td>
            <td style="width: 10%">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="width: 10%">
                &nbsp;
            </td>
            <td align="center">
                <span class="stripeMe">
                    <asp:GridView ID="gvfilelist" runat="server" AutoGenerateColumns="false" 
                     onrowdatabound="gvfilelist_RowDataBound" Width="95%">
                        <Columns>
                            <asp:BoundField DataField="afile_id" HeaderText="afile_id" />
                            <asp:BoundField DataField="afile_parentid" HeaderText="afile_parentid" />
                            <asp:BoundField DataField="afile_exten" HeaderText="afile_exten" />
                            <asp:BoundField DataField="afile_comorsec" HeaderText="afile_comorsec" />
                            <asp:BoundField DataField="afile_stat" HeaderText="afile_stat" />
                            <asp:BoundField DataField="ShowFileName" HeaderText="File Name" />
                            <asp:BoundField DataField="afile_size" HeaderText="File Size" />
                            <asp:TemplateField HeaderText="Download">
                                <ItemTemplate>
                                    <asp:Panel ID="PanelGetFile" runat="server">
                                        <input id="Button1" type="button" value="Retrieve" name="ButtonGetFile" sendid='<%# Eval("sender_id") %>'
                                            notifyflag='<%# Eval("sender_notifyflag") %>' sender_disabled='<%# Eval("sender_disabled") %>'
                                            afile_id='<%# Eval("afile_id") %>' afile_parentid='<%# Eval("afile_parentid") %>'
                                            afile_exten='<%# Eval("afile_exten") %>' afile_comorsec='<%# Eval("afile_comorsec") %>'
                                            class="btn_mouseout" onmouseover="this.className='btn_mouseover'" onmouseout="this.className='btn_mouseout'" />
                                    </asp:Panel>
                                    <asp:Panel ID="PanelShowFileFail" runat="server">
                                        (Fail)
                                    </asp:Panel>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ReSend Password">
                                <ItemTemplate>
                                    <input id="btnReSend" type="button" value="ReSend" name="ButtonReSend" sendid='<%# Eval("sender_id") %>' afile_id='<%# Eval("afile_id") %>' 
                                    class="btn_mouseout" onmouseover="this.className='btn_mouseover'"
                                        onmouseout="this.className='btn_mouseout'" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </span>
                <asp:Panel ID="PaneldownLoadAll" runat="server" >
                </asp:Panel>
                <br />
                <asp:Panel ID="PanelDelUrl" runat="server" CssClass="DelDiv" >
                <p align="center">
                    <asp:Button ID="btnDelUrl" runat="server" Text="Delete this Download URL"  OnClientClick="javascript:return confirm('If you delete this download URL,NOBODY can get file from this URL! \r\n (Please see the description under [Notice] in this page.) \r\n Are you sure to DELETE?');"
                        onclick="btnDelUrl_Click" />
                    <asp:Button ID="btnReSendValidCode" runat="server" Text="Resend authentication code mail" onclick="btnReSendValidCode_Click" />
                </p>
                <p align="left" style="color:#cc3333">
                ※Notice: 
                </p>
                <div align="left">
                    <ul>
                        <li><span class="font-red">After deleting the URL, you will never download files through this URL.</span> If you want the download url again, please contact the sender for restorin this URL or re-sending the file.</li>
                        <li>Normaly, this download URL will be deleted after <span class="font-red">7 days</span> with all files in this URL. You should click this button ONLY when you have to delete this download URL IMMEDIATELY.</li>
                    </ul>       
                </div>
                </asp:Panel>
                
            </td>       
            <td style="width: 10%">
                &nbsp;
            </td>
        </tr>
    </table>

    <div id="divEnterValide" title="Enter CAPTCHA">
        <table width="100%" border="0" cellspacing="3" cellpadding="3">
            <tr>
                <td style="width:30%">
                    CAPTCHA:
                </td>
                <td>
                    <input type="text" id="tbxValide" size="4" style="width:100px"  />
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                Please enter the mail of the number in the CAPTCHA
                </td>
            </tr>
            <tr>
                <td align="right" colspan="2">
                    <input id="loginBtn" type="button" class="btn_mouseout" onmouseover="this.className='btn_mouseover'"
                        onmouseout="this.className='btn_mouseout'" value="Submit" />
                </td>
            </tr>
        </table>
    </div>

        <div id="divEnterValideNoforward" title="Enter CAPTCHA">
        <table width="100%" border="0" cellspacing="3" cellpadding="3">
            <tr>
                <td style="width:30%">
                    CAPTCHA:
                </td>
                <td>
                    <input type="text" id="tbxValideNoforward" size="4" style="width:100px"  />
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                Please enter the mail of the number in the CAPTCHA
                </td>
            </tr>
            <tr>
                <td align="right" colspan="2">
                    <input id="loginBtnNoforward" type="button" class="btn_mouseout" onmouseover="this.className='btn_mouseover'"
                        onmouseout="this.className='btn_mouseout'" value="Submit" />
                </td>
            </tr>
        </table>
    </div>

</div>
</asp:Content>
