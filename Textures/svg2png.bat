@echo off
if "%~1"=="" (
    echo Usage: %0 input_dir output_dir width height
    exit /b 1
)

set "input_dir=%~1"
set "output_dir=%~2"
set "width=%~3"
set "height=%~4"

if not exist "%output_dir%" (
    mkdir "%output_dir%"
)

for %%f in ("%input_dir%\*.svg") do (
    if exist "%output_dir%\%%~nf.png" (
        del "%output_dir%\%%~nf.png"
    )
    @REM magick "%%f" -background none -size %width%x%height% "%output_dir%\%%~nf.png"
    inkscape -w %width% -h %height% "%%f" -o "%output_dir%\%%~nf.png"
)

echo SVG to PNG conversion complete.
