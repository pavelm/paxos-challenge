FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

# copy fsproj and restore as distinct layers
COPY *.fsproj ./
RUN dotnet restore

# copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# build runtime image
FROM microsoft/dotnet:runtime
WORKDIR /app
COPY --from=build-env /app/out ./
COPY --from=build-env /app ./
ENTRYPOINT ["dotnet", "problem2.dll"]

