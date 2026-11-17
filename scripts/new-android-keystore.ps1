<#
.SYNOPSIS
    Create the release keystore that signs the Android app. Run once, then keep the file safe.

.DESCRIPTION
    keytool asks for the passwords itself (nothing is written in a script or in the repository).

    IMPORTANT: back up the keystore and its passwords (for example in the vault itself, and the file
    in a second place). Android only installs an update signed with the same key: if the keystore is
    lost, the app must be uninstalled to install a new build, which deletes its local vault copy
    (the synced copy, if any, is not affected).

    Then, to build a signed release (scripts/publish-android.ps1 or the release workflow), set:
      ANDROID_KEYSTORE           path of the keystore
      ANDROID_KEYSTORE_PASSWORD  keystore password
      ANDROID_KEY_PASSWORD       key password (the same one, unless you chose another)
      ANDROID_KEY_ALIAS          optional, default "passwordvault"

.EXAMPLE
    .\scripts\new-android-keystore.ps1
#>
param(
    [string] $Path = (Join-Path $env:USERPROFILE ".android\passwordvault-release.keystore"),
    [string] $Alias = "passwordvault"
)
$ErrorActionPreference = "Stop"

if (Test-Path $Path) { throw "$Path already exists: it is your signing key, it is never overwritten." }

$keytool = (Get-Command keytool -ErrorAction SilentlyContinue).Source
if (-not $keytool) {
    $keytool = Get-ChildItem "$env:ProgramFiles\Android\openjdk\*\bin\keytool.exe", "$env:ProgramFiles\Microsoft\jdk-*\bin\keytool.exe" -ErrorAction SilentlyContinue |
        Select-Object -First 1 -ExpandProperty FullName
}
if (-not $keytool) { throw "keytool not found: install a JDK (it comes with the .NET Android workload / Visual Studio)." }

New-Item -ItemType Directory -Force (Split-Path $Path) | Out-Null
& $keytool -genkeypair -v -keystore $Path -alias $Alias -keyalg RSA -keysize 4096 -validity 36500 `
    -dname "CN=Password Vault, O=DevElkami"
if ($LASTEXITCODE -ne 0) { throw "keytool failed" }

Write-Host ""
Write-Host "Keystore created: $Path (alias '$Alias')."
Write-Host "Back it up now, with its password. For the GitHub release workflow, add these repository secrets:"
Write-Host "  ANDROID_KEYSTORE_BASE64   = [Convert]::ToBase64String([IO.File]::ReadAllBytes('$Path'))"
Write-Host "  ANDROID_KEYSTORE_PASSWORD, ANDROID_KEY_PASSWORD, ANDROID_KEY_ALIAS ($Alias)"
