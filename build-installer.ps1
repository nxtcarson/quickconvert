Write-Host "QuickConvert Installer Builder" -ForegroundColor Cyan
Write-Host "============================" -ForegroundColor Cyan
Write-Host ""

# Check if Inno Setup is installed
$innoSetupPath = "C:\Program Files (x86)\Inno Setup 6\ISCC.exe"
if (-not (Test-Path $innoSetupPath)) {
    Write-Host "Inno Setup not found at $innoSetupPath" -ForegroundColor Red
    Write-Host "Please download and install Inno Setup from https://jrsoftware.org/isdl.php" -ForegroundColor Yellow
    Write-Host "Press any key to exit..." -ForegroundColor Gray
    $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
    exit
}

# Create Installer directory if it doesn't exist
if (-not (Test-Path ".\Installer")) {
    Write-Host "Creating Installer directory..." -ForegroundColor Yellow
    New-Item -ItemType Directory -Path ".\Installer" | Out-Null
}

# Build the application
Write-Host "Building QuickConvert..." -ForegroundColor Yellow
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true

# Check if build was successful
if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed with exit code $LASTEXITCODE" -ForegroundColor Red
    Write-Host "Press any key to exit..." -ForegroundColor Gray
    $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
    exit
}

# Build the installer
Write-Host "Building installer..." -ForegroundColor Yellow
& $innoSetupPath "QuickConvertSetup.iss"

# Check if installer was built successfully
if ($LASTEXITCODE -ne 0) {
    Write-Host "Installer build failed with exit code $LASTEXITCODE" -ForegroundColor Red
} else {
    $installerPath = ".\Installer\QuickConvert_Setup.exe"
    if (Test-Path $installerPath) {
        Write-Host "Installer built successfully!" -ForegroundColor Green
        Write-Host "Installer location: $((Get-Item $installerPath).FullName)" -ForegroundColor Green
    } else {
        Write-Host "Installer file not found at expected location: $installerPath" -ForegroundColor Red
    }
}

Write-Host ""
Write-Host "Press any key to exit..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown") 