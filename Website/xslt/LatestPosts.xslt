<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:output method="html" />
	
	<xsl:template match="/">
		<ul>
			<xsl:apply-templates select="rss/channel/item" />
		</ul>
	</xsl:template>
	
	<xsl:template match="item">
		<li><a href="index.aspx" title="Data: {pubDate}" class="menu{position() mod 2}"><xsl:value-of select="title" /></a></li>
	</xsl:template>
	
</xsl:stylesheet>

  