<%@ Control Language="c#" AutoEventWireup="false" Codebehind="shop.ascx.cs" Inherits="WebFramework.Ascx.Shop" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="Shop" Namespace="WebFramework.Shop" Assembly="Psantos3" %>

<div id="wiki_header">
	<span id="site_utils">
		<Shop:CartPreview runat="server" />
	</span>
	<span id="site_nav">
		<a href="index.aspx">Home</a> <span>&raquo;</span> <a href="shop.aspx">Loja</a>
	</span>
</div>

<div class="wiki_topic">Loja</div>
<div id="shopList">
	<p>Esta é a minha lojinha onde tenho disponível para venda (ou troca) de certos artigos para os quais já
	não tenho utilidade.</p>

	<Shop:ItemsList runat="server" />
</div>
