<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:output method="html" />
	
	<xsl:template match="/">
		<div id="blog">
			<xsl:apply-templates select="rss/channel/item" />
		</div>
	</xsl:template>
	
	<xsl:template match="item">
		<h1><xsl:value-of select="title" /></h1>
		<xsl:value-of select="description" disable-output-escaping="yes" />
		<div id="postInfo">
			<b>#</b> <a href="{link}">Link</a> | <a href="{comments}"><xsl:value-of select="slash:comments" xmlns:slash="http://purl.org/rss/1.0/modules/slash/" /> Comentário(s)</a> | 
			Data: <xsl:value-of select="pubDate" />
		</div>
	</xsl:template>
	
</xsl:stylesheet>

  