set gdpath=.\GraphDistance\bin\Release\net5.0\win10-x64
if exist %gdpath% del %gdpath% /Q
dotnet publish .\GraphDistance\GraphDistance.csproj -r win-x64 -c Release -p:PublishReadyToRunShowWarnings=true -p:PublishReadyToRun=true -p:PublishSingleFile=true -p:PublishTrimmed=true --self-contained true -p:IncludeNativeLibrariesForSelfExtract=true
PAUSE