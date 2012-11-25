<%@ Control Language="c#" AutoEventWireup="false" Codebehind="LeftMenu.ascx.cs" Inherits="WebFramework.Ascx.LeftMenu" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ OutputCache Duration="60000" VaryByParam="none" %>
<%@ Register TagPrefix="WebFramework" Namespace="WebFramework" Assembly="Psantos3" %>

<div id="left_menu">
	<h2>Site</h2>
	<ul>
		<li><a href="index.aspx" class="menu1">Blog</a></li>
		<li><a href="contents.aspx" class="menu1">&Iacute;ndice</a></li>
		<li><a href="wiki.aspx" class="menu1">Wiki</a></li>
		<li><a href="shop.aspx" class="menu1">Loja</a></li> 
		<li><a href="wiki.aspx?topic=PRE.SobreMim" class="menu1">Contacto</a></li>
	</ul>
	
	<h2>Projectos</h2>
	<ul>
		<li><a href="wiki.aspx?topic=PRE.Orionsbelt" class="menu1">Orion's Belt</a></li>
		<li><a href="wiki.aspx?topic=PRE.Midgard" class="menu1">Midgard</a></li>
	</ul>
	
	<h2>Wiki Index</h2>
	<ul>
		<li><a href="wiki.aspx?topic=PRE.Artigos" class="menu1">Artigos</a></li>
		<li><a href="wiki.aspx?topic=PRE.Projectos" class="menu1">Projectos</a></li>
		<li><a href="wiki.aspx?topic=PRE.Gamedev" class="menu1">Gamedev</a></li>
		<li><a href="wiki.aspx?topic=PRE.Bibliografia" class="menu1">Bibliografia</a></li>
		<li><a href="wiki.aspx?topic=PRE.Trabalhos" class="menu1">Trabalhos</a></li>
		<li><a href="wiki.aspx?topic=PRE.BlogsQueLeio" class="menu1">Outros Blogs</a></li>
		<li><a href="wiki.aspx?topic=PRE.SobreMim" class="menu1">Sobre Mim</a></li>
	</ul>
	
	<h2>&Uacute;ltimos Posts</h2>
	<WebFramework:LatestPosts runat="server" />
	
	<h2>&Uacute;ltimos T&oacute;picos</h2>
	<WebFramework:RecentWikiTopics runat="server" />

</div>
