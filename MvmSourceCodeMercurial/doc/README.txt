This document describes how to set up an asciidoc/docbook/xsl-fo toolchain for generating HTML and PDF documentation from asciidoc-formatted input file(s).  With the exception of python and java, which go into their default installation locations, I put everything into my C:\docbuild directory.  Most packages are simple zip files that you can unpack into docbuild.  Adjust the directory names below based on this.

Install the following software:
- python (I am using version 2.7.3)
- java (1.6.0_31)
- asciidoc (8.6.7)
- saxon (6.5.5, for compliance with XSLT 1.0 which docbook uses)
- apache FOP for PDF generation (1.0)
- XSLTHL for syntax highlighting (2.0.2)
- docbook XML (5.0b9)
- docbook XSL (1.76.1)
- XML commons resolver (1.2)
- pygments python module for syntax highlighting (1.5, get the tar.gz package)
  (install this by running "python setup.py install" from the Pygments directory)

After unzipping everything, add the python and java directories to your PATH environment variable.

Edit the path to the asciidoc directory in xsl-fo.xsl.

Edit the docbook XSL and docbook XML paths in CatalogManager.Properties.

Edit the paths for asciidoc and fop in makehtml.bat and makepdf.bat.

Uncomment this line in docbuild\asciidoc-8.6.7\asciidoc.conf to use pygments:
pygment=

Here is what my C:\docbuild directory looks like (I saved all the zip files in the packages directory):
05/01/2012  10:37 AM    <DIR>          docbook-5.0b9
05/01/2012  10:37 AM    <DIR>          docbook-xsl-1.76.1
05/01/2012  10:17 AM    <DIR>          fop-1.0
05/01/2012  11:26 AM    <DIR>          packages
05/01/2012  10:36 AM    <DIR>          Pygments-1.5
05/01/2012  10:14 AM    <DIR>          saxon-6-5-5
05/01/2012  10:39 AM    <DIR>          xml-commons-resolver-1.2
05/01/2012  10:38 AM    <DIR>          xslthl-2.0.2

Other guides that you may find helpful relating to asciidoc/docbook setup:
http://francisshanahan.com/index.php/2010/setup-asciidoc-fop-pygment-on-windows/
http://www.stevestreeting.com/2010/03/07/building-a-new-technical-documentation-tool-chain/

Now you can generate HTML and PDF documentation with these commands:
mvm -doc
makehtml MvmUserGuide_complete
makepdf

The first MVM command generates a number of files:
MVM.AutoDoc.xml (function/proc/module data, used as raw input for other tools)
MVM.FunctionDocumentation.txt (asciidoc formatted function docs)
MVM.ModuleDocumentation.txt (asciidoc formatted module docs)
MVM.ProcDocumentation.txt (asciidoc formatted proc docs)

The last three files are generated from the first one using functions2doc.xsl, modules2doc.xsl, and procs2doc.xsl.
