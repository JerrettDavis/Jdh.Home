using Jdh.Home.Mobile.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Jdh.Home.Mobile.Services
{
    public interface IWifiService
    {
        Task<string> GetCurrentConnection(
            CancellationToken cancellationToken = default);
        Task<IEnumerable<WifiSignal>> GetWifiSignals(
            CancellationToken cancellationToken = default);
        Task Connect(
            string ssid, 
            string password = null, 
            bool wep = false,
            CancellationToken cancellationToken = default);
    }
}
