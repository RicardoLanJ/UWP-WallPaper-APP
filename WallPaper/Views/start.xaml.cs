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
        private string website = "https://wall.alphacoders.com/by_category.php?id=3";

        public start()
        {
            this.InitializeComponent();
            // LoadWallpaperAsync(theWallPapers);
            theWallPapers = new ObservableCollection<theWallPaper>();
            init();
        }

        private async void init()
        {
            var crawler = new Utils.Crawler();
            await crawler.grabHtml(website);
            crawler.parser(theWallPapers);
        }

    }

}
