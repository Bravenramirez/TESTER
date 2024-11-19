using daslibrary;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Auto_Trader_Platform
{
    public class BPMonitor
    {
        private readonly CMDSocket _socket;
        private readonly Action<double> _onBuyingPowerUpdate;
        private readonly System.Timers.Timer _timer;
        private CancellationTokenSource _cancellationSource;
        private bool _isMonitoring;

        public bool IsMonitoring => _isMonitoring; // Property to check if the monitor is running

        public BPMonitor(CMDSocket socket, Action<double> onBuyingPowerUpdate)
        {
            _socket = socket ?? throw new ArgumentNullException(nameof(socket));
            _onBuyingPowerUpdate = onBuyingPowerUpdate ?? throw new ArgumentNullException(nameof(onBuyingPowerUpdate));

            _timer = new System.Timers.Timer(5000); // 5-second interval
            _timer.Elapsed += async (sender, e) => await FetchAndUpdateBuyingPowerAsync();
        }

        public void Start()
        {
            if (_isMonitoring) return;
            _isMonitoring = true;
            Debug.WriteLine("BPMonitor: Starting...");
            _timer.Start();
        }

        public void Stop()
        {
            if (!_isMonitoring) return;
            Debug.WriteLine("BPMonitor: Stopping...");
            _timer.Stop();
            _cancellationSource?.Cancel();
            _isMonitoring = false;
        }

        private async Task FetchAndUpdateBuyingPowerAsync()
        {
            try
            {
                if (_socket != null && _socket.loginsuccess && Settings.GlobalIsConnected)
                {
                    double buyingPower = await _socket.GetBuyingPowerAsync();
                    Debug.WriteLine($"BPMonitor: Buying Power Fetched - ${buyingPower:N2}");
                    _onBuyingPowerUpdate?.Invoke(buyingPower);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"BPMonitor: Error fetching buying power - {ex.Message}");
            }
        }
    }
}
