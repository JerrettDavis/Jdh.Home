using Android.App;
using Android.Content;
using Android.Net.Wifi;
using Jdh.Home.Mobile.Models;
using Jdh.Home.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Jdh.Home.Mobile.Android.Services
{
    public class WifiService : IWifiService
    {
        private readonly Context _context;
        private readonly WifiManager _wifiManager;

        public WifiService()
        {
            _context = Application.Context;
            _wifiManager = (WifiManager)_context.GetSystemService(Context.WifiService);
        }

        public Task<string> GetCurrentConnection(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_wifiManager?.ConnectionInfo?.SSID);
        }

        public async Task<IEnumerable<WifiSignal>> GetWifiSignals(CancellationToken cancellationToken = default)
        {
            IEnumerable<WifiSignal> signals = new List<WifiSignal>();
            var wifiReceiver = new WifiReceiver(_wifiManager);
            var intentFilter = new IntentFilter(WifiManager.ScanResultsAvailableAction);

            await Task.Run(() =>
            {
                _context.RegisterReceiver(wifiReceiver, intentFilter);
                signals = wifiReceiver.Scan();
            }, cancellationToken);

            return signals;
        }

        public async Task Connect(
            string ssid, 
            string password = null, 
            bool wep = false,
            CancellationToken cancellationToken = default)
        {
            await Task.Run(() =>
            {
                var configuration = new WifiConfiguration { Ssid = $"\"{ssid}\"" };

                if (password == null)
                    configuration.AllowedKeyManagement.Set((int)KeyManagementType.None);
                else if (!wep)
                    configuration.PreSharedKey = $"\"{password}\"";
                else
                {
                    throw new NotImplementedException("Not going to support WEP right now.");
                }

                _wifiManager.AddNetwork(configuration);
                Reconnect(ssid);
            }, cancellationToken);
        }

        private void Reconnect(string ssid)
        {
            var list = _wifiManager.ConfiguredNetworks;
            foreach (var i in list)
            {
                if (i.Ssid == null || !i.Ssid.Equals("\"" + ssid + "\"")) continue;

                _wifiManager.Disconnect();
                _wifiManager.EnableNetwork(i.NetworkId, true);
                _wifiManager.Reconnect();

                break;
            }
        }

        [BroadcastReceiver]
        [IntentFilter(new [] { WifiManager.NetworkStateChangedAction, WifiManager.WifiStateChangedAction })]
        internal class WifiReceiver : BroadcastReceiver
        {
            private readonly WifiManager _wifi;
            private readonly ICollection<WifiSignal> _wifiNetworks;
            private readonly AutoResetEvent _receiverReset;
            private const int TimeoutMillis = 20000; // 20 seconds timeout

            public WifiReceiver() { }

            public WifiReceiver(WifiManager wifi)
            {
                _wifi = wifi;
                _wifiNetworks = new HashSet<WifiSignal>();
                _receiverReset = new AutoResetEvent(false);
            }

            public IEnumerable<WifiSignal> Scan()
            {
                using var _ = new Timer(Timeout, null, TimeoutMillis, System.Threading.Timeout.Infinite);

                _wifi.StartScan();
                _receiverReset.WaitOne();

                return _wifiNetworks;
            }

            public override void OnReceive(Context context, Intent intent)
            {
                var results = _wifi.ScanResults;

                foreach (var network in results)
                {
                    _wifiNetworks.Add(new WifiSignal(network.Ssid, network.Capabilities, network.Level));
                }

                _receiverReset.Set();
            }

            private void Timeout(object sender)
            {
                _receiverReset.Set();
            }
        }
    }
}