using System;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading.Tasks;
using daslibrary;
using Newtonsoft.Json;
using System.Xml;
using System.Diagnostics;
using System.Drawing;

namespace Auto_Trader_Platform
{
    public partial class Settings : Form
    {
        private readonly CMDSocket _cmdSocket;
        private bool _isConnected = false;
        private SettingsData _settings;
        private BPMonitor _bpMonitor;

        public static bool GlobalIsConnected { get; set; }
        public static CMDSocket GlobalCmdSocket { get; set; }

        public class SettingsData
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string AccountNumber { get; set; }
            public int Port { get; set; }
            public string Route { get; set; }
            public double InvestmentAmount { get; set; }
        }

        public Settings()
        {
            InitializeComponent();
            _cmdSocket = GlobalCmdSocket ?? new CMDSocket();
            LblBuyingPower.Text = "Buying Power: $0.00";
            LoadSavedSettings();

            if (GlobalIsConnected && GlobalCmdSocket != null && GlobalCmdSocket.loginsuccess)
            {
                _isConnected = true;
                btnConnect.Text = "Disconnect";
                StartPositionMonitoring();
            }
        }

        private async Task<double> GetBuyingPower(CMDSocket socket)
        {
            return await socket.GetBuyingPowerAsync();
        }

        private void LoadSavedSettings()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");
            _settings = LoadSettings(filePath);
            txtInvestmentAmount.Text = _settings.InvestmentAmount.ToString();
            txtPort.Text = _settings.Port.ToString();
            txtUsername.Text = _settings.Username;
            txtPassword.Text = _settings.Password;
            txtAccountNumber.Text = _settings.AccountNumber;
            txtRoute.Text = _settings.Route;
        }

        public static SettingsData LoadSettings(string filename)
        {
            if (File.Exists(filename))
            {
                string json = File.ReadAllText(filename);
                return JsonConvert.DeserializeObject<SettingsData>(json);
            }
            return new SettingsData();
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            if (!_isConnected)
            {
                if (ValidateInputs())
                {
                    try
                    {
                        _cmdSocket.Connect("127.0.0.1", int.Parse(txtPort.Text));
                        bool loginSuccess = await Task.Run(() => _cmdSocket.Login(
                            txtUsername.Text.Trim(),
                            txtPassword.Text.Trim(),
                            txtAccountNumber.Text.Trim()
                        ));

                        if (loginSuccess)
                        {
                            GlobalIsConnected = true;
                            GlobalCmdSocket = _cmdSocket;
                            double currentBP = await GetBuyingPower(_cmdSocket);
                            _isConnected = true;
                            btnConnect.Text = "Disconnect";
                            LblBuyingPower.Text = $"Buying Power: ${currentBP:N2}";

                            StartPositionMonitoring();

                            if (Owner is Form1 mainForm)
                            {
                                mainForm.StartBPMonitor();
                                mainForm.UpdateSettingsLabel($"Connected - BP: ${currentBP:N2}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Connection error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                DisconnectFromBroker();
            }
        }

        private void StartPositionMonitoring()
        {
            _bpMonitor = new BPMonitor(GlobalCmdSocket, UpdateBuyingPower);
            _bpMonitor.Start();

            System.Windows.Forms.Timer bpTimer = new System.Windows.Forms.Timer
            {
                Interval = 30000 // 30 seconds
            };

            bpTimer.Tick += async (s, e) =>
            {
                if (GlobalIsConnected && GlobalCmdSocket.loginsuccess)
                {
                    double currentBP = await GetBuyingPower(GlobalCmdSocket);
                    LblBuyingPower.Text = $"Buying Power: ${currentBP:N2}";
                    if (Owner is Form1 mainForm)
                    {
                        mainForm.UpdateSettingsLabel($"Connected - BP: ${currentBP:N2}");
                    }
                }
                else
                {
                    bpTimer.Stop();
                    _bpMonitor?.Stop();
                }
            };
            bpTimer.Start();
        }

        private void UpdateBuyingPower(double buyingPower)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateBuyingPower(buyingPower)));
                return;
            }

            LblBuyingPower.Text = $"Buying Power: ${buyingPower:N2}";
            if (Owner is Form1 mainForm)
            {
                mainForm.UpdateSettingsLabel($"Connected - BP: ${buyingPower:N2}");
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtAccountNumber.Text) ||
                string.IsNullOrWhiteSpace(txtRoute.Text))
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!int.TryParse(txtPort.Text, out int port) || port <= 0)
            {
                MessageBox.Show("Please enter a valid port number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!double.TryParse(txtInvestmentAmount.Text, out double amount) || amount <= 0)
            {
                MessageBox.Show("Please enter a valid investment amount.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void DisconnectFromBroker()
        {
            _bpMonitor?.Stop();
            if (_cmdSocket != null && _cmdSocket.loginsuccess)
            {
                _cmdSocket.Disconnect();
            }

            GlobalIsConnected = false;
            GlobalCmdSocket = null;
            _isConnected = false;
            btnConnect.Text = "Connect";
            LblBuyingPower.Text = "Buying Power: $0.00";

            if (Owner is Form1 mainForm)
            {
                mainForm.UpdateSettingsLabel("Disconnected");
            }
        }

        private async void btnBack_Click(object sender, EventArgs e)
        {
            try
            {
                if (Owner is Form1 mainForm)
                {
                    if (GlobalIsConnected && GlobalCmdSocket != null)
                    {
                        double currentBP = await GlobalCmdSocket.GetBuyingPowerAsync();
                        mainForm.UpdateSettingsLabel($"Connected - BP: ${currentBP:N2}");
                        mainForm.StartBPMonitor();
                    }
                    mainForm.Show();
                }
                this.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in btnBack_Click: {ex.Message}");
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void Settings_Load(object sender, EventArgs e)
        {
            LoadSavedSettings();
            if (Settings.GlobalIsConnected && Settings.GlobalCmdSocket != null && Settings.GlobalCmdSocket.loginsuccess)
            {
                _isConnected = true;
                btnConnect.Text = "Disconnect";
                double currentBP = await Settings.GlobalCmdSocket.GetBuyingPowerAsync();
                LblBuyingPower.Text = $"Buying Power: ${currentBP:N2}";
                StartPositionMonitoring();
            }
            else
            {
                _isConnected = false;
                btnConnect.Text = "Connect";
                LblBuyingPower.Text = "Buying Power: $0.00";
            }

            if (this.Owner is Form1 mainForm)
            {
                mainForm.Hide();
            }
        }

        private void btnUpdateSettings_Click(object sender, EventArgs e)
        {
            try
            {
                var settings = new SettingsData
                {
                    InvestmentAmount = double.Parse(txtInvestmentAmount.Text.Trim()),
                    Port = int.Parse(txtPort.Text.Trim()),
                    Username = txtUsername.Text.Trim(),
                    Password = txtPassword.Text.Trim(),
                    AccountNumber = txtAccountNumber.Text.Trim(),
                    Route = txtRoute.Text.Trim()
                };

                string json = JsonConvert.SerializeObject(settings, Newtonsoft.Json.Formatting.Indented);
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");
                File.WriteAllText(filePath, json);

                DisconnectFromBroker();
                btnConnect.Text = "Connect";
                btnUpdateSettings.Text = "Settings Updated";

                var timer = new System.Windows.Forms.Timer { Interval = 3000 };
                timer.Tick += (s, args) =>
                {
                    btnUpdateSettings.Text = "Update Settings";
                    timer.Stop();
                    timer.Dispose();
                };
                timer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_isConnected)
            {
                DisconnectFromBroker();
            }
            base.OnFormClosing(e);
        }

        public static void DisconnectGlobal()
        {
            if (GlobalCmdSocket != null && GlobalCmdSocket.loginsuccess)
            {
                GlobalCmdSocket.Disconnect();
                GlobalIsConnected = false;
                SharedUtils.UpdateConnectionStatus("Disconnected", 0.00);
            }
        }

        private void txtStrategy_TextChanged(object sender, EventArgs e)
        {

        }

    }
}