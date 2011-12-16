@echo off
echo *************************************************************
echo QuotePad program compiler script [Version 0.1.0.2]
echo Copyright (C) 2010-2011 IT WORKS Team
echo This program comes with ABSOLUTELY NO WARRANTY;
echo This is free software, and you are welcome to redistribute it
echo under certain conditions.
echo *************************************************************
echo .
echo .
echo --------------------------- NOTE ----------------------------
echo This script will compile QuotePad program and pack setup.exe.
echo You need to have .NET Framework 4.0 installed to compile and 
echo Inno Setup 5 in order to create installation file setup.exe.
echo -------------------------------------------------------------

set param=%1
if param==Build goto Build
if param==Release goto Release

:Release
echo *** Stage 1. Building Release ***
%WINDIR%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe QuotePad-Release.csproj 
IF EXIST .\bin\Release\QuotePad.exe GOTO Setup
goto error

:Build
:: Here is we should make build of project
goto end

:Setup
echo *** Done ***
echo *** Stage 2. Building installation file ***
"C:\Program Files\Inno Setup 5\Compil32.exe" /cc setup.iss
if exist .\bin\Release\setup.exe goto end
goto error
	
:error
echo ===============
echo BUILDING FAILED
echo ===============

:end
echo *** Done ***