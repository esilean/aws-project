#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["STSService/AWS.Insurance.STS.Api/AWS.Insurance.STS.Api.csproj", "STSService/AWS.Insurance.STS.Api/"]
COPY ["STSService/AWS.Insurance.STS.Infra/AWS.Insurance.STS.Infra.csproj", "STSService/AWS.Insurance.STS.Infra/"]
COPY ["STSService/AWS.Insurance.STS.Application/AWS.Insurance.STS.Application.csproj", "STSService/AWS.Insurance.STS.Application/"]
COPY ["STSService/AWS.Insurance.STS.Domain/AWS.Insurance.STS.Domain.csproj", "STSService/AWS.Insurance.STS.Domain/"]

RUN dotnet restore "STSService/AWS.Insurance.STS.Api/AWS.Insurance.STS.Api.csproj"
COPY . .
WORKDIR "/src/STSService/AWS.Insurance.STS.Api"
RUN dotnet build "AWS.Insurance.STS.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AWS.Insurance.STS.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AWS.Insurance.STS.Api.dll"]