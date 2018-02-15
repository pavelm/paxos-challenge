#! /bin/sh 
dotnet publish -c Release -o deploy

dotnet run deploy/problem2.dll $@