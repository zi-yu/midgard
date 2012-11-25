<%@ Page language="c#" Codebehind="Config.aspx.cs" AutoEventWireup="false" Inherits="FlexWiki.Web.Admin.Config" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" > 

<html>
 <HEAD>
		<title>FlexWiki Configuration</title>
		<LINK href='<%= RootUrl(Request) %>../wiki.css' type='text/css' rel='stylesheet'>
	</HEAD>
	<body>
		<% 
		Configure(); 
		%>
	</body>
</html>
