using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Windows.Controls;

namespace JSONTreeView
{
    public static class TreeViewExtension
    {

        /// <summary>
        /// Removes the tree items and clears the `Tag` property. Does not nullify the tree.
        /// </summary>
        /// <param name="tv">Tree being used.</param>
        /// <returns>Returns the tree passed in.</returns>
        public static TreeView ClearTree(this TreeView tv)
        {
            if (tv != null)
            {
                tv.Tag = null;
                tv.Items.Clear();
            }

            return tv;
        }

        /// <summary>
        /// Open the first item of the tree and give an option to open the children.
        /// </summary>
        /// <param name="tv">Tree being used.</param>
        /// <param name="openChildren">Defaults to false. True opens them up too.</param>
        public static void OpenFirstItem(this TreeView tv, bool openChildren = false)
        {
            if (tv?.Items[0] is TreeViewItem tvi)
            {
                tvi.IsExpanded = true;

                if (openChildren && (tvi.Items.OfType<TreeViewItem>()?.Any() ?? false))
                    foreach (TreeViewItem itm in tvi.Items)
                        itm.IsExpanded = true;
            }
        }

        public static TreeView ProcessJson(this TreeView tv, JsonDocument doc, bool showQuote = false, string name = "Root")
        {
            if (doc != null)
            {
                if (tv.Tag == null)
                    tv.Tag = doc;

                tv.Items.Add(doc.RootElement.ProcessJson(new TreeViewItem() { Header = name }, showQuote));
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
