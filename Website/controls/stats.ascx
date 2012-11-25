<%@ Control Language="c#" AutoEventWireup="false" Codebehind="stats.ascx.cs" Inherits="WebFramework.StatsPage" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="WebFramework" Namespace="WebFramework" Assembly="Psantos3" %>

<div id="wiki_header">
	<span id="site_nav">
        <a href="index.aspx" class="nav">Home</a> <span class="nav">&raquo;</span> <a href="stats.aspx" class="nav">Estat&iacute;sticas</a>
	</span>
</div>

<div class="wiki_topic">Estat&iacute;sticas dos Artigos</div>
<p>Estas estatísticas indicam a quantidade de <i>pageviews</i> de cada artigo.</p>
<WebFramework:ShowStats runat="server" />
