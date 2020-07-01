using System.Collections.Generic;
using System.Collections.ObjectModel;
using Jdh.Home.Mobile.Models;
using Prism.Navigation;

namespace Jdh.Home.Mobile.ViewModels
{
    public class ConfigureDeviceViewModel : ViewModelBase
    {
        private string _hostName;
        private string _ssid;
        private string _password;
        private string _broker;

        public ConfigureDeviceViewModel(INavigationService navigationService) 
            : base(navigationService)
        {
            WifiSignals = new ObservableCollection<WifiSignal>();
        }

        public Device Device { get; set; }
        public ObservableCollection<WifiSignal> WifiSignals { get; private set; }

        public string HostName
        {
            get => _hostName;
            set => SetProperty(ref _hostName, value);
        }

        public string Ssid
        {
            get => _ssid;
            set => SetProperty(ref _ssid, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string Broker
        {
            get => _broker;
            set => SetProperty(ref _broker, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            Device = parameters.GetValue<Device>(ParameterConstants.Device);
            WifiSignals = parameters.GetValue<ObservableCollection<WifiSignal>>(ParameterConstants.WifiSignals);
        }
    }
}
