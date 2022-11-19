using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using CryptoCurrencyRates.Client.Contracts;
using CryptoCurrencyRates.Client.Model;
using CryptoCurrencyRates.Client.Services.Rest;
using CryptoCurrencyRates.Client.ViewModel.Commands;

namespace CryptoCurrencyRates.Client.ViewModel;

internal class MainViewModel : BaseViewModel
{
    private static bool isOn;
    private readonly ICryptoCurrencyRatesService cryptoCurrencyRatesService;

    private string? buttonText;

    private ObservableCollection<CryptoCurrencyModel>? cryptoCurrencyCollection = new();

    public MainViewModel()
    {
        cryptoCurrencyRatesService =
            new CryptoCurrencyRatesService("https://localhost:7208/api/CryptoCurrency/GetCurrentRates");
        InitializeCommands();
    }

    public ObservableCollection<CryptoCurrencyModel>? CryptoCurrencyCollection
    {
        get => cryptoCurrencyCollection;
        set
        {
            cryptoCurrencyCollection = value;
            OnPropertyChanged(nameof(CryptoCurrencyCollection));
        }
    }

    public string ButtonText
    {
        get { return buttonText ??= "Start"; }
        set
        {
            buttonText = value;
            OnPropertyChanged(nameof(ButtonText));
        }
    }

    public ICommand? StartServiceCommand { get; private set; }

    public async void StartServiceExecute()
    {
        try
        {
            var responseData = await cryptoCurrencyRatesService.GetCurrentRatesAsync(CancellationToken.None);
            CryptoCurrencyCollection?.Add(new CryptoCurrencyModel
                { Id = responseData.Bitcoin?.Data?.Id, CurrencyRate = responseData.Bitcoin?.Data?.RateUsd });
            CryptoCurrencyCollection?.Add(new CryptoCurrencyModel
                { Id = responseData.Dogecoin?.Data?.Id, CurrencyRate = responseData.Dogecoin?.Data?.RateUsd });
            CryptoCurrencyCollection?.Add(new CryptoCurrencyModel
                { Id = responseData.Ethereum?.Data?.Id, CurrencyRate = responseData.Ethereum?.Data?.RateUsd });

            ButtonText = isOn ? "Start" : "Stop";
            isOn = !isOn;
        }
        catch (Exception e)
        {
            MessageBox.Show($"{e}", "WPFCryptocurrencyRates app", 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void InitializeCommands()
    {
        StartServiceCommand = new RelayCommand(StartServiceExecute);
    }
}