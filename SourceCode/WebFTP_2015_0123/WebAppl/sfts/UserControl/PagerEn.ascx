<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PagerEn.ascx.cs" Inherits="WebAppl.sfts.UserControl.PagerEn" %>
<div align="center" id="table_pager" runat="server">
    <asp:LinkButton ID="lbtn_first" runat="server" CausesValidation="false" CommandArgument="first"
        OnClick="Query_Click">First</asp:LinkButton>
    <asp:LinkButton ID="lbtn_pre10page" runat="server" Text="|<" CommandArgument="back10page"
        OnClick="Query_Click" CausesValidation="false" />
    <asp:LinkButton ID="lbtn_prepage" runat="server" Text="<" OnClick="Query_Click" CausesValidation="false"
        CommandArgument="backpage" />
    <asp:Repeater ID="rep_page" runat="server" OnItemDataBound="rep_page_ItemDataBound">
        <ItemTemplate>
            <asp:LinkButton ID="lbtn_page" runat="server" CommandArgument='<%#Eval("page") %>'
                OnClick="lbtn_page_Click" CausesValidation="false"><%#Eval("page")%></asp:LinkButton>
        </ItemTemplate>
    </asp:Repeater>
    <asp:LinkButton ID="lbtn_nextpage" runat="server" Text=">" OnClick="Query_Click"
        CausesValidation="false" CommandArgument="Advancepage" />
    <asp:LinkButton ID="lbtn_next10page" runat="server" Text=">|" OnClick="Query_Click"
        CausesValidation="false" CommandArgument="Advance10page" />
    <asp:LinkButton ID="lbtn_last" runat="server" CausesValidation="false" CommandArgument="last"
        OnClick="Query_Click">Last</asp:LinkButton>
    &nbsp;&nbsp;&nbsp;&nbsp;Total pages：<asp:Label ID="lbl_pagecount" runat="server"></asp:Label>
    &nbsp;，Total data：&nbsp;<asp:Label ID="lbl_datacount" runat="server"></asp:Label>&nbsp;
    &nbsp;&nbsp;Move to &nbsp;<asp:DropDownList ID="ddl_gopage" runat="server" AutoPostBack="true"
        OnSelectedIndexChanged="ddl_gopage_SelectedIndexChanged">
    </asp:DropDownList>
    &nbsp;page
</div>
