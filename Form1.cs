using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Auto_Trader_Platform.Models;
using Auto_Trader_Platform.Services;

namespace Auto_Trader_Platform
{
    public partial class Form1 : Form
    {
        private readonly TradingEngine _tradingEngine;
        private readonly List<string> _watchlist = new List<string> { "AAPL", "MSFT", "GOOGL" };
        private System.Windows.Forms.Timer _marketDataTimer;
        private const string ApiKey = "your_api_key";

        private BPMonitor _bpMonitor;

        public Form1()
        {
            InitializeComponent();
            _tradingEngine = new TradingEngine(ApiKey);
            _tradingEngine.OnTradesUpdated += TradingEngine_OnTradesUpdated;
            InitializeTimer();

            // Start BP Monitor when the main form initializes
            if (Settings.GlobalCmdSocket != null && Settings.GlobalIsConnected)
            {
                StartBPMonitor();
            }
        }
        public void StartBPMonitor()
        {
            Debug.WriteLine("Starting BP Monitor...");

            if (_bpMonitor != null)
            {
                Debug.WriteLine("Stopping existing BP Monitor...");
                _bpMonitor.Stop();
            }

            if (Settings.GlobalCmdSocket != null && Settings.GlobalIsConnected)
            {
                _bpMonitor = new BPMonitor(Settings.GlobalCmdSocket, UpdateBuyingPower);
                _bpMonitor.Start();
                Debug.WriteLine("BP Monitor Started Successfully.");
            }
            else
            {
                Debug.WriteLine("BP Monitor Not Started: GlobalCmdSocket is null or not connected.");
            }
        }



        private void InitializeBPMonitor()
        {
            if (Settings.GlobalCmdSocket != null)
            {
                _bpMonitor?.Stop();  // Stop any existing monitor
                _bpMonitor = new BPMonitor(Settings.GlobalCmdSocket, UpdateBuyingPower);
                _bpMonitor.Start();
            }
        }

        public void StartMonitoring()
        {
            if (Settings.GlobalCmdSocket != null && Settings.GlobalIsConnected)
            {
                _bpMonitor?.Stop();
                _bpMonitor = new BPMonitor(Settings.GlobalCmdSocket, (bp) =>
                {
                    Debug.WriteLine($"UI Update Triggered: ${bp:N2}");
                    BeginInvoke(new Action(() =>
                    {
                        lblStatus.Text = $"Connected - BP: ${bp:N2}";
                        lblStatus.BackColor = Color.Lime;
                        Debug.WriteLine($"UI Updated: ${bp:N2}");
                    }));
                });
                _bpMonitor.Start();
            }
        }

