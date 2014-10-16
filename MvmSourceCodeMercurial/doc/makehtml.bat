REM This script generates HTML output for input asciidoc file(s)

xcopy /I /Y C:\docbuild\asciidoc-8.6.7\images\icons images\icons\

for %%f in (%1*.txt) do python C:\docbuild\asciidoc-8.6.7/asciidoc.py -a data-uri --backend xhtml11 --doctype article --out-file "%%~nf.xhtml" "%%f"

