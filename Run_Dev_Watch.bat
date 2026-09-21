@echo off
chcp 65001 >nul
setlocal
cd /d "%~dp0"

REM 1. Kill existing running process
taskkill /f /im RD_AutoInputTool.exe >nul 2>nul

REM 2. Add dotnet to PATH if needed
if exist "%LOCALAPPDATA%\Microsoft\dotnet\dotnet.exe" (
    set "PATH=%LOCALAPPDATA%\Microsoft\dotnet;%PATH%"
)

echo ========================================================
echo  RD AutoInputTool - 실시간 핫 리로드 (dotnet watch) 모드
echo  코드를 수정하고 저장(Ctrl+S)하면 자동으로 반영/재실행됩니다.
echo  종료하려면 이 창에서 Ctrl+C 를 누르세요.
echo ========================================================
dotnet watch run
pause
