# YTPMVScript — Sony Vegas 12–14 script project (VS Code)

This repository contains a Visual Studio Code project scaffold and a Sony Vegas script skeleton (C#) for Vegas Pro 12–14. It exposes a WinForms UI inside Vegas and helper modules for:

- Pitch helper (change pitch of selected audio events)
- Mixer controls (volume/pan for tracks)
- Apply selected plugins to selected tracks/events
- Browse sources and browse audio (open/import media)
- Generate visuals (basic generated media helper)
- Generate audio (tones/noise helper)

Important notes
- This is a scaffold and includes TODO markers where you should adapt to your exact Vegas API version and desired functionality.
- Vegas scripting is done in .NET; the script entrypoint is EntryPoint.FromVegas(Vegas vegas). The code references ScriptPortal.Vegas.dll — update the path in the .csproj or build script to point at your Vegas installation (usually under "C:\Program Files\VEGAS\VEGAS Pro X.X\Script Menu\" or similar).
- Target: .NET Framework (csproj targets .NETFramework 4.6); adjust if necessary for your environment.
- On Windows 8.1 use the provided build.bat or msbuild to compile. The script can also be dropped as a single .cs into Vegas' Script Menu folder for quick testing.

Quick setup
1. Update the csproj HintPath for ScriptPortal.Vegas.dll to match your local installation.
2. Open the folder in Visual Studio Code.
3. Build:
   - Run `.\build.bat` (edit paths at top if msbuild/csc aren't found), or
   - Use the VS Code task (Tasks: Run Build Task).
4. Copy the produced dll or cs file to Vegas Script folder:
   - Example: `C:\Program Files\VEGAS\VEGAS Pro 14.0\Script Menu\MyScripts\YTPMVScript.cs`
5. Launch Vegas Pro 12–14, open the Scripts menu and run "YTPMVScript".

Where to customize (short list)
- src/YTPMVScript/Helpers/* — implement the API calls inside TODO areas.
- src/YTPMVScript/Generators/* — generate media with your desired presets.
- UI layout: src/YTPMVScript/Ui/MainForm.cs

Support / troubleshooting
- If the compiler cannot find ScriptPortal.Vegas.dll, update the csproj HintPath or add the DLL to the same folder for compile-time reference.
- If you prefer to run raw .cs files from Vegas (no compiled DLL), copy src/YTPMVScript/EntryPoint.cs and src/YTPMVScript/* files into Vegas Script Menu folder and run.

License
- You decide — this scaffold is provided without warranty and intended for development and personal use.