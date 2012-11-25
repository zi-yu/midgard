<%@ Page language="c#" Codebehind="default.aspx.cs" AutoEventWireup="false" Inherits="Psantos.__Default" %>
<%@ Register TagPrefix="Left" TagName="Menu" Src="controls/LeftMenu.ascx" %>
<%@ Register TagPrefix="Right" TagName="Menu" Src="controls/RightMenu.ascx" %>
<%@ Register TagPrefix="Footer" TagName="Part" Src="controls/Footer.ascx" %>
<html>
	<head>
		<title>Pedro Santos</title>
		<link href="wiki/wiki.css" rel="stylesheet">
		<link href="styles/style.css" rel="stylesheet">
		<script src="scripts/Javascript.js" type="text/javascript"></script>
	</head>
	<body>
		<form method="post" runat="server" ID="Form1">
			<table width="100%" class="title">
				<tr>
					<td><span class="title">Pedro Santos</span></td>
					<td align="right" valign="middle">
						<asp:TextBox ID="toSearch" Runat="server" />
						<asp:Button ID="doSearch" OnClick="DoSearch" Text="Procurar" Runat="server" />
					</td>
				</tr>
			</table>
			<table width="100%">
				<tr>
					<td valign="top"><Left:Menu runat="server" /></td>
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
