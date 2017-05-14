using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System.UserProfile;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Web.Http;

namespace WallPaper.Utils
{
    class manager
    {
        public async Task download(string url, int tf)
        {
            string filename = url.Substring(url.Length - 10, 10);

            StorageFolder fold = Windows.Storage.ApplicationData.Current.LocalFolder;
            
            var item = await ApplicationData.Current.LocalFolder.TryGetItemAsync(filename);
            if (item == null)
            {
                StorageFile file = await fold.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
                HttpClient client = new HttpClient();

                var buffer = await client.GetBufferAsync(new Uri(url));
                await Windows.Storage.FileIO.WriteBufferAsync(file, buffer);
                if (tf == 1)
                {
                    manager.setWallpaper(filename);
                }
            }

            
        }

        public async Task<List<BitmapImage>> getAllImage()
        {
            List<BitmapImage> res = new List<BitmapImage>();
            StorageFolder fold = Windows.Storage.ApplicationData.Current.LocalFolder;
            IReadOnlyList<StorageFile> all = await fold.GetFilesAsync();
            foreach (var f in all)
            {
                BitmapImage r = await LoadImage(f);
                res.Add(r);
            }
            return res;
        }

        public static async Task<BitmapImage> LoadImage(StorageFile file)
        {
            BitmapImage bitmapImage = new BitmapImage();
            FileRandomAccessStream stream = (FileRandomAccessStream)await file.OpenAsync(FileAccessMode.Read);

            bitmapImage.SetSource(stream);

            return bitmapImage;

        }

        public static async Task<bool> setWallpaper(string filename)
        {
            StorageFolder fold = Windows.Storage.ApplicationData.Current.LocalFolder;
            var file = await fold.GetFileAsync(filename);
            UserProfilePersonalizationSettings profileSettings = UserProfilePersonalizationSettings.Current;
            //bool success = await profileSettings.TrySetLockScreenImageAsync(file);
            bool success = await profileSettings.TrySetWallpaperImageAsync(file);
            
            var timer = new Windows.UI.Xaml.DispatcherTimer { Interval = TimeSpan.FromSeconds(0.5) };
            timer.Tick += (sender, args) =>
            {
                MainPage.initTitlebar();
                timer.Stop();
            };

            timer.Start();
            return success;
        }

        public static async Task deleteFile(string filename)
        {
            StorageFolder fold = Windows.Storage.ApplicationData.Current.LocalFolder;
            var file = await fold.GetFileAsync(filename);
            await file.DeleteAsync();
        }
    }
}
