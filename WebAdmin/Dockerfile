FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY WebAdmin/WebAdmin.csproj WebAdmin/
RUN dotnet restore WebAdmin/WebAdmin.csproj
COPY . .
WORKDIR /src/WebAdmin
RUN dotnet build WebAdmin.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish WebAdmin.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WebAdmin.dll"]
