@echo off
chcp 65001 > nul
title Compilar Instalador C#
color 0a

echo ======================================================================
echo           COMPILANDO INSTALADOR SETUP AUTÓNOMO (C# NATIVO .NET)
echo ======================================================================
echo.

set CSC=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe
if not exist "%CSC%" set CSC=C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe

if not exist "MicorrizasSoft.exe" (
    echo [!] MicorrizasSoft.exe no existe. Compilando primero la aplicacion principal...
    "%CSC%" /nologo /target:winexe /optimize /win32icon:app_icon.ico /r:System.Windows.Forms.dll /r:System.Drawing.dll /r:System.Xml.dll /r:System.Web.Extensions.dll /out:MicorrizasSoft.exe MicorrizasSoft.cs
)

echo [*] Compilando Instalador_Setup.cs con recursos embebidos e icono institucional...

"%CSC%" /nologo /target:winexe /optimize /win32icon:app_icon.ico /resource:MicorrizasSoft.exe /resource:config_seguridad.json /resource:bd_microscopia.json /resource:modelo_metadata.json /resource:guia_cultivo.xml /resource:app_icon.ico /resource:app_icon.png /r:System.Windows.Forms.dll /r:System.Drawing.dll /out:Instalador_MicorrizasSoft_Setup.exe Instalador_Setup.cs

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ======================================================================
    echo   [OK] ¡Instalador_MicorrizasSoft_Setup.exe compilado con éxito!
    echo   - Paquete 100% autónomo (no requiere archivos externos para instalar)
    echo   - Gestión inteligente de permisos UAC (Program Files o C:\MicorrizasSoft)
    echo   - Generación de accesos directos con icono personalizado
    echo ======================================================================
) else (
    echo.
    echo [!] Hubo errores durante la compilación del instalador.
)
pause
