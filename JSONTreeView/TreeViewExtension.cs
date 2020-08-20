using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Windows.Controls;

namespace JSONTreeView
{
    public static class TreeViewExtension
    {

        public static TreeView ProcessJson(this TreeView tv, JsonDocument doc, bool showQuote = false)
        {
            if (doc != null)
            {
                if (tv.Tag == null)
                    tv.Tag = doc;

                tv.Items.Add(doc.RootElement.ProcessJson(new TreeViewItem() { Header = "Root" }, showQuote));
            }

            return tv;
        }


        public static TreeView ProcessJson(this TreeView tv, string json, bool showQuote = false)
        {
            if (tv == null)
                return null;

            var parsedJSON = json.ParseJson();

            if (parsedJSON.IsValid)
            {
                tv.Tag = parsedJSON.ValidJsonDoc;
                tv.ProcessJson(parsedJSON.ValidJsonDoc, showQuote);
            }
            else
            {
                tv.ProcessJson(parsedJSON.ErrorMessageDoc, showQuote);
            }

            return tv;
        }

	}
}
