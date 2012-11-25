<%@ Control Language="c#" AutoEventWireup="false" Codebehind="item.ascx.cs" Inherits="WebFramework.Ascx.Item" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="Shop" Namespace="WebFramework.Shop" Assembly="Psantos3" %>
<%@ Register TagPrefix="Wiki" Namespace="WebFramework" Assembly="Psantos3" %>

<div id="wiki_header">
	<span id="site_utils">
		<Shop:CartPreview runat="server" />
	</span>
	<span id="site_nav">
		<a href="index.aspx">Home</a> » <a href="shop.aspx">Loja</a> » Artigo <asp:Literal id="title" Runat="server" />
	</span>
</div>

<div id="item">
	<h1><asp:Literal id="caption" Runat="server" /></h1>
	<Shop:ItemPreview id="preview" runat="server" />
</div>

<Wiki:WikiTopic id="wikiTopic" ShowTitle="false" runat="server" />
