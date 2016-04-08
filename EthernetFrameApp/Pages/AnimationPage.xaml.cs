using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EthernetFrameApp.Classes;
using System.Collections.ObjectModel;
using System.Xml;

namespace EthernetFrameApp.Pages
{
    /// <summary>
    /// Interaktionslogik für Case1Page.xaml
    /// </summary>
    public partial class AnimationPage : Page
    {
        private ObservableCollection<AnimInfoListBoxItem> animInfoList = new ObservableCollection<AnimInfoListBoxItem>();

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


        private static List<string> AnimationPaths = new List<string>()
        {
            "1.mp4",
            "2.mp4",
            "3.mp4"
        };

        public AnimationPage(int animationCase)
        {
            InitializeComponent();
            this.DataContext = this;

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
                // xml not found / not access
            }

            // TODO: fix
            mediaElement.BeginInit();
            mediaElement.Source = new Uri(string.Format("Animations/videoplayback{0}.mp4", animationCase), UriKind.Relative);
            mediaElement.EndInit();

            if (AnimInfoList.Count > 0)
            {

            }
            mediaElement.Play();
            animationIsPlaying = true;
        }

        private void lv_animInfos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in AnimInfoList)
            {
                item.ContentVisible = Visibility.Collapsed;
            }

            ListView lv = (sender as ListView);
            AnimInfoListBoxItem selectedItem = (lv.SelectedItem as AnimInfoListBoxItem);
            if (selectedItem.ContentVisible == Visibility.Collapsed)
            {
                selectedItem.ContentVisible = Visibility.Visible;
            }
            else
            {
                selectedItem.ContentVisible = Visibility.Collapsed;
            }
            
        }

        private bool animationIsPlaying = false;

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
