﻿<%@ Page Title="" Language="C#" MasterPageFile="~/sfts/MasterPage.Master" AutoEventWireup="true" CodeBehind="page_how.aspx.cs" Inherits="WebAppl.page_how" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="fullblock font-gray lineheight03">
<h3>系統需求：</h3>
本系統建議使用 IE 8 以上版本，1024x768 的螢幕解析度。
<br /><br />
<h3>系統說明：</h3>
<ol>
<li>本系統一次傳輸的<span class="font-red">檔案大小總和上限為2G</span>，若檔案較大，則整個傳輸過程需視當時的網路流量而定，且失敗率會相對提昇。</li>
<li>所有上傳的檔案將在<span class="font-red">上傳 7 天</span>後由系統自動刪除，「傳檔者」無需手動刪除上傳之檔案。</li>
<li>所有上傳的檔案由系統自動維護，<span class="font-red">一般使用者無法直接存取到檔案</span>，傳檔者毋須擔心檔案遭「<span class="font-red">非收檔者</span>」存取（若收檔者任意傳送加密網址給他人，則不在此限）。</li>
<li><span class="font-red">使用本系統進行檔案上傳時，瀏覽器請勿關閉script執行權限，否則將無法順利上傳檔案。</span></li>
</ol>
<h3>院內同仁注意事項：</h3>
<ol>
<li>本服務提供工研院同仁，因<span class="font-red">公務上需求</span>而必須傳送大型檔案時使用。</li>
<li>本服務<span class="font-red">必須經過身份認證</span>後方可使用，「使用者帳號」為「<span class="font-red">全院 ITRI AD 網域(Domain)</span>」的帳號(同仁報到時即自動建立)。</li>
<li>本服務<span class="font-red">允許同仁同時傳送多個檔案</span>，且<span class="font-red">檔案大小不做任何限制</span>。若檔案過大時，則傳輸情況必須視當時網路流量狀況而定，且失敗率會相對提昇。</li>
<li>若同仁所要傳送的檔案大小超過 <span class="font-red">50MB</span>，則建議同仁採取其他途徑(製作成光碟片或高容量的儲存媒體如 USB) 來傳送，以減少您等待的時間，並可節省網路頻寬。</li>
</ol>
<h3>合作夥伴注意事項：</h3>
<ol>
<li>本服務提供<span class="font-red">與本院有業務往來之合作夥伴</span>，傳送檔案給本院各單位或同仁時使用。</li>
<li>任何有業務往來的合作夥伴<span class="font-red">均可以申請帳號</span>。</li>
<li>帳號 <span class="font-red">90 天</span>未使用，系統將自動寄發 E-mail 以確認延長使用期後或停止使用。</li>
<li>所有的帳號申請作業<span class="font-red">一律採用 E-mail 認證</span>，因此，申請人必須有可收發 E-mail 之電子郵件信箱方能申請帳號。</li>
<li>本院同仁若有任何問題，請洽資訊科技服務中心服務窗口：18899</li>
</ol>
維護單位/工研院 資訊科技服務中心 <br />
服務電話/03-5918899
</div>            

</asp:Content>
