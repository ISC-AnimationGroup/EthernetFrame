using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EthernetFrameApp.Classes
{
    public class AnimInfoListBoxItem : INotifyPropertyChanged
    {
        private static BitmapImage IconDefault = new BitmapImage(new Uri("/EthernetFrameApp;component/Icons/Forward-48-grey_c.png", UriKind.Relative));
        private static BitmapImage IconActive = new BitmapImage(new Uri("/EthernetFrameApp;component/Icons/Forward-48_c.png", UriKind.Relative));

        private string header;
        private TimeSpan openTrigger;
        private string contentText;
        private Visibility contentVisible;
        private BitmapImage icon;
        private SolidColorBrush headerColor;

        // Data
        public string Header
        {
            get { return header; }
            set
            {
                if (header != value)
                {
                    header = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public TimeSpan OpenTrigger
        {
            get { return openTrigger; }
            set { openTrigger = value; }
        }
        public string ContentText
        {
            get { return contentText; }
            set { contentText = value; }
        }
        // Visuals
        public Visibility ContentVisible
        {
            get { return contentVisible; }
            private set
            {
                if (contentVisible != value)
                {
                    contentVisible = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public BitmapImage Icon
        {
            get { return icon; }
            private set
            {
                if (icon != value)
                {
                    icon = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public SolidColorBrush HeaderColor
        {
            get { return headerColor; }
            private set
            {
                if (HeaderColor != value)
                {
                    headerColor = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public AnimInfoListBoxItem()
        {
            ContentVisible = Visibility.Collapsed;
            OpenTrigger = new TimeSpan(0,0,-1);
            HeaderColor = AppConfig.AppColors.SecondaryText;
            Icon = IconDefault;
        }

        public void Open()
        {
            ContentVisible = Visibility.Visible;
            HeaderColor = AppConfig.AppColors.PrimaryText;
            Icon = IconActive;
        }
        public void Close()
        {
            ContentVisible = Visibility.Collapsed;
            HeaderColor = AppConfig.AppColors.SecondaryText;
            Icon = IconDefault;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
