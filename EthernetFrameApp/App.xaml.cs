using System.Windows;
using System.Windows.Media;

namespace EthernetFrameApp
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
    }

    public static class AppConfig
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

        private static ThemeColors appColors = Theme1;
        public static ThemeColors AppColors {
            get
            {
                return appColors;
            }
        }
    }
}


