FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY FileService/FileService.csproj FileService/
COPY Base/Base.csproj Base/
RUN dotnet restore FileService/FileService.csproj
COPY . .
WORKDIR /src/FileService
RUN dotnet build FileService.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish FileService.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "FileService.dll"]
