using Jdh.Home.Mobile.Models;
using Jdh.Home.Mobile.Views;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;

namespace Jdh.Home.Mobile.ViewModels
{
    public class AddDeviceViewModel : ViewModelBase
    {
        public AddDeviceViewModel(
            INavigationService navigationService)
            : base(navigationService)
        {
            // TODO: Feed this from an API.

            Devices = new List<Device>
            {
                new Device(
                    "TempSense V1", 
                    "JTS001", 
                    new Uri("https://i.imgur.com/6G1Q35P.png"), 
                    "TempSense",
                    "TempSense")
            };

            SelectDeviceTapped = new DelegateCommand<Device>(SelectDevice);
        }

        public IList<Device> Devices { get; set; }

        public DelegateCommand<Device> SelectDeviceTapped { get; set; }

        public async void SelectDevice(Device device)
        {
            var parameters = new NavigationParameters
            {
                { AddDeviceParameters.Device, device }
            };
            await NavigationService.NavigateAsync(nameof(ConnectToDevice), parameters);
        }
    }

    public struct AddDeviceParameters
    {
        public static string Device = "Device";
    }
}
