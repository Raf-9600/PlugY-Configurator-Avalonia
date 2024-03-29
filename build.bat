setlocal enableDelayedExpansion
set "inName=PlugY Configurator Avalonia"
set "outName=PlugY Configurator"
set "csprojName=PlugY Configurator Avalonia.csproj"
set "pubDir=R:\PlugY Configurator Avalonia\pub\"
set "mainParam=-c Release /p:PublishSingleFile=true /p:PublishSelfContained=true /p:IncludeNativeLibrariesForSelfExtract=true /p:IncludeAllContentForSelfExtract=true /p:BuiltInComInteropSupport=true /p:PublishTrimmed=true /p:TrimmerRemoveSymbols=true /p:EnableCompressionInSingleFile=false /p:TrimMode=partial /p:Platform=x64  /p:TargetFramework=net8.0"

dotnet publish "%csprojName%" -r win-x64 %mainParam% /p:PublishDir="%pubDir%"
del "%pubDir%%outName% for Windows 10.exe"
ren "%pubDir%%inName%.exe" "%outName% for Windows 10.exe"
dotnet publish "%csprojName%" -r linux-x64 %mainParam% /p:PublishDir="%pubDir%"
del "%pubDir%%outName% for Linux"
ren "%pubDir%%inName%" "%outName% for Linux"
dotnet publish "%csprojName%" -r osx-x64 %mainParam% /p:PublishDir="%pubDir%"
del "%pubDir%%outName% for macOS"
ren "%pubDir%%inName%" "%outName% for macOS"