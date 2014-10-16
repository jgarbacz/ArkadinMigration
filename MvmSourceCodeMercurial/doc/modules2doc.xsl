<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:output method="text"/>
  <xsl:template match="/">
    <xsl:text>== Module Reference&#10;&#10;</xsl:text>
    <xsl:text></xsl:text>
    <xsl:apply-templates/>
  </xsl:template>
  <xsl:variable name="category_level">
    <xsl:choose>
      <xsl:when test="/modules/category_group">=</xsl:when>
      <xsl:otherwise/>
    </xsl:choose>
  </xsl:variable>
  <xsl:template match="category_group">
    <xsl:text>=== </xsl:text>
    <xsl:value-of select="@name"/>
    <xsl:text>&#10;&#10;</xsl:text>
    <xsl:apply-templates select="./*">
      <xsl:with-param name="depth" select="0"/>
      <xsl:with-param name="maxDepth" select="0"/>
      <xsl:with-param name="group" select="''"/>
    </xsl:apply-templates>
  </xsl:template>
  <xsl:template match="element">
    <xsl:param name="depth"/>
    <xsl:param name="maxDepth"/>
    <xsl:param name="group"/>
    <xsl:variable name="myMaxDepth">
      <xsl:choose>
        <xsl:when test="./module_depth">
          <xsl:value-of select="./module_depth + 1"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$maxDepth"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="currentElement">
      <xsl:text disable-output-escaping="yes">| &lt;</xsl:text>
      <xsl:value-of select="@name"/>
      <xsl:text disable-output-escaping="yes">&gt; | </xsl:text>
      <xsl:value-of select="$group"/>
      <xsl:text>| </xsl:text>
      <xsl:value-of select="@mode"/>
      <xsl:text>| </xsl:text>
      <xsl:value-of select="@datatype"/>
      <xsl:text>| </xsl:text>
      <xsl:call-template name="getRequired"/>
      <xsl:text>| </xsl:text>
      <xsl:call-template name="getDescription"/>
      <xsl:text>&#10;</xsl:text>
    </xsl:variable>
    <xsl:variable name="closeElement">
      <xsl:text disable-output-escaping="yes">| &lt;/</xsl:text>
      <xsl:value-of select="@name"/>
      <xsl:text disable-output-escaping="yes">&gt; | | | | | &#10;</xsl:text>
    </xsl:variable>
    <xsl:variable name="prefix">
      <xsl:choose>
        <xsl:when test="($myMaxDepth - $depth) &gt; 0">
          <xsl:value-of select='substring("| | | | | | | | | | ", 1, $depth * 2)'/>
          <xsl:value-of select="$myMaxDepth + 1 - $depth"/>
          <xsl:text>+</xsl:text>
        </xsl:when>
        <xsl:otherwise/>
      </xsl:choose>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="$depth = 0">
        <xsl:value-of select="$category_level"/>
        <xsl:text>=== </xsl:text>
        <xsl:value-of select="@name"/>
        <xsl:text>&#10;&#10;</xsl:text>
        <xsl:value-of select="./module_description"/>
        <xsl:text>&#10;&#10;</xsl:text>
        <xsl:text>[caption="",width="100%",cols="</xsl:text>
        <xsl:value-of select='substring("1,1,1,1,1,1,1,1,1,1,", 1, $myMaxDepth * 2)'/>
        <xsl:text>12,^5,^5s,^5,^5,25",frame="topbot",options="header",grid="rows"]&#10;</xsl:text>
        <xsl:text>|===========&#10;</xsl:text>
        <xsl:value-of select="$prefix"/>
        <xsl:text>| Element | Group | Mode | Type | Required | Description&#10;</xsl:text>
      </xsl:when>
      <xsl:otherwise/>
    </xsl:choose>
    <xsl:value-of select="$prefix"/>
    <xsl:value-of select="$currentElement"/>
    <xsl:apply-templates select="./*">
      <xsl:with-param name="depth" select="$depth + 1"/>
      <xsl:with-param name="maxDepth" select="$myMaxDepth"/>
      <xsl:with-param name="group" select="$group"/>
    </xsl:apply-templates>
    <xsl:choose>
      <xsl:when test="descendant::element[1]">
        <xsl:value-of select="$prefix"/>
        <xsl:value-of select="$closeElement"/>
      </xsl:when>
      <xsl:otherwise/>
    </xsl:choose>
    <xsl:choose>
      <xsl:when test="$depth = 0">
        <xsl:text>|===========&#10;&#10;</xsl:text>
      </xsl:when>
      <xsl:otherwise/>
    </xsl:choose>
  </xsl:template>
  <xsl:template match="attribute">
    <xsl:param name="depth"/>
    <xsl:param name="maxDepth"/>
    <xsl:param name="group"/>
    <xsl:variable name="currentElement">
      <xsl:text disable-output-escaping="yes">| </xsl:text>
      <xsl:value-of select="@name"/>
      <xsl:text disable-output-escaping="yes">= | | </xsl:text>
      <xsl:value-of select="@mode"/>
      <xsl:text>| </xsl:text>
      <xsl:value-of select="@datatype"/>
      <xsl:text>| </xsl:text>
      <xsl:call-template name="getRequired"/>
      <xsl:text>| </xsl:text>
      <xsl:call-template name="getDescription"/>
      <xsl:text>&#10;</xsl:text>
    </xsl:variable>
    <xsl:variable name="prefix">
      <xsl:value-of select='substring("| | | | | | | | | | ", 1, $depth * 2)'/>
      <xsl:choose>
        <xsl:when test="($maxDepth - $depth) &gt; 0">
          <xsl:value-of select="$maxDepth + 1 - $depth"/>
          <xsl:text>+</xsl:text>
        </xsl:when>
        <xsl:otherwise/>
      </xsl:choose>
    </xsl:variable>
    <xsl:value-of select="$prefix"/>
    <xsl:value-of select="$currentElement"/>
    <xsl:apply-templates select="./*">
      <xsl:with-param name="depth" select="$depth + 1"/>
      <xsl:with-param name="maxDepth" select="$maxDepth"/>
      <xsl:with-param name="group" select="$group"/>
    </xsl:apply-templates>
  </xsl:template>
  <xsl:template name="getDescription">
    <xsl:variable name="desc">
      <xsl:value-of select="normalize-space(./description)"/>
    </xsl:variable>
    <xsl:value-of select="./description"/>
    <xsl:choose>
      <xsl:when test="string-length($desc) &gt; 0 and not(substring($desc, string-length($desc), 1) = '.')">
        <xsl:text>.</xsl:text>
      </xsl:when>
      <xsl:otherwise/>
    </xsl:choose>
    <xsl:choose>
      <xsl:when test="./value">
        <xsl:text> Possible values: </xsl:text>
      </xsl:when>
      <xsl:otherwise/>
    </xsl:choose>
    <xsl:for-each select="./value">
      <xsl:value-of select="@name"/>
      <xsl:text> (</xsl:text>
      <xsl:value-of select="."/>
      <xsl:text>)</xsl:text>
      <xsl:if test="position() != last()">
        <xsl:text>, </xsl:text>
      </xsl:if>
    </xsl:for-each>
    <xsl:choose>
      <xsl:when test="./value">
        <xsl:text>.</xsl:text>
      </xsl:when>
      <xsl:otherwise/>
    </xsl:choose>
    <xsl:choose>
      <xsl:when test="./default">
        <xsl:text> Default value: </xsl:text>
        <xsl:value-of select="./default"/>
        <xsl:text>.</xsl:text>
      </xsl:when>
      <xsl:otherwise/>
    </xsl:choose>
  </xsl:template>
  <xsl:template name="getRequired">
    <xsl:choose>
      <xsl:when test='@use = "required"'>
        <xsl:text>Yes</xsl:text>
      </xsl:when>
      <xsl:when test='@use = "optional"'>
        <xsl:text>No</xsl:text>
      </xsl:when>
      <xsl:when test="@min &gt; 0">
        <xsl:text>Yes</xsl:text>
      </xsl:when>
      <xsl:when test="@min = 0">
        <xsl:text>No</xsl:text>
      </xsl:when>
      <xsl:when test='name(.) = "element"'>
        <xsl:text>Yes</xsl:text>
      </xsl:when>
      <xsl:otherwise>
        <xsl:text>No</xsl:text>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template match="group">
    <xsl:param name="depth"/>
    <xsl:param name="maxDepth"/>
    <xsl:param name="group"/>
    <xsl:apply-templates select="./*">
      <xsl:with-param name="depth" select="$depth"/>
      <xsl:with-param name="maxDepth" select="$maxDepth"/>
      <xsl:with-param name="group" select="$group"/>
    </xsl:apply-templates>
  </xsl:template>
  <xsl:template match="choice">
    <xsl:param name="depth"/>
    <xsl:param name="maxDepth"/>
    <xsl:param name="group"/>
    <xsl:variable name="choiceId">
      <xsl:value-of select="@id"/>
    </xsl:variable>
    <xsl:for-each select="./*">
      <xsl:variable name="groupChar">
        <xsl:number value="position()" format="A"/>
      </xsl:variable>
      <xsl:apply-templates select=".">
        <xsl:with-param name="depth" select="$depth"/>
        <xsl:with-param name="maxDepth" select="$maxDepth"/>
        <xsl:with-param name="group" select="concat($choiceId, $groupChar)"/>
      </xsl:apply-templates>
    </xsl:for-each>
  </xsl:template>
  <xsl:template match="module_depth">
    <xsl:param name="depth"/>
    <xsl:param name="maxDepth"/>
    <xsl:param name="group"/>
  </xsl:template>
  <xsl:template match="module_description">
    <xsl:param name="depth"/>
    <xsl:param name="maxDepth"/>
    <xsl:param name="group"/>
  </xsl:template>
  <xsl:template match="module_desc">
    <xsl:param name="depth"/>
    <xsl:param name="maxDepth"/>
    <xsl:param name="group"/>
  </xsl:template>
  <xsl:template match="description">
    <xsl:param name="depth"/>
    <xsl:param name="maxDepth"/>
    <xsl:param name="group"/>
  </xsl:template>
  <xsl:template match="default">
    <xsl:param name="depth"/>
    <xsl:param name="maxDepth"/>
    <xsl:param name="group"/>
  </xsl:template>
  <xsl:template match="value">
    <xsl:param name="depth"/>
    <xsl:param name="maxDepth"/>
    <xsl:param name="group"/>
  </xsl:template>
  <xsl:template match="desc">
    <xsl:param name="depth"/>
    <xsl:param name="maxDepth"/>
    <xsl:param name="group"/>
  </xsl:template>
</xsl:stylesheet>
