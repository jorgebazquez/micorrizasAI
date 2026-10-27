@echo off
chcp 65001 > nul
title Compilar Todo C# - MicorrizasSoft AI
color 0e

echo ======================================================================
echo           COMPILADOR INTEGRAL C# - MICORRIZASSOFT AI
echo ======================================================================
echo.

set CSC=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe
if not exist "%CSC%" set CSC=C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe

if not exist "%CSC%" (
    echo [ERROR] No se encontró el compilador de C# (csc.exe) en .NET Framework.
    pause
    exit /b 1
)

echo [1/2] Compilando MicorrizasSoft.exe (Aplicación Principal con Icono y Seguridad)...
"%CSC%" /nologo /target:winexe /optimize /win32icon:app_icon.ico /r:System.Windows.Forms.dll /r:System.Drawing.dll /r:System.Xml.dll /r:System.Web.Extensions.dll /out:MicorrizasSoft.exe MicorrizasSoft.cs

if %ERRORLEVEL% NEQ 0 (
    echo [!] Error al compilar MicorrizasSoft.exe. Abortando.
    pause
    exit /b 1
)

echo [2/2] Compilando Instalador_MicorrizasSoft_Setup.exe (Instalador Setup Autónomo)...
"%CSC%" /nologo /target:winexe /optimize /win32icon:app_icon.ico /resource:MicorrizasSoft.exe /resource:config_seguridad.json /resource:bd_microscopia.json /resource:modelo_metadata.json /resource:guia_cultivo.xml /resource:app_icon.ico /resource:app_icon.png /r:System.Windows.Forms.dll /r:System.Drawing.dll /out:Instalador_MicorrizasSoft_Setup.exe Instalador_Setup.cs

if %ERRORLEVEL% NEQ 0 (
    echo [!] Error al compilar Instalador_MicorrizasSoft_Setup.exe.
    pause
    exit /b 1
)

echo.
echo ======================================================================
echo           ¡COMPILACIÓN INTEGRAL FINALIZADA CON ÉXITO!
echo           ✔ MicorrizasSoft.exe (1366x766 + Barra Lateral Expansible)
echo           ✔ Instalador_MicorrizasSoft_Setup.exe (Autónomo + Icono PNG)
echo ======================================================================
pause
