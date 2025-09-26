@echo off
REM Edit these to match your machine paths if needed
SET MSBUILD_PATH="C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe"
SET SOLUTION_DIR=%~dp0
SET PROJECT=src\YTPMVScript\YTPMVScript.csproj

IF NOT EXIST %MSBUILD_PATH% (
  ECHO MSBuild not found at %MSBUILD_PATH%. Trying dotnet msbuild...
  dotnet msbuild "%PROJECT%" /t:Rebuild /p:Configuration=Release
  GOTO :EOF
)

"%MSBUILD_PATH%" "%PROJECT%" /t:Rebuild /p:Configuration=Release
IF %ERRORLEVEL% EQU 0 (
  ECHO Build succeeded.
) ELSE (
  ECHO Build failed. Check msbuild output above.
)
pause