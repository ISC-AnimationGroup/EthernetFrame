using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        private void bt_close_MouseUp(object sender, MouseButtonEventArgs e)
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
                    break;
                default:
                    break;
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
    }
}
