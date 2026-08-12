@echo off

:loop
if "%~1"=="" goto end

echo ˆ—’†: %~nx1

if not exist "%~dp1end" mkdir "%~dp1end"

rembg i "%~1" "%~dp1end\%~n1_nobg.png"

shift
goto loop

:end
echo.
echo ===== Š®—¹ =====
pause