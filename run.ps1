# Quick run script for Windows
Write-Host "🚀 Starting Popsicle Factory API..." -ForegroundColor Green
if ($args[0] -eq "dev") {
    Write-Host "🔧 Running in development mode with hot reload..." -ForegroundColor Yellow
    dotnet watch run --project API
} else {
    Write-Host "▶️ Running in normal mode..." -ForegroundColor Blue
    dotnet run --project API
}