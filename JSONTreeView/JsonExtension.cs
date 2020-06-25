﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Windows.Controls;

namespace JSONTreeView
{
    public static class JsonExtension
    {
        public static TreeViewItem ProcessJson(this JsonElement je, TreeViewItem root, bool showQuote = false)
        {

            if (je.ValueKind == JsonValueKind.Object)
            {
                root.BookEnd("{", "}", () =>
                {
                    je.EnumerateObject().Select(jElement => new TreeViewItemEx(jElement, showQuote))
                        .ToList()
                        .ForEach(tvi => root.Items.Add(tvi));
                });
            }
            else
            {
                var arrItems = je.EnumerateArray();

                if (arrItems.Any())
                {
                    arrItems.Select((jElement, index) => new TreeViewItemEx(jElement, index, showQuote))
                        .ToList()
                        .ForEach(tvi => root.Items.Add(tvi));
                }
            }

            return root;
        }


        public static void BookEnd(this TreeViewItem root, string start, string end, Action operation)
        {
            root.Items.Add(new TreeViewItem() { Header = start });
            operation.Invoke();
            root.Items.Add(new TreeViewItem() { Header = end });
        }


        public static string ToFormattedJsonString(this JsonDocument doc)
            => JsonSerializer.Serialize(doc, new JsonSerializerOptions() {WriteIndented = true});

    }
}