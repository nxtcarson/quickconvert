@echo off
echo QuickConvert GitHub Publish Helper
echo ================================
echo.

REM Initialize Git repository if not already done
if not exist .git (
    echo Initializing Git repository...
    git init
    echo.
)

REM Add all files to staging
echo Adding files to Git...
git add .
echo.

REM Commit changes
echo Creating commit...
git commit -m "Initial commit: QuickConvert with dark theme"
echo.

REM Connect to GitHub repository
echo Connecting to GitHub repository...
git remote add origin https://github.com/nxtcarson/quickconvert.git
echo.

REM Push to GitHub
echo Pushing to GitHub...
echo You will need to authenticate with your GitHub credentials.
git push -u origin main
echo.

REM Create a tag for release
echo Creating release tag...
git tag -a v1.0.0 -m "QuickConvert 1.0.0"
git push origin v1.0.0
echo.

echo Done! GitHub Actions will now build and publish your release.
echo Visit https://github.com/nxtcarson/quickconvert/actions to monitor the build.
echo.
echo After the build completes, your release will be available at:
echo https://github.com/nxtcarson/quickconvert/releases
echo.
pause 