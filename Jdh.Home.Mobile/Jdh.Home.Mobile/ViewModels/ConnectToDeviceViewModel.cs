using Jdh.Home.Mobile.Events;
using Jdh.Home.Mobile.Models;
using Jdh.Home.Mobile.Services;
using Jdh.Home.Mobile.ViewModels.Dialogs;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using Jdh.Home.Mobile.Views;

namespace Jdh.Home.Mobile.ViewModels
{
    public class ConnectToDeviceViewModel : ViewModelBase, IDisposable
    {
        private readonly IWifiService _wifiService;
        private readonly IDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;
        private bool _scanningWifi;
        private bool _connectingToDevice;

        private CancellationTokenSource _cancellationSource;

        public ConnectToDeviceViewModel(
            INavigationService navigationService, 
            IWifiService wifiService, 
            IDialogService dialogService, 
            IEventAggregator eventAggregator) 
            : base(navigationService)
        {
            _wifiService = wifiService;
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;

            WifiSignals = new ObservableCollection<WifiSignal>();
            DeviceWifiSignals = new ObservableCollection<WifiSignal>();
            ScanWifiCommand = new DelegateCommand(ScanWifi);
            ConnectToWifiCommand = new DelegateCommand<WifiSignal>(ConnectToWifi);
        }

        public async void ScanWifi()
        {
            using (_cancellationSource = new CancellationTokenSource())
            {
                var id = Guid.NewGuid();
                var parameters = new DialogParameters
                {
                    {ScanningWifiParams.CancellationTokenSource, _cancellationSource},
                    {ScanningWifiParams.Identifier,  id}
                };

                _dialogService.ShowDialog(DialogConstants.ScanningWifi, parameters);
                var signals = (await _wifiService.GetWifiSignals(_cancellationSource.Token))
                    .Where(p => !string.IsNullOrWhiteSpace(p.Ssid))
                    .ToList();
                var modules = signals.Where(p => p.Ssid.StartsWith(Device.SsidPrefix))
                    .OrderByDescending(p => p.Level)
                    .ToList();
                WifiSignals.Clear();
                DeviceWifiSignals.Clear();
                signals.ForEach(WifiSignals.Add);
                modules.ForEach(DeviceWifiSignals.Add);

                _eventAggregator.GetEvent<CloseDialogEvent>().Publish(id);
            }
        }

        public async void ConnectToWifi(WifiSignal signal)
        {
            using (_cancellationSource = new CancellationTokenSource())
            {
                var id = Guid.NewGuid();
                var parameters = new DialogParameters
                {
                    {ConnectingToDeviceParams.CancellationTokenSource, _cancellationSource},
                    {ConnectingToDeviceParams.Identifier,  id}
                };

                _dialogService.ShowDialog(DialogConstants.ConnectingToDevice, parameters);
                await _wifiService.Connect(signal.Ssid, Device.DefaultPassword,
                        false, _cancellationSource.Token);

                _eventAggregator.GetEvent<CloseDialogEvent>().Publish(id);

                var configParams = new NavigationParameters
                {
                    {ParameterConstants.Device, Device },
                    {ParameterConstants.WifiSignals, WifiSignals }
                };
                await NavigationService.NavigateAsync(nameof(ConfigureDevice), configParams);
            }
        }

        public Device Device { get; private set; }

        public bool ScanningWifi
        {
            get => _scanningWifi;
            set => SetProperty(ref _scanningWifi, value);
        }

        public bool ConnectingToDevice
        {
            get => _connectingToDevice;
            set => SetProperty(ref _connectingToDevice, value);
        }

        public DelegateCommand ScanWifiCommand { get; }
        public DelegateCommand<WifiSignal> ConnectToWifiCommand { get; private set; }

        public ObservableCollection<WifiSignal> WifiSignals { get; }
        public ObservableCollection<WifiSignal> DeviceWifiSignals { get; }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            Device = parameters.GetValue<Device>(AddDeviceParameters.Device);

            ScanWifi();
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            _cancellationSource?.Cancel();
            _cancellationSource?.Dispose();
            base.OnNavigatedFrom(parameters);
        }

        public void Dispose()
        {
            _cancellationSource?.Dispose();
        }
    }
}
