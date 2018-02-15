dotnet build

dotnet publish -c Release -o deploy

docker build -t problem1 .