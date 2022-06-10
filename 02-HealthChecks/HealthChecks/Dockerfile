#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["02-HealthChecks/HealthChecks/HealthChecks.csproj", "02-HealthChecks/HealthChecks/"]
RUN dotnet restore "02-HealthChecks/HealthChecks/HealthChecks.csproj"
COPY . .
WORKDIR "/src/02-HealthChecks/HealthChecks"
RUN dotnet build "HealthChecks.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HealthChecks.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HealthChecks.dll"]