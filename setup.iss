[Setup]
AppId={{7032E130-8F44-49F5-92C0-3443CE581AB5}}
AppName=Цитатник 1.0 BETA
AppVerName=Цитатник 1.0 BETA
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
LicenseFile=dep\license.txt

[Languages]
Name: "russian"; MessagesFile: "compiler:Languages\Russian.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "dep\license.txt";                  DestDir: "{app}"; Flags: ignoreversion
Source: "dep\readme.rtf";                  DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\QuotePad.exe";         DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\db.mdb";               DestDir: "{app}"; Flags: onlyifdoesntexist uninsneveruninstall  
Source: "bin\Release\ItWorksTeam.NET.dll";    DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\ItWorksTeam.UI.dll";    DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\ItWorksTeam.Utils.dll"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\Цитатник";         Filename: "{app}\QuotePad.exe"; WorkingDir: "{app}"
Name: "{commondesktop}\Цитатник"; Filename: "{app}\QuotePad.exe"; WorkingDir: "{app}"; Comment: "Запустить Цитатник"; Tasks: desktopicon

[Registry]
; Save path to database (for QuotePad Gadget)
Root: HKLM; SubKey: "Software\ItWorksTeam\Quotepad"; ValueType: string; ValueName: "Database"; ValueData: "{app}\db.mdb"; Flags: uninsdeletekeyifempty
; Set default password
Root: HKLM; SubKey: "Software\ItWorksTeam\Quotepad"; ValueType: string; ValueName: "Password"; ValueData: "sIHb6F4ew//D1OfQInQAzQ=="; Flags: createvalueifdoesntexist uninsdeletekeyifempty
; Set default TraceEnabled
Root: HKLM; SubKey: "Software\ItWorksTeam\Quotepad"; ValueType: string; ValueName: "TraceEnabled"; ValueData: "false"; Flags: createvalueifdoesntexist

[Run]
; Ask to run QuotePad
Filename: "{app}\QuotePad.exe"; WorkingDir: "{app}"; Description: "{cm:LaunchProgram,Цитатник}"; Flags: nowait postinstall skipifsilent