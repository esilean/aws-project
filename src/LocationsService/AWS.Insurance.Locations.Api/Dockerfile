#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["LocationsService/AWS.Insurance.Locations.Api/AWS.Insurance.Locations.Api.csproj", "LocationsService/AWS.Insurance.Locations.Api/"]
COPY ["LocationsService/AWS.Insurance.Locations.Infra/AWS.Insurance.Locations.Infra.csproj", "LocationsService/AWS.Insurance.Locations.Infra/"]
COPY ["LocationsService/AWS.Insurance.Locations.Domain/AWS.Insurance.Locations.Domain.csproj", "LocationsService/AWS.Insurance.Locations.Domain/"]

RUN dotnet restore "LocationsService/AWS.Insurance.Locations.Api/AWS.Insurance.Locations.Api.csproj"
COPY . .
WORKDIR "/src/LocationsService/AWS.Insurance.Locations.Api"
RUN dotnet build "AWS.Insurance.Locations.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AWS.Insurance.Locations.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AWS.Insurance.Locations.Api.dll"]