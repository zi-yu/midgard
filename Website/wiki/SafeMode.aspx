<%@ Page language="c#" Codebehind="SafeMode.aspx.cs" AutoEventWireup="false" Inherits="FlexWiki.Web.Default" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<TITLE>
		<%= GetTopicName().Name %>
	</TITLE>
	<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	<% DoHead(); %>
	<%= InsertStylesheetReferences() %>
	<style type="text/css"> .History { background: #FFCC00; color: black; font-size: 8pt; width: 140px; overflow: auto; }
	.History a { color: black; text-decoration:none; }
	.History a:hover { color: black; text-decoration:underline; }
	.HistoryHeader { background: #6D1514; color: white; height: 18px; font-size: 8pt; padding: 3px; }
	.HistoryHeader a { color: gold; text-decoration:none; }
	.HistoryHeader a:hover { color: gold; text-decoration:underline; }
	.Hist { padding: 3px; height: expression(document.body.clientHeight - 22 - 28 - 20 - 15 - 18); overflow: auto; }
	</style>
	<script  type="text/javascript"  language="javascript">
			

			/////////////////////////////////////////////////////////////////////////

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
						
	</script>
	<% DoPage(); %>
</HTML>
