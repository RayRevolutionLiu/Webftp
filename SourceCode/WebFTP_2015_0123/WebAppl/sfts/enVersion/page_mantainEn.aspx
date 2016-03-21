<%@ Page Title="" Language="C#" MasterPageFile="~/sfts/enVersion/MasterPageEn.Master" AutoEventWireup="true" CodeBehind="page_mantainEn.aspx.cs" Inherits="WebAppl.sfts.enVersion.page_mantainEn" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    $(document).ready(function () {
        $(document).on("click", ".toggplus", function (event) {
            if ($(this).attr("src").indexOf("bullet_toggle_plus") != -1) {
                $(this).attr("src", "../layout/images/bullet_toggle_minus.png");
                $(this).parents("table .OpenUpTable").find("div").attr("style", "");
                var divShow = $(this).parents("table .OpenUpTable").find("div");
                var parentid = $(this).attr("parentid");
                var imgid = $(this).attr("id");
                if (imgid != "maindesc") {
                    //打開
                    $.ajax({
                        type: "POST",
                        data: {
                            parentid: parentid,
                            imgid: imgid
                        },
                        url: "../handler/mantain.ashx",
                        dataType: "json",
                        success: function (response) {
                            var appendStr;
                            if (response.length <= 0) {
                                appendStr = "No Records";
                            }
                            else {
                                divShow.children().remove();
                                appendStr = "<span class='font-size2 underlineClass'><table width='100%' cellpadding='0' cellspacing='0' class='OpenUpTable'>";
                                if (imgid == "fileOpen") {
                                    for (var i = 0; i < response.length; i++) {
                                        appendStr += "<tr><td width='20px' style='font-weight:bold'>" + parseInt(i + 1) + ".</td><td>" + response[i].fileName + "</td>";
                                        if (response[i].main_stat != "N") {//如果後台此筆沒有被刪除才可以出現圖片
                                            if (response[i].stat == "Y") {
                                                appendStr += "<td width='1px'><img src='../layout/images/checkbox_no.png' style='cursor:pointer' title='disabled' class='imgcheckbox' name='NamefileOpen' afileid='" + response[i].id + "' /></td></tr>";
                                            }
                                            else {
                                                appendStr += "<td width='1px'><img src='../layout/images/checkbox_yes.png' style='cursor:pointer' title='enabled' class='imgcheckbox' name='NamefileOpen' afileid='" + response[i].id + "' /></td></tr>";
                                            }
                                        }
                                    }
                                    appendStr += "</table></span>";
                                    divShow.append(appendStr);
                                }
                                if (imgid == "senderOpen") {
                                    for (var i = 0; i < response.length; i++) {
                                        appendStr += "<tr><td width='40%'>" + response[i].email + "</td><td  width='20%'>Attempt " + response[i].trytimes + " download</td><td width='39%'>last time " + response[i].lasttrydate + "</td>";
                                        if (response[i].main_stat != "N") {//如果後台此筆沒有被刪除才可以出現圖片
                                            if (response[i].queryenable == "N") {
                                                appendStr += "<td width='1px'><img src='../layout/images/checkbox_yes.png' style='cursor:pointer' title='enabled' class='imgcheckbox' name='NamesenderOpen' senderid='" + response[i].id + "' /></td></tr>";
                                            }
                                            else {
                                                appendStr += "<td width='1px'><div style='display:none' name='loadgif'><img src='../layout/images/ajax-loader.gif' /></div><img src='../layout/images/mail_post_to3.png' style='cursor:pointer' title='Resend' class='imgReSend' name='NameReSend' senderid='" + response[i].id + "' email='" + response[i].email + "' /></td>";
                                                appendStr += "<td width='1px'><img src='../layout/images/checkbox_no.png' style='cursor:pointer' title='disabled' class='imgcheckbox' name='NamesenderOpen' senderid='" + response[i].id + "' /></td></tr>";
                                            }
                                        }
                                    }
                                    appendStr += "</table>";
                                    divShow.append(appendStr);
                                }
                            }
                            //                        $(".imgcheckbox").tooltip(); //後面勾勾的提示
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(thrownError);
                            return;
                        }
                    });
                }
            }
            else {
                $(this).attr("src", "../layout/images/bullet_toggle_plus.png");
                $(this).parents("table .OpenUpTable").find("div").attr("style", "display:none");
                //關閉
            }
        });

        $(document).on("click", "img[name='NamefileOpen']", function (event) {
            var thisEvn = $(this);
            var id = thisEvn.attr("afileid");
            var value;
            if (thisEvn.attr("src").indexOf("checkbox_yes") != -1) {
                value = "Y";
            }
            else {
                value = "N";
            }
            $.ajax({
                type: "POST",
                data: {
                    nameType: "NamefileOpen",
                    id: id,
                    value: value
                },
                url: "../handler/cbxChangeImg.ashx",
                success: function (response) {
                    if (thisEvn.attr("src").indexOf("checkbox_yes") != -1) {
                        thisEvn.attr("src", "../layout/images/checkbox_no.png");
                        thisEvn.attr("title", "disabled");
                    }
                    else {
                        thisEvn.attr("src", "../layout/images/checkbox_yes.png");
                        thisEvn.attr("title", "enabled");
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(thrownError);
                    return;
                }
            });
        });

        $(document).on("click", "img[name='NamesenderOpen']", function (event) {
            var thisEvn = $(this);
            var id = thisEvn.attr("senderid");
            var value;
            if (thisEvn.attr("src").indexOf("checkbox_yes") != -1) {
                value = "Y";
            }
            else {
                value = "N";
            }
            $.ajax({
                type: "POST",
                data: {
                    nameType: "NamesenderOpen",
                    id: id,
                    value: value
                },
                url: "../handler/cbxChangeImg.ashx",
                async: false,
                success: function (response) {
                    if (thisEvn.attr("src").indexOf("checkbox_yes") != -1) {
                        thisEvn.attr("src", "../layout/images/checkbox_no.png").attr("title", "disabled");
                        thisEvn.parent().parent().find("img[class='imgReSend']").css("display", "");
                    }
                    else {
                        thisEvn.attr("src", "../layout/images/checkbox_yes.png").attr("title", "enabled");
                        thisEvn.parent().parent().find("img[class='imgReSend']").css("display", "none");
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(thrownError);
                    return;
                }
            });
        });

        $(document).on("click", "img[name='NameReSend']", function (event) {
            var thisEvent = $(this);
            var senderid = thisEvent.attr("senderid");
            if (window.confirm("Are you sure to Re-send notiffication mail to " + thisEvent.attr("email") + "?")) {
                $.ajax({
                    type: "POST",
                    data: {
                        senderid: senderid
                    },
                    url: "handlerEn/ReSendMailEn.ashx",
                    success: function (response) {
                        alert("Success");
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert("File being processed ,Try it later");
                        return;
                    },
                    beforeSend: function () {
                        thisEvent.parent().find("div[name='loadgif']").show();
                        thisEvent.hide();
                    },
                    complete: function () {
                        thisEvent.parent().find("div[name='loadgif']").hide();
                        thisEvent.show();
                    }
                });
            }
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
                Dear <asp:Label ID="lbName" runat="server" ForeColor="#4bafe3"></asp:Label>
                 &nbsp;your transfer data in 7 days are listed below:<br />
&nbsp;</td>
             <td style="width:10%">&nbsp;</td>
        </tr>
    </table>
            <span class="stripeMe">
                <asp:GridView ID="gvmainlist" runat="server" AutoGenerateColumns="false" 
                onrowdatabound="gvmainlist_RowDataBound" Width="98%">
                    <Columns>
                        <asp:BoundField DataField="main_id" HeaderText="main_id" />
                        <asp:BoundField DataField="main_infno" HeaderText="main_infno" />
                        <asp:BoundField DataField="main_parentid" HeaderText="main_parentid" />
                        <asp:BoundField DataField="main_isempno" HeaderText="main_isempno" />
                        <asp:BoundField DataField="main_stat" HeaderText="main_stat" />
                        <asp:BoundField DataField="main_title" HeaderText="Subject" />
                        <asp:BoundField DataField="main_createdate" HeaderText="Send Time" DataFormatString="{0:r}" HeaderStyle-Width="15%" />
                        <asp:TemplateField HeaderText="Message brief" HeaderStyle-Width="63%">
                            <ItemTemplate>
                                <span class="tableInside">
                                    <table width="100%" cellpadding="0" cellspacing="0" class="OpenUpTable">
                                        <tr>
                                            <td style="width: 1px">
                                                <img src="../layout/images/bullet_toggle_plus.png" class="toggplus" id="maindesc" style="cursor:pointer" />
                                            </td>
                                            <td>
                                                Description
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div style="display: none"><%# Eval("main_desc")%></div>
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="100%" cellpadding="0" cellspacing="0" class="OpenUpTable">
                                        <tr>
                                            <td style="width:1px">
                                                <img src="../layout/images/bullet_toggle_plus.png" class="toggplus" id="fileOpen" parentid='<%# Eval("main_parentid") %>' style="cursor:pointer" />
                                            </td>
                                            <td align="left">
                                                File List
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div style="display: none"></div>
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="100%" cellpadding="0" cellspacing="0" class="OpenUpTable">
                                        <tr>
                                            <td style="width: 1px">
                                                <img src="../layout/images/bullet_toggle_plus.png" class="toggplus" id="senderOpen" parentid='<%# Eval("main_parentid") %>' style="cursor:pointer" />
                                            </td>
                                            <td>
                                                 Receiver List
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div style="display: none"></div>
                                            </td>
                                        </tr>
                                    </table>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Delete">
                            <ItemTemplate>
                                <asp:Button ID="btnDel" runat="server" Text="Delete" OnClientClick="javascript:return confirm('Do you want to delete?The data can NOT be restored after you delete it');" OnClick="btnDel_Click" />
                                <asp:Panel ID="Panel1" runat="server" Visible="false">
                                [<%# Eval("main_deletedate") %>Delete]
                                </asp:Panel>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </span>
            <br />
        <div class="DelDiv">
            <p align="left" style="color: #cc3333">
                ※說明：
            </p>
            <div align="left">
                <ul>
                    <li>If you want to disable any Retrieve URL or any file, press the button<img alt="有效" src="../layout/images/checkbox_no.png" />(enabled)，to change it to<img alt="有效" src="../layout/images/checkbox_yes.png" />(disabled)。</li>
                    <li>If you want to completely delete any transfer record include all receivers and files, press the button <span class="font-red">Delete</span>， beside. Notice: The data can <span class="font-red">NOT</span> be restored after you delete it!!</li>
                </ul>
            </div>
        </div>
    </div>
</asp:Content>
