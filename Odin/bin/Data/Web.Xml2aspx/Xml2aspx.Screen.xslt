<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" 
xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:asp="http://www.microsoft.com/schemas"
  xmlns:m="http://midgard.zi-yu.com/schemas/Xml2aspx"
  xmlns:Sms="http://midgard.zi-yu.com/schemas/projects/$project">

  <!-- ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    Screen rendering
  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~-->

  <xsl:template match="m:screen">
    <xsl:call-template name="writeAspxPageHeader" />
    <asp:Content ContentPlaceHolderID="{ @placeHolder }" runat="server">
      <xsl:apply-templates select="*" />
    </asp:Content>
  </xsl:template>

</xsl:stylesheet>
