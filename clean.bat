@echo off
set /a count = 0

for /f "delims=" %%a in ('wmic OS Get localdatetime  ^| find "."') do set dt=%%a
set dt=%dt:~6,2%/%dt:~4,2%/%dt:~0,4% %dt:~8,2%h%dt:~10,2%m%dt:~12,2%s

echo Recursao Iniciada em %dt%
time /t
echo. ----------------------
echo.
for /D  %%i in (".\*") do (	
	echo %%i
	call:recursao "%%i"	
	set /a count= count + 1
)
echo.
echo. ----------||------------
echo Arquivos/Diretorios Verificados: %count%
for /f "delims=" %%a in ('wmic OS Get localdatetime  ^| find "."') do set dtfin=%%a
set dtfin=%dtfin:~6,2%/%dtfin:~4,2%/%dtfin:~0,4% %dtfin:~8,2%h%dtfin:~10,2%m%dtfin:~12,2%s
echo Ini em: %dt% 
echo Fim em:%dtfin%
echo.-----------||-----------
echo.
pause
goto EOF

:recursao
call:deletar "%~1"
for /D  %%j in ("%~1\*") do (	
	set /a count= count + 1
	call:recursao "%%j"
)

:deletar
if EXIST "%~1\bin" rd "%~1\bin" /s /q
IF EXIST "%~1\obj" rd "%~1\obj" /s /q
IF EXIST "%~1\*.exe"	del "%~1\*.exe" /s
::IF EXIST "%~1\*.~*"	del "%~1\*.~*" /s
IF EXIST "%~1\*.dsk"	del "%~1\*.dsk" /s
IF EXIST "%~1\*.identcache"	del "%~1\*.identcache" /s
IF EXIST "%~1\*.local"	del "%~1\*.local" /s
IF EXIST "%~1\*.drc"	del "%~1\*.drc" /s
IF EXIST "%~1\*.map"	del "%~1\*.map" /s
IF EXIST "%~1\*.dof"	del "%~1\*.dof" /s
IF EXIST "%~1\*.dsk"	del "%~1\*.dsk" /s
IF EXIST "%~1\*.dc?il"	del "%~1\*.dc?il" /s
IF EXIST "%~1\*.dcu"	del "%~1\*.dcu" /s
IF EXIST "%~1\*.cfg"	del  "%~1\*.cfg" /s
IF EXIST "%~1\*.pdb"	del "%~1\*.pdb" /s 
IF EXIST "%~1\*.cfb"	del "%~1\*.cfb" /s
IF EXIST "%~1\*.rsp"	del "%~1\*.rsp" /s
IF EXIST "%~1\*.tgs"	del "%~1\*.tgs" /s
IF EXIST "%~1\*.tgw"	del "%~1\*.tgw" /s
IF EXIST "%~1\*.ddp"	del "%~1\*.ddp" /s
IF EXIST "%~1\*.bak"	del "%~1\*.bak" /s
IF EXIST "%~1\*.old"	del "%~1\*.old" /s
IF EXIST "%~1\*.tvsconfig"	del "%~1\*.tvsconfig" /s
IF EXIST "%~1\*.od*"	del "%~1\*.od*" /s
IF EXIST "%~1\*.Bin"	del "%~1\*.Bin" /s
IF EXIST "%~1\*.lck"	del "%~1\*.lck" /s
IF EXIST "%~1\*.cbk"	del "%~1\*.cbk" /s
IF EXIST "%~1\*_history"	rd "%~1\*_history" /s /q
	


