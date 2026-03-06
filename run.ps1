#!/usr/bin/env pwsh

Write-Host "Select the project Environment (default: 1 - Development):"
Write-Host "1 - Development"
Write-Host "2 - Staging"
Write-Host "3 - Production"
$option = Read-Host

if ($option -eq "" -or $option -eq "1") {
    $env:ASPNETCORE_ENVIRONMENT = "Development"
}
elseif ($option -eq "2") {
    $env:ASPNETCORE_ENVIRONMENT = "Staging"
}
elseif ($option -eq "3") {
    $env:ASPNETCORE_ENVIRONMENT = "Production"
}
else {
    Write-Host "Invalid option. Use 1, 2 or 3 (or press Enter for Development)."
    exit 1
}

$buildConfig = if ($env:ASPNETCORE_ENVIRONMENT -eq "Development") { "Debug" } else { "Release" }

# Ensure HTTPS dev certificate is trusted (no-op if already trusted)
dotnet dev-certs https --trust | Out-Null

Write-Host "Starting API in $($env:ASPNETCORE_ENVIRONMENT) mode with HTTPS..."
dotnet run --project src/Domus.API --launch-profile https --no-self-contained -c $buildConfig
