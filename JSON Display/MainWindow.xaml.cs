using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

using JSON_Display.Controls;
using JSON_Display.Models;
using JSON_Display.Operation;
using JSON_Enumerate.Implementation;
using JSONTreeView;
using Newtonsoft.Json.Linq;


namespace JSON_Display
{

    public partial class MainWindow : Window
    {
        public MainVM VM { get; set; }

        public MainWindow()
        {

            (AppDomain.CurrentDomain).UnhandledException += (s, e) =>
                LogUnhandledException(e.ExceptionObject as Exception, nameof(AppDomain.CurrentDomain));

            if (Dispatcher != null)
                Dispatcher.UnhandledException += (s, e) =>
                {
                    LogUnhandledException(e.Exception, nameof(Dispatcher));
                    e.Handled = true;
                };

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                LogUnhandledException(e.Exception, nameof(TaskScheduler));
                e.SetObserved();
            };

            InitializeComponent();

            DataContext = VM = new MainVM();

            LoadCommands();

        }

        private static void LogUnhandledException(Exception exception, string v)
        {
            MessageBox.Show(exception.Demystify().ToString(), $"JSON Framework App ({v})");
            Application.Current.Shutdown();
            //   Log.Fatal(exception, v);
        }

        private void LoadCommands()
        {
            VM.FileSaveCmd = new OperationCommand((_) =>
            {

                TabItem ti = tbMain.SelectedItem as TabItem;

                if (ti.Tag == null) return;

                var items = ti.Tag.ToString().Split('$');
                var dialog = new Microsoft.Win32.SaveFileDialog();
                dialog.FileName = string.IsNullOrWhiteSpace(VM.CSharp.Name) ? "Document": VM.CSharp.Name; // Default file name
                dialog.DefaultExt = items[0]; // Default file extension
                dialog.Filter = items[1]; // Filter files by extension
                
                // Show open file dialog box
                bool? result = dialog.ShowDialog();

                // Process open file dialog box results
                if (result == true)
                {
                    // Open document
                    string filename = dialog.FileName;


                    switch (ti.Header)
                    {
                        case "Tree"       :
                        case "Text"       : File.WriteAllText(dialog.FileName, VM.JSONText); break;
                        case "C#"         : File.WriteAllText(dialog.FileName, VM.CSharpText); break;
                        case "SQL Server" : File.WriteAllText(dialog.FileName, VM.SQLTableText); break;
                        case "Table Type" : File.WriteAllText(dialog.FileName, VM.SQLTableTypeText); break;
                        case "SQL Merge"  : File.WriteAllText(dialog.FileName, VM.SQLMergeText); break;
                        case "Blazorise"  : File.WriteAllText(dialog.FileName, VM.BlazoriseText); break;
                    };
                }


            });

            VM.ReloadFromTextCmd = new OperationCommand((_) =>
            {
                if (string.IsNullOrWhiteSpace(VM.JSONText)) return;

                VM.ClearAll(true); // True tells it to not touch the, most likely, changed json text.

                ShowJSON(VM.JSONText, "Root", true);
            });

            VM.ErrorCopyToClipboard = new OperationCommand((_) =>
            {
                if (!string.IsNullOrEmpty(VM.Error))
                    Clipboard.SetText(VM.Error);
            });

            VM.JSONLoadInternal = new OperationCommand(o =>
                ShowJSON(
                    "[{\"EngagementId\":666,\"FacilityNumber\":\"FacNumber1\",\"FacilityName\":\"FacName1\",\"CostCenterNumber\":\"CostCenterNumber1\",\"CostCenterName\":\"CostCenterName1\",\"UOSDescription\":\"UOSDESC #1\",\"BudgetedVolumeForUOS\":1.0000,\"BudgetedTotalWorkedFTEs\":2.0000,\"BudgetedTotalPaidFTEs\":3.0000,\"WHPUOS\":5.1000,\"Year\":\"2020-01-01\"},{\"EngagementId\":666,\"FacilityNumber\":\"FacNumber3\",\"FacilityName\":\"FacName3\",\"CostCenterNumber\":\"CostCenterNumber3\",\"CostCenterName\":\"CostCenterName3\",\"UOSDescription\":\"UOSDESC #2\",\"BudgetedVolumeForUOS\":5.0000,\"BudgetedTotalWorkedFTEs\":6.0000,\"BudgetedTotalPaidFTEs\":7.0000,\"WHPUOS\":8.9000,\"Year\":\"2020-01-01\"}]"

                        //@"[{""BatchId"":0,""AccessionChanges"":[{""LabId"":8675309,""InstanceChanges"":[{""Property"":""Note"",""ChangedTo"":""Jabberwocky"",""UniqueId"":null,""SummaryInstance"":null},{""Property"":""Instrument"",""ChangedTo"":""instrumented"",""UniqueId"":null,""SummaryInstance"":null}],""DetailChanges"":[{""Property"":""Comments"",""ChangedTo"":""2nd Comment"",""UniqueId"":null,""SummaryInstance"":null},{""Property"":""CCC"",""ChangedTo"":""XR71"",""UniqueId"":null,""SummaryInstance"":null}]}]}]"
                        , "Static Data"));

            VM.ClearResults = new OperationCommand(o => ClearAll());

            VM.JSONLoadFromClipboard = new OperationCommand(o =>
            ShowJSON(Clipboard.ContainsText()
                ? Clipboard.GetText(TextDataFormat.Text)
                : string.Empty,
                "Clipboard"));

            //    VM.JSONLoadFromDatabase = new OperationCommand(LoadFromDatabaseOperation);


        }

