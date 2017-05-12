using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            //large = l;
            //var aStringBuilder = new StringBuilder(s);
            //aStringBuilder.Remove(41, 3).Insert(41, "1920");
            large = Regex.Replace(s, @"thumb-350", "thumb-1920");
        }
    }

}
