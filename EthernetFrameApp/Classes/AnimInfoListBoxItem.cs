using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace EthernetFrameApp.Classes
{
    public class AnimInfoListBoxItem : INotifyPropertyChanged
    {
        private Visibility contentVisible;
        private string header;
        private int openTrigger;
        private string contentText;

        public Visibility ContentVisible
        {
            get { return contentVisible; }
            set
            {
                if (contentVisible != value)
                {
                    contentVisible = value;
                    NotifyPropertyChanged();
                }
            }
        }
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
        public int OpenTrigger
        {
            get { return openTrigger; }
            set { openTrigger = value; }
        }
        public string ContentText
        {
            get { return contentText; }
            set { contentText = value; }
        }


        public AnimInfoListBoxItem()
        {
            ContentVisible = Visibility.Collapsed;
            OpenTrigger = -1;
        }

        public void Open()
        {
            ContentVisible = Visibility.Visible;
        }
        public void Close()
        {
            ContentVisible = Visibility.Collapsed;
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
