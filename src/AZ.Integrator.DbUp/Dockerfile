FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["AZ.Integrator.DbUp/AZ.Integrator.DbUp.csproj", "AZ.Integrator.DbUp/"]
RUN dotnet restore "AZ.Integrator.DbUp/AZ.Integrator.DbUp.csproj"
COPY . .
WORKDIR "/src/AZ.Integrator.DbUp"
RUN dotnet build "AZ.Integrator.DbUp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AZ.Integrator.DbUp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "AZ.Integrator.DbUp.dll"]