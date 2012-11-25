<%@ Control Language="c#" AutoEventWireup="false" Codebehind="wiki.ascx.cs" Inherits="WebFramework.WikiPage" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="Wiki" Namespace="WebFramework" Assembly="Psantos3" %>

<div id="wiki_header">
	<span id="site_utils">
		<asp:HyperLink Text="T&oacute;picos Relacionados" ID="related" Runat="server" />
	</span>
	<span id="site_nav">
		<a href="index.aspx">Home</a> <span>&raquo;</span> <a href="wiki.aspx">Wiki</a> <Wiki:Parent id="parent" runat="server"/> &raquo; <asp:Literal id="caption" Runat="server" />
	</span>
</div>

<Wiki:WikiTopic ShowTitle="true" ID="wikiTopic" runat="server" />
