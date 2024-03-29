; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define App_name "SimpleText"
#define App_version "1.1.1"
#define App_author "Ganesh H"
#define App_homepage "https://app.gn3.sh/SimpleText/"

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{561009F6-BA44-490C-B99B-9078E69BABB3}
AppName={#App_name}
AppVersion={#App_version}
AppVerName=SimpleText
VersionInfoVersion={#App_version}
AppPublisher={#App_author}
AppCopyright=(C) {#App_author}
AppPublisherURL={#App_homepage}
AppSupportURL={#App_homepage}
AppUpdatesURL={#App_homepage}
DefaultDirName={autopf}\SimpleText
DisableProgramGroupPage=yes
LicenseFile=..\LICENSE.MD
; Remove the following line to run in administrative install mode (install for all users.)
PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=commandline
OutputBaseFilename=SimpleText_Setup
OutputDir=bin\Release
SetupIconFile=Resources\SimpleText.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern
WizardSmallImageFile=..\assets\SimpleText_Logo_Square.bmp
ChangesAssociations=yes
UninstallDisplayIcon={app}\SimpleText.exe

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
; Executable File
Source: "bin\Release\SimpleText.exe"; DestDir: "{app}"; Flags: ignoreversion
; License File
Source: "..\LICENSE.MD"; DestDir: "{app}"; DestName: "License.txt"; Flags: ignoreversion


[Icons]
Name: "{autoprograms}\SimpleText"; Filename: "{app}\SimpleText.exe"
Name: "{autodesktop}\SimpleText"; Filename: "{app}\SimpleText.exe"; Tasks: desktopicon

[Registry]
; Add app name and company to Open With
Root: HKA; Subkey: "Software\Classes\Local Settings\Software\Microsoft\Windows\Shell\MuiCache"; ValueType: string; ValueName: "{app}\SimpleText.exe.FriendlyAppName"; ValueData: "SimpleText"; Flags: uninsdeletekey
Root: HKA; Subkey: "Software\Classes\Local Settings\Software\Microsoft\Windows\Shell\MuiCache"; ValueType: string; ValueName: "{app}\SimpleText.exe.ApplicationCompany"; ValueData: "Ganesh H"; Flags: uninsdeletekey
; Set command to handle Open With action
Root: HKA; Subkey: "Software\Classes\Applications\SimpleText.exe\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\SimpleText.exe"" ""%1"""; Flags: uninsdeletekey
; Register supported file types
Root: HKA; Subkey: "Software\Classes\Applications\SimpleText.exe\SupportedTypes"; ValueType: string; ValueName: ".txt"; ValueData: ""; Flags: uninsdeletekey
Root: HKA; Subkey: "Software\Classes\Applications\SimpleText.exe\SupportedTypes"; ValueType: string; ValueName: ".md"; ValueData: ""; Flags: uninsdeletekey
Root: HKA; Subkey: "Software\Classes\Applications\SimpleText.exe\SupportedTypes"; ValueType: string; ValueName: ".ini"; ValueData: ""; Flags: uninsdeletekey
Root: HKA; Subkey: "Software\Classes\Applications\SimpleText.exe\SupportedTypes"; ValueType: string; ValueName: ".xml"; ValueData: ""; Flags: uninsdeletekey
Root: HKA; Subkey: "Software\Classes\Applications\SimpleText.exe\SupportedTypes"; ValueType: string; ValueName: ".bat"; ValueData: ""; Flags: uninsdeletekey

[Run]
Filename: "{app}\SimpleText.exe"; Description: "{cm:LaunchProgram,SimpleText}"; Flags: nowait postinstall skipifsilent

[UninstallDelete]
Type: filesandordirs; Name:  "{userappdata}\SimpleText"

