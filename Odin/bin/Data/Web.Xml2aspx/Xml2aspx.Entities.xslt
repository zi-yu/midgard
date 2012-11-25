<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" 
 xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:asp="http://www.microsoft.com/schemas"
  xmlns:m="http://midgard.zi-yu.com/schemas/Xml2aspx"
  xmlns:Sms="http://midgard.zi-yu.com/schemas/projects/$project">

  <!-- ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    EntityItem rendering
  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~-->

  <xsl:template match="m:entity">
    <xsl:element name="{ $prefix }:{ @type }Item">
      <xsl:call-template name="addSource" />
      <xsl:call-template name="addRunat" />
      <dl class="entityItem">
        <xsl:apply-templates select="m:fields/m:field" mode="listItem" />
      </dl>
    </xsl:element>
  </xsl:template>

  <!-- ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    EntityEditor rendering
  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~-->

  <xsl:template match="m:entity[ @method = 'Editor' ]">
    <xsl:element name="{ $prefix }:{ @type }Editor">
      <xsl:call-template name="addSource" />
      <xsl:call-template name="addRunat" />
      <dl class="entityItem">
        <xsl:apply-templates select="m:fields/m:field" mode="editor" />
      </dl>
      <xsl:call-template name="writeUpdateButton" />
    </xsl:element>
  </xsl:template>

  <!-- ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    EntityList rendering
  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~-->

  <xsl:template match="m:entity[ @method = 'List' ]">
    <xsl:element name="{ $prefix }:{ @type }List">
      <xsl:call-template name="addRunat" />
      <table class="list">
        <thead>
          <tr>
            <xsl:apply-templates select="m:fields/m:field" mode="th"/>
          </tr>
        </thead>
        <tbody>
          <xsl:element name="{ $prefix }:{ @type }Item">
            <xsl:call-template name="addRunat" />
            <tr>
              <xsl:apply-templates select="m:fields/m:field" mode="td"/>
            </tr>
          </xsl:element>
        </tbody>
      </table>
    </xsl:element>
  </xsl:template>

  <!-- ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    Field rendering
  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~-->

  <xsl:template match="m:field" mode="listItem">
    <dt>
      <xsl:call-template name="writeLabel">
        <xsl:with-param name="value" select="@name" />
      </xsl:call-template>
    </dt>
    <dd>
      <xsl:call-template name="writeItemField">
        <xsl:with-param name="entity" select="../../@type" />
        <xsl:with-param name="field" select="@name" />
      </xsl:call-template>
    </dd>
  </xsl:template>

  <xsl:template match="m:field" mode="td">
    <td>
      <xsl:call-template name="writeItemField">
        <xsl:with-param name="entity" select="../../@type" />
        <xsl:with-param name="field" select="@name" />
      </xsl:call-template>
    </td>
  </xsl:template>

  <xsl:template match="m:field" mode="editor">
    <dt>
      <xsl:call-template name="writeLabel">
        <xsl:with-param name="value" select="@name" />
      </xsl:call-template>
    </dt>
    <dd>
      <xsl:call-template name="writeEditorField">
        <xsl:with-param name="entity" select="../../@type" />
        <xsl:with-param name="field" select="@name" />
      </xsl:call-template>
    </dd>
  </xsl:template>

  <xsl:template match="m:field" mode="th">
    <th>
      <xsl:call-template name="writeLabel">
        <xsl:with-param name="value" select="@name" />
      </xsl:call-template>
    </th>
  </xsl:template>

</xsl:stylesheet>
