<#
.SYNOPSIS
    Build the signed Android release APK (sideloading) and, with -Bundle, the AAB (Google Play).

.DESCRIPTION
    Needs ANDROID_KEYSTORE, ANDROID_KEYSTORE_PASSWORD and ANDROID_KEY_PASSWORD (see new-android-keystore.ps1).
    Output: out\android\com.github.vault-Signed.apk (+ .aab).

    -NoAot: skip ahead-of-time compilation (slower startup). Needed when the repository path contains
    non-ASCII characters (the Android AOT compiler cannot read its response file there).

.EXAMPLE
    .\scripts\publish-android.ps1 -Version 4.0.0
#>
param(
    [string] $Version = "",
    [switch] $Bundle,
    [switch] $NoAot
)
$ErrorActionPreference = "Stop"
$root = Split-Path $PSScriptRoot -Parent
$out = Join-Path $root "out\android"

if (-not $env:ANDROID_KEYSTORE -or -not (Test-Path $env:ANDROID_KEYSTORE)) {
    throw "Set ANDROID_KEYSTORE to your release keystore (create one with scripts\new-android-keystore.ps1)."
}
if (-not $env:ANDROID_KEYSTORE_PASSWORD -or -not $env:ANDROID_KEY_PASSWORD) {
    throw "Set ANDROID_KEYSTORE_PASSWORD and ANDROID_KEY_PASSWORD."
}

$extra = @()
if ($Version) { $extra += "-p:Version=$Version" }
if ($NoAot) { $extra += "-p:RunAOTCompilation=false" }
$formats = if ($Bundle) { "apk;aab" } else { "apk" }

if (Test-Path $out) { Remove-Item $out -Recurse -Force }
dotnet publish (Join-Path $root "VaultGui\VaultGui.csproj") `
    -c Release -f net10.0-android "-p:AndroidPackageFormats=$formats" @extra -o $out
if ($LASTEXITCODE -ne 0) { throw "dotnet publish failed" }

Get-ChildItem $out -Include *-Signed.apk, *-Signed.aab -Recurse | ForEach-Object { Write-Host "Package: $($_.FullName)" }
