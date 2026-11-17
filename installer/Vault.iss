; Windows installer for Vault (Inno Setup 6 or 7).
; Built by scripts/publish-windows.ps1 or the release workflow:
;   ISCC.exe /DMyAppVersion=4.0.0 /DSourceDir=<publish folder> /DOutputDir=<folder> installer\Vault.iss
;
; Same AppId as the 3.x installer: installing 4.x upgrades an existing 3.x installation in place.
; The vault itself is not in the install folder (Documents\my.vault) and is never touched;
; a 3.x vault is converted on the first unlock.

#ifndef MyAppVersion
  #define MyAppVersion "4.0.0"
#endif
#ifndef SourceDir
  #define SourceDir "..\out\windows"
#endif
#ifndef OutputDir
  #define OutputDir "..\out"
#endif

#define MyAppName "Vault"
#define MyAppPublisher "DevElkami"
#define MyAppURL "https://github.com/DevElkami/PasswordVault"
#define MyAppExeName "VaultGui.exe"

[Setup]
; Kept byte for byte from the 3.x script (including the doubled closing brace) so the upgrade is detected.
AppId={{A89DD960-5E9E-484D-B499-AB7DBA6C372A}}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
AppCopyright=(c) 2021-2026, DevElkami. See LICENSE.TXT for details.
VersionInfoVersion={#MyAppVersion}
VersionInfoDescription={#MyAppName} setup
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
LicenseFile=..\LICENSE.md
SetupIconFile=Vault.ico
UninstallDisplayIcon={app}\{#MyAppExeName}
OutputDir={#OutputDir}
OutputBaseFilename=Vault-{#MyAppVersion}-win-x64-setup
Compression=lzma2/max
SolidCompression=yes
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
; Windows 10 1809 or later (WinUI 3).
MinVersion=10.0.17763
; Per-machine by default; the user can choose a per-user install (no admin rights) instead.
PrivilegesRequired=admin
PrivilegesRequiredOverridesAllowed=dialog
CloseApplications=yes
WizardStyle=modern
DisableReadyPage=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "french"; MessagesFile: "compiler:Languages\French.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[InstallDelete]
; Leftovers of the 3.x WinForms app (same install folder). Program files only: no user data lives here.
Type: files; Name: "{app}\WinformPasswordVault.*"
Type: files; Name: "{app}\ReaLTaiizor.dll"
Type: files; Name: "{app}\Microsoft.Web.WebView2.*"
Type: filesandordirs; Name: "{app}\runtimes"
; Former name of the app assembly (now VaultGui).
Type: files; Name: "{app}\MauiPasswordVault.*"
; Shortcuts under the former name ("Password vault"); they are created again as "Vault".
Type: files; Name: "{group}\Password vault.lnk"
Type: files; Name: "{autodesktop}\Password vault.lnk"

[Files]
Source: "{#SourceDir}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "..\LICENSE.md"; DestDir: "{app}"; DestName: "LICENSE.TXT"; Flags: ignoreversion

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent
