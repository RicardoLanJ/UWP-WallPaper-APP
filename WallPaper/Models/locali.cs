using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace WallPaper.Models
{
    public class locali
    {
        public string name;
        public BitmapImage bitmap;
        public locali(string name_, BitmapImage bitmap_)
        {
            name = name_;
            bitmap = bitmap_;
        }
    }
}
