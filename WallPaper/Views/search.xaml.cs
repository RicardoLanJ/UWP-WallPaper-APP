﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    public sealed partial class search : Page
    {
        private string website = "https://wall.alphacoders.com/sub_categories.php?id=3";
        private List<string> tags;
        public search()
        {
            this.InitializeComponent();
            addTags();
        }

        private async Task addTags()
        {
            var crawler = new Utils.Crawler();
            await Task.Run(() => crawler.grabHtml(website));
            tags  = crawler.parserTag();
        }

        private void asb_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // We only want to get results when it was a user typing, 
            // otherwise we assume the value got filled in by TextMemberPath 
            // or the handler for SuggestionChosen
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                //var matchingContacts = ContactSampleDataSource.GetMatchingContacts(sender.Text);
                List<string> matchs = new List<string>();
                foreach(var tag in tags)
                {
                    if (tag.Contains(sender.Text)) matchs.Add(tag);
                }

                //sender.ItemsSource = matchingContacts.ToList();
                sender.ItemsSource = matchs;
            }
        }

        //private void asb_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        //{
        //    if (args.ChosenSuggestion != null)
        //    {
        //        // User selected an item, take an action on it here
        //        SelectContact((Contact)args.ChosenSuggestion);
        //    }
        //    else
        //    {
        //        // Do a fuzzy search on the query text
        //        var matchingContacts = ContactSampleDataSource.GetMatchingContacts(args.QueryText);

        //        // Choose the first match, or clear the selection if there are no matches.
        //        SelectContact(matchingContacts.FirstOrDefault());
        //    }
        //}


    }
}
