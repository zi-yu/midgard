<%@ Control Language="c#" AutoEventWireup="false" Codebehind="contents.ascx.cs" Inherits="WebFramework.Ascx.Contents" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="Wiki" Namespace="WebFramework" Assembly="Psantos3" %>

<div id="wiki_header">
	<span id="site_utils">
	</span>
	<span id="site_nav">
		<a href="index.aspx">Home</a> <span>&raquo;</span> <a href="contents.aspx">&Iacute;ndice de Conte&uacute;dos</a>
	</span>
</div>

<div class="wiki_topic">&Iacute;ndice de Conte&uacute;dos</div>

<p/>
<asp:Literal ID="index" Runat="server" />

<Wiki:WikiSearch ID="search" runat="server" />
