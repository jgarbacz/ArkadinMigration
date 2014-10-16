<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:fo="http://www.w3.org/1999/XSL/Format"
				xmlns:xslthl="http://xslthl.sf.net"
                exclude-result-prefixes="xslthl">
	<!-- Include basic AsciiDoc FOP formatting -->
	<xsl:import href="file:///c:/docbuild/asciidoc-8.6.7/docbook-xsl/fo.xsl"/>
	<!-- Include source syntax highlighting -->
	<xsl:import href="http://docbook.sourceforge.net/release/xsl/current/highlighting/common.xsl"/>
	<!-- This contains the default source highlight styling rules -->
	<xsl:import href="http://docbook.sourceforge.net/release/xsl/current/fo/highlight.xsl"/>
	<!-- Without the below, line numbering doesn't work -->
	<xsl:param name="use.extensions" select="'1'"/>
	<xsl:param name="linenumbering.extension" select="'1'"/>
	<xsl:param name="linenumbering.everyNth" select="'1'"/>
	<!-- My style customisations -->
	<xsl:template match='xslthl:keyword' mode="xslthl">
		<fo:inline font-weight="normal" color="#AA22FF"><xsl:apply-templates mode="xslthl"/></fo:inline>
	</xsl:template>
	<xsl:template match="xslthl:doccomment|xslthl:doctype" mode="xslthl">
		<fo:inline font-weight="normal" color="green">
			<xsl:apply-templates mode="xslthl"/>
		</fo:inline>
	</xsl:template>
	<xsl:template match="xslthl:annotation" mode="xslthl">
		<fo:inline font-weight="normal" color="teal">
			<xsl:apply-templates mode="xslthl"/>
		</fo:inline>
	</xsl:template>
	<xsl:template match="xslthl:string" mode="xslthl">
		<fo:inline font-weight="normal" font-style="italic" color="brown">
			<xsl:apply-templates mode="xslthl"/>
		</fo:inline>
	</xsl:template>
	<xsl:template match="xslthl:directive" mode="xslthl">
		<fo:inline font-weight="normal" font-style="italic" color="blue">
			<xsl:apply-templates mode="xslthl"/>
		</fo:inline>
	</xsl:template>
</xsl:stylesheet>
