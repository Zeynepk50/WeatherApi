# Start Backend (Background)
Start-Process -FilePath "dotnet" -ArgumentList "run --project App.API/App.API.csproj --launch-profile http" -WorkingDirectory "$PSScriptRoot" -NoNewWindow

# Start Frontend
Write-Host "Starting Frontend..."
Set-Location "$PSScriptRoot/App.UI"
npm run dev
