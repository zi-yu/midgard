<%@ Page language="c#" Codebehind="Dump.aspx.cs" AutoEventWireup="false" Inherits="FlexWiki.Web.Admin.Dump" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" > 
<html>
 <HEAD>
		<title>Wiki Dump</title>
		<%= MainStylesheetReference() %>
	</HEAD>
	<body>
		<% DoDump(); %>
	</body>
</html>
