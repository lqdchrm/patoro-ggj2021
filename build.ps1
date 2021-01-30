dotnet build /p:Configuration=Release /p:Platform="Any CPU"

if ($LastExitCode -ne 0) {
   exit
}

iex ".\bin\Release\net5.0\laf-ggj2021.exe"

