@echo off
setlocal
set "EXE=%~dp0..\MicroslopPeon\bin\Release\net8.0-windows\win-x64\publish\MicroslopPeon.exe"
if not exist "%EXE%" set "EXE=%~dp0..\MicroslopPeon\bin\Release\net8.0-windows\MicroslopPeon.exe"
set "TEMPJSON=%TEMP%\peon-test-input-%RANDOM%.json"
echo {^"hook_event_name^":^"input.required^",^"conversation_id^":^"bat-test^"} > "%TEMPJSON%"
type "%TEMPJSON%" | "%EXE%"
del "%TEMPJSON%" 2>nul
endlocal
