using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using JSON_Display.Operation;
using Newtonsoft.Json.Linq;

namespace JSON_Display
{
    public  class MainVM : INotifyPropertyChanged
    {
        #region Commands

        public ICommand JSONLoadInternal     { get; set; } 
        public ICommand JSONLoadFromClipboard { get; set; }
        public ICommand JSONLoadFromDatabase => new OperationCommand(LoadFromDatabase);

        public ICommand ClearResults { get; set; }
        public ICommand CMDSettingsShow => new OperationCommand((o) =>
            {
                ShowSettingsDialog = !ShowSettingsDialog;
            }); 


        #endregion

        #region Events
        /// <summary>Event raised when a property changes. </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Variables
        #endregion

        #region Properties


        private bool _ShowSettingsDialog;

        public bool ShowSettingsDialog
        {
            get => _ShowSettingsDialog;
            set { _ShowSettingsDialog = value; OnPropertyChanged(nameof(ShowSettingsDialog)); }
        }

        private OperationSettings _CSharp;

        public OperationSettings CSharp
        {
            get => _CSharp;
            set { _CSharp = value; OnPropertyChanged(nameof(CSharp)); }
        }


        private string _CSharpText;

        public string CSharpText
        {
            get => _CSharpText;
            set { _CSharpText = value; OnPropertyChanged(nameof(CSharpText)); }
        }

        private string _JSONText;

        public string JSONText
        {
            get => _JSONText;
            set
            {
                _JSONText = string.IsNullOrWhiteSpace(value) ? string.Empty : value;
                OnPropertyChanged(nameof(JSONText));
            }
        }


        private string _SQLTableText;

        public string SQLTableText
        {
            get => _SQLTableText;
            set { _SQLTableText = value; OnPropertyChanged(nameof(SQLTableText)); }
        }


        private string _SQLTableTypeText;

        public string SQLTableTypeText
        {
            get => _SQLTableTypeText;
            set { _SQLTableTypeText = value; OnPropertyChanged(nameof(SQLTableTypeText)); }
        }

        private double _MainFontSize;

        public double MainFontSize
        {
            get => _MainFontSize;
            set { _MainFontSize = value; OnPropertyChanged(nameof(MainFontSize)); }
        }


        private ObservableCollection<string> _RecentJsons;

        public ObservableCollection<string> RecentJsons
        {
            get { return _RecentJsons; }
            set { _RecentJsons = value; OnPropertyChanged(nameof(RecentJsons)); }
        }

        #endregion

        #region Construction/Initialization

        public MainVM()
        {
            CSharp = new OperationSettings();
            RecentJsons = new ObservableCollection<string>() { @"C:\Temp\Initial.Json", @"C:\Temp\Full.Json" };
        }
        #endregion

        #region Methods

        public void LoadFromDatabase(object obj)
        {

        }


        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        internal void ClearAll()
        {
            CSharpText = JSONText = SQLTableText = SQLTableTypeText = null;
        }

        #endregion
    }
}
