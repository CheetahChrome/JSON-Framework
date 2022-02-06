using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Windows.Controls;

namespace JSONTreeView
{
    public class TreeViewItemEx : TreeViewItem
    {

        public bool ShowQuote { get; set; }

        public TreeViewItemEx(JsonProperty jp, bool showQuote = true)
        {
            var jElement = jp.Value;
            ShowQuote = showQuote;

            base.ToolTip = jElement.ValueKind.ToString();
            var key = GetKeyForHeader(jp, jElement);

            switch (jElement.ValueKind)
            {
                case JsonValueKind.Number:
                case JsonValueKind.String:
                    base.Header = $"{key} : {jElement.GetRawText()}";
                    break;

                case JsonValueKind.Null:
                    base.Header = $"{key} : null";
                    break;

                case JsonValueKind.Object:
                    base.Header = key;
                    jElement.ProcessJson(this);
                    break;

                case JsonValueKind.Array:
                    base.Header = key;
                    base.ToolTip += $" ({jElement.GetArrayLength()})";

                    jElement.ProcessJson(this);
                    break;


                case JsonValueKind.True:
                    base.Header += $"{key} : true";
                    break;

                case JsonValueKind.False:
                    base.Header += $"{key} : false";
                    break;

                case JsonValueKind.Undefined:
                    base.Header += $"{key} : -undefined-";
                    break;


                default:
                    base.Header = key;
                    break;
            }
        }


        public TreeViewItemEx(JsonElement jsub, int arrayIndex, bool showQuote = true)
        {
            this.Header = $"[ {arrayIndex} ]";

            jsub.ProcessJson(this);

        }

        public void BookEnd(string start, string end, Action operation)
        {
            this.Items.Add(new TreeViewItem() { Header = start });
            operation.Invoke();
            this.Items.Add(new TreeViewItem() { Header = end });
        }

        public string GetKeyForHeader(JsonProperty jp, JsonElement je)
        {
            bool addQuote = ShowQuote && (je.ValueKind == JsonValueKind.String || je.ValueKind == JsonValueKind.Number || je.ValueKind == JsonValueKind.Null);

            return addQuote ? $"\x22{jp.Name}\x22" : $"{jp.Name}";

        }

    }

}
