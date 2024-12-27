@echo off
if "%~1"=="" (
    echo 使用方法: %0 input_dir output_dir width height
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
    if not exist "%output_dir%\%%~nf.png" (
        @REM inkscape -w %width% -h %height% "%%f" -o "%output_dir%\%%~nf.png"
        convert "%%f" -size %width%x%height% "%output_dir%\%%~nf.png"
    ) else (
        echo 文件已存在，跳过转换: "%output_dir%\%%~nf.png"
    )
)

echo 完成所有 SVG 文件的转换
