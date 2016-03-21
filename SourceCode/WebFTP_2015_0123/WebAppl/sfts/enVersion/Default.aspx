<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebAppl.sfts.enVersion.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>ITRI Secured File Transfer System</title>
    <script src="../layout/js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../layout/js/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
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
                    title: 'Outside Partner login'
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
                alert("Please Enter E-mail And PassWord");
                return;
            }

            $("#loginBtn").attr("disabled", "disabled");

            $.ajax({
                type: "POST",
                url: "handlerEn/loginEn.ashx",
                data: {
                    tbxUserName: $("#tbxUserName").val(),
                    tbxPwd: $("#tbxPwd").val()
                },
                error: function (xhr) {
                    alert("Please enter the correct E-mail and Password");
                    $("#tbxPwd").val("");
                },
                success: function (response) {
                    if (response == "success") {
                        if ($.getParamValue('en') != "") {
                            location.replace(window.location.href.replace("Default", "VeryflyEn"));
                            //location.href = "Veryfly.aspx?en=" + $.getParamValue('en');
                        }
                        else {
                            alert("Welcome To The ITRI Secured File Transfer System");
                            location.href = "FileEditPageEn.aspx";
                        }
                    }
                    else {
                        alert("Please enter the correct Username and Password");
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
                location.href = "../../CheckSSO.aspx?en=" + $.getParamValue('en') + "&lang=en";
                //                //location.href = "Veryfly.aspx?en=" + $.getParamValue('en');
            }
            else {
                location.href = "../../CheckSSO.aspx?lang=en";
            }
        });

        $("#forgetpassw").click(function () {
            $("#forgetDIV").show();
            $("#mainDIV").hide();
            $('#dialog').dialog({
                title: 'Forgot Password'
            });
        });

        $("#btnRemail").click(function () {
            if ($("#tbxforgetAccount").val().replace(/\r\n|\n|\r/g, "") != "") {
                $.ajax({
                    type: "POST",
                    url: "handlerEn/ForGetPassWEn.ashx",
                    data: {
                        tbxforgetAccount: $("#tbxforgetAccount").val()
                    },
                    error: function (xhr) {
                        alert(xhr);
                    },
                    success: function (response) {
                        if (response == "success") {
                            alert("The change password Email has been sent,please check your mailbox");
                            location.href = "Default.aspx";
                        }
                        else {
                            alert(response);
                            return;
                        }
                    }
                });
            }
            else {
                alert("Please Enter E-mail Address");
                return;
            }
        });
    })
</script>

    <link href="../layout/css/styleEn.css" rel="stylesheet" type="text/css" />
    <link href="../layout/css/redmond/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
</head>

<body>
    <form id="form1" runat="server">
    <div class="wrapper-enter">
        <div class="logo">
            <h1>
                ITRI Secured File Transfer System</h1>
        </div>
        <a href="javascript:void(0);" target="_self" id="loginSSO">
            <div class="btnITRI ITRIgeneral">
                <h2>
                    ITRI Staff</h2>
            </div>
        </a><a href="javascript:void(0);" target="_self" id="opener">
            <div class="btnFIRM FIRMgeneral">
                <h2>
                    Outside Partner</h2>
            </div>
        </a>
        <div class="text font-lightgray lineheight02">
            WebFTP provides an easy tool for ITRI staff and partner to exchange large file(s) efficiently via a friendly web-based interface。<br />
<%--            <a href="http://webftp.itri.org.tw" style="color:Blue; text-decoration:underline;">WebFtp Previous version v2.2</a>
            &nbsp;&nbsp;&nbsp;--%>
            <a href="../Default.aspx" style="color:Blue; text-decoration:underline;">中文版</a>
            </div>
        <div class="footer font-lightgray">
        【ITRI WebFTP v3.2】 Copyright © 2013 Industrial Technology Research Institute<br />
            <a href="http://www.itri.org.tw/chi/contact/080.asp" title="Comments">Comments</a> | <a href="http://www.itri.org.tw/chi/ip.asp" title="IP Policy Statement">IP Policy Statement</a> | <a href="http://www.itri.org.tw/chi/law.asp" title="Legal Notice">Legal Notice</a> | <a href="http://w3.itri.org.tw/iprivacy/privacy.html" title="Privacy Statement">Privacy Statement</a>
        </div>
    </div>
    <!--{* wrapper-enter *}-->
    <div id="dialog" class="font-gray" title="Outside Partner">
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
                        PassWord:
                    </td>
                    <td>
                        <input type="password" id="tbxPwd" />
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                        <a href="createaccEn.aspx" target="_self" id="createaccount"
                            tabindex="1">applying a new account</a>
                            <br />
                         <a href="javascript:void(0);" id="forgetpassw" target="_self">Password Assistance</a><br />

                    </td>
                </tr>
                <tr>
                    <td align="right" colspan="2">
                         <input id="loginBtn" type="button" class="btn_mouseout" onmouseover="this.className='btn_mouseover'"
                            onmouseout="this.className='btn_mouseout'" value="login" />                   
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
                    Please enter your Email。
                    </div>
                    </td>
                </tr>
                <tr>
                    <td align="right" colspan="2">
                        <input id="btnRemail" type="button" class="btn_mouseout" onmouseover="this.className='btn_mouseover'"
                            onmouseout="this.className='btn_mouseout'" value="ReSend" />
                    </td>
                </tr>
            </table>
        
        </div>
    </div>
    <!--{* dialog *}-->
    </form>
</body>
</html>
