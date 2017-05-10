using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using WallPaper.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace WallPaper.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class start : Page
    {

        public ObservableCollection<theWallPaper> theWallPapers { get; set; }
        public List<string> UriCache { get; set; }
        private string website = "https://wall.alphacoders.com/by_category.php?id=3&page="; //stringbulider
        private int page = 1;

        public start()
        {
            this.InitializeComponent();
           
            
            if (MainPage.theWallPapers != null)
            {
                theWallPapers = MainPage.theWallPapers;
            } else
            {
                theWallPapers = new ObservableCollection<theWallPaper>();
                MainPage.theWallPapers = theWallPapers;
                init();
            }
         
        }

        private async void init()
        {
            MainProgressRing.IsActive = true;
            await addwallpaper();
            MainProgressRing.IsActive = false;
        }

        private async Task addwallpaper()
        {
            var crawler = new Utils.Crawler();
            await crawler.grabHtml(website + page.ToString());
            crawler.parser(theWallPapers);
            page++;
        }

        private void makeWp(int from, int to)
        {
            foreach (var u in UriCache.Skip(from).Take(to))
            {
                theWallPapers.Add(new theWallPaper("test", new Thumbnail(u, u)));
            }
        }

        private async void OnScrollViewerViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var verticalOffset = SV.VerticalOffset;
            var maxVerticalOffset = SV.ScrollableHeight; //sv.ExtentHeight - sv.ViewportHeight;

            if (maxVerticalOffset < 0 ||
                verticalOffset == maxVerticalOffset)
            {
                SecondProgressRing.IsActive = true;
                await addwallpaper();
                SecondProgressRing.IsActive = false;
            }
            else
            {
                
            }
        }

    }

}
