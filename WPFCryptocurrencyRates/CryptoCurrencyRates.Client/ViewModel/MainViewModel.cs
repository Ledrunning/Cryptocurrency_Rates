using System;
using System.Collections.ObjectModel;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using CryptoCurrencyRates.Client.Contracts;
using CryptoCurrencyRates.Client.Model;
using CryptoCurrencyRates.Client.ViewModel.Commands;

namespace CryptoCurrencyRates.Client.ViewModel;

internal class MainViewModel : BaseViewModel
{
    private static bool isOn;
    private readonly ICryptoCurrencyRatesService cryptoCurrencyRatesService;
    //NOTICE! I prefer to put such values to settings file, but in this context
    //I used readonly field because there is no need for a current test task I guess
    private readonly TimeSpan updateGridIntervalInMs = TimeSpan.Parse(ApplicationResource.UpdateInterval);
    private string? buttonText;
    private CancellationTokenSource cancelTokenSource;

    private ObservableCollection<CryptoCurrencyModel>? cryptoCurrencyCollection = new();
    private CancellationToken token;

    public MainViewModel(ICryptoCurrencyRatesService cryptoCurrencyRatesService)
    {
        this.cryptoCurrencyRatesService = cryptoCurrencyRatesService;
        InitializeCommands();
        GetWindowsServices();
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
        get { return buttonText ??= ApplicationResource.Start; }
        set
        {
            buttonText = value;
            OnPropertyChanged(nameof(ButtonText));
        }
    }

    public ICommand? StartServiceCommand { get; private set; }

    public ObservableCollection<string> ServiceNames { get; set; } = new();

    private void StartRequest()
    {
        cancelTokenSource = new CancellationTokenSource();
        token = cancelTokenSource.Token;
    }

    private void StopRequest()
    {
        cancelTokenSource.Cancel();
        cancelTokenSource.Dispose();
    }

    public async void StartServiceExecute()
    {
        ButtonText = isOn ? ApplicationResource.Start : ApplicationResource.Stop;
        isOn = !isOn;

        if (isOn)
        {
            StartRequest();
        }
        else
        {
            StopRequest();
        }

        await Dispatcher.CurrentDispatcher.Invoke(async () => { await FetchData(); });
    }

    private async Task FetchData()
    {
        while (!token.IsCancellationRequested)
        {
            try
            {
                //NOTICE! For current context I didn't use created token in Start() method
                //If someone needed to handle operation canceled error token is the must!
                var model = await cryptoCurrencyRatesService.GetCurrentRatesAsync(CancellationToken.None);
                CryptoCurrencyCollection?.Clear();
                CryptoCurrencyCollection?.Add(new CryptoCurrencyModel
                    { Id = model.Bitcoin?.Data?.Id, CurrencyRate = model.Bitcoin?.Data?.RateUsd });
                CryptoCurrencyCollection?.Add(new CryptoCurrencyModel
                    { Id = model.Dogecoin?.Data?.Id, CurrencyRate = model.Dogecoin?.Data?.RateUsd });
                CryptoCurrencyCollection?.Add(new CryptoCurrencyModel
                    { Id = model.Ethereum?.Data?.Id, CurrencyRate = model.Ethereum?.Data?.RateUsd });
                await Task.Delay(updateGridIntervalInMs, CancellationToken.None);
            }
            catch (Exception e)
            {
                ShowMessage($"{e}");
            }
        }
    }

    private void InitializeCommands()
    {
        StartServiceCommand = new RelayCommand(StartServiceExecute);
    }

    private void GetWindowsServices()
    {
        try
        {
            var services = ServiceController.GetServices();
            foreach (var service in services)
            {
                ServiceNames.Add(service.DisplayName);
            }
        }
        catch (Exception e)
        {
            ShowMessage($"{e}");
        }
    }

    private static void ShowMessage(string errorMessage, MessageBoxButton
        button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.Error)
    {
        MessageBox.Show(errorMessage, ApplicationResource.ApplicationName,
            button, icon);
    }
}