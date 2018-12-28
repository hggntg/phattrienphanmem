FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY ["OnlineReading.API/OnlineReading.API.csproj", "OnlineReading.API/"]
COPY ["Base/Base.csproj", "Base/"]
RUN dotnet restore "OnlineReading.API/OnlineReading.API.csproj"
COPY . .
WORKDIR "/src/OnlineReading.API"
RUN dotnet build "OnlineReading.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "OnlineReading.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "OnlineReading.API.dll"]