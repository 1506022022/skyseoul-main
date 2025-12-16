@echo off
git fetch
echo [main] git fetch


setlocal enabledelayedexpansion

cd /d "%~dp0Assets" || (
    echo not exits Assets folder
    pause
    exit /b 1
)

for /d %%D in (*) do (
    if exist "%%D\.git" (
        echo.
        echo [%%D] git fetch
        pushd "%%D"
        git fetch
        popd
    ) else (
        echo.
        echo [%%D] not git repository
    )
)

echo.
pause