using System;
using System.Windows.Controls;

namespace EthernetFrameApp.Pages
{
    /// <summary>
    /// Interaktionslogik für SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        private static AppConfig Config;

        public SettingsPage()
        {
            Config = new AppConfig();

            InitializeComponent();

            g_pauseDelay.IsEnabled = !Config.StopStep;
        }

        private void cb_StopStep_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Properties.Settings.Default.Save();
            g_pauseDelay.IsEnabled = (Properties.Settings.Default["StopStep"] as Nullable<bool>) == false ? true : false;
        }

        private void bt_apply_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Properties.Settings.Default.Save();
        }
    }
}
