<%@ Page language="c#" Codebehind="Blade.aspx.cs" AutoEventWireup="false" Inherits="FlexWiki.Web.Blade" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" > 

<html>
      <HEAD>
        <title></title>
        <meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
        <meta name="CODE_LANGUAGE" Content="C#">
        <meta name="vs_defaultClientScript" content="JavaScript">
        <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
        <% 
			string sheet = (string)(Request.QueryString["stylesheet"]);
			if (sheet != null)
				Response.Write("<LINK href=\"" + sheet + "\" type=\"text/css\" rel=\"stylesheet\">");
		%>
    </HEAD>
    <body scroll='no'>
    <% DoPage(); %>

    </body>
</html>
