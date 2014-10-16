<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:output method="text"/>
  <xsl:template match="/">
    <xsl:text>== Proc Reference&#10;&#10;</xsl:text>
    <xsl:text></xsl:text>
    <xsl:apply-templates/>
  </xsl:template>
  <xsl:variable name="category_level">
    <xsl:choose>
      <xsl:when test="/procs/category_group">=</xsl:when>
      <xsl:otherwise/>
    </xsl:choose>
  </xsl:variable>
  <xsl:template match="category_group">
    <xsl:text>=== </xsl:text>
    <xsl:value-of select="@name"/>
    <xsl:text>&#10;&#10;</xsl:text>
    <xsl:apply-templates select="proc"/>
  </xsl:template>
  <xsl:template match="proc">
    <xsl:choose>
      <xsl:when test="not(member/visibility = 'private')">
        <xsl:value-of select="$category_level"/>
        <xsl:text>=== </xsl:text>
        <xsl:value-of select="@name"/>
        <xsl:text>&#10;</xsl:text>
        <xsl:text>[horizontal]&#10;</xsl:text>
        <xsl:text>*Namespace:*:: </xsl:text>
        <xsl:value-of select="@namespace"/>
        <xsl:text>&#10;</xsl:text>
        <xsl:text>*Location:*:: </xsl:text>
        <xsl:value-of select="@location"/>
        <xsl:text>&#10;&#10;</xsl:text>
        <xsl:apply-templates select="member"/>
      </xsl:when>
      <xsl:otherwise/>
    </xsl:choose>
  </xsl:template>
  <xsl:template match="member">
    <xsl:value-of select="normalize-space(summary)"/>
    <xsl:text>&#10;&#10;</xsl:text>
    <xsl:choose>
      <xsl:when test="param">
        <xsl:text>.Parameters&#10;&#10;</xsl:text>
        <xsl:text>[caption="",width="100%",cols="2,^1s,^2,10",frame="topbot",options="header"]&#10;</xsl:text>
        <xsl:text>|===========&#10;</xsl:text>
        <xsl:text>| Name | Mode | Default | Description&#10;</xsl:text>
        <xsl:apply-templates select="param"/>
        <xsl:text>|===========&#10;</xsl:text>
      </xsl:when>
      <xsl:otherwise>
        <xsl:text>(no parameters)&#10;</xsl:text>
      </xsl:otherwise>
    </xsl:choose>
    <xsl:text>&#10;</xsl:text>
  </xsl:template>
  <xsl:template match="param">
    <xsl:text>|</xsl:text>
    <xsl:value-of select="@name"/>
    <xsl:text>|</xsl:text>
    <xsl:value-of select="@mode"/>
    <xsl:text>|</xsl:text>
    <xsl:value-of select="@default"/>
    <xsl:text>|</xsl:text>
    <xsl:value-of select="@description"/>
    <xsl:text>&#10;</xsl:text>
  </xsl:template>
</xsl:stylesheet>
