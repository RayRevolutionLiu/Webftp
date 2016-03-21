<%@ Page Title="" Language="C#" MasterPageFile="~/sfts/MasterPage.Master" AutoEventWireup="true" CodeBehind="Veryfly.aspx.cs" Inherits="WebAppl.Veryfly" %>
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
                    url: "handler/downloadfile.ashx",
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
                                alert("此筆資料寄件者已刪除，檔案下載失敗");
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
                                url: "handler/nonforward.ashx",
                                success: function (response) {
                                    if (response == "0") {
                                        //表示還未取過檔 
                                        alert("由於您第一次取檔，系統將會寄送一封含有認證碼的信件至您的信箱\r\n\r\n請收取信件後再輸入信件內認證碼圖片之數字");
                                    }
                                    else if (response == "1") {
                                        alert("由於取檔密碼時效為30分鐘，如今已超越30分鐘，系統將再寄含有新認證碼的信件至您的信箱\r\n\r\n請收取信件後再輸入信件內認證碼圖片之數字");
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
                    url: "handler/ReSendEmail.ashx",
                    success: function (response) {
                        if (response == "OK") {
                            alert("解壓縮密碼已重新發送至您的信箱，請收信查看");
                        }
                        else if (response == "NO_downLoad") {
                            alert("尚未下載檔案");
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
                    url: "handler/Vaild.ashx",
                    success: function (response) {
                        alert(response);
                        if (response == "驗證成功!確認下載完成之後，解壓縮密碼會寄到您的信箱內") {
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
                    url: "handler/VaildNoforward.ashx",
                    success: function (response) {
                        if (response[0].err_msg == "") {
                            alert("驗證成功!");
                            $("#divEnterValideNoforward").dialog("close");

                            var sourceN = response[0].afile_origiFileName == "" ? response[0].afile_encryptfileName : response[0].afile_origiFileName;

                            $.fileDownload("handler/filedownload.ashx?filename=" + response[0].afile_encryptfileName + "&source=" +
                            sourceN +"&afileID=" + response[0].afile_id + "&exten=" + response[0].afile_exten + "&aid=" + response[0].sender_id +
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
                    $.fileDownload("handler/filedownload.ashx?filename=" + $(this).attr("filename") + "&source=" + $(this).attr("source") + "&afileID=" + $(this).attr("afileID") + "&exten=" + $(this).attr("exten") + "&aid=" + $(this).attr("aid") + "&vailde=&notifyflag=");
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
                        url: "handler/nonforward.ashx",
                        success: function (response) {
                            if (response == "0") {
                                //表示還未取過檔 
                                alert("由於您第一次取檔，系統將會寄送一封含有認證碼的信件至您的信箱\r\n\r\n請收取信件後再輸入信件內認證碼圖片之數字");
                            }
                            else if (response == "1") {
                                alert("由於取檔密碼時效為30分鐘，如今已超越30分鐘，系統將再寄含有新認證碼的信件至您的信箱\r\n\r\n請收取信件後再輸入信件內認證碼圖片之數字");
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
                您好：<br />
                <asp:Label ID="lbName" runat="server" ForeColor="#4bafe3"></asp:Label>&lt;<asp:Label ID="lbemail" runat="server" ForeColor="#4bafe3"></asp:Label>
                &gt;君在
                <asp:Label ID="lbCreatedate" runat="server" ForeColor="#4bafe3"></asp:Label>
                寄給您的檔案列表如下，這些檔案將在
                <asp:Label ID="lbdeldate" runat="server" ForeColor="#4bafe3"></asp:Label>
                刪除。<br />
                &nbsp;請您儘快利用下列的 [取檔] 按鈕來取回您的檔案，謝謝您！
                如果您有任何使用上的問題，歡迎您與我們連絡。
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
        <h3>主旨及留言內容：</h3>
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
                        主旨：
                        </th>
                        <td>
                            <asp:Label ID="lbTitle" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th>
                        留言內容：
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
                    檔案下載：</h3>
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
                            <asp:BoundField DataField="ShowFileName" HeaderText="檔案名稱" />
                            <asp:BoundField DataField="afile_size" HeaderText="檔案大小" />
                            <asp:TemplateField HeaderText="下載檔案">
                                <ItemTemplate>
                                    <asp:Panel ID="PanelGetFile" runat="server">
                                        <input id="Button1" type="button" value="取檔" name="ButtonGetFile" sendid='<%# Eval("sender_id") %>'
                                            notifyflag='<%# Eval("sender_notifyflag") %>' sender_disabled='<%# Eval("sender_disabled") %>'
                                            afile_id='<%# Eval("afile_id") %>' afile_parentid='<%# Eval("afile_parentid") %>'
                                            afile_exten='<%# Eval("afile_exten") %>' afile_comorsec='<%# Eval("afile_comorsec") %>'
                                            class="btn_mouseout" onmouseover="this.className='btn_mouseover'" onmouseout="this.className='btn_mouseout'" />
                                    </asp:Panel>
                                    <asp:Panel ID="PanelShowFileFail" runat="server">
                                        (檔案已失效)
                                    </asp:Panel>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="重寄解壓密碼">
                                <ItemTemplate>
                                    <input id="btnReSend" type="button" value="重寄" name="ButtonReSend" sendid='<%# Eval("sender_id") %>' afile_id='<%# Eval("afile_id") %>' 
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
                    <asp:Button ID="btnDelUrl" runat="server" Text="刪除此取檔網址"  OnClientClick="javascript:return confirm('刪除後任何人都將無法經由此取檔網址取檔（請詳見本頁面「※請注意」下之說明）\r\n \r\n 您確定要刪除此取檔網址嗎？');"
                        onclick="btnDelUrl_Click" />
                    <asp:Button ID="btnReSendValidCode" runat="server" Text="重發認證碼" onclick="btnReSendValidCode_Click" />
                </p>
                <p align="left" style="color:#cc3333">
                ※請注意： 
                </p>
                <div align="left">
                    <ul>
                        <li><span class="font-red">刪除後您將無法經由此取檔網址取檔</span>，若想要回覆取檔網址，請和原傳檔者聯繫，請傳檔者重新啟動此網址，或重新傳送檔案。</li>
                        <li>一般狀況下此取檔網址會在<span class="font-red">7天</span>後被一併刪除，若您必須立即刪除此取檔網址才需要使用這項手動刪除功能。</li>
                    </ul>       
                </div>
                </asp:Panel>
                
            </td>       
            <td style="width: 10%">
                &nbsp;
            </td>
        </tr>
    </table>

    <div id="divEnterValide" title="輸入驗證碼">
        <table width="100%" border="0" cellspacing="3" cellpadding="3">
            <tr>
                <td style="width:30%">
                    驗證碼:
                </td>
                <td>
                    <input type="text" id="tbxValide" size="4" style="width:100px"  />
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                請輸入信件內認證碼圖片之數字
                </td>
            </tr>
            <tr>
                <td align="right" colspan="2">
                    <input id="loginBtn" type="button" class="btn_mouseout" onmouseover="this.className='btn_mouseover'"
                        onmouseout="this.className='btn_mouseout'" value="確定" />
                </td>
            </tr>
        </table>
    </div>

        <div id="divEnterValideNoforward" title="輸入驗證碼">
        <table width="100%" border="0" cellspacing="3" cellpadding="3">
            <tr>
                <td style="width:30%">
                    驗證碼:
                </td>
                <td>
                    <input type="text" id="tbxValideNoforward" size="4" style="width:100px"  />
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                請輸入信件內認證碼圖片之數字
                </td>
            </tr>
            <tr>
                <td align="right" colspan="2">
                    <input id="loginBtnNoforward" type="button" class="btn_mouseout" onmouseover="this.className='btn_mouseover'"
                        onmouseout="this.className='btn_mouseout'" value="確定" />
                </td>
            </tr>
        </table>
    </div>
</div>
</asp:Content>
