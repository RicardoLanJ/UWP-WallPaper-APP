﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;


//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace WallPaper
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            contentFrame.Navigate(typeof(Views.start));
            initTitlebar();
            fuck();
        }

        private void initTitlebar()
        {
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            var color = (Color)this.Resources["SystemAccentColor"];
            var Brightness = (int)Math.Sqrt(
                color.R * color.R * .299 +
                color.G * color.G * .587 +
                color.B * color.B * .114);
            //var foreColor = Brightness > 130 ? Colors.Black : Colors.White;
            var foreColor = Colors.White;

            titleBar.BackgroundColor = color;
            titleBar.ForegroundColor = foreColor;

            titleBar.ButtonBackgroundColor = color;
            titleBar.ButtonForegroundColor = foreColor;

            titleBar.ButtonHoverBackgroundColor = Color.FromArgb(color.A, (byte)(color.R + 10), (byte)(color.G + 10), (byte)(color.B + 10));
            titleBar.ButtonHoverForegroundColor = foreColor;

            titleBar.ButtonPressedBackgroundColor = color;
            titleBar.ButtonPressedForegroundColor = foreColor;

            titleBar.ButtonInactiveBackgroundColor = color;
            titleBar.ButtonInactiveForegroundColor = foreColor;

            titleBar.InactiveBackgroundColor = Color.FromArgb(color.A, (byte)(color.R - 10), (byte)(color.G - 10), (byte)(color.B - 10));
            titleBar.InactiveForegroundColor = foreColor;
        }

        private async void fuck()
        {
            BitmapImage bitmapImage = new BitmapImage();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    using (var response = await client.GetAsync(new Uri("https://images6.alphacoders.com/607/thumb-350-607185.jpg")))
                    {
                        response.EnsureSuccessStatusCode();

                        using (var stream = await response.Content.ReadAsStreamAsync())
                        {
                            using (var memStream = new MemoryStream())
                            {
                                await stream.CopyToAsync(memStream);
                                memStream.Position = 0;

                                bitmapImage.SetSource(memStream.AsRandomAccessStream());
                            }
                        }
                    }
                }
                //test.Source = bitmapImage;
                //test.Source = new BitmapImage(new Uri("https://images6.alphacoders.com/607/thumb-350-607185.jpg"));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Failed to load the image: {0}", ex.Message);
            }
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }
    }

}