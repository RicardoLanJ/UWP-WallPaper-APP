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
using Windows.UI.Core;
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
        private string title = "发现";

        public start()
        {
            this.InitializeComponent();


            //if (MainPage.theWallPapers != null)
            //{
            //    theWallPapers = MainPage.theWallPapers;
            //    page = MainPage.page;
            //}
            //else
            //{
            //    theWallPapers = new ObservableCollection<theWallPaper>();
            //    MainPage.theWallPapers = theWallPapers;
            //    init();
            //}
            //theWallPapers = new ObservableCollection<theWallPaper>();
            //init();
        }

        private async Task init()
        {
            MainProgressRing.IsActive = true;
            await addwallpaper();
            MainProgressRing.IsActive = false;
        }

        private async Task addwallpaper()
        {
            var crawler = new Utils.Crawler();
            await Task.Run(() => crawler.grabHtml(website + page.ToString()));
            crawler.parser(theWallPapers);
            page++;
            MainPage.page = page;
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
                
                SV.ViewChanged -= OnScrollViewerViewChanged;
                SecondProgressRing.IsActive = true;
                await addwallpaper();
                SecondProgressRing.IsActive = false;
                SV.ViewChanged += OnScrollViewerViewChanged;
            }
            else
            {
                
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            
        }

        private theWallPaper selectedItem;
        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            selectedItem = (theWallPaper)e.ClickedItem;
            Frame.Navigate(typeof(Views.detail), selectedItem);
        }

        private async Task init2()
        {
            MainProgressRing.IsActive = true;
            await addwallpaper2();
            MainProgressRing.IsActive = false;
        }

        private async Task addwallpaper2()
        {
            var crawler = new Utils.Crawler();
            await Task.Run(() => crawler.grabHtml(website));
            crawler.parser(theWallPapers);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter != null)
            {
                var paras = (KeyValuePair<string, string>)e.Parameter;
                title = paras.Key;
                website = paras.Value;
                theWallPapers = new ObservableCollection<theWallPaper>();
                init2();
                Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += (s, a) =>
                {
                    if (Frame.CanGoBack)
                    {
                        Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                        //Frame.GoBack();
                        Frame.Navigate(typeof(Views.search));
                        a.Handled = true;
                    }
                };
            }
            else
            {
                if (MainPage.theWallPapers != null)
                {
                    theWallPapers = MainPage.theWallPapers;
                    page = MainPage.page;
                }
                else
                {
                    theWallPapers = new ObservableCollection<theWallPaper>();
                    MainPage.theWallPapers = theWallPapers;
                    init();
                }
            }
        }

        private void myRightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            //GridView gridView = (GridView)sender;
            //MenuFlyout.ShowAt(gridView, e.GetPosition(gridView));
            FrameworkElement senderElement = sender as FrameworkElement;
            var MF = (MenuFlyout)FlyoutBase.GetAttachedFlyout(senderElement);

            MF.ShowAt(senderElement, e.GetPosition(senderElement));
        }

        private void downloadandset_Click(object sender, RoutedEventArgs e)
        {
            theWallPaper a = (theWallPaper)((FrameworkElement)e.OriginalSource).DataContext;

        }

        private void download_Click(object sender, RoutedEventArgs e)
        {

        }
    }

}
