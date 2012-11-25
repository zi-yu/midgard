<%@ Page language="c#" Codebehind="AccessDenied.aspx.cs" AutoEventWireup="false" Inherits="FlexWiki.Web.AccessDenied" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Wiki Logon</title>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<%= InsertStylesheetReferences() %>
	</HEAD>
	<body class='Dialog'>
<fieldset>
	<legend class='DialogTitle'>Security</legend>

		<form id="LogonForm" runat="server" method="post">
			<table>
				<TBODY>
					<tr>
						<td><asp:Label id="Msg" ForeColor="red" Font-Name="Verdana" Font-Size="10" runat="server">Access Denied Message</asp:Label>
		</form>
		</TD></TR></TBODY></TABLE>
</fieldset>
	</body>
</HTML>
