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

namespace EthernetFrameApp.Pages
{
    /// <summary>
    /// Interaktionslogik für AnimationPage.xaml
    /// </summary>
    public partial class AnimationPage : Page
    {
        private static List<List<string>> Animations = new List<List<string>>()
        {
            // { Path, PageTitle }
            new List<string> { "Animation1.wmv", "Lorem Ipsum 1" },
            new List<string> { "Animation2.wmv", "Lorem Ipsum 2" },
            new List<string> { "Animation1.mp4", "Lorem Ipsum 3" },
            new List<string> { "Animation2.mp4", "Lorem Ipsum 4" }
        };
        private Timer OpenNextInfoTimer = new Timer();
        private int CurrentInfoItem;
        private ObservableCollection<AnimInfoListBoxItem> animInfoList = new ObservableCollection<AnimInfoListBoxItem>();
        private bool animationIsPlaying = false;

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

        /// <summary>
        /// WPF-Page which contains Mediaplayer for animation playback.
        /// </summary>
        /// <param name="animationCase">Defines ID of Animation (beginning with 1)</param>
        public AnimationPage(int animationCase)
        {
            InitializeComponent();
            PageTitle = Animations[animationCase - 1][1];
            this.DataContext = this;

            animationIsPlaying = false;
            OpenNextInfoTimer.Elapsed += new ElapsedEventHandler(OpenNextInfoTimerHandler);

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

                Debug.WriteLine(string.Format("MediaElement.BeginInit Timestamp: {0}.{1}", DateTime.Now, DateTime.Now.Millisecond));
                mediaElement.BeginInit();
                // mediaElement.Source = new Uri("C:\\Users\\schneima4\\Documents\\Visual Studio 2015\\Projects\\EthernetFrameApp\\EthernetFrameApp\\Animations\\videoplayback1.mp4", UriKind.Absolute);
                Uri mediaSource = new Uri(string.Format("Animations\\{0}", Animations[animationCase - 1][0]), UriKind.Relative);
                mediaElement.Source = mediaSource;
                mediaElement.EndInit();
                mediaElement.Play();
                animationIsPlaying = true;
            }
            catch (Exception e)
            {
                // Animation file not found / no access
                MessageBox.Show(e.Message);
            }
        }

        private void MediaLoadedHandler(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(string.Format("MediaElement.MediaLoaded Timestamp: {0}.{1}", DateTime.Now, DateTime.Now.Millisecond));
            if (AnimInfoList.Count > 0)
            {
                // Initialize and run Timers to show Info about Animation
                AnimInfoList[0].Open();
                CurrentInfoItem = 0;
                OpenNextInfoTimer.Interval = AnimInfoList[1].OpenTrigger.TotalMilliseconds;
                OpenNextInfoTimer.Start();
            }
        }

        private void MediaFailedHandler(object sender, ExceptionRoutedEventArgs e)
        {
            MessageBox.Show("Die Animation kann nicht wiedergegeben werden, da der benötigte Video-Codec fehlt (MP4)." + Environment.NewLine + e.ErrorException.Message);
        }

        private void OpenNextInfoTimerHandler(object sender, ElapsedEventArgs e)
        {
            // TODO: fix here
            //Debug.WriteLine(string.Format("mediaElement.Position = {0}ms # OpenTrigger = {1}ms", mediaElement.Position.TotalMilliseconds, AnimInfoList[CurrentInfoItem].OpenTrigger.TotalMilliseconds));

            OpenNextInfoTimer.Stop();
            CurrentInfoItem++;
            foreach (var item in AnimInfoList)
            {
                item.Close();
            }
            AnimInfoList[CurrentInfoItem].Open();
            if (CurrentInfoItem < AnimInfoList.Count)
            {
                OpenNextInfoTimer.Interval = AnimInfoList[CurrentInfoItem + 1].OpenTrigger.TotalMilliseconds - AnimInfoList[CurrentInfoItem].OpenTrigger.TotalMilliseconds;
                OpenNextInfoTimer.Start();
            }
        }

        private void lv_animInfos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OpenNextInfoTimer.Stop();

            foreach (var item in AnimInfoList)
            {
                item.Close();
            }

            ListView lv = (sender as ListView);
            AnimInfoListBoxItem selectedItem = (lv.SelectedItem as AnimInfoListBoxItem);
            if (selectedItem.ContentVisible == Visibility.Collapsed)
            {
                selectedItem.Open();
                mediaElement.Pause();
                mediaElement.Position = selectedItem.OpenTrigger;
                mediaElement.Play();
                CurrentInfoItem = AnimInfoList.IndexOf(selectedItem);
                if (CurrentInfoItem < AnimInfoList.Count - 1)
                {
                    OpenNextInfoTimer.Interval = AnimInfoList[CurrentInfoItem + 1].OpenTrigger.TotalMilliseconds - AnimInfoList[CurrentInfoItem].OpenTrigger.TotalMilliseconds;
                    OpenNextInfoTimer.Start();
                }
            }
            else
            {
                // Hint: Is actually never been called since SelectionChanged won't fire by another Click.
                selectedItem.Close();
            }
            
        }

        //private MediaState GetMediaState(MediaElement myMedia)
        //{
        //    FieldInfo hlp = typeof(MediaElement).GetField("_helper", BindingFlags.NonPublic | BindingFlags.Instance);
        //    object helperObject = hlp.GetValue(myMedia);
        //    FieldInfo stateField = helperObject.GetType().GetField("_currentState", BindingFlags.NonPublic | BindingFlags.Instance);
        //    MediaState state = (MediaState)stateField.GetValue(helperObject);
        //    return state;
        //}

        public void Page_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    break;
                case Key.Space:
                case Key.MediaPlayPause:
                    if (animationIsPlaying)
                    {
                        if (mediaElement.CanPause)
                        {
                            OpenNextInfoTimer.Stop();
                            mediaElement.Pause();
                            animationIsPlaying = false;
                        }
                    }
                    else
                    {
                        OpenNextInfoTimer.Interval = (AnimInfoList[CurrentInfoItem + 1].OpenTrigger - mediaElement.Position).TotalMilliseconds;
                        OpenNextInfoTimer.Start();
                        mediaElement.Play();
                        animationIsPlaying = true;
                    }
                    break;
                case Key.Left:
                case Key.Up:
                    break;
                case Key.Right:
                case Key.Down:
                    break;
                case Key.F11:
                    break;
                case Key.MediaNextTrack:
                    break;
                case Key.MediaPreviousTrack:
                    break;
                case Key.MediaStop:
                    break;
                default:
                    break;
            }
        }

    }
}
