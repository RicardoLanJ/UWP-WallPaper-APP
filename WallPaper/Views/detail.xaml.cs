using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using WallPaper.Models;
using WallPaper.Utils;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Composition;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace WallPaper.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class detail : Page
    {
        private theWallPaper wp;

        public detail()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            wp = (theWallPaper)e.Parameter;

            im.Source = new BitmapImage(new Uri(wp.thumbnail.small));
            await im.Blur(duration: 0, delay: 0, value: 10).StartAsync();

            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += (s, a) =>
            {
                if (Frame.CanGoBack)
                {
                    //Frame.GoBack();
                    Frame.Navigate(typeof(Views.start));
                    a.Handled = true;
                }
            };

            var bitmap = new BitmapImage(new Uri(wp.thumbnail.large));
            force.Source = bitmap;
            bitmap.ImageOpened += (s, t) =>
            {
                im.Source = bitmap;
                im.Blur(duration: 2000, delay: 0, value: 0).StartAsync();
            };


        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        private async void downloadandsetclick(object sender, RoutedEventArgs e)
        {
            var mg = new manager();
            await mg.download(wp.thumbnail.large, 1);
        }

        private async void downloadclick(object sender, RoutedEventArgs e)
        {
            var mg = new manager();
            await mg.download(wp.thumbnail.large, 0);
        }
    }
}

                //<interactivity:Interaction.Behaviors>
                //    <behaviors:Blur x:Name="blur" Value="10" Duration="10" Delay="10" AutomaticallyStart="true"/>
                //</interactivity:Interaction.Behaviors>