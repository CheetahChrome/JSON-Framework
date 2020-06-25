using System;
using System.Collections.Generic;
using System.Linq;
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
using JSON_Display.Operation;
using JSONTreeView;
using Newtonsoft.Json.Linq;

namespace JSON_Display
{

    public partial class MainWindow : Window
    {
        public MainVM VM { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            DataContext = VM = new MainVM();

            LoadCommands();
        }

        private void LoadCommands()
        {
            VM.JSONLoadInternal = new OperationCommand(o =>
            ShowJSON(
                @"[{""BatchId"":0,""AccessionChanges"":[{""LabId"":8675309,""InstanceChanges"":[{""Property"":""Note"",""ChangedTo"":""Jabberwocky"",""UniqueId"":null,""SummaryInstance"":null},{""Property"":""Instrument"",""ChangedTo"":""instrumented"",""UniqueId"":null,""SummaryInstance"":null}],""DetailChanges"":[{""Property"":""Comments"",""ChangedTo"":""2nd Comment"",""UniqueId"":null,""SummaryInstance"":null},{""Property"":""CCC"",""ChangedTo"":""XR71"",""UniqueId"":null,""SummaryInstance"":null}]}]}]"
                    ));

            VM.JSONLoadFromClipboard = new OperationCommand(o =>
            ShowJSON(Clipboard.ContainsText()
                ? Clipboard.GetText(TextDataFormat.Text)
                : string.Empty));

        }

        private void ShowJSON(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return;

            tView.ProcessJson(text);

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
    }
}
