REM This script generates PDF output for an input asciidoc file

python C:\docbuild\asciidoc-8.6.7\asciidoc.py --backend docbook --doctype article --out-file "%1.xml" "%1.txt"

java -cp ".;c:/docbuild/saxon-6-5-5/saxon.jar;c:/docbuild/xslthl-2.0.2/xslthl-2.0.2.jar;c:/docbuild/docbook-xsl-1.76.1/extensions/saxon65.jar;c:/docbuild/xml-commons-resolver-1.2/resolver.jar;c:/docbuild/" -Dxslthl.config="file:///c:/docbuild/docbook-xsl-1.76.1/highlighting/xslthl-config.xml" com.icl.saxon.StyleSheet -x org.apache.xml.resolver.tools.ResolvingXMLReader -y org.apache.xml.resolver.tools.ResolvingXMLReader -r org.apache.xml.resolver.tools.CatalogResolver -o "%1.fo" "%1.xml" xsl-fo.xsl

C:\docbuild\fop-1.0\fop -fo "%1.fo" -pdf "%2.pdf"

