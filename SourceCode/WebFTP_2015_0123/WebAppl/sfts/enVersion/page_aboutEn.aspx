<%@ Page Title="" Language="C#" MasterPageFile="~/sfts/enVersion/MasterPageEn.Master" AutoEventWireup="true" CodeBehind="page_aboutEn.aspx.cs" Inherits="WebAppl.sfts.enVersion.page_aboutEn" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <div class="fullblock font-gray lineheight03">
WebFTP provides an easy tool for ITRI staff and partner to exchange large file(s) efficiently via a friendly web-based interface.<br />
 Features:
<ol>
<li>A user who wants to transfer large file(s) should firstly specify receiver(s) and uploads the file(s) to the server. The system will automatically send an email to notify the receiver(s) to get the file(s) from the system.</li>
<li>Only specified receiver(s) can get files from the system.</li>
<li>WebFTP is a web-based system which only requires a web browser to activate it.</li>
<li>WebFTP uses SSL encryption transmission.</li>
<li>WebFTP accesses files with an unique encoded URL. <span class="font-red"> DO NOT forward your URL to others by haphazard</span>.</li>

</ol>
<span class="font-red">WebFTP is for office use only. It is not allowed to transfer unauthorized software or personal files.</span><br />
For suggestions/advices, please contact system administrator via phone or email.<br />
ITRI IT Service and Development Center <br />
Tel: +886-3-591-8899
</div>
<br />
<div class="fullblock font-gray lineheight03">
<h3> System requirement:</h3>
IE 8(or above) with an  800X600 resolution is recommended.
<br /><br />
<h3>Summary:</h3>
<ol>
<li>WebFTP uploading maximum size is <span class="font-red">2G</span> once. However, if the file is large, it will cause transmission failure. The delivering speed depends on the traffic flow at the time of transmission.</li>
<li>All upload file(s) will be deleted automatically after <span class="font-red">seven days</span>.</li>
<li>All upload file(s) will be protected by the system. Unauthorized user(s) will not be able to access or modify the upload file(s). Unauthorized means that users who are neither specified receiver(s) nor original file-sender. Therefore, it is safe to use WebFTP to send files without worrying about being stolen or modified by someone else. （Unless the receiver passes his/her encoded URL to others.）</li>
<li><span class="font-red">WebFTP controls file-uploading by scripts. Do not disable javascript when uploading files.</span></li>
</ol>
<h3>Note to ITRI Staff:</h3>
<ol>
<li>WebFTP provides an easy-to-use tool for ITRI staff to transfer business related large file(s).</li>
<li>This FTP service can only be used by authenticated users. For ITRI staff an user account is created spontaneously in ITRI's AD domain when he/she becomes an ITRI employee.</li>
<li>WebFTP is capable of transferring multiple files simultaneously and the file size is unlimited by default. The larger the file size, the more likely it will cause transmission failure.</li>
<li>If the file is larger than <span class="font-red">50MB</span>, it is recommended to send the file via other storage media (such as CD-R, USB and etc) instead of using WebFTP to save both network bandwidth and time.</li>
</ol>
<h3>Note to ITRI partner:</h3>
<ol>
<li>This service is for ITRI partner to deliver business related large files to ITRI staff.</li>
<li>Anyone who has business connection with ITRI can crate a new account to use WebFTP service.</li>
<li>If the account is left unused for 90 days, the system will send an E-mail to ask whether to extend this account for more days or to cancel the account.</li>
<li>It is required that the user must have at least one available E-mail address in order to create a new WebFTP account.</li>
</ol>
ITRI IT Service and Development Center <br />
Tel: +886-3-591-8899
</div>            
</asp:Content>
