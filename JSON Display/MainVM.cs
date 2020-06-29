using System;
using System.Collections.Generic;
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

        public ICommand  JSONLoadInternal     { get; set; } 
        public ICommand JSONLoadFromClipboard { get; set; }

        #endregion

        #region Events
        /// <summary>Event raised when a property changes. </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Variables
        #endregion

        #region Properties

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

        private double _MainFontSize;

        public double MainFontSize
        {
            get => _MainFontSize;
            set { _MainFontSize = value; OnPropertyChanged(nameof(MainFontSize)); }
        }
        #endregion

        #region Construction/Initialization

        #endregion

        #region Methods

        #endregion

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
