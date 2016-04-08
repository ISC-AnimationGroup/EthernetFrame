using System.Windows;
using System.Windows.Input;

namespace EthernetFrameApp
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public double LastWidth { get; set; }
        public double LastHeight { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void bt_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F11:
                    if (this.Topmost != true)
                    {
                        LastWidth = this.Width;
                        LastHeight = this.Height;
                        this.Visibility = Visibility.Collapsed;
                        this.WindowState = WindowState.Maximized;
                        this.ResizeMode = ResizeMode.NoResize;
                        this.WindowStyle = WindowStyle.None;
                        this.Topmost = true;
                        this.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        this.WindowState = WindowState.Normal;
                        this.Topmost = false;
                        this.WindowStyle = WindowStyle.SingleBorderWindow;
                        this.ResizeMode = ResizeMode.CanResize;
                        this.Width = LastWidth;
                        this.Height = LastHeight;
                    }
                    break;
                case Key.Escape:
                    if (this.Topmost == true)
                    {
                        this.WindowState = WindowState.Normal;
                        this.Topmost = false;
                        this.WindowStyle = WindowStyle.SingleBorderWindow;
                        this.ResizeMode = ResizeMode.CanResize;
                        this.Width = LastWidth;
                        this.Height = LastHeight;
                    }
                    else
                    {
                        MessageBoxResult result = MessageBox.Show("Anwendung beenden?", "Beenden?", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.Yes)
                        {
                            this.Close();
                        }
                    }
                    break;
                default:
                    break;
            }

            if (frame.Content.GetType() == typeof(Pages.AnimationPage))
            {
                // KeyDown-Event an die im Frame geladene Page weiterleiten
                Pages.AnimationPage page = (frame.Content as Pages.AnimationPage);
                page.Page_KeyDown(sender, e);
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.Topmost != true)
            {
                LastWidth = e.NewSize.Width;
                LastHeight = e.NewSize.Height;
            }
        }

        private void bt_settings_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new Pages.SettingsPage());
        }

        private void bt_anim1_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new Pages.AnimationPage(1));
        }

        private void bt_anim2_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new Pages.AnimationPage(2));
        }

        private void bt_anim3_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new Pages.AnimationPage(3));
        }
    }
}
