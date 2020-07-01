using Jdh.Home.Mobile.ViewModels;
using Jdh.Home.Mobile.ViewModels.Dialogs;
using Jdh.Home.Mobile.Views;
using Jdh.Home.Mobile.Views.Dialogs;
using Prism;
using Prism.Ioc;
using Xamarin.Forms;

namespace Jdh.Home.Mobile
{
    public partial class App
    {

        public App(IPlatformInitializer initializer) 
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            var result = await NavigationService.NavigateAsync("NavigationPage/MainPage");

            if (!result.Success)
            {
                System.Diagnostics.Debugger.Break();
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<AddDevice, AddDeviceViewModel>();
            containerRegistry.RegisterForNavigation<ConnectToDevice, ConnectToDeviceViewModel>();
            containerRegistry.RegisterForNavigation<ConfigureDevice, ConfigureDeviceViewModel>();

            containerRegistry.RegisterDialog<ScanningWifi, ScanningWifiViewModel>(DialogConstants.ScanningWifi);
            containerRegistry.RegisterDialog<ConnectingToDevice, ConnectingToDeviceViewModel>(DialogConstants.ConnectingToDevice);
        }
    }
}
