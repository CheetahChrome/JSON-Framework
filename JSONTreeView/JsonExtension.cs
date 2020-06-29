using JSON_Enumerate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Windows.Controls;
using JSON_Enumerate.Implementation;

namespace JSONTreeView
{
    public static class JsonExtension
    {
        public static TreeViewItem ProcessJson(this JsonElement je, TreeViewItem root, bool showQuote = false)
        {

            switch (je.ValueKind)
            {
                case JsonValueKind.Object:
                    root.BookEnd("{", "}", () =>
                    {
                        je.EnumerateObject().Select(jElement => new TreeViewItemEx(jElement, showQuote))
                            .ToList()
                            .ForEach(tvi => root.Items.Add(tvi));
                    });
                    break;

                case JsonValueKind.Array:
                    var arrItems = je.EnumerateArray();

                    if (arrItems.Any())
                    {
                        arrItems.Select((jElement, index) => new TreeViewItemEx(jElement, index, showQuote))
                            .ToList()
                            .ForEach(tvi => root.Items.Add(tvi));
                    }
                    break;

                case JsonValueKind.Number:
                case JsonValueKind.String:
                   root.Header += $" : {je.GetRawText()}";
                   break;

                case JsonValueKind.Null:
                    root.Header += $" : null";
                    break;

                case JsonValueKind.True:
                    root.Header += $" : true";
                    break;

                case JsonValueKind.False:
                    root.Header += $" : false";
                    break;

                case JsonValueKind.Undefined:
                    root.Header += $" : -undefined-";
                    break;
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

        public static string ToCSharpClassesString(this JsonDocument doc, string topClass = "Top")
        {

            return doc.RootElement.WalkStructure<CSharpClass, CSharpProperty>
                (new CSharpClass(topClass), null).ToString();

        }

    }
}
