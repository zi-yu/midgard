<%@ Page language="c#" Codebehind="DoSearch.aspx.cs" AutoEventWireup="false" Inherits="FlexWiki.Web.DoSearch" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title runat="server" id="title">Search</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<%= InsertStylesheetReferences() %>
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<p>Search for:
				<asp:textbox runat="server" id="searchString" text="type regex search" />
				<asp:button runat="server" onclick="Search" text="Search" ID="Button1" />
				<script runat="server">
			void Search(object sender, EventArgs e)
			{
				Response.Redirect("search.aspx?search=" + searchString.Text);
			}
				</script>
		</form>
		</P>
	</body>
</HTML>
