using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EthernetFrameApp.Classes;
using System.Collections.ObjectModel;
using System.Xml;

namespace EthernetFrameApp.Pages
{
    /// <summary>
    /// Interaktionslogik für AnimationPage.xaml
    /// </summary>
    public partial class AnimationPage : Page
    {
        private static List<string> AnimationPaths = new List<string>()
        {
            "videoplayback1.mp4",
            "2.mp4",
            "3.mp4",
            "4.mp4"
        };
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

        /// <summary>
        /// WPF-Page which contains Mediaplayer for animation playback.
        /// </summary>
        /// <param name="animationCase">Defines ID of Animation (beginning with 1)</param>
        public AnimationPage(int animationCase)
        {
            InitializeComponent();
            this.DataContext = this;

            animationIsPlaying = false;

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
            catch (Exception)
            {
                // xml not found / no access
            }

            try
            {
                mediaElement.BeginInit();
                mediaElement.Source = new Uri(string.Format("Animations\\{0}", AnimationPaths[animationCase - 1]), UriKind.Relative);
                mediaElement.EndInit();

                if (AnimInfoList.Count > 0)
                {
                    // Initialize and run Timers to show Info about Animation
                }
                mediaElement.Play();
                animationIsPlaying = true;
            }
            catch (Exception)
            {
                // Animation file not found / no access
            }
        }

        private void lv_animInfos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in AnimInfoList)
            {
                item.Close();
            }

            ListView lv = (sender as ListView);
            AnimInfoListBoxItem selectedItem = (lv.SelectedItem as AnimInfoListBoxItem);
            if (selectedItem.ContentVisible == Visibility.Collapsed)
            {
                selectedItem.Open();
                mediaElement.Position = selectedItem.OpenTrigger;
            }
            else
            {
                // Hint: Is actually never been called since SelectionChanged won't fire by another Click.
                selectedItem.Close();
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
                    if (animationIsPlaying)
                    {
                        if (mediaElement.CanPause)
                        {
                            mediaElement.Pause();
                            animationIsPlaying = false;
                        }
                    }
                    else
                    {
                        mediaElement.Play();
                        animationIsPlaying = true;
                    }
                    break;
                case Key.Left:
                    break;
                case Key.Up:
                    break;
                case Key.Right:
                    break;
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
