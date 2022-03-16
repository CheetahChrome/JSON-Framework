using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using JSON_Display.Models;
using JSON_Display.Operation;
using System.Diagnostics;
using System.Linq;

namespace JSON_Display
{
    public  class MainVM : INotifyPropertyChanged
    {
        #region Commands

        public ICommand FileSaveCmd { get; set; }
        public ICommand ReloadFromTextCmd { get; set; }
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


        private ObservableCollection<MRU> _MRUS;

        public ObservableCollection<MRU> MRUS
        {
            get { return _MRUS; }
            set { _MRUS = value; OnPropertyChanged(nameof(MRUS)); }
        }

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

        private OperationSettingsDatabase _Database;

        public OperationSettingsDatabase Database
        {
            get => _Database;
            set { _Database = value; OnPropertyChanged(nameof(Database)); }
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


        private string _BlazoriseText;

        public string BlazoriseText
        {
            get { return _BlazoriseText; }
            set { _BlazoriseText = value; OnPropertyChanged(nameof(BlazoriseText)); }
        }


        private string _ComponentText;

        public string ComponentText
        {
            get { return _ComponentText; }
            set { _ComponentText = value; OnPropertyChanged(nameof(ComponentText)); }
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

        private string _Error;

        public string Error
        {
            get => _Error;
            set { _Error = value; OnPropertyChanged(nameof(Error)); OnPropertyChanged(nameof(HasError)); }
        }

        public bool HasError => !string.IsNullOrEmpty(Error);

        // Nuget on using Microsoft.Expression.Interactivity.Core; for ICommand
        public ICommand ClearError => new OperationCommand((_) => { Error = string.Empty; });
        public ICommand ErrorCopyToClipboard { get; set; }

        #endregion

        #region Construction/Initialization

        public MainVM()
        {
            CSharp = new OperationSettings();
            Database = new OperationSettingsDatabase();
            RecentJsons = new ObservableCollection<string>() { @"C:\Temp\Initial.Json", @"C:\Temp\Full.Json" };
            MRUS = new ObservableCollection<MRU>(GetMRUS());
        }
        #endregion

        #region Methods

        public void LoadFromDatabase(object obj)
        {

        }

        public List<MRU> GetMRUS()
        {
            try
            {
                var mruText = Properties.Settings.Default.MRUS;
                return string.IsNullOrWhiteSpace(mruText) ? new List<MRU>()
                    : JsonSerializer.Deserialize<List<MRU>>(mruText);
            }
            catch (Exception ex)
            {
                Error = ex.ToStringDemystified();
            }

            return null;
        }

        public void AddMRUToUserSettings(MRU mRU)
        {
            try
            {
                MRUS.Add(mRU);
                var asList = MRUS.ToList<MRU>();
                Properties.Settings.Default.MRUS = JsonSerializer.Serialize(asList);
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                Error = ex.Demystify().ToString();
            }        
        }

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        internal void ClearAll(bool ignoreJsonText = false)
        {
            CSharpText = SQLTableText = SQLTableTypeText = BlazoriseText = null;

            Error = null;

            if (!ignoreJsonText)
                JSONText = null;
        }

        #endregion
    }
}