        private static void LoadFromDatabaseOperation(object _)
        {
            var lfd = new LoadFromDatabaseDialog();

            lfd.ShowDialog();

        }

        private void ClearAll()
        {
            tView.ClearTree();
            VM.ClearAll();
        }

        private void ShowJSON(string text, string source = "Root", bool skipJsonText = false)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(text)) return;

                tView.ClearTree();

                SettingsSingleton.Settings = VM.CSharp;

                var (IsValid, ValidJsonDoc, ErrorMessageDoc) = text.ParseJson();

                if (!IsValid)
                {
                    VM.JSONText = text;

                    tView.ProcessJson(ErrorMessageDoc, false, source);

                    return;
                }

                var overrideCommand = new OperationCommand(o => MessageBox.Show("Override"));

                // If we are sorting, C# will sort it, so process that first.
                VM.CSharpText = ValidJsonDoc.ToCSharpClassesString(VM.CSharp, overrideCommand);

                if (string.IsNullOrEmpty(VM.CSharp.Name))
                    VM.CSharp.Name = VM.CSharp.ClassName;

                VM.SQLTableText = ValidJsonDoc.ToSqlTableString(VM.CSharp, null);

                VM.SQLTableTypeText = ValidJsonDoc.ToSqlTableTypeString(VM.CSharp);

                VM.BlazoriseText = ValidJsonDoc.ToBlazoriseDataGridString(VM.CSharp);

                VM.ComponentText = ValidJsonDoc.ToComponentString(VM.CSharp);

                VM.SQLMergeText = ValidJsonDoc.ToSQLMergeString(VM.CSharp);

                tView.ProcessJson(ValidJsonDoc, false, source);

                tView.OpenFirstItem(true);

                if (!skipJsonText)
                    VM.JSONText = (tView.Tag as JsonDocument).ToFormattedJsonString();
            }
            catch (System.ApplicationException sax) when (sax.Message.StartsWith("Invalid JSON snippet received, must start with object or array of objects."))
            {
                VM.Error = $"Parsing Error{Environment.NewLine}Json must start as an object or an array.{Environment.NewLine} {text[..100]}{Environment.NewLine}";
            }
            catch (Exception ex)
            {
                VM.Error = ex.Demystify().ToString();
            }

        }

        private void MouseWheelScroll(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers != ModifierKeys.Control)
                return;
            var delta = (e.Delta > 0) ? 2 : -2;
            var change = VM.MainFontSize + delta;
            if (change < 8)
                change = 8;
            VM.MainFontSize = change;
        }

        private void DNDEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop) ||
    sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void DragFeedback(object sender, GiveFeedbackEventArgs e)
        {
            base.OnGiveFeedback(e);
            Mouse.SetCursor(Cursors.Cross);
        }

        private void DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                string[] droppedFilePaths = e.Data.GetData(DataFormats.FileDrop, true) as string[];

                foreach (var file in droppedFilePaths)
                {
                    var mru = new MRU(file);

                    if (mru.IsValid)
                    {
                        try
                        {
                            VM.ClearAll();

                            this.Title = mru.Name;

                            ShowJSON(File.ReadAllText(mru.Address));

                            VM.AddMRUToUserSettings(mru);
                        }
                        catch (Exception ex)
                        {
                            VM.Error = ex.Demystify().ToString();
                        }
                    }
                    else
                    {
                        VM.Error = $"File {file} is not a valid JSON file";
                    }

                    break; // Only one file for now.
                }
            }
        }

        private void SelectMRU(object sender, RoutedEventArgs e)
        {
            try
            {
                var mru = (e.OriginalSource as MenuItem).DataContext as MRU;

                if (mru is not null)
                {
                    VM.ClearAll();

                    this.Title = mru.Name;

                    ShowJSON(File.ReadAllText(mru.Address));
                }
            }
            catch (Exception ex)
            {
                VM.Error = ex.Demystify().ToString();
            }
        }

        private bool IsValidSelection(TreeViewItemEx node)
        {
            if (node == null)
            {
                MessageBox.Show("Please highlight a node to process.");
                return false;
            }

            if (!node.HasItems)
            {
                MessageBox.Show($"{node.Header.ToString()} does not have any items.");
                return false;
            }

            return true;
        }
        private void CopyChildrenToClipboard(object sender, RoutedEventArgs e)
        {
            var node = tView.SelectedItem as TreeViewItemEx;

            if (!IsValidSelection(node)) return;

            var lst = string.Join(Environment.NewLine, node.Items.OfType<TreeViewItemEx>().Select(nd => nd.Header.ToString()));

            //MessageBox.Show(lst);

            Clipboard.SetText(lst);

        }



        private void ChildrenToMarkdownTable(object sender, RoutedEventArgs e)
        {
            var n2 = tView.SelectedItem as TreeViewItem;

            if ((n2 is null) || (!n2.GetType().IsAssignableFrom(typeof(TreeViewItem))))
            {
                MessageBox.Show("Need an array parent");
                return; 
            }

            var items = n2.Items.Cast<TreeViewItemEx>().ToList();

            var result = items.Select(arrItem => arrItem.Items.OfType<TreeViewItemEx>()
                                                              .Select(itm => Regex.Match(itm.Header.ToString(), @"(?<Header>[^\s]+)[:\s\x22\\]+(?<Value>[^\x22]+)"))
                                                              .Select(mt => new { Header = mt.Groups["Header"].Value, Value = mt.Groups["Value"].Value })
                                                              .ToList()
                                   );

            var sb = new StringBuilder();
            // Generate Headers

            var individualHeaders = result.First().Select(itm => itm.Header.ToString()).ToList();
            var header = string.Join($" | ", individualHeaders);
            sb.AppendLine($"| {header} |");
            var split = string.Join($" | ", individualHeaders.Select(itm => " - "));
            sb.AppendLine($"| {split} |");

            result.ToList().ForEach(itm => sb.AppendLine("| " + string.Join(" | ", itm.Select(i2 => i2.Value)) + " |")) ;

            var md = sb.ToString();
            MessageBox.Show(md);

            Clipboard.SetText(md);
        }
    }

    public class StaticResourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value == DependencyProperty.UnsetValue || value == null
                ? DependencyProperty.UnsetValue
                : Application.Current.MainWindow.Resources[value];


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException("INTERNAL WPF GUI ISSUE");

    }

}
