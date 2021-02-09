dotnet build ./FindLosty/FindLosty.csproj /p:Configuration=Release /p:Platform="Any CPU"

if ($LastExitCode -ne 0) {
   exit
}

$WorkDir = '.\FindLosty\bin\Any CPU\Release\net5.0\'
$FileName = 'FindLosty.exe'
Start-Process -FilePath $WorkDir\$FileName -WorkingDirectory $WorkDir
