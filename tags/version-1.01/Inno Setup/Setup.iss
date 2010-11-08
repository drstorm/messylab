[Setup]
AppName=Messy Lab
AppVersion=1.01
AppVerName=Messy Lab 1.01
AppCopyright=Copyright ¬ 2010 Milos Andjelkovic
AppPublisher=Messy Lab
AppPublisherURL=http://www.messylab.com
AppID={{8FAD026B-CA55-408F-82A4-FDBC58200387}
VersionInfoVersion=1.01
VersionInfoCompany=Messy Lab
VersionInfoCopyright=Copyright ¬ 2010 Milos Andjelkovic
VersionInfoProductName=Messy Lab
VersionInfoProductVersion=1.01
OutputDir=Inno Setup\bin
OutputBaseFilename=MessyLab-1.01
DirExistsWarning=no
CreateAppDir=true
SourceDir=..
AllowNoIcons=true
UsePreviousGroup=true
UsePreviousAppDir=true
SolidCompression=true

DefaultGroupName=Messy Lab
DefaultDirName={pf}\Messy Lab

UninstallDisplayIcon={app}\MessyLab.exe
UninstallDisplayName=Messy Lab
Uninstallable=true

MinVersion=0,5.0
PrivilegesRequired=none
ArchitecturesAllowed=x86 x64 ia64
ArchitecturesInstallIn64BitMode=x64 ia64
ShowLanguageDialog=auto

LicenseFile=LICENSE.txt

[Files]
Source: MessyLab\bin\Release\Debugger.exe; DestDir: {app}
Source: MessyLab\bin\Release\grammatica-1.5.dll; DestDir: {app}
Source: MessyLab\bin\Release\ICSharpCode.TextEditor.dll; DestDir: {app}
Source: MessyLab\bin\Release\MessyLab.exe; DestDir: {app}
Source: MessyLab\bin\Release\MessyLab.exe.config; DestDir: {app}
Source: MessyLab\bin\Release\PicoAsm.exe; DestDir: {app}
Source: MessyLab\bin\Release\PicoVM.exe; DestDir: {app}
Source: MessyLab\bin\Release\WeifenLuo.WinFormsUI.Docking.dll; DestDir: {app}
Source: MessyLab\bin\Release\Platforms\Pico\DefaultDock.xml; DestDir: {app}\Platforms\Pico
Source: MessyLab\bin\Release\Platforms\Pico\Empty.pca; DestDir: {app}\Platforms\Pico
Source: Images\Project.ico; DestDir: {app}
Source: NOTICE.txt; DestDir: {app}
Source: LICENSE.txt; DestDir: {app}

[Registry]
Root: HKCR; Subkey: .mlp; ValueType: string; ValueName: ; ValueData: MessyLabProjectFile; Flags: uninsdeletevalue; Tasks: associateproject
Root: HKCR; Subkey: MessyLabProjectFile; ValueType: string; ValueName: ; ValueData: Messy Lab Project File; Flags: uninsdeletekey; Tasks: associateproject
Root: HKCR; Subkey: MessyLabProjectFile\DefaultIcon; ValueType: string; ValueData: {app}\Project.ico; Tasks: associateproject
Root: HKCR; Subkey: MessyLabProjectFile\shell\open\command; ValueType: string; ValueName: ; ValueData: """{app}\MessyLab.exe"" ""%1"""; Tasks: associateproject

[CustomMessages]
DotNetMissing=This application needs Microsoft .NET Framework 3.5 which is not yet installed. Do you like to download it now?
AssociateMlp=Associate Project Files (MLP) with Messy Lab
FileAssoc=File Associations:

[Tasks]
Name: associateproject; Description: {cm:AssociateMlp}; GroupDescription: {cm:FileAssoc}
Name: desktopicon; Description: {cm:CreateDesktopIcon}; GroupDescription: {cm:AdditionalIcons}

[Icons]
Name: {group}\Messy Lab; Filename: {app}\MessyLab.exe
Name: {group}\{cm:UninstallProgram,Messy Lab}; Filename: {uninstallexe}
Name: {commondesktop}\Messy Lab; Filename: {app}\MessyLab.exe; Tasks: desktopicon

[Run]
Filename: {app}\MessyLab.exe; Description: {cm:LaunchProgram,Messy Lab}; Flags: nowait postinstall skipifsilent

[Code]
function InitializeSetup(): Boolean;
var
    ErrorCode: Integer;
    netFrameWorkInstalled : Boolean;
    isInstalled: Cardinal;
begin
  result := true;

    // Check for the .Net 3.5 framework
  isInstalled := 0;
  netFrameworkInstalled := RegQueryDWordValue(HKLM, 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5', 'Install', isInstalled);
  if ((netFrameworkInstalled)  and (isInstalled <> 1)) then netFrameworkInstalled := false;

  if netFrameworkInstalled = false then
  begin
    if (MsgBox(ExpandConstant('{cm:DotNetMissing}'),
        mbConfirmation, MB_YESNO) = idYes) then
    begin
      ShellExec('open',
      'http://www.microsoft.com/downloads/details.aspx?FamilyID=AB99342F-5D1A-413D-8319-81DA479AB0D7&displaylang=en',
      '','',SW_SHOWNORMAL,ewNoWait,ErrorCode);
    end;
    result := false;
  end;

end;


