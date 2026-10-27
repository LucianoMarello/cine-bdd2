@echo off
rem ======================================================================
rem  regenerar_base.bat
rem  Borra CineDB y la vuelve a generar completa, ejecutando en orden los
rem  scripts 01 a 05. Los datos de prueba quedan hasta el dia de hoy.
rem
rem  Uso: doble clic, o desde una consola:  regenerar_base.bat
rem       Con /s no pide confirmacion ni espera al final.
rem
rem  Si SQL Server tiene otro nombre de instancia, cambiar SERVIDOR.
rem ======================================================================
setlocal
set SERVIDOR=.\SQLEXPRESS
cd /d "%~dp0"

if /i not "%~1"=="/s" (
    echo ATENCION: se va a BORRAR la base CineDB en %SERVIDOR% y generar de nuevo.
    echo Se pierden todos los datos, incluidos los usuarios y sus claves.
    choice /M "Continuar"
    if errorlevel 2 exit /b 1
)

for %%f in (01_crear_bd.sql 02_datos_maestros.sql 03_datos_prueba_ventas.sql 04_vistas_analisis.sql 05_kpis.sql) do (
    echo.
    echo === Ejecutando %%f
    sqlcmd -S %SERVIDOR% -E -C -b -i %%f
    if errorlevel 1 (
        echo.
        echo ERROR al ejecutar %%f. No se continua.
        if /i not "%~1"=="/s" pause
        exit /b 1
    )
)

echo.
echo Listo. Abri la aplicacion: va a pedir crear el administrador.
if /i not "%~1"=="/s" pause
exit /b 0
