@echo off
chcp 65001 >nul
setlocal
cd /d "%~dp0"

REM 1. Kill existing running process to release file locks
taskkill /f /im RD_AutoInputTool.exe >nul 2>nul

REM 2. Locate dotnet SDK
if exist "%LOCALAPPDATA%\Microsoft\dotnet\dotnet.exe" (
    set "DOTNET_EXE=%LOCALAPPDATA%\Microsoft\dotnet\dotnet.exe"
) else (
    set "DOTNET_EXE=dotnet"
)

REM 3. Fast incremental publish to Publish folder
"%DOTNET_EXE%" publish -c Release -o Publish --nologo -v q

REM 4. Launch latest built application
start "" "%~dp0Publish\RD_AutoInputTool.exe"
exit
