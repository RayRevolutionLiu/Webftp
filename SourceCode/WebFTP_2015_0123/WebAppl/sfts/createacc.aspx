<%@ Page Title="" Language="C#" MasterPageFile="~/sfts/MasterPage.Master" AutoEventWireup="true" CodeBehind="createacc.aspx.cs" Inherits="WebAppl.createacc" %>
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
                    url: "handler/registerAcc.ashx",
                    type: 'POST',
                    data: {
                        tbxNameQ: tbxName,
                        tbxAccountQ: tbxAccount,
                        typeQ: type
                    },
                    error: function (xhr,ajaxOptions,thrownError) {
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
                alert("您並未同意使用規約 無法接受您的申請");
            }
        });

        $("#divSuccess").dialog({
            autoOpen: false,
            height: 240,
            width: 570,
            modal: true,
            draggable: false, //可拖動 
            closeOnEscape: true, //讓Esc可以關掉視窗
            close: function (event, ui) { window.location.href = "../sfts/Default.aspx"; }
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
                <h3>申請帳號</h3>
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
                        <input id="tbxName" type="text" maxlength="35" /><span class="font-red">*</span></td>
                </tr>
                <tr>
                    <td style="width: 40%" align="right">電子郵件：</td>
                    <td align="left">
                        <input id="tbxAccount" type="text" maxlength="60" /><span class="font-red">*</span></td>
                </tr>
                <tr>
                <td colspan="2">
                    <br />
                    <div  style="width:98%">
                        <p align="left" style="color: #cc3333">
                            使用規約及隱私說明：
                        </p>
                        <div align="left">
                            <ul>
                                <li>本服務只提供傳送公務上的電子檔案之用，申請者不得使用本服務傳送非公務用之其他檔案。</li>
                                <li>申請者不得利用此服務傳送未授權的軟體。</li>
                                <li>申請者不得利用各種途徑，破壞本系統，一旦發現將停止使用權限。</li>
                                <li>個人資料與隱私保護聲明：本服務由工研院提供，帳號申請時系統將蒐集您的姓名及電子郵件地址，僅用以使傳送檔案之功能正常運作，若您不願意提供資料，則本系統將無法使用。若您希望停止使用本系統並將已存在系統的資料消除，請洽服務窗口03-5918899。</li>
                            </ul>
                        </div>
                    </div>
                </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <input type="radio" name="type" value="agree"/>&nbsp;我同意&nbsp;&nbsp;<input
                            type="radio" name="type" value="disagree" checked="checked" />&nbsp;我不同意
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="right">
                        <input id="btnsubmit" type="button" value="送出" class="btn_mouseout" onmouseover="this.className='btn_mouseover'"
                            onmouseout="this.className='btn_mouseout'" />
                        <input id="btnclear" type="button" value="清除資料" class="btn_mouseout" onmouseover="this.className='btn_mouseover'"
                            onmouseout="this.className='btn_mouseout'" />
                    </td>
                </tr>
                <tr>
                <td colspan="2">
                    <br />
                    <div class="DelDiv" style="width:98%">
                        <p align="left" style="color: #cc3333">
                            注意事項：
                        </p>
                        <div align="left">
                            <ul>
                                <li>(<span class="font-red">*</span>)的欄位為必填欄位。</li>
                                <li>若您的「<span class="font-red">電子郵件</span>」非工研院內部信箱，則此服務只提供「<span class="font-red">收檔者</span>」為<span class="font-red"><b>工研院內部員工</b></span>。</li>
                                <li>您的「<span class="font-red">電子郵件</span>」將成為您登入本系統的「使用者帳號」。</li>
                                <li>登入用的資訊將透過您所輸入的「<span class="font-red">電子郵件</span>」通知您。</li>
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

    <div id="divSuccess" title="使用者帳號申請完成" class="font-gray block font-size2">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td colspan="2">
                    親愛的<span style="color:#4bafe3" id="resultName"></span>您好：
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
                    您填寫的資料如下：
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
                                中文姓名：
                            </td>
                            <td>
                                <span style="color:#4bafe3" id="resultNameList"></span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                電子郵件：
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
                                您的帳號將透過 E-mail 寄送到<span style="color:#4bafe3" id="resultEmailList"></span>給您。請您立即檢查您的電子郵件信箱，是否有您的帳號資料，並點擊信件內附帶之網址進入系統驗證。如有任何問題，請與我們連絡。
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
                    <input id="btnback" type="button" value="回到登入畫面" class="btn_mouseout" onmouseover="this.className='btn_mouseover'"
                        onmouseout="this.className='btn_mouseout'" onclick="javascript:location.href='../sfts/Default.aspx'" />
                </td>
            </tr>
        </table>
    </div>

</div>
</asp:Content>
