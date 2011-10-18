[Setup]
AppId={{7032E130-8F44-49F5-92C0-3443CE581AB5}}
AppName=Цитатник 0.1 DEV
AppVerName=Цитатник 0.1 DEV
AppVersion=0.1
AppPublisher=IT WORKS Team
DefaultDirName={pf}\QuotePad
DefaultGroupName=Цитатник
AllowNoIcons=yes
SetupIconFile=dep\1297098121_notebook_boy.ico
Compression=lzma
SolidCompression=yes
UninstallDisplayIcon={app}\quotepad.exe
OutputDir=.\bin\Release
OutputBaseFilename=setup

[Languages]
Name: "russian"; MessagesFile: "compiler:Languages\Russian.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: ".\bin\Release\QuotePad.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\bin\Release\db.mdb"; DestDir: "{app}"; Flags: confirmoverwrite
Source: ".\bin\Release\ItWorks.OleDb.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\bin\Release\ItWorks.RTFed.dll"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\Цитатник"; Filename: "{app}\QuotePad.exe"; WorkingDir: "{app}"
Name: "{commondesktop}\Цитатник"; Filename: "{app}\QuotePad.exe"; WorkingDir: "{app}";Tasks: desktopicon

[Run]
Filename: "{app}\QuotePad.exe"; WorkingDir: "{app}"; Description: "{cm:LaunchProgram,Цитатник}"; Flags: nowait postinstall skipifsilent