<%@ Page language="c#" Codebehind="default.aspx.cs" AutoEventWireup="false" Inherits="FlexWiki.Web.Admin.Default" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title></title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="VisualStudio.HTML" name="ProgId">
		<meta content="Microsoft Visual Studio .NET 7.1" name="Originator">
		<%= MainStylesheetReference() %>
	</HEAD>
	<body class='Dialog'>
		<H1><FONT face="Verdana">Administration Home</FONT></H1>
		<H2><FONT face="Verdana" size="4">Administration</FONT></H2>
		<P>
			<TABLE id="Table1" cellSpacing="1" cellPadding="1" border="0">
				<TR>
					<TD vAlign="top">
						<img src="../images/go.gif"><FONT face="Verdana"></FONT>
					</TD>
					<TD vAlign="top"><A href="Namespaces.aspx"><FONT face="Verdana">Namespaces</FONT></A><FONT face="Verdana">
						</FONT>
					</TD>
				</TR>
				<TR>
					<TD vAlign="top"><FONT face="Verdana"></FONT></TD>
					<TD vAlign="top"><FONT face="Verdana"><FONT color="dimgray"><FONT size="2">List, modify and 
									create namespaces</FONT> </FONT></FONT>
					</TD>
				</TR>
				<TR>
					<TD vAlign="top"><A href="Newsletter.aspx">
							<asp:Image id="Image3" runat="server" ImageUrl="../images/go.gif"></asp:Image><FONT face="Verdana"></FONT></A></TD>
					<TD vAlign="top"><A href="config.aspx"><FONT face="Verdana">Validate Configuration</FONT></A>
					</TD>
				</TR>
				<TR>
					<TD vAlign="top"></TD>
					<TD vAlign="top"><FONT face="Verdana" color="#696969" size="2">Validate that your 
							FlexWiki site is correctly configured</FONT>
					</TD>
				</TR>
			</TABLE>
		</P>
		<H2><FONT face="Verdana" size="4">Debugging</FONT></H2>
		<P>
			<TABLE id="Table2" cellSpacing="1" cellPadding="1" border="0">
				<TR>
					<TD vAlign="top">
						<asp:Image id="Image2" runat="server" ImageUrl="../images/go.gif"></asp:Image></TD>
					<TD vAlign="top"><A href="Newsletter.aspx"><FONT face="Verdana">Newsletter Daemon</FONT></A><FONT face="Verdana">
						</FONT>
					</TD>
				</TR>
				<TR>
					<TD vAlign="top"><FONT face="Verdana"></FONT></TD>
					<TD vAlign="top"><FONT face="Verdana"><FONT color="dimgray"><FONT size="2">Show information 
									about the newsletter delivery daemon status</FONT> </FONT></FONT>
					</TD>
				</TR>
				<TR>
					<TD vAlign="top"><A href="ShowCache.aspx">
							<asp:Image id="Image4" runat="server" ImageUrl="../images/go.gif"></asp:Image><FONT face="Verdana"></FONT></A></TD>
					<TD vAlign="top"><A href="ShowCache.aspx"><FONT face="Verdana">Cache </FONT></A>
					</TD>
				</TR>
				<TR>
					<TD vAlign="top"><FONT face="Verdana"></FONT></TD>
					<TD vAlign="top"><FONT face="Verdana"><FONT color="dimgray"><FONT size="2">Show a list of 
									everything in the cache</FONT> </FONT></FONT>
					</TD>
				</TR>
				<TR>
					<TD vAlign="top"><A href="Dump.aspx">
							<asp:Image id="Image5" runat="server" ImageUrl="../images/go.gif"></asp:Image><FONT face="Verdana"></FONT></A>
					</TD>
					<TD vAlign="top"><A href="Dump.aspx"><FONT face="Verdana">Federation </FONT></A>
					</TD>
				</TR>
				<TR>
					<TD vAlign="top"><FONT face="Verdana"></FONT></TD>
					<TD vAlign="top"><FONT face="Verdana"><FONT color="dimgray"><FONT size="2">Show information 
									about the federation and all namespaces</FONT> </FONT></FONT>
					</TD>
				</TR>
			</TABLE>
		</P>
	</body>
</HTML>
