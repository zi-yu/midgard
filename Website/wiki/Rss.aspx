<%@ Page language="c#" contenttype="text/xml" Codebehind="Rss.aspx.cs" AutoEventWireup="false" Inherits="FlexWiki.Web.Rss" %>
<%@ OutputCache Duration="1" VaryByParam="namespace;inherited" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" > 
<% 
	Response.Clear(); 
	Response.ContentType="text/xml"; 
	DoSearch();	
%>
