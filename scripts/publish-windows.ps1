<#
.SYNOPSIS
    Build the Windows release: self-contained app folder, then the installer (if Inno Setup is installed).

.DESCRIPTION
    Output in out\:
      out\windows\                               the app (no .NET or Windows App SDK runtime needed)
      out\Vault-<version>-win-x64-setup.exe

    Optional code signing (removes the "unknown publisher" warning): set WINDOWS_CERT_PFX (path to a .pfx)
    and WINDOWS_CERT_PASSWORD. signtool.exe from the Windows SDK must be installed.

.EXAMPLE
    .\scripts\publish-windows.ps1 -Version 4.0.0
#>
param(
    [string] $Version = "",
    [string] $Iscc = ""
)
$ErrorActionPreference = "Stop"
$root = Split-Path $PSScriptRoot -Parent
$out = Join-Path $root "out"
$app = Join-Path $out "windows"

$versionArgs = @()
if ($Version) { $versionArgs = @("-p:Version=$Version") }
else { $Version = ([xml](Get-Content (Join-Path $root "Directory.Build.props"))).Project.PropertyGroup.Version | Where-Object { $_ } | Select-Object -First 1 }

if (Test-Path $app) { Remove-Item $app -Recurse -Force }
dotnet publish (Join-Path $root "VaultGui\VaultGui.csproj") `
    -c Release -f net10.0-windows10.0.19041.0 `
    -p:WindowsPackageType=None -p:WindowsAppSDKSelfContained=true -p:SelfContained=true `
    @versionArgs -o $app
if ($LASTEXITCODE -ne 0) { throw "dotnet publish failed" }

function Sign([string] $file) {
    if (-not $env:WINDOWS_CERT_PFX) { return }
    $signtool = Get-ChildItem "${env:ProgramFiles(x86)}\Windows Kits\10\bin\*\x64\signtool.exe" -ErrorAction SilentlyContinue |
        Sort-Object FullName -Descending | Select-Object -First 1
    if (-not $signtool) { throw "signtool.exe not found (install the Windows SDK)" }
    & $signtool.FullName sign /f $env:WINDOWS_CERT_PFX /p $env:WINDOWS_CERT_PASSWORD /fd SHA256 /tr http://timestamp.digicert.com /td SHA256 $file
    if ($LASTEXITCODE -ne 0) { throw "signing $file failed" }
}

Sign (Join-Path $app "VaultGui.exe")

if (-not $Iscc) {
    # Inno Setup 7 first (64-bit edition, then 32-bit), then Inno Setup 6, then whatever is on the PATH.
    $roots = @($env:ProgramFiles, ${env:ProgramFiles(x86)}, "$env:LOCALAPPDATA\Programs")
    $Iscc = @(7, 6) | ForEach-Object { $major = $_; $roots | ForEach-Object { Join-Path $_ "Inno Setup $major\ISCC.exe" } } |
        Where-Object { Test-Path $_ } | Select-Object -First 1
}
if (-not $Iscc) {
    $onPath = Get-Command ISCC.exe -ErrorAction SilentlyContinue | Select-Object -First 1
    if ($onPath) { $Iscc = $onPath.Source }
}
if (-not $Iscc) {
    Write-Warning "Inno Setup 6 or 7 not found: installer skipped. The app is in $app"
    return
}
Write-Host "Inno Setup compiler: $Iscc"

& $Iscc "/DMyAppVersion=$Version" "/DSourceDir=$app" "/DOutputDir=$out" (Join-Path $root "installer\Vault.iss")
if ($LASTEXITCODE -ne 0) { throw "Inno Setup failed" }

$setup = Join-Path $out "Vault-$Version-win-x64-setup.exe"
Sign $setup
Write-Host "Installer: $setup"
