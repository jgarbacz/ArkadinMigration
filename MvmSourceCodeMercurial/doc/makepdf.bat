REM This script generates PDF output for input asciidoc file(s)

call makehtml MvmUserGuide_split

call makepdf_portrait MvmUserGuide_split_1 MvmUserGuide

call makepdf_landscape MvmUserGuide_split_2 MvmReference

