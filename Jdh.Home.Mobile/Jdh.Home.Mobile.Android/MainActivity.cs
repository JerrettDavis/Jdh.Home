
using Android.App;
using Android.Content.PM;
using Android.OS;
using Jdh.Home.Mobile.Android.Services;
using Jdh.Home.Mobile.Services;
using Prism;
using Prism.Ioc;
using Xamarin.Forms.Platform.Android;

namespace Jdh.Home.Mobile.Android
{
    [Activity(Label = "JDH Home", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity, IPlatformInitializer
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::Xamarin.Forms.FormsMaterial.Init(this, savedInstanceState);
            LoadApplication(new App(this));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IWifiService, WifiService>();
        }
    }
}