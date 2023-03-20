#define MyAppName "Password vault"
#define MyAppVersion "3.2.0.0"
#define MyAppPublisher "DevElkami"
#define MyAppURL "https://github.com/DevElkami/PasswordVault"
#define MyAppDescription "Password vault"
#define MyAppCopyright "(c) 2021-2023, DevElkami. See LICENSE.TXT for details."

[Setup]
AppId={{A89DD960-5E9E-484D-B499-AB7DBA6C372A}}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
LicenseFile=..\..\LICENSE
OutputBaseFilename=Password-Vault-{#MyAppVersion}
Compression=none
SolidCompression=yes
VersionInfoVersion={#MyAppVersion}
VersionInfoDescription={#MyAppDescription}
VersionInfoTextVersion={#MyAppVersion}
ArchitecturesInstallIn64BitMode=x64
VersionInfoCopyright={#MyAppCopyright}
DisableReadyMemo=yes
AlwaysShowComponentsList=no
DisableReadyPage=yes
CloseApplications=no
OutputDir=Output
MinVersion=6.1sp1
PrivilegesRequired=admin
PrivilegesRequiredOverridesAllowed=dialog
WizardStyle=modern

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}";

[Files]
Source: ..\bin\Release\net7.0-windows7.0\publish\WinformPasswordVault.exe; DestDir: {app}; Flags: ignoreversion overwritereadonly uninsremovereadonly;
Source: ..\bin\Release\net7.0-windows7.0\publish\*.dll; DestDir: {app}; Flags: ignoreversion overwritereadonly uninsremovereadonly;
Source: ..\..\LICENSE; DestDir: {app}; Flags: overwritereadonly uninsremovereadonly; DestName: LICENSE.TXT

[Icons]
Name: {group}\Password Vault; Filename: {app}\WinformPasswordVault.exe; Flags: excludefromshowinnewinstall
Name: "{userdesktop}\{#MyAppName}"; Filename: "{app}\WinformPasswordVault.exe"; Tasks: desktopicon