using System.Windows.Forms;

namespace Auto_Trader_Platform
{
    public static class SharedUtils
    {
        public static void UpdateConnectionStatus(string status, double buyingPower)
        {
            if (Application.OpenForms["Form1"] is Form1 mainForm)
            {
                mainForm.UpdateSettingsLabel($"{status} - BP: ${buyingPower:N2}");
            }

            if (Application.OpenForms["Settings"] is Settings settingsForm)
            {
                settingsForm.LblBuyingPower.Text = $"Buying Power: ${buyingPower:N2}";
                settingsForm.btnConnect.Text = status.Contains("Connected") ? "Disconnect" : "Connect";
            }
        }


    }
}
