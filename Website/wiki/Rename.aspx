<%@ Page language="c#" Codebehind="Rename.aspx.cs" AutoEventWireup="false" Inherits="FlexWiki.Web.Rename" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Rename Topic</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="Robots" content="NOINDEX, NOFOLLOW">
		<%= InsertStylesheetReferences() %>
	</HEAD>
	<body class='Dialog'>
		<fieldset><legend class='DialogTitle'>
				<%
	if (Request.HttpMethod == "POST")
	{
		Response.Write("Rename <b>" + HttpUtility.HtmlEncode(OldName)  + "</b></legend>");
		PerformRename();
	}
	else
	{
	%>
				Rename <b>
					<%= HttpUtility.HtmlEncode(AbsTopicName.Name)  %>
				</b>
			</legend>
			<p><b>Important guidelines</b> (rename is not always straightforward!):
				<ul>
					<li>
						When you rename a topic, you can ask to have references to the topic 
						automatically updated.
						<ul>
							<li>
							References from other namespaces are not updated
							<li>
							Plural references are not found
						</ul>
					<li>
					Because not all references can be reliably found (e.g., because a topic has 
					been bookmarked by someone, referenced from another namespace), a page will be 
					automatically generated that tells people where the new page is.
					<li>
					Often when you rename a topic you are changing its meaning. As a result, you 
					might want to change the text surrounding the references to the topic; please 
					consider reviewing the current references when you're done renaming to be sure 
					they still make sense.
				</ul>
				<hr noshade size='1'>
				<form id="RenameForm" method="post" ACTION="rename.aspx">
					<input style='display: none' type="text"  name="oldName" value ="<%= HttpUtility.HtmlEncode(AbsTopicName.Name)  %>">
					<b>Old</b> name:
					<%= HttpUtility.HtmlEncode(AbsTopicName.Name)  %>
					<br />
					<b>New</b> name: <input style='font-size: x-small' type="text"  name="newName" value ="<%= HttpUtility.HtmlEncode(AbsTopicName.Name)  %>">
					<p>
						<input type="checkbox" id="fixup" name="fixup"><label for="fixup">Automatically 
							update references</label>
						<div style='display: none'>
							<br />
							<input type="checkbox" checked id="leaveRedirect" name="leaveRedirect"><label for="leaveRedirect">Generate 
								a redirect page under the old name</label>
						</div>
					</p>
					<p>
						<input style='display: none' type="text"  name="namespace" value ="<%= HttpUtility.HtmlEncode(AbsTopicName.Namespace)  %>">
						<input type='submit' ID="SaveButton" Value="Rename" />
					</p>
				</form>
				<%
	}
	%>
			</div>
		</fieldset>
	</body>
</HTML>
