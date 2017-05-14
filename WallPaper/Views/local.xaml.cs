using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using WallPaper.Models;
using WallPaper.Utils;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace WallPaper.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class local : Page
    {
        public ObservableCollection<locali> theWallPapers { get; set; }
        //private manager mg = new manager();
        public local()
        {
            this.InitializeComponent();
            theWallPapers = new ObservableCollection<locali>();
            Loaded += OnLoaded;
        }

        private void downloadandset_Click(object sender, RoutedEventArgs e)
        {
            locali a = (locali)((FrameworkElement)e.OriginalSource).DataContext;
            manager.setWallpaper(a.name);
        }

        private void myRightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            var MF = (MenuFlyout)FlyoutBase.GetAttachedFlyout(senderElement);

            MF.ShowAt(senderElement, e.GetPosition(senderElement));
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            //wplist.Items.Clear();

            //var files = await KnownFolders.PicturesLibrary.GetFilesAsync();

            StorageFolder fold = Windows.Storage.ApplicationData.Current.LocalFolder;
            IReadOnlyList<StorageFile> all = await fold.GetFilesAsync();

            foreach (var file in all)
            {
                
                var bitmap = new BitmapImage();

                bitmap = await manager.LoadImage(file);

                //wplist.Items.Add(new locali(file.Name, bitmap));
                theWallPapers.Add(new locali(file.Name, bitmap));

            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            locali a = (locali)((FrameworkElement)e.OriginalSource).DataContext;
            theWallPapers.Remove(a);
            manager.deleteFile(a.name);
        }
    }
}
