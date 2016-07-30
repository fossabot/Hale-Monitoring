@echo off

set DEBUG=0
set ENVPATH=%~dp0..\.env
set /p DB=<%ENVPATH%\db.txt
set ASSEMBLY=%~dp0Hale.Migrations\bin\Debug\Hale.Migrations.dll
set MIGRATE=%~dp0..\packages\FluentMigrator.Tools.1.6.2\tools\AnyCPU\40\Migrate
set /p CONSTR=<%ENVPATH%\constr.txt

if %DEBUG%==0 goto run

:: For debugging:
echo %MIGRATE%
echo -a %ASSEMBLY%
echo -db %DB%
echo -con "%CONSTR%"
echo "%*"

goto run

:run

:: For machine.config, not working atm -NM 2016-07-30
::%MIGRATE% -a %ASSEMBLY% -db %DB% -c HaleDB --configPath %ENVPATH%\machine.config %*

%MIGRATE% -a %ASSEMBLY% -db %DB% -c "%CONSTR%" -v %*
