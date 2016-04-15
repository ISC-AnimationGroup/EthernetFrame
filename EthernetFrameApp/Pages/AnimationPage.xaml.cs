using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EthernetFrameApp.Classes;
using System.Collections.ObjectModel;
using System.Xml;
using System.Timers;
using System.Reflection;
using System.Diagnostics;
using System.ComponentModel;

namespace EthernetFrameApp.Pages
{
    /// <summary>
    /// Interaktionslogik für AnimationPage.xaml
    /// </summary>
    public partial class AnimationPage : Page, INotifyPropertyChanged
    {
        private static AppConfig Config;
        private static List<List<string>> Animations = new List<List<string>>()
        {
            // { Path, PageTitle }
            new List<string> { "Animation1.mp4", "Ethernet Frame" },
            new List<string> { "Animation2.mp4", "MAC Learning" },
            new List<string> { "Animation3.mp4", "Portbased VLAN" },
            new List<string> { "Animation4.mp4", "Tagged VLAN" }
        };
        private Timer openNextInfoTimer = new Timer();
        private Timer stepPauseTimer = new Timer();

        private int currentInfoItem;
        private ObservableCollection<AnimInfoListBoxItem> animInfoList = new ObservableCollection<AnimInfoListBoxItem>();
        private bool animationIsPlaying = false;

        public int CurrentInfoItem
        {
            get
            {
                return currentInfoItem;
            }
            set
            {
                if (currentInfoItem != value)
                {
                    currentInfoItem = value;
                    NotifyPropertyChanged("CurrentInfoItem");
                }
            }
        }
        public ObservableCollection<AnimInfoListBoxItem> AnimInfoList
        {
            get
            {
                return animInfoList;
            }
            set
            {
                if (animInfoList != value)
                {
                    animInfoList = value;
                }
            }
        }
        public string PageTitle { get; set; }
        public Visibility SideVisible { get; private set; }
        public double HeaderHeight { get; private set; }
        public double SideWidth { get; private set; }

        /// <summary>
        /// WPF-Page which contains Mediaplayer for animation playback.
        /// </summary>
        /// <param name="animationCase">Defines ID of Animation (beginning with 1)</param>
        public AnimationPage(int animationCase)
        {
            Config = new AppConfig();
            SideVisible = Config.SideVisible;
            HeaderHeight = Config.HeaderHeight;
            SideWidth = Config.SideWidth;
            PageTitle = Animations[animationCase - 1][1];
            currentInfoItem = 0;

            InitializeComponent();
            this.DataContext = this;

            animationIsPlaying = false;
            openNextInfoTimer.Elapsed += new ElapsedEventHandler(OpenNextInfoTimerHandler);
            stepPauseTimer.Elapsed += new ElapsedEventHandler(stepPauseTimerHandler);

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(string.Format("Strings\\de\\Animation{0}.xml", animationCase));
                XmlElement root = doc.DocumentElement;

                foreach (XmlNode node in root)
                {
                    AnimInfoListBoxItem infoItem = new AnimInfoListBoxItem();
                    TimeSpan trigger;
                    infoItem.Header = node["header"].InnerText;
                    infoItem.ContentText = node["content"].InnerText;
                    if (TimeSpan.TryParse(node.Attributes["trigger"].Value, out trigger))
                    {
                        infoItem.OpenTrigger = trigger;
                    }
                    AnimInfoList.Add(infoItem);
                }
                AnimInfoList[CurrentInfoItem].Open();
            }
            catch (Exception e)
            {
                // xml not found / no access
                MessageBox.Show(e.Message);
            }

            try
            {
                mediaElement.MediaOpened += new RoutedEventHandler(MediaLoadedHandler);
                mediaElement.MediaFailed += MediaFailedHandler;
                mediaElement.MediaEnded += new RoutedEventHandler(MediaEndedHandler);

                Debug.WriteLine(string.Format("MediaElement.BeginInit Timestamp: {0}.{1}", DateTime.Now, DateTime.Now.Millisecond));
                mediaElement.BeginInit();
                // mediaElement.Source = new Uri("C:\\Users\\schneima4\\Documents\\Visual Studio 2015\\Projects\\EthernetFrameApp\\EthernetFrameApp\\Animations\\videoplayback1.mp4", UriKind.Absolute);
                Uri mediaSource = new Uri(string.Format("Animations\\{0}", Animations[animationCase - 1][0]), UriKind.Relative);
                mediaElement.Source = mediaSource;
                mediaElement.EndInit();
                animationIsPlaying = true;
                mediaElement.Play();
            }
            catch (Exception e)
            {
                // Animation file not found / no access
                MessageBox.Show(e.Message);
            }
        }

