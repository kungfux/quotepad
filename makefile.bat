@echo off

:: This file is a part of QuotePad project
:: Copyright IT WORKS Team, 2010-2011
:: Created by: Fuks Alexander

set param=%1
if param==Build goto Build
if param==Release goto Release

:Release
echo Building Release...
%WINDIR%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe QuotePad-Release.csproj
goto end

:Build
:: Here is we should make build of project
goto end
	
:end