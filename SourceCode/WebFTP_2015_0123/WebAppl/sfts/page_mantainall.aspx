<%@ Page Title="" Language="C#" MasterPageFile="~/sfts/MasterPage.Master" AutoEventWireup="true" CodeBehind="page_mantainall.aspx.cs" Inherits="WebAppl.sfts.page_mantainall" %>
<%@ Register src="UserControl/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="lineheight03">
    <table width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td style="width:20%">&nbsp;</td>
            <td>
                <br />
                查詢您所有傳輸資料：
                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td width="90px">關鍵字：(主旨&內文)
                        </td>
                        <td style="padding:0 0 5px 0;"><asp:TextBox ID="tbxkeyword" runat="server" 
                                Width="233px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td width="90px">寄信日期：
                        </td>
                        <td><asp:TextBox ID="tbxstartdate" runat="server" Width="100px" CssClass="datepicker"></asp:TextBox>～<asp:TextBox 
                                ID="tbxenddate" runat="server" Width="100px" CssClass="datepicker"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                    <td colspan="2" align="right">
                        <asp:Button ID="Button1" runat="server" Text="查詢" onclick="Button1_Click" checkbtn="true" />
                    </td>
                    </tr>
                </table>
            </td>
             <td style="width:13%">&nbsp;</td>
        </tr>
    </table>
    <br />
            <span class="stripeMe">
            <uc1:Pager ID="Pager1" runat="server" />
                <asp:GridView ID="gvmainlist" runat="server" AutoGenerateColumns="false" 
                onrowdatabound="gvmainlist_RowDataBound" Width="98%" PagerSettings-Visible="false" PageSize="10" AllowPaging="true">
                    <Columns>
                        <asp:BoundField DataField="main_id" HeaderText="main_id" />
                        <asp:BoundField DataField="main_infno" HeaderText="main_infno" />
                        <asp:BoundField DataField="main_parentid" HeaderText="main_parentid" />
                        <asp:BoundField DataField="main_isempno" HeaderText="main_isempno" />
                        <asp:BoundField DataField="main_stat" HeaderText="main_stat" />
                        <asp:BoundField DataField="main_title" HeaderText="主旨" />
                        <asp:BoundField DataField="main_createdate" HeaderText="寄信時間" DataFormatString="{0:f}" HeaderStyle-Width="19%" />
                        <asp:TemplateField HeaderText="訊息節錄" HeaderStyle-Width="55%">
                            <ItemTemplate>
                                <span class="tableInside">
                                    <table width="100%" cellpadding="0" cellspacing="0" class="OpenUpTable">
                                        <tr>
                                            <td style="width: 1px">
                                                <img src="../layout/images/bullet_toggle_minus.png" class="toggplus" id="maindesc" />
                                            </td>
                                            <td>
                                                留言內容
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
                                                檔案列表
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
                                                收件人列表
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
                <%--<uc1:Pager ID="Pager1" runat="server" />--%>
            </span>
    </div>
  <script>
      $(function () {
          $(".datepicker").datepicker({
              showOn: "button",
              buttonImage: "AllJqueryUI/jquery-ui-1.10.0/demos/images/calendar.gif",
              buttonImageOnly: true,
              buttonText: "Select date",
              dateFormat: "yy/mm/dd"
          });

          $("input[checkbtn='true']").click(function () {
              var startdate = $(".datepicker").val().match(/^\d{4}\/\d{2}\/\d{2}$/);
              if ($(".datepicker").val() != "" && startdate === null) {
                  alert('日期格式錯誤 ex:("2014/10/12")');
                  return false;
              }

              if ($("#<% =tbxstartdate.ClientID %>").val() != "" && $("#<% =tbxenddate.ClientID %>").val() == "") {
                  alert("請輸入結束日期");
                  return false;
              }
              if ($("#<% =tbxstartdate.ClientID %>").val() == "" && $("#<% =tbxenddate.ClientID %>").val() != "") {
                  alert("請輸入開始日期");
                  return false;
              }
          });
      });
  </script>
</asp:Content>
