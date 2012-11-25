<%@ Control Language="c#" AutoEventWireup="false" %>

<div id="wiki_header">
	<span id="site_utils">
	</span>
	<span id="site_nav">
		<a href="index.aspx">Home</a> <span>&raquo;</span> Request Headers
	</span>
</div>

<div class="wiki_topic">Request Headers/Information</div>

<table>
	<tr>
		<td>Authority</td>
		<td><%= Request.Url.Authority %></td>
	</tr>
	<tr>
		<td>ApplicationPath</td>
		<td>(<%= Request.ApplicationPath %>) <%= WebFramework.Utility.AppPath %></td>
	</tr>
	<%
		foreach( string key in Request.Headers.AllKeys ) {
			Response.Write(string.Format("<tr><td>{0}</td><td>{1}</td></tr>", key, Request.Headers[key]));
		}
	%>
</table>
