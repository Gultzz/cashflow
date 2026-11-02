FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base-cashflow
WORKDIR /app

COPY src/ .

WORKDIR /app/CashFlow.Api

RUN dotnet restore

RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=base-cashflow /app/out .

ENTRYPOINT ["dotnet", "CashFlow.Api.dll"]