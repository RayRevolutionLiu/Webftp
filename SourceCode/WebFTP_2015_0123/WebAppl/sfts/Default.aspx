<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebAppl.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>工研院檔案傳輸系統</title>
    <script src="layout/js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="layout/js/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
<script type="text/javascript">
    $(document).ready(function () {
        $(".btnITRI").mouseover(function () {
            $(this).addClass('ITRIover');
            $(".btnFIRM").addClass('FIRMnone');
        }).mouseout(function () {
            $(this).removeClass('ITRIover');
            $(".btnFIRM").removeClass('FIRMnone');
        });
        $(".btnFIRM").mouseover(function () {
            $(this).addClass('FIRMover');
            $(".btnITRI").addClass('ITRInone');
        }).mouseout(function () {
            $(this).removeClass('FIRMover');
            $(".btnITRI").removeClass('ITRInone');
        });
        $(window).keydown(function (event) {
            if (event.keyCode == 13) {
                event.preventDefault();
                return false;
            }
        });

    });
</script>
    <script type="text/javascript">
        $(document).ready(function () {
            $.extend({
                getParamValue: function (paramName) {
                    ///	<summary>
                    ///		Get the value of input parameter from the querystring
                    ///	</summary>
                    ///	<param name="paramName" type="String">The input parameter whose value is to be extracted</param>
                    ///	<returns type="String">The value of input parameter from the querystring</returns>

                    parName = paramName.replace(/[\[]/, '\\\[').replace(/[\]]/, '\\\]');
                    var pattern = '[\\?&]' + paramName + '=([^&#]*)';
                    var regex = new RegExp(pattern);
                    var matches = regex.exec(window.location.href);
                    if (matches == null) return '';
                    else return decodeURIComponent(matches[1].replace(/\+/g, ' '));
                }
            });
        });
    </script>
<script type="text/javascript">
    $(function () {
        $("#dialog").dialog({
            modal: true,
            position: ["center", 100],
            width: 295,
            autoOpen: false,
            show: {
                duration: 300
            },
            hide: {
                duration: 300
            },
            closeOnEscape: false, //讓Esc可以關掉視窗
            close: function () {
                $("#forgetDIV").hide();
                $("#mainDIV").show();
                $('#dialog').dialog({
                    title: '合作夥伴登入'
                });
                window.onkeydown = function (e) {
                    var currKey = 0;
                    e = e || event;
                    currKey = e.keyCode || e.which || e.charCode;
                    //判斷是否按下Enter鍵

                    if (currKey == 13) {
                        //document.getElementById('loginBtn').click();
                    }
                }
            }
        });
        $("#opener").click(function () {
            $("#dialog").dialog("open");
            window.onkeydown = function (e) {
                var currKey = 0;
                e = e || event;
                currKey = e.keyCode || e.which || e.charCode;
                //判斷是否按下Enter鍵

                if (currKey == 13) {
                    document.getElementById('loginBtn').click();
                }
            }
        });
    });
</script>

<script type="text/javascript">
    $(document).ready(function () {
        $("#loginBtn").click(function () {
            if ($.trim($("#tbxUserName").val()) == "" || $.trim($("#tbxPwd").val()) == "") {
                alert("E-mail或密碼請勿空白");
                return;
            }

            $("#loginBtn").attr("disabled", "disabled");

            $.ajax({
                type: "POST",
                url:  "handler/login.ashx",
                data: {
                    tbxUserName: $("#tbxUserName").val(),
                    tbxPwd: $("#tbxPwd").val()
                },
                error: function (xhr) {
                    alert("請輸入正確之帳號及密碼");
                    $("#tbxPwd").val("");
                },
                success: function (response) {
                    if (response == "success") {
                        if ($.getParamValue('en') != "") {
                            location.replace(window.location.href.replace("Default", "Veryfly"));
                            //location.href = "Veryfly.aspx?en=" + $.getParamValue('en');
                        }
                        else {
                            alert("歡迎登入大檔案傳輸系統");
                            location.href = "FileEditPage.aspx";
                        }
                    }
                    else {
                        alert(response);
                        $("#loginBtn").removeAttr("disabled");
                        $("#tbxPwd").val("");
                        return;
                    }
                }
            });
        });

        $("#loginSSO").click(function () {
            //location.replace(window.location.href.replace("Default", "CheckSSO"));
            if ($.getParamValue('en') != "") {
                location.href = "../CheckSSO.aspx?en=" + $.getParamValue('en');
                //                //location.href = "Veryfly.aspx?en=" + $.getParamValue('en');
            }
            else {
                location.href = "../CheckSSO.aspx";
            }
        });

        $("#forgetpassw").click(function () {
            $("#forgetDIV").show();
            $("#mainDIV").hide();
            $('#dialog').dialog({
                title: '忘記密碼'
            });
        });

        $("#btnRemail").click(function () {
            if ($("#tbxforgetAccount").val().replace(/\r\n|\n|\r/g, "") != "") {
                $.ajax({
                    type: "POST",
                    url: "../sfts/handler/ForGetPassW.ashx",
                    data: {
                        tbxforgetAccount: $("#tbxforgetAccount").val()
                    },
                    error: function (xhr) {
                        alert(xhr);
                    },
                    success: function (response) {
                        if (response == "success") {
                            alert("修改密碼信件已送出，請至您的信箱查看");
                            location.href = "../sfts/Default.aspx";
                        }
                        else {
                            alert(response);
                            return;
                        }
                    }
                });
            }
            else {
                alert("請輸入E-mail");
                return;
            }
        });
    })
