<%@ Control Language="c#" AutoEventWireup="false" Codebehind="LeftMenu.ascx.cs" Inherits="WebFramework.Ascx.LeftMenu" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="WebFramework" Namespace="WebFramework" Assembly="Psantos3" %>

<div id="right_menu">
	<h2>Site</h2>
	<ul>
		<li><a href="index.aspx" class="menu1">Home</a></li>
		<li><a href="http://www.cc.isel.ipl.pt/CS/blogs/midgard/default.aspx" class="menu1">Blog (CC)</a></li>
		<li><a href="contents.aspx" class="menu1">&Iacute;ndice</a></li>
		<li><a href="wiki.aspx" class="menu1">Wiki</a></li>
		<li><a href="wiki.aspx?topic=Midgard.Apresentacao" class="menu1">Apresenta&ccedil;&atilde;o</a></li>
        <li><a href="stats.aspx" class="menu1">Estat&iacute;sticas</a></li>
	</ul>

	<h2>O Seu Historial</h2>
	<WebFramework:RequestViewer runat="server" />
	
	<h2>&Uacute;ltimos T&oacute;picos</h2>
	<WebFramework:RecentWikiTopics runat="server" />

	<h2>&Uacute;ltimos Referrals</h2>
	<WebFramework:ShowReferrals runat="server" ID="Showreferrals1"/>

</div>
