using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml.Linq;
using WallPaper.Models;
using WallPaper.Utils;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;



//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace WallPaper
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static ObservableCollection<theWallPaper> theWallPapers { get; set; }
        public static int page = 1;

        public MainPage()
        {
            this.InitializeComponent();
            contentFrame.Navigate(typeof(Views.start));
            initTitlebar();
            LiveTitle();
        }

        private void LiveTitle()
        {
            XDocument xdoc = XDocument.Load("LiveTitle.xml");
            Windows.Data.Xml.Dom.XmlDocument doc = new Windows.Data.Xml.Dom.XmlDocument();
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.Clear();
            updater.EnableNotificationQueue(true);

            doc.LoadXml(xdoc.ToString());
            TileNotification notification = new TileNotification(doc);
            updater.Update(notification);

            for (int i = 0; i < 5; i++)
            {
                foreach (XElement xe in xdoc.Descendants("binding"))
                {
                    foreach (XElement xxe in xdoc.Descendants("image"))
                    {
                        xxe.SetAttributeValue("src", "Assets/SplashScreen.scale-100.png");
                    }
                }
                doc.LoadXml(xdoc.ToString());
                notification = new TileNotification(doc);
                updater.Update(notification);
            }

            //await ChangeTitle();
        }

        private async Task ChangeTitle()
        {
            for (int i = 0; i < 5; i++)
            {
                Uri uri = new Uri("https://source.unsplash.com/random/");
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        HttpResponseMessage response = await client.GetAsync(uri);
                        if (response != null)
                        {
                            string filename = "title" + i.ToString() + ".jpg";
                            var imageFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
                            using (IRandomAccessStream stream = await imageFile.OpenAsync(FileAccessMode.ReadWrite))
                            {
                                await response.Content.WriteToStreamAsync(stream);
                            }
                            StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(filename);
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        public static void initTitlebar()
        {
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            
            var color = (Color)Application.Current.Resources["SystemAccentColor"];
            
            var Brightness = (int)Math.Sqrt(
                color.R * color.R * .299 +
                color.G * color.G * .587 +
                color.B * color.B * .114);
            //var foreColor = Brightness > 130 ? Colors.Black : Colors.White;
            var foreColor = Colors.White;

            var lc = colorConvert.Colorlightener(color);
            var dc = colorConvert.ColorDarker(color);

            titleBar.BackgroundColor = color;
            titleBar.ForegroundColor = foreColor;

            titleBar.ButtonBackgroundColor = color;
            titleBar.ButtonForegroundColor = foreColor;

            titleBar.ButtonHoverBackgroundColor = lc;//Color.FromArgb(color.A, (byte)(color.R + 10), (byte)(color.G + 10), (byte)(color.B + 10));
            titleBar.ButtonHoverForegroundColor = foreColor;

            titleBar.ButtonPressedBackgroundColor = lc;
            titleBar.ButtonPressedForegroundColor = foreColor;

            titleBar.ButtonInactiveBackgroundColor = dc;
            titleBar.ButtonInactiveForegroundColor = foreColor;

            titleBar.InactiveBackgroundColor = dc;//Color.FromArgb(color.A, (byte)(color.R - 10), (byte)(color.G - 10), (byte)(color.B - 10));
            titleBar.InactiveForegroundColor = foreColor;

        }


        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void Option1_Click(object sender, RoutedEventArgs e)
        {
            if (contentFrame.GetType() != typeof(Views.start))
            {
                contentFrame.Navigate(typeof(Views.start));
            }
        }

        private void Option3_Click(object sender, RoutedEventArgs e)
        {
            if (contentFrame.GetType() != typeof(Views.search))
            {
               // MainPage.initTitlebar();
                contentFrame.Navigate(typeof(Views.search));
            }
        }

        private void Option2_Click(object sender, RoutedEventArgs e)
        {
            if (contentFrame.GetType() != typeof(Views.local))
            {
                contentFrame.Navigate(typeof(Views.local));
            }
        }
    }

}
