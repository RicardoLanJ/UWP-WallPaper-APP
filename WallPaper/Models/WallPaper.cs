using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WallPaper.Models
{
    public class theWallPaper
    {
        public string name { get; set; }
        public Thumbnail thumbnail { get; set; }
        public theWallPaper(string name_, Thumbnail thumbnail_)
        {
            name = name_;
            thumbnail = thumbnail_;
        }
    }

    public class Thumbnail
    {
        public string small { get; set; }
        public string large { get; set; }
        public Thumbnail(string s, string l)
        {
            small = s;
            large = l;
        }
    }

}
