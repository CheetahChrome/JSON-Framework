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

            try
            {
                var doc = JsonDocument.Parse(json);
                tv.Tag = doc;
                tv.ProcessJson(doc, showQuote);
            }
            catch (Exception ex)
            {
                var errorJSON = $@"{{ ""Error"" : ""{ex.Message}"", ""Exception"" : ""{ex.GetType()}"" }}";

                tv.ProcessJson(JsonDocument.Parse(errorJSON), showQuote);
            }

            return tv;
        }

	}
}
