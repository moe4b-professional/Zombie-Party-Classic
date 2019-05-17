:://Unity launches the batch file at the root of the project folder
::so we CD to where the batch file is actually located to ensure similar behaviour
::when launhing from inside Unity and when executing externally
cd /D "%~dp0"

cd ..
explorer.exe /select,Assets