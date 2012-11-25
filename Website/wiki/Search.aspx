<%@ Page language="c#" Codebehind="Search.aspx.cs" AutoEventWireup="false" Inherits="FlexWiki.Web.Search" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title id="title">Search</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<%= InsertStylesheetReferences() %>
	</HEAD>
	<body class='Dialog'>
		<% DoSearch(); %>
	</body>
</HTML>