        private void UpdateBuyingPower(double buyingPower)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateBuyingPower(buyingPower)));
                return;
            }

            lblStatus.Text = $"Connected - BP: ${buyingPower:N2}";
            lblStatus.BackColor = Color.Lime;
            Debug.WriteLine($"Buying Power Updated: ${buyingPower:N2}"); // Debug statement
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            // Hide trading controls initially
            btnStart.Visible = false;
            btnStop.Visible = false;
            btnLogout.Visible = false;
            dataGridViewTrades.Visible = false;
            Panel1.Visible = false;
            lblStatus.Visible = false;
            btnSettings.Visible = false;
        }

        #region Login and Logout Methods
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (username == "admin" && password == "password")
            {
                MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Hide login controls
                txtUsername.Visible = false;
                txtPassword.Visible = false;
                lblUsername.Visible = false;
                lblPassword.Visible = false;
                btnLogin.Visible = false;

                // Show trading controls
                btnStart.Visible = true;
                btnStop.Visible = true;
                btnLogout.Visible = true;
                dataGridViewTrades.Visible = true;
                Panel1.Visible = true;
                lblStatus.Visible = true;
                btnSettings.Visible = true;
            }
            else
            {
                MessageBox.Show("Invalid credentials", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            // Stop trading if active
            if (_marketDataTimer.Enabled)
            {
                _marketDataTimer.Stop();
            }

            // Disconnect from DAS
            if (Settings.GlobalIsConnected && Settings.GlobalCmdSocket != null)
            {
                Settings.DisconnectGlobal();  // This handles the global state
                UpdateSettingsLabel("Disconnected");
            }

            // Show login controls
            txtUsername.Visible = true;
            txtPassword.Visible = true;
            lblUsername.Visible = true;
            lblPassword.Visible = true;
            btnLogin.Visible = true;

            // Hide trading controls
            btnStart.Visible = false;
            btnStop.Visible = false;
            btnLogout.Visible = false;
            Panel1.Visible = false;
            lblStatus.Visible = false;
            btnSettings.Visible = false;
            dataGridViewTrades.Visible = false;

            // Clear credentials
            txtUsername.Text = string.Empty;
            txtPassword.Text = string.Empty;

            MessageBox.Show("Logged out successfully", "Logout", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }



        #endregion

        #region Trading Controls
        private void InitializeTimer()
        {
            _marketDataTimer = new System.Windows.Forms.Timer
            {
                Interval = 1000 // 1 second
            };
            _marketDataTimer.Tick += MarketDataTimer_Tick;
        }

        private async void MarketDataTimer_Tick(object sender, EventArgs e)
        {
            foreach (var symbol in _watchlist)
            {
                await _tradingEngine.ProcessMarketData(symbol);
            }
        }

        private void TradingEngine_OnTradesUpdated(object sender, List<Trade> trades)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateTradeList(trades)));
                return;
            }
            UpdateTradeList(trades);
        }

        private void UpdateTradeList(List<Trade> trades)
        {
            dataGridViewTrades.DataSource = null;
            dataGridViewTrades.DataSource = trades;

            // Customize column headers
            dataGridViewTrades.Columns["Symbol"].HeaderText = "Symbol";
            dataGridViewTrades.Columns["EntryPrice"].HeaderText = "Entry Price";
            dataGridViewTrades.Columns["ExitPrice"].HeaderText = "Exit Price";
            dataGridViewTrades.Columns["Profit"].HeaderText = "Profit";
            dataGridViewTrades.Columns["Status"].HeaderText = "Status";
            dataGridViewTrades.Columns["ExitDate"].HeaderText = "Exit Date";
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            if (Settings.GlobalIsConnected)
            {
                _marketDataTimer.Start();
                double currentBP = await Settings.GlobalCmdSocket.GetBuyingPowerAsync();
                UpdateSettingsLabel($"Connected - Trading Active - BP: ${currentBP:N2}");
                MessageBox.Show("Trading started!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Please connect to DAS first!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _marketDataTimer.Stop();
            if (Settings.GlobalIsConnected)
            {
                UpdateSettingsLabel($"Connected - Trading Stopped");
            }
            else
            {
                UpdateSettingsLabel("Disconnected");
            }
            MessageBox.Show("Trading stopped!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void btnSettings_Click(object sender, EventArgs e)
        {
            using (var settingsForm = new Settings())
            {
                settingsForm.Owner = this;

                if (Settings.GlobalIsConnected && Settings.GlobalCmdSocket != null)
                {
                    // Ensure the BP Monitor is running
                    StartBPMonitor();

                    double currentBP = await Settings.GlobalCmdSocket.GetBuyingPowerAsync();
                    UpdateSettingsLabel($"Connected - BP: ${currentBP:N2}");
                }

                settingsForm.ShowDialog();
            }

            // Refresh connection status after returning from settings
            await RefreshConnectionStatus();
        }







        public async Task RefreshConnectionStatus()
        {
            if (Settings.GlobalIsConnected && Settings.GlobalCmdSocket != null)
            {
                double currentBP = await Settings.GlobalCmdSocket.GetBuyingPowerAsync();
                lblStatus.Text = $"Connected - BP: ${currentBP:N2}";
                lblStatus.BackColor = Color.Lime;
            }
            else
            {
                lblStatus.Text = "Disconnected";
                lblStatus.BackColor = Color.Red;
            }
        }


        public void UpdateSettingsLabel(string statusText)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateSettingsLabel(statusText)));
                return;
            }

            lblStatus.Text = statusText;
            lblStatus.BackColor = statusText.Contains("Connected") ? Color.Lime : Color.Red;
            lblStatus.ForeColor = Color.Black;
        }

        #endregion



    }


}
