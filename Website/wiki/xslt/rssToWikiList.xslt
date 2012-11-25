<?xml version="1.0" encoding="UTF-8" ?>
<!-- 
#region License Statement
// Copyright (c) Microsoft Corporation.  All rights reserved.
//
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
#endregion
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:cs="urn:cs">
	<xsl:output method="text" />
	<msxsl:script language="c#" implements-prefix="cs">
<![CDATA[
	public string ToLocalTime(string pubDate)
	{
		DateTime t = DateTime.Parse(pubDate);
		DateTime l = t.ToLocalTime();
		return t.ToString("yyyy/M/d(ddd) tt h:mm");
	}
]]>
</msxsl:script>
	<xsl:template match="/">
		<xsl:for-each select="rss/channel/item">
			<xsl:if test="position() &lt; 5">
	*"<xsl:value-of select="title" />":<xsl:value-of select="guid" /> ""<xsl:value-of select="cs:ToLocalTime(pubDate)" />""</xsl:if>
		</xsl:for-each>
	</xsl:template>
</xsl:stylesheet>