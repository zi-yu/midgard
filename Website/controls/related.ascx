<%@ Control Language="c#" AutoEventWireup="false" Codebehind="related.ascx.cs" Inherits="WebFramework.Related" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="Wiki" Namespace="WebFramework" Assembly="Psantos3" %>

<div id="wiki_header">
	<span id="site_nav">
		<a href="index.aspx" class="nav">Home</a> <span class="nav">&raquo;</span> <a href="wiki.aspx" class="nav">Wiki</a>
<span class="nav">&raquo;</span> <a href="" class="nav">Procura por T&oacute;pico</a>
	</span>
</div>

<div class="wiki_topic">T&oacute;picos Relacionados</div>
<Wiki:WikiSearch id="search" runat="server" />

