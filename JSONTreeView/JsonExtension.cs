using JSON_Enumerate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Windows.Controls;
using JSON_Enumerate.Implementation;
using JSON_Enumerate.Operation;

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
                    root.Header += " : null";
                    break;

                case JsonValueKind.True:
                    root.Header += " : true";
                    break;

                case JsonValueKind.False:
                    root.Header += " : false";
                    break;

                case JsonValueKind.Undefined:
                    root.Header += " : -undefined-";
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

        public static string ToCSharpClassesString(this JsonDocument doc, IJsonSettings settings, System.Windows.Input.ICommand overrideProperty = null)
            => doc.RootElement.WalkStructure<CSharpClass, CSharpProperty>
                (new CSharpClass(settings, overrideProperty), null).ToString();

        public static string ToSqlTableString(this JsonDocument doc, IJsonSettings settings, System.Windows.Input.ICommand overrideProperty = null)
            => doc.RootElement.WalkStructure<SQLTable, SQLProperty>
                (new SQLTable(settings, overrideProperty), null).ToString();

        public static string ToSqlTableTypeString(this JsonDocument doc, IJsonSettings settings, System.Windows.Input.ICommand overrideProperty = null)
            => doc.RootElement.WalkStructure<SqlTableType, SQLProperty>
                (new SqlTableType(settings, overrideProperty), null).ToString();

        public static string ToBlazoriseDataGridString(this JsonDocument doc, IJsonSettings settings, System.Windows.Input.ICommand overrideProperty = null)
            => doc.RootElement.WalkStructure<BlazorizeDataGrid, BlazoriseDataGridProperty>
                (new BlazorizeDataGrid(settings, overrideProperty), null).ToString();

        /// <summary>
        /// Take a JSON string and parse it into a JsonDocument.
        /// </summary>
        /// <remarks>Invalid or failed json returns the `errorMessage` json document</remarks>
        /// <param name="json">Valid JSON</param>
        /// <returns></returns>
        public static (bool IsValid, JsonDocument ValidJsonDoc, JsonDocument ErrorMessageDoc) ParseJson(this string json)
        {
            JsonDocument errorMessage = null;
            JsonDocument result = null;
            var isValid = false;
            var errorJSON = string.Empty;

            try
            {
                result = JsonDocument.Parse(json);
                isValid = (result != null);

                if (isValid)
                    errorJSON = $@"{{ ""Error"" : ""No JSON Doc Returned from Parse."", ""Exception"" : ""-None-"" }}";
            }
            catch (Exception ex)
            {
                errorJSON = $@"{{ ""Error"" : ""{ex.Message}"", ""Exception"" : ""{ex.GetType()}"" }}";
            }

            if (!string.IsNullOrEmpty(errorJSON))
                errorMessage = JsonDocument.Parse(errorJSON);

            return (isValid, result, errorMessage);
        }
    }
}
