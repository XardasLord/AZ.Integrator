FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["AZ.Integrator.Api/AZ.Integrator.Api.csproj", "AZ.Integrator.Api/"]
RUN dotnet restore "AZ.Integrator.Api/AZ.Integrator.Api.csproj"
COPY . .
WORKDIR "/src/AZ.Integrator.Api"
RUN dotnet build "AZ.Integrator.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AZ.Integrator.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AZ.Integrator.Api.dll"]
