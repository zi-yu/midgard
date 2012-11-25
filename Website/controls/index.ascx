<%@ Control Language="c#" AutoEventWireup="false" Codebehind="index.ascx.cs" Inherits="WebFramework.Ascx.Index" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="Wiki" Namespace="WebFramework" Assembly="Psantos3" %>

<div id="wiki_header">
	<span id="site_utils">
		<a href="<%= WebFramework.Utility.BlogUrl %>">Blog Feed :: XML</a>
	</span>
	<span id="site_nav">
		<a href="index.aspx">Home</a> <span>&raquo;</span> <a href="wiki.aspx">Blog</a>
	</span>
</div>

<Wiki:BlogViewer runat="server" />
<hr/>
<p><asp:Label id="label" CssClass="nav" Runat="server" /></p>
