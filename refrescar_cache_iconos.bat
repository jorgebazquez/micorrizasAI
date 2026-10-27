@echo off
chcp 65001 > nul
title Refrescar Caché de Iconos de Windows
color 0a

echo ======================================================================
echo           REINICIANDO CACHÉ DE ICONOS DE WINDOWS EXPLORER
echo ======================================================================
echo.
echo [*] Cerrando Windows Explorer temporalmente...
taskkill /f /im explorer.exe >nul 2>&1
timeout /t 2 /nobreak >nul

echo [*] Eliminando bases de datos de iconos en caché...
del /f /q "%localappdata%\IconCache.db" >nul 2>&1
del /f /q "%localappdata%\Microsoft\Windows\Explorer\iconcache_*.db" >nul 2>&1
del /f /q "%localappdata%\Microsoft\Windows\Explorer\thumbcache_*.db" >nul 2>&1

echo [*] Reiniciando Windows Explorer con iconos limpios...
start explorer.exe
timeout /t 2 /nobreak >nul

echo.
echo ======================================================================
echo   [OK] ¡Caché de iconos limpiada y Windows Explorer reiniciado!
echo   Todos los ejecutables e instalador mostrarán el icono verde institucional.
echo ======================================================================
echo.
timeout /t 3
