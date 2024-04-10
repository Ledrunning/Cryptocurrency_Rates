FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the entire contents of the WPFCryptocurrencyRates directory into the container
COPY . .

# Restore dependencies and build the CryptocurrencyRates.Gateway project
RUN dotnet restore CryptocurrencyRates.Gateway/CryptocurrencyRates.Gateway.csproj
RUN dotnet build CryptocurrencyRates.Gateway/CryptocurrencyRates.Gateway.csproj -c Release -o /app/build

# Set the working directory to the output directory of CryptocurrencyRates.Gateway
WORKDIR /app/CryptocurrencyRates.Gateway/bin/Release/net6.0-windows

# Copy the built files into the container
COPY CryptocurrencyRates.Gateway/bin/Release/net6.0 .

# Switch back to the main working directory
WORKDIR /app

# Build the CryptoCurrencyRates.Client project
RUN dotnet build CryptoCurrencyRates.Client/CryptoCurrencyRates.Client.csproj -c Release -o /app/build

# Set the working directory to the output directory of CryptoCurrencyRates.Client
WORKDIR /app/CryptoCurrencyRates.Client/bin/Release/net6.0-windows

# Copy the built files into the container
COPY CryptoCurrencyRates.Client/bin/Release/net6.0-windows .

# Switch back to the main working directory
WORKDIR /app

# Set the entry point for the container
ENTRYPOINT ["dotnet", "CryptocurrencyRates.Gateway.exe"]
