using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WallPaper.Models;

namespace WallPaper.Utils
{
    class Crawler
    {
        public HtmlDocument htmlDoc = new HtmlDocument();
        public enum sortby  {newest, favorites, views, downloads};
      
        private string postDataStr = "view=paged&min_resolution=0x0&resolution_equals=%3E%3D&sort=" + "favorites";

        public async Task grabHtml(string website)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(website);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            Stream myRequestStream = await request.GetRequestStreamAsync();
            byte[] bs = Encoding.ASCII.GetBytes(postDataStr);
            myRequestStream.Write(bs, 0, bs.Length);

            WebResponse response = await request.GetResponseAsync();
            Stream stream = response.GetResponseStream();
            var result = "";
            using (StreamReader sr = new StreamReader(stream))
            {
                result = sr.ReadToEnd();
            }
            //HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(result);
        }

        public void parser(ObservableCollection<theWallPaper> wallpapers)
        {
            HtmlNode rootnode = htmlDoc.DocumentNode;
            //string xpathstring = "//div[@class='boxgrid']/a/img";
            //HtmlNodeCollection aa = rootnode.SelectNodes(xpathstring);  
            List<HtmlNode> images = rootnode.Descendants().Where
                (x => (x.Name == "div" && x.Attributes["class"] != null && x.Attributes["class"].Value.Contains("boxgrid"))).ToList();
            List<string> uris = new List<string>();
            foreach (var image in images)
            {
                var temp = image.Descendants("a").ToList()[0].Descendants("img").ToList()[0].GetAttributeValue("src", null);
                uris.Add(temp);
            }
            int i = 0;
        }

        //public Task<theWallPaper>
    }


}
