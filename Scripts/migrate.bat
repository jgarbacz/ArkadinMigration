echo off
rem setlocal MTRMPBIN=D:\MetraTech\RMP\Bin
echo Running migrate proc 
rem mvm -proc=migrate -charset=UTF8 -trim_input_fields=1 -nlog_config=%MTRMPBIN%\mc\nlog.config -override_extensions_dir=%MTRMP%\MetraConvert\MetraConvertExtensions -data_directory=%MTRMP%\MetraConvert\MetraConvertData -run_mode=unicode
rem mvm -proc=migrate -charset=UTF8 -trim_input_fields=1 -nlog_config=%MTRMPBIN%\mc\nlog.config -override_extensions_dir=%MTRMP%\MetraConvert\MetraConvertExtensions -data_directory=%MTRMP%\MetraConvert\MetraConvertData -run_mode=Unicode -benchmark=y
rem  -qckhier=y

mvm -proc=migrate -charset=UTF8 -trim_input_fields=1 -nlog_config=%MTRMPBIN%\mc\nlog.config -override_extensions_dir=%MTRMP%\MetraConvert\MetraConvertExtensions -data_directory=%MTRMP%\MetraConvert\MetraConvertData -run_mode=Unicode -benchmark=y -qckhier=y

pause
