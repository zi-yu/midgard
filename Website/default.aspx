<%@ Page language="c#" Codebehind="default.aspx.cs" AutoEventWireup="false" Inherits="WebFramework.__Default" %><%@ Register TagPrefix="Right" TagName="Menu" Src="controls/RightMenu.ascx" %>
<%@ Register TagPrefix="Footer" TagName="Part" Src="controls/Footer.ascx" %>
<%@ Register TagPrefix="WebFramework" Namespace="WebFramework" Assembly="Psantos3" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>Midgard - RAD</title>
		<link href="wiki/wiki.css" rel="stylesheet" type="text/css" media="screen" />
		<link href="styles/style.css" rel="stylesheet" type="text/css" media="screen" />
		<link href="styles/print.css" rel="stylesheet" type="text/css" media="print" />
		<link rel="alternate" type="application/rss+xml" title="Blog Feed" href="<%= WebFramework.Utility.BlogUrl %>" />
		<meta name="DC.title" content="Pedro Santos Blog/Wiki" />
		<script src="scripts/Javascript.js" type="text/javascript"></script>
	</head>
	<body id="body">
		<div id='TopicTip' class='TopicTip' ></div>
		<form method="post" runat="server" id="Form1">
			<div id="header">
				<span id="header-search">
					<asp:TextBox ID="toSearch" Runat="server" />
					<asp:Button ID="doSearch" OnClick="DoSearch" Text="Procurar" Runat="server" />
				</span>
				<div id="header-title">:: Midgard</div>
				<div id="header-subtitle">Ferramenta RAD para suporte ao desenvolvimento de aplicações web</div>
			</div>
			<table id="content">
				<tr>
					<td valign="top" width="100%">
						<div class="content">
							<asp:PlaceHolder ID="content" Runat="server" />
						</div>
					</td>
					<td valign="top"><Right:Menu runat="server" /></td>
				</tr>
			</table>
		</form>
		<Footer:Part runat="server" />
	</body>
</html>
