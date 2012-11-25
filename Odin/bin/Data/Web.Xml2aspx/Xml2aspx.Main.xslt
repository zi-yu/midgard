<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" 
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:asp="http://www.microsoft.com/schemas"
  xmlns:m="http://midgard.zi-yu.com/schemas/Xml2aspx"
  xmlns:Sms="http://midgard.zi-yu.com/schemas/projects/$project">

  <!-- ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    Imports
  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~-->
  
  <xsl:import href="Xml2aspx.NamedTemplates.xslt"/>
  <xsl:import href="Xml2aspx.Entities.xslt"/>
  <xsl:import href="Xml2aspx.Screen.xslt"/>

  <!-- ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    Output
  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~-->
  
  <xsl:output method="html" indent="yes" omit-xml-declaration="yes" />

  <!-- ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    Variables
  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~-->

  <xsl:variable name="prefix" select=" '$project'" />
  <xsl:variable name="updateButtonClass" select=" 'UpdateButton' " />

</xsl:stylesheet>
