#! /bin/sh

dotnet publish -c Release -o deploy

dotnet run deploy/problem1.dll