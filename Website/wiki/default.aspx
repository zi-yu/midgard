<%@ Page language="c#" Codebehind="default.aspx.cs" AutoEventWireup="false" Inherits="FlexWiki.Web.Default2" %>
<%
	StartPage();
%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<TITLE>
			<%= GetTopicName().Name %>
		</TITLE>
		<META name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<% DoHead(); %>
		<%= InsertStylesheetReferences() %>
		<SCRIPT type="text/javascript" language="javascript">
			function MainHeight()
			{
				var answer = document.body.clientHeight;
				var e;
				 return answer;
			}
						
			function MainWidth()
			{
				var answer = document.body.clientWidth;
				var e;
				
				e = document.getElementById("Sidebar");
				if (e != null)
					answer -= e.scrollWidth;
				 return answer;
			}

			function nav(s)
			{
				window.navigate(s);
			}
			
			function showChanges()
			{
				nav("<%= urlForDiffs %>");
			}
			
			function hideChanges()
			{
				nav("<%= urlForNoDiffs %>");
			}
			
			function diffToggle()
			{
				if (showDiffs.checked)
					showChanges();
				else
					hideChanges();
			}

			function showVersion()
			{
				nav(VersionList.value);
			}
		</SCRIPT>
	</HEAD>
	<% DoPage(); %>
</HTML>
<%
	EndPage();
%>
