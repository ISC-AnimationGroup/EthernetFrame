using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EthernetFrameApp
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Control.IsTabStopProperty.OverrideMetadata(typeof(Control), new FrameworkPropertyMetadata(false));
        }
    }

    public class AppConfig : INotifyPropertyChanged
    {
        public struct ThemeColors
        {
            public SolidColorBrush
                DarkPrimary,
                Primary,
                LightPrimary,
                TextIcons,
                Accent,
                PrimaryText,
                SecondaryText,
                Divider;

            public ThemeColors(SolidColorBrush darkPrimary, SolidColorBrush primary, SolidColorBrush lightPrimary, SolidColorBrush textIcons,
                SolidColorBrush accent, SolidColorBrush primaryText, SolidColorBrush secondaryText, SolidColorBrush divider)
            {
                DarkPrimary = darkPrimary;
                Primary = primary;
                LightPrimary = lightPrimary;
                TextIcons = textIcons;
                Accent = accent;
                PrimaryText = primaryText;
                SecondaryText = secondaryText;
                Divider = divider;
            }
        }

        private static ThemeColors Theme1 = new ThemeColors(
            (SolidColorBrush)(new BrushConverter().ConvertFrom("#303F9F")),
            (SolidColorBrush)(new BrushConverter().ConvertFrom("#3F51B5")),
            (SolidColorBrush)(new BrushConverter().ConvertFrom("#C5CAE9")),
            (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFF")),
            (SolidColorBrush)(new BrushConverter().ConvertFrom("#03A9F4")),
            (SolidColorBrush)(new BrushConverter().ConvertFrom("#212121")),
            (SolidColorBrush)(new BrushConverter().ConvertFrom("#727272")),
            (SolidColorBrush)(new BrushConverter().ConvertFrom("#B6B6B6")));

        private ThemeColors appColors;
        private Visibility sideVisible = Visibility.Visible;
        private double headerHeight = 113;
        private double sideWidth = 200;
        private bool stopStep;
        private int pauseDelay = 0;

        public ThemeColors AppColors {
            get
            {
                return appColors;
            }
            set
            {
                appColors = value;
                NotifyPropertyChanged("AppColors");
            }
        }
        public Visibility SideVisible
        {
            get { return sideVisible; }
            set
            {
                sideVisible = value;
                NotifyPropertyChanged("SideVisible");
            }
        }
        public double HeaderHeight
        {
            get { return headerHeight; }
            set
            {
                headerHeight = value;
                NotifyPropertyChanged("HeaderHeight");
            }
        }
        public double SideWidth
        {
            get { return sideWidth; }
            set
            {
                sideWidth = value;
                NotifyPropertyChanged("SideWidth");
            }
        }
        public bool StopStep
        {
            get { return stopStep; }
            set
            {
                stopStep = value;
                NotifyPropertyChanged("StopStep");
            }
        }
        public int PauseDelay
        {
            get { return pauseDelay; }
            set
            {
                pauseDelay = value;
                NotifyPropertyChanged("PauseDelay");
            }
        }

        public AppConfig()
        {
            AppColors = Theme1;

            Nullable<bool> showSide = (Properties.Settings.Default["SideVisible"] as Nullable<bool>);
            if (showSide == false)
            {
                SideVisible = Visibility.Collapsed;
                HeaderHeight = 0;
                SideWidth = 0;
            }

            StopStep = (Properties.Settings.Default["StopStep"] as Nullable<bool>) == false ? false : true;
            if (!StopStep)
            {
                PauseDelay = (Properties.Settings.Default["PauseDelay"] as Nullable<int>) ?? 5;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}


