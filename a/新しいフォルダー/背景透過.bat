@echo off
setlocal

set "INPUT=%~dp0"
set "OUTPUT=%~dp0end"

if not exist "%OUTPUT%" mkdir "%OUTPUT%"

for %%F in ("%INPUT%*.png" "%INPUT%*.jpg" "%INPUT%*.jpeg" "%INPUT%*.webp") do (
    echo èàóùíÜ: "%%~nxF"
    rembg i "%%~F" "%OUTPUT%\%%~nF_nobg.png"
)

echo.
echo ===== äÆóπ =====
pause