<%@ Page Title="" Language="C#" MasterPageFile="~/sfts/enVersion/MasterPageEn.Master" AutoEventWireup="true" CodeBehind="page_mantainallEn.aspx.cs" Inherits="WebAppl.sfts.enVersion.page_mantainallEn" %>
<%@ Register src="../UserControl/PagerEn.ascx" tagname="PagerEn" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="lineheight03">
    <table width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td style="width:20%">&nbsp;</td>
            <td>
                <br />
                Check all your transfer information：
                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td width="90px">KeyWord：(Subject&Description)
                        </td>
                        <td style="padding:0 0 5px 0;"><asp:TextBox ID="tbxkeyword" runat="server" 
                                Width="233px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td width="90px">SendDate：
                        </td>
                        <td><asp:TextBox ID="tbxstartdate" runat="server" Width="100px" CssClass="datepicker"></asp:TextBox>～<asp:TextBox 
                                ID="tbxenddate" runat="server" Width="100px" CssClass="datepicker"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                    <td colspan="2" align="right">
                        <asp:Button ID="Button1" runat="server" Text="submit" onclick="Button1_Click" checkbtn="true" />
                    </td>
                    </tr>
                </table>
            </td>
             <td style="width:13%">&nbsp;</td>
        </tr>
    </table>
    <br />
            <span class="stripeMe">
            <uc1:PagerEn ID="PagerEn1" runat="server" />
                <asp:GridView ID="gvmainlist" runat="server" AutoGenerateColumns="false" 
                onrowdatabound="gvmainlist_RowDataBound" Width="98%" PagerSettings-Visible="false" PageSize="10" AllowPaging="true">
                    <Columns>
                        <asp:BoundField DataField="main_id" HeaderText="main_id" />
                        <asp:BoundField DataField="main_infno" HeaderText="main_infno" />
                        <asp:BoundField DataField="main_parentid" HeaderText="main_parentid" />
                        <asp:BoundField DataField="main_isempno" HeaderText="main_isempno" />
                        <asp:BoundField DataField="main_stat" HeaderText="main_stat" />
                        <asp:BoundField DataField="main_title" HeaderText="Subject" />
                        <asp:BoundField DataField="main_createdate" HeaderText="Send Time" DataFormatString="{0:f}" HeaderStyle-Width="19%" />
                        <asp:TemplateField HeaderText="Message brief" HeaderStyle-Width="55%">
                            <ItemTemplate>
                                <span class="tableInside">
                                    <table width="100%" cellpadding="0" cellspacing="0" class="OpenUpTable">
                                        <tr>
                                            <td style="width: 1px">
                                                <img src="../layout/images/bullet_toggle_minus.png" class="toggplus" id="maindesc" />
                                            </td>
                                            <td>
                                                Description
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div><%# Eval("main_desc")%></div>
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="100%" cellpadding="0" cellspacing="0" class="OpenUpTable">
                                        <tr>
                                            <td style="width:1px">
                                                <img src="../layout/images/bullet_toggle_minus.png" class="toggplus" id="fileOpen" parentid='<%# Eval("main_parentid") %>' />
                                            </td>
                                            <td align="left">
                                                File List
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Panel runat="server" ID="panelfile"></asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="100%" cellpadding="0" cellspacing="0" class="OpenUpTable">
                                        <tr>
                                            <td style="width: 1px">
                                                <img src="../layout/images/bullet_toggle_minus.png" class="toggplus" id="senderOpen" parentid='<%# Eval("main_parentid") %>' />
                                            </td>
                                            <td>
                                                Receiver List
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Panel runat="server" ID="panelsender"></asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <%--<uc1:PagerEn ID="PagerEn1" runat="server" />--%>
            </span>
    </div>

<script>
    $(function () {
        $(".datepicker").datepicker({
            showOn: "button",
            buttonImage: "../AllJqueryUI/jquery-ui-1.10.0/demos/images/calendar.gif",
            buttonImageOnly: true,
            buttonText: "Select date",
            dateFormat: "yy/mm/dd"
        });

        $("input[checkbtn='true']").click(function () {
            var startdate = $(".datepicker").val().match(/^\d{4}\/\d{2}\/\d{2}$/);
            if ($(".datepicker").val() != "" && startdate === null) {
                alert('Datetime format error. ex:("2014/10/12")');
                return false;
            }

            if ($("#<% =tbxstartdate.ClientID %>").val() != "" && $("#<% =tbxenddate.ClientID %>").val() == "") {
                alert("Please enter end date.");
                return false;
            }
            if ($("#<% =tbxstartdate.ClientID %>").val() == "" && $("#<% =tbxenddate.ClientID %>").val() != "") {
                alert("Please enter start date.");
                return false;
            }
        });
    });
</script>
</asp:Content>
