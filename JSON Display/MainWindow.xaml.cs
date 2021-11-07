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
using JSON_Display.Controls;
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

        private void LogUnhandledException(Exception exception, string v)
        {
            MessageBox.Show(exception.Demystify().ToString(), "JSON Framework App");
            Application.Current.Shutdown();
            //   Log.Fatal(exception, v);
        }

        private void LoadCommands()
        {
            VM.JSONLoadInternal = new OperationCommand(o =>
                ShowJSON(
                    "[{\"EngagementId\":666,\"FacilityNumber\":\"FacNumber1\",\"FacilityName\":\"FacName1\",\"CostCenterNumber\":\"CostCenterNumber1\",\"CostCenterName\":\"CostCenterName1\",\"UOSDescription\":\"UOSDESC #1\",\"BudgetedVolumeForUOS\":1.0000,\"BudgetedTotalWorkedFTEs\":2.0000,\"BudgetedTotalPaidFTEs\":3.0000,\"WHPUOS\":5.1000,\"Year\":\"2020-01-01\"},{\"EngagementId\":666,\"FacilityNumber\":\"FacNumber3\",\"FacilityName\":\"FacName3\",\"CostCenterNumber\":\"CostCenterNumber3\",\"CostCenterName\":\"CostCenterName3\",\"UOSDescription\":\"UOSDESC #2\",\"BudgetedVolumeForUOS\":5.0000,\"BudgetedTotalWorkedFTEs\":6.0000,\"BudgetedTotalPaidFTEs\":7.0000,\"WHPUOS\":8.9000,\"Year\":\"2020-01-01\"}]"

                    //@"[{""BatchId"":0,""AccessionChanges"":[{""LabId"":8675309,""InstanceChanges"":[{""Property"":""Note"",""ChangedTo"":""Jabberwocky"",""UniqueId"":null,""SummaryInstance"":null},{""Property"":""Instrument"",""ChangedTo"":""instrumented"",""UniqueId"":null,""SummaryInstance"":null}],""DetailChanges"":[{""Property"":""Comments"",""ChangedTo"":""2nd Comment"",""UniqueId"":null,""SummaryInstance"":null},{""Property"":""CCC"",""ChangedTo"":""XR71"",""UniqueId"":null,""SummaryInstance"":null}]}]}]"
                        , "Static Data"));

            //                ShowJSON(@"{
            //    ""Pre"": null,
            //    ""Text"": ""1"",
            //    ""iPage"": 1,
            //    ""Post"": null,
            //    ""IsValid"" : true,
            //    ""Uri"": ""http://Testing.org/2019.html"",
            //    ""PageCount"": 1,
            //    ""TopicTitle"": ""???"",
            //    ""PageLinks"": [""http://www.test.org"", ""http://www.test2.org"", ""http://www.test3.org""],
            //    ""WebPages"": [{
            //        ""Link"": ""http://www.Alpha.org/institute.html"",
            //        ""URI"": ""http://www.Beta.org/t986132.html"",
            //        ""Infos"": [],
            //        ""IsCached"": true,
            //        ""PageHTML"": ""Cleared"",
            //        ""PageNumber"": ""1""
            //    }]
            //}"));

            VM.ClearResults = new OperationCommand(o => ClearAll());

            VM.JSONLoadFromClipboard = new OperationCommand(o =>
            ShowJSON(Clipboard.ContainsText()
                ? Clipboard.GetText(TextDataFormat.Text)
                : string.Empty,
                "Clipboard"));

        //    VM.JSONLoadFromDatabase = new OperationCommand(LoadFromDatabaseOperation);
        }

        private void LoadFromDatabaseOperation(object ignored)
        {
            var lfd = new LoadFromDatabaseDialog();

            lfd.ShowDialog();

        }

        private void ClearAll()
        {
            tView.ClearTree();
            VM.ClearAll();
        }

        private void ShowJSON(string text, string source = "Root")
        {
            if (string.IsNullOrWhiteSpace(text)) return;

            tView.ClearTree();

            SettingsSingleton.Settings = VM.CSharp;

            var parseResult = text.ParseJson();

            if (!parseResult.IsValid)
            {
                VM.JSONText = text;

                tView.ProcessJson(parseResult.ErrorMessageDoc, false, source);

                return;
            }

            var overrideCommand = new OperationCommand(o => MessageBox.Show("Override"));

            // If we are sorting, C# will sort it, so process that first.
            VM.CSharpText = parseResult.ValidJsonDoc.ToCSharpClassesString(VM.CSharp, overrideCommand);

            VM.SQLTableText = parseResult.ValidJsonDoc.ToSqlTableString(VM.CSharp, null);

            VM.SQLTableTypeText = parseResult.ValidJsonDoc.ToSqlTableTypeString(VM.CSharp);

            tView.ProcessJson(parseResult.ValidJsonDoc, false, source);

            tView.OpenFirstItem(true);

            VM.JSONText = (tView.Tag as JsonDocument).ToFormattedJsonString();


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
                    
                    var name = System.IO.Path.GetFileName(file);

                    this.Title = name;

                    ShowJSON(File.ReadAllText(file));

                }
            }
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
