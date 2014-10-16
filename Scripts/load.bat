echo off
rem setlocal MTRMPBIN=D:\MetraTech\RMP\Bin
echo Running load proc 
mvm -proc=load -nlog_config=%MTRMPBIN%\mc\nlog.config -override_extensions_dir=%MTRMP%\MetraConvert\MetraConvertExtensions -data_directory=%MTRMP%\MetraConvert\MetraConvertData -run_mode=Unicode
Pause
