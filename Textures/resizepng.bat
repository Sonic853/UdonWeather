@echo off
if "%~1"=="" (
    echo 使用方法: %0 input_dir output_dir width height
    exit /b 1
)

REM 获取命令行参数
set "inputPath=%~1"
set "outputPath=%~2"
set "width=%~3"
set "height=%~4"

if not exist "%outputPath%" (
    mkdir "%outputPath%"
)

REM 遍历路径下的所有 PNG 文件并调整大小
for %%i in ("%inputPath%\*.png") do (
    echo 处理文件 %%i
    magick convert "%%i" -resize %width%x%height% -gravity center -background none -extent %width%x%height% "%outputPath%\%%~ni.png"
)

echo 完成。
