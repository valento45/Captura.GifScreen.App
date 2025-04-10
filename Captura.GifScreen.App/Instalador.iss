[Setup]
AppName=GifScreenApp
AppVersion=1.0
DefaultDirName={pf}\GifScreenApp
DefaultGroupName=GifScreenApp
OutputBaseFilename=GifScreenApp_Installer
Compression=lzma
SolidCompression=yes
SetupIconFile=Resources\icon.ico

[Files]
Source: "bin\Release\net8.0-windows\win-x64\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\GifScreenApp"; Filename: "{app}\GifScreenApp.exe"
Name: "{group}\Desinstalar GifScreenApp"; Filename: "{uninstallexe}"