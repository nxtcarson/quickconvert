Write-Host "QuickConvert GitHub Publish Helper" -ForegroundColor Cyan
Write-Host "================================" -ForegroundColor Cyan
Write-Host ""

# Initialize Git repository if not already done
if (-not (Test-Path .git)) {
    Write-Host "Initializing Git repository..." -ForegroundColor Yellow
    git init
    Write-Host ""
}

# Add all files to staging
Write-Host "Adding files to Git..." -ForegroundColor Yellow
git add .
Write-Host ""

# Commit changes
Write-Host "Creating commit..." -ForegroundColor Yellow
git commit -m "Initial commit: QuickConvert with dark theme"
Write-Host ""

# Connect to GitHub repository
Write-Host "Connecting to GitHub repository..." -ForegroundColor Yellow
git remote add origin https://github.com/nxtcarson/quickconvert.git
Write-Host ""

# Push to GitHub
Write-Host "Pushing to GitHub..." -ForegroundColor Yellow
Write-Host "You will need to authenticate with your GitHub credentials." -ForegroundColor Yellow
git push -u origin main
Write-Host ""

# Create a tag for release
Write-Host "Creating release tag..." -ForegroundColor Yellow
git tag -a v1.0.0 -m "QuickConvert 1.0.0"
git push origin v1.0.0
Write-Host ""

Write-Host "Done! GitHub Actions will now build and publish your release." -ForegroundColor Green
Write-Host "Visit https://github.com/nxtcarson/quickconvert/actions to monitor the build." -ForegroundColor Green
Write-Host ""
Write-Host "After the build completes, your release will be available at:" -ForegroundColor Green
Write-Host "https://github.com/nxtcarson/quickconvert/releases" -ForegroundColor Green
Write-Host ""
Write-Host "Press any key to continue..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown") 