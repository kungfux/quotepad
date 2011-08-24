@echo off
SET CSC=c:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe
SET SOURCEFILES=

%CSC% /warnaserror+ /checked+ /unsafe- /debug+ /nologo /out:Bin\QuotePad.debug.exe /t:winexe %SOURCEFILES%