        private void MediaEndedHandler(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                mediaElement.Stop();
            }));
        }

        private void MediaLoadedHandler(object sender, RoutedEventArgs e)
        {
            // Debug.WriteLine(string.Format("MediaElement.MediaLoaded Timestamp: {0}.{1} # playing = {2}", DateTime.Now, DateTime.Now.Millisecond, animationIsPlaying.ToString()));
            // Pause it in any case
            
            if (animationIsPlaying)
            {
                PlayPause();
            }
            SetToCurrentFrame();
            // Debug.WriteLine(string.Format("MediaLoaded->PlayPause() Timestamp: {0}.{1} # playing = {2}", DateTime.Now, DateTime.Now.Millisecond, animationIsPlaying.ToString()));

            if (Config.PauseDelay > 0)
            {
                // Auto start after PauseDelay
                stepPauseTimer.Interval = Config.PauseDelay * 1000;
                stepPauseTimer.Start();
            }
        }

        private void MediaFailedHandler(object sender, ExceptionRoutedEventArgs e)
        {
            MessageBox.Show("Die Animation kann nicht wiedergegeben werden, da der benötigte Video-Codec fehlt (MP4)." + Environment.NewLine + e.ErrorException.Message);
        }

        private void OpenNextInfoTimerHandler(object sender, ElapsedEventArgs e)
        {
            openNextInfoTimer.Stop();
            NextStep();
        }

        private void stepPauseTimerHandler(object sender, ElapsedEventArgs e)
        {
            stepPauseTimer.Stop();
            PlayPause();
            Debug.WriteLine(string.Format("stepPauseTimerHandler->PlayPause() Timestamp: {0}.{1} # playing = {2}", DateTime.Now, DateTime.Now.Millisecond, animationIsPlaying.ToString()));
        }

        private void lv_animInfos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            openNextInfoTimer.Stop();
            stepPauseTimer.Stop();

            foreach (var item in AnimInfoList)
            {
                item.Close();
            }

            ListView lv = (sender as ListView);
            AnimInfoListBoxItem selectedItem = (lv.SelectedItem as AnimInfoListBoxItem);
            if (selectedItem.ContentVisible == Visibility.Collapsed)
            {
                selectedItem.Open();
                this.Dispatcher.Invoke((Action)(() =>
                {
                    mediaElement.Pause();
                }));
                CurrentInfoItem = AnimInfoList.IndexOf(selectedItem);
                SetToCurrentFrame();

                if (Config.PauseDelay > 0)
                {
                    // Auto start after PauseDelay
                    stepPauseTimer.Interval = Config.PauseDelay * 1000;
                    stepPauseTimer.Start();
                }
            }
        }
        

        private void SetToCurrentFrame()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                mediaElement.Position = AnimInfoList[CurrentInfoItem].OpenTrigger;
                animationIsPlaying = true;
                mediaElement.Play();
            }));
            Timer timeout = new Timer(100);
            timeout.Elapsed += new ElapsedEventHandler(LoadSomeFrameHandler);
            timeout.Start();
        }

        private void LoadSomeFrameHandler(object sender, ElapsedEventArgs e)
        {
            (sender as Timer).Stop();
            this.Dispatcher.Invoke((Action)(() =>
            {
                animationIsPlaying = false;
                mediaElement.Pause();
            }));
        }

        private void PlayPause()
        {
            stepPauseTimer.Stop();
            openNextInfoTimer.Stop();

            if (animationIsPlaying)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    if (mediaElement.CanPause)
                    {
                        animationIsPlaying = false;
                        mediaElement.Pause();
                    }
                }));
            }
            else
            {
                animationIsPlaying = true;
                this.Dispatcher.Invoke((Action)(() =>
                {
                    mediaElement.Play();
                }));

                if (CurrentInfoItem < AnimInfoList.Count - 1)
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        double interval = (AnimInfoList[CurrentInfoItem + 1].OpenTrigger - mediaElement.Position).TotalMilliseconds;
                        openNextInfoTimer.Interval = interval > 0 ? interval : 500;
                    }));
                    openNextInfoTimer.Start();
                }
            }
        }

        private void NextStep()
        {
            PlayPause();

            if (CurrentInfoItem < AnimInfoList.Count - 1)
            {
                CurrentInfoItem++;
                SetToCurrentFrame();
            }
        }

        private void PreviousStep()
        {
            PlayPause();

            if (CurrentInfoItem > 0)
            {
                CurrentInfoItem--;
                SetToCurrentFrame();
            }
        }

        public void Page_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    break;
                case Key.Space:
                case Key.MediaPlayPause:
                    PlayPause();
                    break;
                case Key.MediaPreviousTrack:
                case Key.PageUp:
                //case Key.Up:
                    PreviousStep();
                    break;
                case Key.MediaNextTrack:
                case Key.PageDown:
                //case Key.Down:
                    NextStep();
                    break;
                case Key.MediaStop:
                    break;
                default:
                    break;
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                mediaElement.Stop();
            }));
            openNextInfoTimer.Stop();
            stepPauseTimer.Stop();
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
