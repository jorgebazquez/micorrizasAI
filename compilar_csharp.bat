@echo off
chcp 65001 > nul
title Compilar MicorrizasSoft C#
color 0b

echo ======================================================================
echo           COMPILANDO MICORRIZASSOFT AI (C# NATIVO .NET)
echo ======================================================================
echo.

set CSC=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe
if not exist "%CSC%" set CSC=C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe

if not exist "%CSC%" (
    echo [ERROR] No se encontró el compilador de C# (csc.exe) en .NET Framework.
    pause
    exit /b 1
)

echo [*] Utilizando compilador: %CSC%
echo [*] Compilando MicorrizasSoft.cs con icono institucional y protecciones...

"%CSC%" /nologo /target:winexe /optimize /win32icon:app_icon.ico /r:System.Windows.Forms.dll /r:System.Drawing.dll /r:System.Xml.dll /r:System.Web.Extensions.dll /out:MicorrizasSoft.exe MicorrizasSoft.cs

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ======================================================================
    echo   [OK] ¡MicorrizasSoft.exe compilado con éxito!
    echo   - Icono institucional incrustado (app_icon.ico)
    echo   - Resolución base: 1366x766 con barra lateral expansible
    echo   - Consulta pública de base de datos protegida con contraseña maestra
    echo   - Captura nativa de cámara USB / microscopio óptico
    echo ======================================================================
) else (
    echo.
    echo [!] Hubo errores durante la compilación.
)
pause
