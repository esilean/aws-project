#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["OperationsService/AWS.Insurance.Operations.Api/AWS.Insurance.Operations.Api.csproj", "OperationsService/AWS.Insurance.Operations.Api/"]
COPY ["OperationsService/AWS.Insurance.Operations.Infra/AWS.Insurance.Operations.Infra.csproj", "OperationsService/AWS.Insurance.Operations.Infra/"]
COPY ["OperationsService/AWS.Insurance.Operations.Application/AWS.Insurance.Operations.Application.csproj", "OperationsService/AWS.Insurance.Operations.Application/"]
COPY ["OperationsService/AWS.Insurance.Operations.Data/AWS.Insurance.Operations.Data.csproj", "OperationsService/AWS.Insurance.Operations.Data/"]
COPY ["OperationsService/AWS.Insurance.Operations.Domain/AWS.Insurance.Operations.Domain.csproj", "OperationsService/AWS.Insurance.Operations.Domain/"]

RUN dotnet restore "OperationsService/AWS.Insurance.Operations.Api/AWS.Insurance.Operations.Api.csproj"
COPY . .
WORKDIR "/src/OperationsService/AWS.Insurance.Operations.Api"
RUN dotnet build "AWS.Insurance.Operations.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AWS.Insurance.Operations.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AWS.Insurance.Operations.Api.dll"]