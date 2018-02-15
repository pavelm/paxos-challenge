#! /bin/sh 
dotnet publish -c Release -o deploy

dotnet run deploy/problem3.dll $@