﻿using System;
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


            VM.JSONLoadFromClipboard = new OperationCommand(o =>
            ShowJSON(Clipboard.ContainsText()
                ? Clipboard.GetText(TextDataFormat.Text)
                : string.Empty));

        }

        private void ShowJSON(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return;

            SettingsSingleton.Settings = VM.CSharp;

            var parseResult = text.ParseJson();

            if (!parseResult.IsValid)
            {
                tView.ProcessJson(parseResult.ErrorMessageDoc);
                return;
            }

            // If we are sorting, C# will sort it, so process that first.
            VM.CSharpText = parseResult.ValidJsonDoc.ToCSharpClassesString(VM.CSharp.ClassName);

            tView.ProcessJson(parseResult.ValidJsonDoc);

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