</script>

    <link href="layout/css/style.css" rel="stylesheet" type="text/css" />
    <link href="layout/css/redmond/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
</head>

<body>
    <form id="form1" runat="server">
    <div class="wrapper-enter">
        <div class="logo">
            <h1>
                工研院檔案傳輸系統</h1>
        </div>
        <a href="javascript:void(0);" target="_self" id="loginSSO">
            <div class="btnITRI ITRIgeneral">
                <h2>
                    工研人登入</h2>
            </div>
        </a><a href="javascript:void(0);" target="_self" id="opener">
            <div class="btnFIRM FIRMgeneral">
                <h2>
                    合作夥伴登入</h2>
            </div>
        </a>
        <div class="text font-lightgray lineheight02">
            本系統提供工研院同仁間或與合作夥伴透過 Web 化介面傳送大型檔案，讓同仁與合作夥伴可輕鬆且有效率的交換電子檔案。<br />
<%--            <a href="http://webftp.itri.org.tw" style="color:Blue; text-decoration:underline;">連結至舊版大檔案傳輸系統 v2.2</a>
            &nbsp;&nbsp;&nbsp;--%>
            <asp:LinkButton ID="lkEnVersion" runat="server" onclick="lkEnVersion_Click" style="color:Blue; text-decoration:underline;">English Version</asp:LinkButton>
            </div>
        <div class="footer font-lightgray">
            【工研院大檔案傳輸 v3.2】 版權所有© 2013 工業技術研究院<br />
            <a href="http://www.itri.org.tw/chi/contact/080.asp" title="聯絡我們">聯絡我們</a> | <a href="http://www.itri.org.tw/chi/ip.asp" title="智權政策">智權政策</a> | <a href="http://www.itri.org.tw/chi/law.asp" title="法律聲明">法律聲明</a> | <a href="http://w3.itri.org.tw/iprivacy/privacy.html" title="隱私保護聲明">隱私保護聲明</a>
        </div>
    </div>
    <!--{* wrapper-enter *}-->
    <div id="dialog" class="font-gray" title="合作夥伴登入">
        <div id="mainDIV">
            <table width="100%" border="0" cellspacing="3" cellpadding="3">
                <tr>
                    <td>
                        E-mail:
                    </td>
                    <td>
                        <input type="text" id="tbxUserName" />
                    </td>
                </tr>
                <tr>
                    <td>
                        密碼:
                    </td>
                    <td>
                        <input type="password" id="tbxPwd" />
                    </td>
                </tr>
                <tr>
                    <td align="right" colspan="2">
                        <a href="javascript:location.href='../sfts/createacc.aspx'" target="_self" id="createaccount"
                            tabindex="1">申請帳號</a>&nbsp;<a href="javascript:void(0);" id="forgetpassw" target="_self">忘記密碼</a>&nbsp;
                        <input id="loginBtn" type="button" class="btn_mouseout" onmouseover="this.className='btn_mouseover'"
                            onmouseout="this.className='btn_mouseout'" value="登入" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="forgetDIV" style="display:none">
            <table width="100%" border="0" cellspacing="3" cellpadding="3">
                <tr>
                    <td width="26%">
                        E-mail:
                    </td>
                    <td>
                        <input type="text" id="tbxforgetAccount" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                    <div class="lineheight03 block font-size2">
                    請輸入您的Email，系統會重寄一封認證信請您重新修改密碼。
                    </div>
                    </td>
                </tr>
                <tr>
                    <td align="right" colspan="2">
                        <input id="btnRemail" type="button" class="btn_mouseout" onmouseover="this.className='btn_mouseover'"
                            onmouseout="this.className='btn_mouseout'" value="重新寄送" />
                    </td>
                </tr>
            </table>
        
        </div>
    </div>
    <!--{* dialog *}-->
    </form>
</body>
</html>
