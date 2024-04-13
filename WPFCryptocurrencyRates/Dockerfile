FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /app

COPY *.sln .
COPY . .

RUN dotnet restore

# Build the solution with verbose output
RUN dotnet build -c Release --no-restore -v normal

WORKDIR /app/CryptocurrencyRates.Gateway
# Build the gateway project with verbose output
RUN dotnet build -c Release -o /app/build -v normal

WORKDIR /app/CryptoCurrencyRates.Client
# Build the client project with verbose output
RUN dotnet build -c Release -o /app/build -v normal

WORKDIR /app

COPY CryptocurrencyRates.Gateway/bin/Release/net6.0/ ./
COPY CryptoCurrencyRates.Client/bin/Release/net6.0-windows/ ./

ENTRYPOINT ["dotnet", "CryptocurrencyRates.Gateway.dll"]
