<%@ Page language="c#" Codebehind="Print.aspx.cs" AutoEventWireup="false" Inherits="FlexWiki.Web.Print" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<meta name="Robots" content="NOINDEX, NOFOLLOW">
		<title>
			<%= GetTopicName() %>
		</title>
		<%= InsertStylesheetReferences() %>
		<style type="text/css">
			body 
			{
				margin: 4px;
			}
		</style>
	</HEAD>
	<body>
		<% DoPage(); %>
	</body>
</HTML>
