<%@ Page language="c#" Codebehind="Login.aspx.cs" AutoEventWireup="false" Inherits="FlexWiki.Web.Login" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Wiki Logon</title>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<%= InsertStylesheetReferences() %>
	</HEAD>
	<body class="Dialog">
		<fieldset><legend class="DialogTitle">Login</legend>
			<form id="LogonForm" method="post" runat="server">
				<table>
					<tr>
						<td><asp:label id="userEmailLabel" runat="server">Email:</asp:label></td>
						<td><asp:textbox id="userEmail" runat="server"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" ErrorMessage="*" Display="Static" ControlToValidate="userEmail"></asp:requiredfieldvalidator><asp:regularexpressionvalidator id="RegexValidator" runat="server" ErrorMessage="Invalid format for e-mail address."
								Display="Static" ControlToValidate="userEmail" EnableClientScript="false" ValidationExpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"></asp:regularexpressionvalidator></td>
					</tr>
					<tr>
						<td><asp:label id="userPasswordLabel" runat="server">Password:</asp:label></td>
						<td><asp:textbox id="userPassword" runat="server" TextMode="Password"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator2" runat="server" ErrorMessage="*" Display="Static" ControlToValidate="userPassword"></asp:requiredfieldvalidator></td>
					</tr>
					<tr>
						<td><asp:label id="userPersistLabel" runat="server">Remember Me:</asp:label></td>
						<td><asp:checkbox id="userPersist" runat="server"></asp:checkbox></td>
					</tr>
					<tr>
						<td><asp:button id="logonButton" runat="server" Text="Logon"></asp:button></td>
						<td><asp:button id="RemindMeButton" runat="server" Visible="False" Text="Request Password"></asp:button></td>
					</tr>
				</table>
				<asp:label id="Msg" runat="server" Font-Size="10" Font-Name="Verdana" ForeColor="red"></asp:label></form>
		</fieldset>
	</body>
</HTML>
