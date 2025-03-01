[Setup]
AppName=QuickConvert
AppVersion=1.0
WizardStyle=modern
DefaultDirName={autopf}\QuickConvert
DefaultGroupName=QuickConvert
UninstallDisplayIcon={app}\QuickConvert.App.exe
Compression=lzma2
SolidCompression=yes
OutputDir=.\Installer
OutputBaseFilename=QuickConvert_Setup
PrivilegesRequired=admin

[Files]
; Include your application files
Source: "bin\Release\net6.0-windows\win-x64\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\QuickConvert"; Filename: "{app}\QuickConvert.App.exe"
Name: "{commondesktop}\QuickConvert"; Filename: "{app}\QuickConvert.App.exe"

[Registry]
; Add application to PATH
Root: HKLM; Subkey: "SYSTEM\CurrentControlSet\Control\Session Manager\Environment"; \
    ValueType: expandsz; ValueName: "Path"; ValueData: "{olddata};{app}"; \
    Check: NeedsAddPath(ExpandConstant('{app}'))

; Register for Windows search
Root: HKLM; Subkey: "SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\QuickConvert.App.exe"; \
    ValueType: string; ValueName: ""; ValueData: "{app}\QuickConvert.App.exe"; Flags: uninsdeletekey
Root: HKLM; Subkey: "SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\QuickConvert.App.exe"; \
    ValueType: string; ValueName: "Path"; ValueData: "{app}"; Flags: uninsdeletekey

[Code]
function NeedsAddPath(Param: string): boolean;
var
  OrigPath: string;
begin
  if not RegQueryStringValue(HKEY_LOCAL_MACHINE,
    'SYSTEM\CurrentControlSet\Control\Session Manager\Environment',
    'Path', OrigPath)
  then begin
    Result := True;
    exit;
  end;
  Result := Pos(';' + Param + ';', ';' + OrigPath + ';') = 0;
end; 