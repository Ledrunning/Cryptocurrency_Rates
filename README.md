# WPF Client - Server Cryptocurrency rates

[![.NET](https://github.com/Ledrunning/Cryptocurrency_Rates/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/Ledrunning/Cryptocurrency_Rates/actions/workflows/dotnet-desktop.yml)


The project demonstrates getting and updating cryptocurrency exchange rates getting data about running Windows services and showing names of services:

- Actual cryptocurrency rates from https://coincap.io/

- Windows services that are currently running

The program is written in C# language on .NET 6 in Visual Studio Enterprise 2022
- The project included server-side microservice **CryptocurrencyRates.Gateway** for getting rates from coincap API
- Service library for concap API **CryptocurrencyRates.Services**
- Common library for further app extension **CryptocurrencyRates.Common**
- WPF desktop application **CryptoCurrencyRates.Client**

In development mode in gateway microservice, it can use Swagger UI for server-side test app and coincap API. 
The logging is available in **CryptocurrencyRates.Gateway** and write into a file. 
You be able to find it in **CryptocurrencyRates.Gateway** bin folder called Logs

Programm setup:
- CryptocurrencyRates.Gateway: use appsettings.json and the next section
```
"ConcapApi": {
    "BaseUrl": "https://api.coincap.io/v2/rates",
    "Timeout":  -1 
  }
```
  
- CryptoCurrencyRates.The client uses appsettings.json and the next section to change the host IP address for CryptocurrencyRates.Gateway
```
"CryptocurrencyRatesGateway": {
    "GatewayUrl": "https://localhost:7208/api/CryptoCurrency/GetCurrentRates"
  }
```
  
  ## UI Layouts
  
  ![](currencyRates.gif)
