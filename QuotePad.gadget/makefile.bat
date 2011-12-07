@echo off
echo *************************************************************
echo QuotePad Gadget Copyright (C) 2010-2011 IT WORKS Team
echo This program comes with ABSOLUTELY NO WARRANTY;
echo This is free software, and you are welcome to redistribute it
echo under certain conditions.
echo *************************************************************
echo This script will create .gadget
echo You need to have installed 7-Zip on this computer

:begin
echo Packing QuotePad Gadget from sources...
set InternalName=QuotePad
set SourceFiles=images js gadget.xml QuotePad.html QuotePad.css settings.html
echo Previously created gadget will be removed if necessary...
if exist %InternalName%.gadget del %InternalName%.gadget
echo Packing gadget...
if exist C:\PROGRA~1\7-Zip\7z.exe goto packing1
if exist C:\PROGRA~2\7-Zip\7z.exe goto packing2
goto failed

:packing1
C:\PROGRA~1\7-Zip\7z a -r -tzip %InternalName%.gadget %SourceFiles%
if exist %InternalName%.gadget goto success
goto failed

:packing2
C:\PROGRA~2\7-Zip\7z a -r -tzip %InternalName%.gadget %SourceFiles%
if exist %InternalName%.gadget goto success
goto failed

:failed
echo Packing FAILED!
goto end

:success
echo All jobs DONE!
goto end

:end
pause

rem You can test working copy in this folder
rem %USERPROFILE%\AppData\Local\Microsoft\Windows Sidebar\Gadgets