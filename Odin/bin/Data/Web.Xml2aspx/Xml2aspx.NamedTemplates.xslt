<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" 
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:asp="http://www.microsoft.com/schemas"
  xmlns:m="http://midgard.zi-yu.com/schemas/Xml2aspx"
  xmlns:Sms="http://midgard.zi-yu.com/schemas/projects/$project">

  <!-- ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    Writes the ASP.NET aspx Page directive
  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~-->
  
  <xsl:template name="writeAspxPageHeader">
    <xsl:text disable-output-escaping="yes">&lt;%@ Page Language="C#" AutoEventWireup="false" </xsl:text>
    <xsl:text>Inherits="</xsl:text>
    <xsl:value-of select="@inherits"/>
    <xsl:text>" </xsl:text>
    <xsl:text>MasterPageFile="~/</xsl:text>
    <xsl:value-of select="@master"/>
    <xsl:text>" </xsl:text>
    <xsl:text disable-output-escaping="yes">%&gt;</xsl:text>
    <xsl:text>
</xsl:text>
  </xsl:template>

  <!-- ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    Adds the runat="server" attribute
  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~-->
  
  <xsl:template name="addRunat">
    <xsl:attribute name="runat">
      <xsl:text>server</xsl:text>
    </xsl:attribute>
  </xsl:template>

  <!-- ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    Adds the Source="<param>" attribute
  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~-->

  <xsl:template name="addSource">
    <xsl:if test=" normalize-space(@source) != '' ">
      <xsl:attribute name="Source">
        <xsl:value-of select="@source"/>
      </xsl:attribute>
    </xsl:if>
  </xsl:template>

  <!-- ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    Writes a label (may be localized)
  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~-->

  <xsl:template name="writeLabel">
    <xsl:param name="value" />
    <xsl:value-of select="$value"/>
  </xsl:template>

  <!-- ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    Writes the update button
  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~-->

  <xsl:template name="writeUpdateButton">
    <xsl:element name="{ $prefix }:{ $updateButtonClass }" >
      <xsl:call-template name="addRunat" />
    </xsl:element>
  </xsl:template>

  <!-- ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    Writes field
  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~-->

  <xsl:template name="writeItemField">
    <xsl:param name="entity" />
    <xsl:param name="field" />
    <xsl:element name="{ $prefix }:{ $entity }{ $field }">
      <xsl:call-template name="addRunat" />
    </xsl:element>
  </xsl:template>

  <xsl:template name="writeEditorField">
    <xsl:param name="entity" />
    <xsl:param name="field" />
    <xsl:element name="{ $prefix }:{ $entity }{ $field }Editor">
      <xsl:call-template name="addRunat" />
    </xsl:element>
  </xsl:template>

</xsl:stylesheet>
