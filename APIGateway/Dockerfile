FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY APIGateway/APIGateway.csproj APIGateway/
RUN dotnet restore APIGateway/APIGateway.csproj
COPY . .
WORKDIR /src/APIGateway
RUN dotnet build APIGateway.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish APIGateway.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "APIGateway.dll"]
