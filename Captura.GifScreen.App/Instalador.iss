[Setup]
AppName=GifScreenApp
AppPublisher=IHCDigitalSolutions
AppPublisherURL=https://simpli-gestao.com.br
AppSupportURL=https://simpli-gestao.com.br
AppVersion=1.0.1
DefaultDirName={pf}\GifScreenApp
DefaultGroupName=GifScreenApp
OutputBaseFilename=GifScreenApp_Installer
Compression=lzma
SolidCompression=yes
SetupIconFile=Resources\icon.ico


[Tasks]
Name: "desktopicon"; Description: "Criar atalho na Área de Trabalho"; GroupDescription: "Opções adicionais:"

[Files]
Source: "bin\Release\net8.0-windows\win-x64\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "Resources\icon.ico"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\GifScreenApp"; Filename: "{app}\Captura.GifScreen.App.exe"; IconFilename: "{app}\icon.ico"; WorkingDir: "{app}"
Name: "{group}\Desinstalar GifScreenApp"; Filename: "{uninstallexe}"
Name: "{commondesktop}\GifScreenApp"; Filename: "{app}\Captura.GifScreen.App.exe"; Tasks: desktopicon; IconFilename: "{app}\icon.ico"; WorkingDir: "{app}" 

