<%@ Page Title="" Language="C#" MasterPageFile="~/sfts/MasterPage.Master" AutoEventWireup="true" CodeBehind="FileEditPage.aspx.cs" Inherits="WebAppl.sfts.FileEditPage" ValidateRequest="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="AllJqueryUI/dynatree/src/skin/ui.dynatree.css" rel="stylesheet" type="text/css" />
    <link href="layout/css/pluploadstyle.css" rel="stylesheet" type="text/css" />

    <script src="AllJqueryUI/dynatree/jquery/jquery.cookie.js" type="text/javascript"></script>
    <script src="AllJqueryUI/dynatree/src/jquery.dynatree.js" type="text/javascript"></script>

    <script src="js/plupload.full.min.js" type="text/javascript"></script>

    <!--全院通訊錄-->
    <script type="text/javascript">
        var treeList = new Array;
        var indexList = new Array;
        var historyList = new Array;
        var OutList = new Array;
        var txtaResultArray = new Object();
        var txtaResultArrayFinal = new Object();
        //    var treeData = [
        //    { "title": "Item 1" },
        //    { "title": "Folder 2", "isFolder": true, "key": "folder2", "noLink": true, "hideCheckbox": true,
        //        "children": [
        //            { "title": "Sub-item 2.1" },
        //            { "title": "Sub-item 2.2" }
        //            ]
        //    },
        //    { "title": "Folder 3", "isFolder": true, "key": "folder3", "noLink": true, "hideCheckbox": true,
        //        "children": [
        //            { "title": "Sub-item 3.1" },
        //            { "title": "Sub-item 3.2" }
        //            ]
        //    },
        //    { "title": "Item 5" }
        //];
        $(function () {
            $("#tree3").dynatree({
                checkbox: true,
                selectMode: 2, //3是關聯式(子項目全選的話 母項目也會被勾選,2是單獨的)
                initAjax: {
                    url: "handler/GenJson.ashx" //json URL
                },
                //children: treeData,
                onSelect: function (select, node) {
                    // Get a list of all selected nodes, and convert to a key array:        
                    //var selKeys = $.map(node.tree.getSelectedNodes(), function (node) {
                    //return node.data.key;
                    //});
                    // Get a list of all selected TOP nodes        
                    var selRootNodes = node.tree.getSelectedNodes(true);
                    //myList[myList.length] = { email: selRootNodes[j].data.mailadd, account: selRootNodes[j].data.empno, PopType: "dictionary" };

                    var selRootmailadd = $.map(selRootNodes, function (node) {
                        return node.data.mailadd;
                    });
                    var selRootempno = $.map(selRootNodes, function (node) {
                        return node.data.empno;
                    });

                    $("#tbxListGroup").children().remove();
                    treeList.splice(0, treeList.length);

                    for (var i = 0; i < selRootempno.length; i++) {
                        $("#tbxListGroup").append("<div style='display:block;' account='" + selRootempno[i] + "' poptype='dictionary'>" + selRootmailadd[i] + "</div>");
                        treeList[i] = { email: selRootmailadd[i], account: selRootempno[i], PopType: "dictionary" };
                    }

                },
                onLazyRead: function (node) {
                    $.ajax({
                        type: "POST",
                        data: {
                            orgcd: node.data.key,
                            deptcd3: node.data.deptcd3
                        },
                        url: "handler/GenJson.ashx",
                        success: function (data, textStatus) {
                            // Convert the response to a native Dynatree JavaScipt object. 
                            //var list = data[0].title;
                            res = [];
                            for (var i = 0; i < data.length; i++) {
                                res.push({
                                    key: data[i].key,
                                    empno: data[i].empno,
                                    title: data[i].title,
                                    isFolder: data[i].isFolder,
                                    noLink: data[i].noLink,
                                    hideCheckbox: data[i].hideCheckbox,
                                    isLazy: data[i].isLazy,
                                    deptcd3: data[i].deptcd3,
                                    mailadd: data[i].mailadd
                                });
                                // PWS status OK 

                            }
                            node.setLazyNodeStatus(DTNodeStatus_Ok);
                            node.addChild(res);
                        }
                    })
                },

                onDblClick: function (node, event) {
                    node.toggleSelect();
                },
                onKeydown: function (node, event) {
                    if (event.which == 32) {
                        node.toggleSelect();
                        return false;
                    }
                }
                // The following options are only required, if we have more than one tree on one page://        
                //            initId: "treeData",
                //            cookieId: "dynatree-Cb3",
                //            idPrefix: "dynatree-Cb3-"
            });

            $("#divOpen").dialog({
                autoOpen: false,
                height: 395,
                width: 670,
                modal: true,
                draggable: false, //可拖動  
                closeOnEscape: false, //讓Esc可以關掉視窗
                //            open: function (event, ui) {
                //                $('#divOpen').css('overflow', 'auto'); //把scrollbar隱藏要把auto改成hidden
                //                $(".ui-dialog-titlebar").hide();//把上面的titleBar整個隱藏起來
                //            },
                buttons: {
                    "取消": function () {
                        $(this).dialog("close");
                    },
                    "確定": function () {
                        BingAllPopUpValue();
                        //                        $("#tbxListGroup").children().remove();
                        //                        $("#tree3").dynatree("getTree").reload();
                        $(this).dialog("close");
                    }

                }
                //            Cancel: { "取消":
                //                 function () {
                //                     $(this).dialog("cancel");
                //                 }
                //            }
            });

            $("#btnSelect1").click(function () {
                $("#divOpen").dialog("open");
            });
        });
    </script>
    <!--全文檢索-->
    <script type="text/javascript">
        $(document).ready(function () {
            $("#btnCheckSearch").click(function () {
                if ($("#tbxSearch").val().replace(/\r\n|\n|\r/g, "") == "") {
                    alert("請輸入查詢條件");
                    return;
                }

                $.ajax({
                    url: "handler/Search.ashx",
                    type: 'POST',
                    data: { key_word: $("#tbxSearch").val()
                    },
                    error: function (xhr) {
                        alert(xhr);
                        return;
                    },
                    success: function (data, textStatus) {
                        var appendStr = "";
                        $("#divOpenerArea").children().remove();
                        if (data == "empty") {
                            $("#divOpenerArea").append("<div align='center'>查無資料</div>");
                        }
                        else {
                            appendStr = '<table class="appendtable">';
                            for (var i = 0; i < data.length; i++) {
                                appendStr += '<tr>';
                                appendStr += '<td>&nbsp;</td>';
                                appendStr += '<td><input type="checkbox" class="chktranValue" /></td>';
                                appendStr += '<td><img src="layout/images/edit_user.png" /></td>';
                                appendStr += '<td><span class="spanRowOne" empno="' + data[i].com_empno + '" email="' + data[i].com_mailadd + '">' + data[i].com_cname + '&lt; ' + data[i].com_mailadd + ' &gt;</span><br />';
                                appendStr += '<span class="spanRowTwo">【' + data[i].org_abbr_chnm1 + '】 ' + data[i].com_deptcd + ' ' + data[i].dep_deptname + '</span></td>';
                                appendStr += '</tr>';
                            }
                            appendStr += '</table>';
                            $("#divOpenerArea").append(appendStr);

                        }
                    }
                });
            });

            var TMPDD = '';

            $(document).on("click", ".chktranValue", function (event) {
                if (this.checked) {
                    indexList[indexList.length] =
                        { email: $(this).parent().parent().children().find("span[class='spanRowOne']").attr("email").replace(/\r\n|\n|\r/g, "")
                        , account: $(this).parent().parent().children().find("span[class='spanRowOne']").attr("empno").replace(/\r\n|\n|\r/g, "")
                        , PopType: "Search"
                        };
                }
                else {
                    //這裡處理把勾勾取消
                    var account = $(this).parent().parent().children().find("span[class='spanRowOne']").attr("empno").replace(/\r\n|\n|\r/g, "");
                    for (var i = 0; i < indexList.length; i++) {
                        if (indexList[i].account == account) {
                            //如果有抓到被取消勾勾的那個選項在已經在input裡面的話
                            indexList.splice(i, 1);
                            break;
                        }
                    }
                }
                $("#tbxSearchSure").children().remove();
                for (var i = 0; i < indexList.length; i++) {
                    $("#tbxSearchSure").append("<span style='display:block;' account='" + indexList[i].account + "' poptype='Search'>" + indexList[i].email + "</span>");
                }
            });


            $("#divSearch").dialog({
                autoOpen: false,
                height: 430,
                width: 700,
                modal: true,
                draggable: false, //可拖動 
                closeOnEscape: false, //讓Esc可以關掉視窗
                //            open: function (event, ui) {
                //                $('#divOpen').css('overflow', 'auto'); //把scrollbar隱藏要把auto改成hidden
                //                $(".ui-dialog-titlebar").hide();//把上面的titleBar整個隱藏起來
                //            },
                buttons: {
                    "取消": function () {
                        $(this).dialog("close");
                    },
                    "確定": function () {
                        BingAllPopUpValue();
                        $(this).dialog("close");
                    }
                }
                //            Cancel: { "取消":
                //                 function () {
                //                     $(this).dialog("cancel");
                //                 }
                //            }
            });

            $("#btnopener").click(function () {
                $("#divSearch").dialog("open");
            });
        });
    </script>
    <!--歷史收件人-->
    <script type="text/javascript">
        $(document).ready(function () {
            var currentYear = (new Date).getFullYear();
            var optionYear, optionMonth;
            for (var i = 0; i < 4; i++) {
                optionYear += "<option value='" + (parseInt(currentYear) - i).toString() + "'>" + (parseInt(currentYear) - i).toString() + "</option>";
            }
            for (var j = 0; j < 12; j++) {
                optionMonth += "<option value='" + (j + 1).toString() + "'>" + (j + 1).toString() + "</option>";
            }

            $("#FromYear,#ToYear").append(optionYear);
            $("#FromMonth,#ToMonth").append(optionMonth);
            $("#FromMonth option").get(parseInt((new Date).getMonth())).selected = true; //getMonth從 0開始
            $("#ToMonth option").get(parseInt((new Date).getMonth())).selected = true; //getMonth從 0開始

            $("#divhistory").dialog({
                autoOpen: false,
                height: 440,
                width: 620,
                modal: true,
                draggable: false, //可拖動 
                closeOnEscape: false, //讓Esc可以關掉視窗
                buttons: {
                    "取消": function () {
                        $(this).dialog("close");
                    },
                    "確定": function () {
                        BingAllPopUpValue();
                        $(this).dialog("close");
                    }
                }
            });

            $("#btnhistory").click(function () {
                $("#divhistory").dialog("open");
            });

            $(document).on("click", "#histBtn", function (event) {
                $("#divhistoryArea").dynatree({
                    checkbox: true,
                    selectMode: 3, //3是關聯式(子項目全選的話 母項目也會被勾選,2是單獨的)
                    initAjax: {
                        url: "handler/getHistory.ashx", //json URL
                        data: {
                            FromYear: $("#FromYear :selected").val(),
                            FromMonth: $("#FromMonth :selected").val(),
                            ToYear: $("#ToYear :selected").val(),
                            ToMonth: $("#ToMonth :selected").val()
                        },
                        type: 'POST'
                    },
                    onDblClick: function (node, event) {
                        node.toggleSelect();
                    },
                    onKeydown: function (node, event) {
                        if (event.which == 32) {
                            node.toggleSelect();
                            return false;
                        }
                    },
                    onSelect: function (select, node) {
                        // Get a list of all selected nodes, and convert to a key array:
                        var selectedNodes = node.tree.getSelectedNodes();
                        var selectedKeys = $.map(selectedNodes, function (node) {
                            return node.data.key;
                        });
                        var selectedisempno = $.map(selectedNodes, function (node) {
                            return node.data.isempno;
                        });

                        $("#tbxhistorySure").children().remove();
                        historyList.splice(0, historyList.length);

                        for (var i = 0; i < selectedKeys.length; i++) {//因為replaceSTR只有KEY有 ISEMPNO會少一個 所以要先把它刪除 再去跑回圈
                            if (selectedKeys[i] == "replaceStr") {
                                selectedKeys.splice(i, 1);
                            }
                        }

                        for (var i = 0; i < selectedKeys.length; i++) {
                            if (selectedKeys[i] != "replaceStr") {
                                historyList[historyList.length] = { email: selectedKeys[i], account: selectedisempno[i], PopType: "history" };
                                $("#tbxhistorySure").append("<span style='display:block;' account='" + selectedisempno[i] + "' poptype='history'>" + selectedKeys[i] + "</span>");
                            }
                        }
                    }
                });

                $("#divhistoryArea").dynatree("getTree").reload();
            });
        });
    </script>
    <!--院外廠商輸入-->
    <script type="text/javascript">
        $(document).ready(function () {
            $("#divOutCompany").dialog({
                autoOpen: false,
                height: 420,
                width: 380,
                modal: true,
                draggable: false, //可拖動 
                closeOnEscape: false, //讓Esc可以關掉視窗
                buttons: {
                    "取消": function () {
                        $(this).dialog("close");
                    },
                    "確定": function () {
                        BingAllPopUpValue();
                        $(this).dialog("close");
                    }
                }
            });

            $("#btnOutComp").click(function () {
                $("#divOutCompany").dialog("open");
            });

            $("#btnOutCompSubmit").click(function () {
                $.ajax({
                    url: "handler/AddComp.ashx",
                    type: 'POST',
                    data:
                    {
                        tbxOutComp: $("#tbxOutComp").val()
                    },
                    //error: function (xhr, ajaxOtions, thrownError) {
                    error: function (xhr) {
                        alert("Email格式錯誤");
                        //$("budy").append(xhr.status);
                        //$("budy").append(xhr.responseText);
                        //alert(xhr.status);
                        //alert(thrownError);
                        //alert(xhr.responseText);
                        //alert(data[i].email + "+" + data[i].account);                        
                    },
                    dataType: "json",
                    success: function (data, textStatus) {
                        for (var i = 0; i < OutList.length; i++) {
                            if (OutList[i].email == $("#tbxOutComp").val()) {
                                alert("重複的Email");
                                return false;
                            }
                        }

                        for (var i = 0; i < data.length; i++) {
                            $("#divOutComp").append("<span style='display:block;' account='" + data[i].account + "' poptype='company'>" + data[i].email + "</span>");
                            OutList[OutList.length] = { email: data[i].email, account: data[i].account, PopType: "company" };
                            //alert(data[i].email +"+"+ data[i].account);
                        }
                        $("#tbxOutComp").val("");

                    }
                });
            });
        });
    
    </script>

    <!--父視窗收件人處理-->
    <script type="text/javascript">
        function BingAllPopUpValue() {
            var browser = navigator.appName;
            if (browser == "Netscape") {
                //除了IE要去跑ECMAScript5之外,其他都不跑
                //merge函數 只能兩個陣列 所以三個要再做一次(merge是破壞性陣列合併 會把第一個參數的陣列=第一個+第二個)
                var firstArry = $.merge([], treeList);
                var twoMerge = $.merge(firstArry, indexList);
                var secArry = $.merge([], historyList);
                var threeArry = $.merge(secArry, twoMerge);
                var fourthArry = $.merge([], OutList);
                txtaResultArray = $.merge(fourthArry, threeArry);


                txtaResultArray = toClearRepeatValue(txtaResultArray);
                $("#divResult").children().remove();

                for (var i = 0; i < txtaResultArray.length; i++) {
                    if (txtaResultArray[i]) {
                        $("#divResult").append("<div><span>&nbsp;&nbsp;</span><a account='" + txtaResultArray[i].account + "' PopType='" + txtaResultArray[i].PopType + "' href='javascript:void(0);' class='delcls' >（刪除）</a>" + txtaResultArray[i].email + "<div>");
                    }
                }
            }
            else {
                jQuery.ajax({
                    type: "GET",
                    url: 'AllJqueryUI/ECMAScript5.js',
                    dataType: "script",
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(XMLHttpRequest.responseText);
                        console.log('error ', errorThrown);
                    },
                    success: function () {
                        //merge函數 只能兩個陣列 所以三個要再做一次(merge是破壞性陣列合併 會把第一個參數的陣列=第一個+第二個)
                        var firstArry = $.merge([], treeList);
                        var twoMerge = $.merge(firstArry, indexList);
                        var secArry = $.merge([], historyList);
                        var threeArry = $.merge(secArry, twoMerge);
                        var fourthArry = $.merge([], OutList);
                        txtaResultArray = $.merge(fourthArry, threeArry);


                        txtaResultArray = toClearRepeatValue(txtaResultArray);
                        $("#divResult").children().remove();

                        for (var i = 0; i < txtaResultArray.length; i++) {
                            if (txtaResultArray[i]) {
                                $("#divResult").append("<div><span>&nbsp;&nbsp;</span><a account='" + txtaResultArray[i].account + "' PopType='" + txtaResultArray[i].PopType + "' href='javascript:void(0);' class='delcls' >（刪除）</a>" + txtaResultArray[i].email + "<div>");
                                //                    if (txtaResultArray[i].indexOf("<") != -1 || txtaResultArray[i].indexOf(">") != -1) {
                                //                        newArray.push(txtaResultArray[i].substring(txtaResultArray[i].indexOf("<") + 1, txtaResultArray[i].indexOf(">")));
                                //                    }
                                //                    else {
                                //                        newArray.push(txtaResultArray[i]);
                                //                    }    
                            }
                        }
                    }
                });
            }
        }
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            try {
                var uploader = new plupload.Uploader({
                    runtimes: 'html5,flash,silverlight,html4',
                    browse_button: 'pickfiles', // you can pass in id...
                    container: document.getElementById('main'), // ... or DOM Element itself
                    url: 'handler/UploadFileFirst.ashx',

                    flash_swf_url: 'js/Moxie.swf',
                    silverlight_xap_url: 'js/Moxie.xap',
                    multipart_params: {
                        guid: $("#<% =tbxGenGuid.ClientID %>").val(),
                        type: $('input[name=type]:checked').val()
                    },
                    filters: {
                        // Maximum file size             
                        max_file_size: '2000mb',
                        // Specify what files to browse for             
                        mime_types: [
                            { title: "all files", extensions: "*" }
                            ]
                    },
                    init: {
                        Init: function (up, params) {
                            //$('#warningStr').html("<div>目前瀏覽器優先使用: " + params.runtime + "</div>");
                        },
                        FilesAdded: function (up, files) {
                            if (up.total.size >= 2147483648) {
                                up.removeFile(files[0]);
                                alert("所有檔案相加其大小不可超過2GB");
                                return;
                            }
                            var deleteHandle = function (uploaderObject, fileObject) {
                                return function (event) {
                                    event.preventDefault();
                                    uploaderObject.removeFile(fileObject);
                                    $(this).closest("li#" + fileObject.id).remove();
                                };
                            };

                            var Flist = '<ul>';
                            var fileListTable = '<table width="100%" border="0" cellspacing="0" cellpadding="0">';
                            fileListTable += '<tr>';
                            fileListTable += '<td class="plupload_file_name">檔案名稱</td>';
                            fileListTable += '<td class="plupload_file_size">大小</td>';
                            fileListTable += '<td class="plupload_delCol">&nbsp;</td>';
                            fileListTable += '</tr></table>';
                            if ($("#plupload_content").find("table").length == 0) {
                                $("#plupload_content").prepend(fileListTable);
                            }

                            $.each(files, function (i, file) {
                                Flist += '<li id="' + file.id + '"><div class="plupload_fileNameList"><span>' + file.name + '</span></div><div class="plupload_fileSizeList"><span>' + plupload.formatSize(file.size) + '</span></div>';
                                Flist += '<div class="plupload_delbtn"><a href="javascript:void(0);"><img src="js/jquery.plupload.queue/img/delete.gif" border="0" id="deleteFile' + files[i].id + '" /></a></div><b></b></li>';
                            });
                            Flist += '</ul>';
                            $('#filelist').append(Flist);

                            for (var i in files) {
                                $('#deleteFile' + files[i].id).click(deleteHandle(up, files[i]));
                                //讓li 產生斷行之後還能自行調整HEIGHT
                                $("li#" + files[i].id + "").height(function () {
                                    return $(this).children(".plupload_fileNameList").height() + 10;
                                });
                            }
                        },
                        FileUploaded: function (up, files) {
                            if ((uploader.total.uploaded) == uploader.files.length) {
                                var iframe = $('<iframe name="postiframe" id="postiframe" style="display: none" />');

                                var JsonValue = "";
                                for (var i = 0; i < txtaResultArray.length; i++) {//把人員挑選結果轉換成input 丟過去
                                    if (JsonValue != '')
                                        JsonValue += ',';
                                    JsonValue += "{'account':'" + txtaResultArray[i].account + "','email':'" + txtaResultArray[i].email + "'}";
                                }
                                if (JsonValue != '')
                                    JsonValue = '[' + JsonValue + ']';

                                var JsonSTR = $('<input name="hiddenInputJson" id="hiddenInputJson" type="hidden" value="' + JsonValue + '" />');
                                var hidGuid = $('<input name="hidGuid" id="hidGuid" type="hidden" value="' + $("#<% =tbxGenGuid.ClientID %>").val() + '" />');
                                //var form = document.body.getElementsByTagName('form')[0];
                                var form = $("form");
                                form.append(iframe);
                                form.append(JsonSTR);
                                form.append(hidGuid);

                                form.attr("action", "handler/fileAjax.ashx");
                                form.attr("method", "post");
                                form.attr("enctype", "multipart/form-data");
                                form.attr("encoding", "multipart/form-data");
                                form.attr("target", "postiframe");
                                form.submit();
                            }
                            //$('#' + file.id + " b").html("100%");
                        },
                        UploadProgress: function (up, file) {
                            $("#progressbar").progressbar("value", (up.total.loaded / up.total.size) * 100);
                            $('#percentNum').html(((up.total.loaded / up.total.size) * 100).toFixed(0) + "%");
                            //$('#' + file.id + " b").html(file.percent + "%");
                        },
                        Error: function (up, err) {
                            if (err.code == "-600") {
                                alert("請選擇低於2GB的檔案上傳");
                            }
                            else {
                                $('#warningStr').append("<div>Error: " + err.code + ", Message: " + err.message + (err.file ? ", File: " + err.file.name : "") +
                    "</div>");
                            }
                            up.refresh(); // Reposition Flash/Silverlight
                        }
                    }
                });

            }
            catch (err) {
                alert(err);
            }

            uploader.init();


            $(document).on("click", ".delcls", function (event) {
                var account = $(this).attr("account");
                var poptype = $(this).attr("poptype");
                if (poptype == "dictionary") {
                    var selects = $("#tree3").dynatree("getTree").getSelectedNodes();
                    for (var i = 0; i < selects.length; i++) {
                        if (selects[i].data.empno == account) {
                            treeList.splice(i, 1);
                            selects[i].select(false);
                        }
                    }
                    $("#tbxListGroup").find("span[account='" + account + "']").remove();
                }
                else if (poptype == "Search") {
                    for (var i = 0; i < indexList.length; i++) {
                        if (indexList[i].account == account) {
                            indexList.splice(i, 1);
                        }
                    }
                    if ($("#divOpenerArea").find("span[empno='" + account + "']").parent().parent().find("input[type='checkbox']").length != 0) {
                        $("#divOpenerArea").find("span[empno='" + account + "']").parent().parent().find("input[type='checkbox']")[0].checked = false;
                    }
                    $("#tbxSearchSure").find("span[account='" + account + "']").remove();

                }
                else if (poptype == "history") {
                    var selects = $("#divhistoryArea").dynatree("getTree").getSelectedNodes();
                    for (var i = 0; i < selects.length; i++) {
                        if (selects[i].data.isempno == account) {//因為replaceStr永遠不等於account 所以這邊不過濾
                            historyList.splice(i, 1);
                            selects[i].select(false);
                        }
                    }
                    $("#tbxhistorySure").find("span[account='" + account + "']").remove();

                }
                else if (poptype == "company") {
                    for (var i = 0; i < OutList.length; i++) {
                        if (OutList[i].account == account) {//因為replaceStr永遠不等於account 所以這邊不過濾
                            OutList.splice(i, 1);
                        }
                    }
                    $("#divOutComp").find("span[account='" + account + "']").remove();
                }

                BingAllPopUpValue();
            });

            //            $(window).keydown(function (event) {
            //                if (event.keyCode == 13) {
            //                    event.preventDefault();
            //                    return false;
            //                }
            //            });

            $(window).keydown(function (event) {
                if (event.keyCode == 13 && $("#tbxtitle").focus()) {
                    $("#tbxdesc").focus();
                }
            });

            $("#btnopener").tooltip(); //為了讓那三個圖片變得更好看的提示
            $("#btnSelect1").tooltip(); //為了讓那三個圖片變得更好看的提示
            $("#btnhistory").tooltip(); //為了讓那三個圖片變得更好看的提示
            $("#btnOutComp").tooltip(); //為了讓那三個圖片變得更好看的提示

            $("#btnDesc").tooltip({
                content: function (callback) {
                    callback($(this).prop('title').replace(/\|/g, '<br />'));
                },
                open: function (event, ui) {
                    ui.tooltip.css("max-width", "500px");
                }
            }); //為了讓那三個圖片變得更好看的提示

            $("#WaitGif").dialog({
                position: 'center',
                autoOpen: false,
                height: 130,
                width: 450,
                modal: true,
                draggable: false, //可拖動 
                closeOnEscape: false, //讓Esc可以關掉視窗
                resizable: false, //可調整視窗大小
                open: function (event, ui) {
                    //$('#divOpen').css('overflow', 'hidden'); //把scrollbar隱藏要把auto改成hidden
                    $(".ui-dialog-titlebar").hide(); //把上面的整個隱藏起來
                }
            });

            //            $(document).on("change", "input[name='File1']", function (event) {
            //                if ($(this).val() != "") {
            //                    var fileLength = $("input[name='File1']").length;
            //                    $("#UploadTable").append("<tr><td><input type='file' multiple='multiple' id='File" + fileLength + "' name='File1' /></td><td><input id=\"CanCelBtn" + fileLength + "\" type=\"button\" value=\"取消\" name='cancelBtn' class=\"btn_mouseout\" onmouseover=\"this.className='btn_mouseover'\" onmouseout=\"this.className='btn_mouseout'\" /></td></tr>");
            //                }
            //            });



            //            $(document).on("click", "#if1Btn", function (event) {
            //                if (alertMessage() == false) {
            //                    return;
            //                }

            //                $("#WaitGif").dialog("open");
            //                var iframe = $('<iframe name="postiframe" id="postiframe" style="display: none" />');

            //                var JsonValue = "";
            //                for (var i = 0; i < txtaResultArray.length; i++) {//把人員挑選結果轉換成input 丟過去
            //                    if (JsonValue != '')
            //                        JsonValue += ',';
            //                    JsonValue += "{'account':'" + txtaResultArray[i].account + "','email':'" + txtaResultArray[i].email + "'}";
            //                }
            //                if (JsonValue != '')
            //                    JsonValue = '[' + JsonValue + ']';

            //                var JsonSTR = $('<input name="hiddenInputJson" id="hiddenInputJson" type="hidden" value="' + JsonValue + '" />');
            //                //var form = document.body.getElementsByTagName('form')[0];
            //                var form = $("form");
            //                form.append(iframe);
            //                form.append(JsonSTR);

            //                form.attr("action", "handler/fileAjax.ashx");
            //                form.attr("method", "post");
            //                form.attr("enctype", "multipart/form-data");
            //                form.attr("encoding", "multipart/form-data");
            //                form.attr("target", "postiframe");
            //                form.submit();
            //            });

            //            $(document).on("click", "input[name='cancelBtn']", function (event) {//on在1.7版以上用 用來在畫面有變動之後重新Binding
            //                if ($("input[name='File1']").size() > 1) {
            //                    $(this).parent().parent().remove();
            //                }
            //                else {
            //                    var cloneUpload = $(this).parent().parent().children().find("input[name='File1']").clone();
            //                    $(this).parent().parent().children().find("input[name='File1']").remove();
            //                    $(this).parent().parent().find("td:first").append(cloneUpload);
            //                }

            //            });

            $(document).on("click", "input[name='type']", function (event) {
                if ($(this).val() == "security") {
                    $("#notifyflag").get(0).checked = true;
                    $("#tbxnotifyflag").val("Y");
                    $("#notifyflag").get(0).disabled = true;

                }
                else {
                    $("#notifyflag").get(0).checked = false;
                    $("#tbxnotifyflag").val("");
                    $("#notifyflag").get(0).disabled = false;
                }
            });

            $(document).on("click", "#disableRCPTDELLINK", function (event) {
                if ($("#disableRCPTDELLINK").get(0).checked == true) {
                    $("#tbxdisableRCPTDELLINK").val("Y");
                }
                else {
                    $("#tbxdisableRCPTDELLINK").val("");
                }
            });

            $(document).on("click", "#notifyflag", function (event) {
                if ($("#notifyflag").get(0).checked == true) {
                    $("#tbxnotifyflag").val("Y");
                }
                else {
                    $("#tbxnotifyflag").val("");
                }
            });

            $("#progressbar").progressbar({
                value: false
            });

            $('#uploadfiles').click(function (e) {
                if (uploader.files.length > 0) {
                    if (alertMessage() == false) {
                        return;
                    }
                    else {
                        $("#WaitGif").dialog("open");
                        uploader.settings.multipart_params.type = $('input[name=type]:checked').val();
                        uploader.start();
                        e.preventDefault();
                        return false;
                    }
                }
                else {
                    alert("請選擇要上傳之檔案");
                    return false;
                }
            });
        });
    </script>

    <script type="text/javascript">
        function Isfinish() {
            $("#WaitGif").dialog("close");
        }
        function feedbackFun(pn) {
            var form = document.body.getElementsByTagName('form')[0];
            form.target = '';
            form.method = "post";
            form.enctype = "application/x-www-form-urlencoded";
            form.encoding = "application/x-www-form-urlencoded";
            form.action = location;
            $("#WaitGif").dialog("close");
            alert("傳送成功");
            location.replace(window.location.href.replace("FileEditPage.aspx", "uploadSuccess.aspx?pn=" + pn));
        }


        function ArryCompara(self, C) {
            //email, account
            var i = 0, exists = false;
            var SelfCount = self.length;
            for (i = 0; i < SelfCount; i++) {
                if (self[i].email.indexOf(C.email) >= 0 &&
                    self[i].account.indexOf(C.account) >= 0) {
                    exists = true;
                    break;
                }
            }

            return exists;
        }

        function toClearRepeatValue(self) {
            var n = []; //一个新的临时数组
            var copy = Object.create(Object.getPrototypeOf(self));
            var propNames = Object.getOwnPropertyNames(self);
            var i = 0;
            propNames.forEach(function (name) {
                var names = Object.getOwnPropertyNames(copy);
                var desc = Object.getOwnPropertyDescriptor(txtaResultArray, name);
                if ((n.length <= 0 || !ArryCompara(n, { email: desc.value.email, account: desc.value.account })) && name.toString().toLowerCase() != 'length') {
                    n.push(desc.value);
                    var prop = propNames[desc];
                    Object.defineProperty(copy, name, desc);
                }
            });
            return n;
        }
    </script>

    <!-- 警告集合區 -->
    <script type="text/javascript">
        function alertMessageASHX(mem) {
            if (mem != "") {
                alert(mem);
                $("#WaitGif").dialog("close");
                $("input[name='hiddenInputJson']").remove(); //收件人中沒有院內人士 跳出警告並清空JSON暫存值
                $(".ui-dialog-titlebar").show();
                var form = document.body.getElementsByTagName('form')[0];
                form.target = '';
                form.method = "post";
                form.enctype = "application/x-www-form-urlencoded";
                form.encoding = "application/x-www-form-urlencoded";
                form.action = location;
                return false;
            }
        }
        function alertMessage() {
            //            var fileList = $("input[name='File1']").length;
            //            var booljudge = false;
            //            for (var i = 0; i < fileList; i++) {
            //                if ($("input[name='File1']").get(i).value != "") {
            //                    booljudge = true;
            //                }
            //            }

            //            if (booljudge == false) {
            //                alert("請選擇要上傳之檔案");
            //                return false;
            //            }

            if (txtaResultArray.length == 0 || typeof (txtaResultArray.length) == "undefined") {
                alert("請選擇收件人");
                return false;
            }
            if ($("input[name='type']").val() == "") {
                alert("請選擇類型");
                return false;
            }
            if ($("#tbxtitle").val() == "") {
                alert("請輸入主旨");
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main" id="main">
        <span class="linetable font-gray font-size3">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td align="right" class="inputsizeS">
                        <img alt="說明" src="layout/images/list_error.png" id="btnDesc" title="[不可轉寄]：取檔需輸入取檔密碼，取檔密碼有時效性。|[檔案加密]：取檔需輸入取檔密碼，檔案會打包加密，檔案需解壓密碼。"/>類型:
                    </td>
                    <td>
                        <input type="radio" name="type" value="common" checked="checked" />&nbsp;一般檔案&nbsp;&nbsp;<input
                            type="radio" name="type" value="security" />&nbsp;檔案加密&nbsp;&nbsp;<input
                            type="radio" name="type" value="nonforward" />&nbsp;不可轉寄
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        &nbsp;</td>
                    <td>
                    <div class="font-lightgray font-size2">
                        <input type="checkbox" id="notifyflag" name="notifyflag"/>                      
                        <label for="notifyflag">收件者下載檔案或刪除取檔網址時通知我。</label>
                        <input type="hidden" id="tbxnotifyflag" name="tbxnotifyflag" />
                        <br />
                        <input type="checkbox" id="disableRCPTDELLINK" name="disableRCPTDELLINK" />
                        <label for="disableRCPTDELLINK">禁止收件者自己刪除自己的取檔網址。</label>
                        <input type="hidden" id="tbxdisableRCPTDELLINK" name="tbxdisableRCPTDELLINK" />
                     </div>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        主旨:
                    </td>
                    <td>
                        <input type="text" class="inputsizeLL" maxlength="150" name="tbxtitle" id="tbxtitle" tabindex="0" />
                    </td>
                </tr>
                <tr>
                    <td align="right" valign="top">
                        留言內容:
                    </td>
                    <td>
                        <textarea rows="8" class="inputsizeLL" name="tbxdesc" id="tbxdesc"></textarea>
                    </td>
                </tr>
                <tr>
                    <td align="right" valign="top">
                        檔案清單:
                    </td>
                    <td valign="top">
                        <div id="divif">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td>
                                        <div id="container">
                                            <input type="button" id="pickfiles" value="選擇檔案" class="btn_mouseout" onmouseover="this.className='btn_mouseover'"
                                                onmouseout="this.className='btn_mouseout'" />
                                            <asp:TextBox ID="tbxGenGuid" runat="server" style="display:none;"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div id="plupload_content">
                                            <div id="filelist">
                                            </div>
                                        </div>
                                        <%--<div id="warningStr">您的瀏覽器不支援Flash,Silverlight以及HTML5</div>--%>
                                        <div id="warningStr"></div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </span>
        <div class="controlblock">
            &nbsp;<input type="submit" id="cancelbtn" value="清除內容" class="btn_mouseout" onmouseover="this.className='btn_mouseover'"
                onmouseout="this.className='btn_mouseout'" />
            <input type="button" class="btn_mouseout" onmouseover="this.className='btn_mouseover'"
                onmouseout="this.className='btn_mouseout'" id="uploadfiles" value="確定送出" />
<%--            <input type="button" class="btn_mouseout" onmouseover="this.className='btn_mouseover'"
                onmouseout="this.className='btn_mouseout'" id="Button1" value="確定送出" />--%>
        </div>
    </div>
    <!--{* main *}-->
    <div class="sidebar lineheight02">
        <span class="font-size2">寄件者:
            <asp:Label ID="lbName" runat="server"></asp:Label>
            <span class="font-lightgray font-size2">
                <asp:Label ID="lbEmail" runat="server"></asp:Label>
            </span></span>
        <div class="block">
            <div class="left">
                <span class="font-size4 font-bold">收件者:</span></div>
            <div class="right">
                <table id="imglist" width="80px" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td style="width: 50%">
                            <asp:Panel ID="hiddenUser" runat="server">
                                <table id="Table1" width="100%" cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td>
                                            <a id="btnopener" href="javascript:void(0);" target="_self" title="查詢">
                                                <img src="layout/images/btn-search.gif" border="0" alt="查詢" /></a>
                                        </td>
                                        <td>
                                            <a id="btnSelect1" href="javascript:void(0);" target="_self" title="全院通訊錄">
                                                <img src="layout/images/btn-address.gif" border="0" alt="全院通訊錄" />
                                            </a>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                        <td style="width:25%">
                            <a href="javascript:void(0);" target="_self" id="btnOutComp" title="Email輸入">
                                <img src="layout/images/user_group.png" border="0" alt="Email輸入" />
                            </a>
                        </td>
                        <td style="width:25%">
                            <a href="javascript:void(0);" target="_self" id="btnhistory" title="歷史收件者">
                                <img src="layout/images/btn-history.gif" border="0" alt="歷史收件者" />
                            </a>
                        </td>
                    </tr>
                </table>
            </div>
            <!--{* right *}-->
        </div>
        <!--{* block *}-->
        <div class="block font-size2">
        <div id="divResult" class="SubpickArea"></div>
            <%--<textarea rows="5" style="width: 340px;" id="txtaResult" name="txtaResult"></textarea>--%>
        </div>
        <!--{* block *}-->
        <div class="block">
            <img src="layout/images/icon-note.gif" /><span class="font-bold">注意事項</span>
            <div class="noteblock">
                <ul>
                    <li><span class="font-red">本系統僅限公務使用，使用者不得傳送未授權的軟體或非公務相關檔案。</span><br /></li>
                    <li>請注意：檔案寄送時務必檢查確認收檔者E-mail，信件寄出後本系統將<span class="font-red">無法追蹤或限制</span>該信件的轉寄動作</li>
                    <li>E-mail 地址<span class="font-red">請輸入完整的 E-mail 地址</span>，例如：john@foo.com</li>
                    <li>若需要上傳的檔案數量很多，建議您<span class="font-red">將檔案打包成一個壓縮檔</span>再上傳，以避免上傳時需花大量時間選擇檔案，也能避免收檔者下載時必須點選多次下載按鈕的困擾。</li>
                    <li>本系統一次傳輸的<span class="font-red">檔案大小總和上限為2G</span></li>
                </ul>
            </div>
        </div>
        <!--{* block *}-->
    </div>
    <!--{* sidebar *}-->
    <div id="WaitGif" title="上傳進度" class="font-gray block font-size2">
        <div id="progressbar" class="progressbar">
            <div class="progress-label">
                Loading...</div>
        </div>
     <div id="percentNum"></div>
<%--    <img src="images/loading.gif" />--%>
    </div>
    <div id="divOpen" title="全院通訊錄" class="font-gray block font-size2">      
                <div id="tree3" class="DelDiv">
                </div>
                <div id="tbxArea" class="sidebar lineheight02" style="height:230px">
                收件人:<br />
                <div id="tbxListGroup" class="SubpickArea"></div>
                </div>
    </div>

    <div id="divSearch" title="查詢" class="font-gray block font-size2">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td align="left">
                    <input id="tbxSearch" type="text" />
                    <input id="btnCheckSearch" type="button" value="查詢" class="btn_mouseout" onmouseover="this.className='btn_mouseover'"
                        onmouseout="this.className='btn_mouseout'" />
                        <br />
                    查詢結果:
                    <div id="divOpenerArea" class="DelDiv">
                    </div>
                </td>
                <td>
                    收件人:
                    <div class="sidebar lineheight02">
                        <div id="tbxSearchSure" class="SubpickArea">
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td rowspan="2" style="width:60%">
                    <div class="block font-size2">

                    </div>
                </td>
                <td align="center" style="width:40%">

                </td>
            </tr>
            <tr>
                <td>
                    <div id="treeOpener">
                    </div>
                </td>
            </tr>
        </table> 
    </div>

    <div id="divhistory" title="歷史通訊錄" class="font-gray block font-size2">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td>
                    <table width="90%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td>
                                從:<select id="FromYear"></select>年<select id="FromMonth"></select>月
                            </td>
                        </tr>
                        <tr>
                            <td>
                                至:<select id="ToYear"></select>年<select id="ToMonth"></select>月 的紀錄
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <input id="histBtn" type="button" value="查詢" class="btn_mouseout" onmouseover="this.className='btn_mouseover'" onmouseout="this.className='btn_mouseout'"/>
                            </td>
                        </tr>
                    </table>
                    查詢結果:
                        <div id="divhistoryArea" class="DelDiv">
                        </div>
                </td>
                <td>
                    收件人:
                    <div class="sidebar lineheight02">
                        <div id="tbxhistorySure" class="SubpickArea">
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td rowspan="2" style="width:50%">
                    <div class="block font-size2">

                    </div>
                </td>
                <td align="center" style="width:50%">

                </td>
            </tr>
            <tr>
                <td>
                    <div id="Div2">
                    </div>
                </td>
            </tr>
        </table> 
    </div>

    <div id="divOutCompany" class="font-gray block font-size2" title="Email輸入">
     <table width="100%" border="0" cellspacing="0" cellpadding="0">
         <tr>
             <td>

                 Email：<textarea id="tbxOutComp" rows="3" cols="30"></textarea>
             </td>
             <td style="vertical-align:bottom">
                <input id="btnOutCompSubmit" type="button" value="新增" class="btn_mouseout" onmouseover="this.className='btn_mouseover'" onmouseout="this.className='btn_mouseout'" />
             </td>
         </tr>
         <tr>
             <td colspan="2">
             <span class="font-red">多筆輸入請用『;』號隔開Email address<br></span>
             <span class="font-red">多筆輸入時請檢查最後一筆Email address請勿加『;』</span>
             </td>
         </tr>
         <tr>
         <td colspan="2">&nbsp;</td>
         </tr>
         <tr>
             <td colspan="2">
                 <div id="divOutComp" class="DelDiv">
                 </div>
             </td>
         </tr>
     </table>
    </div>
</asp:Content>