using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using JSON_Enumerate.Operation;

namespace JSON_Display.Operation
{
    public class OperationSettings : INotifyPropertyChanged, IJsonSettings
    {

        #region Variables
        private bool _AddDTOSuffix;
        private string _ClassName;
        private bool _AddTableTypeConstraint;
        private bool _IsSortProperties;
        private bool _AskOnProperties;
        private bool _AddJsonProperty;
        private bool _AddJsonPropertyName;

        #endregion

        #region Properties


        private string _Name;

        public string Name
        {
            get => _Name;
            set { _Name = value; OnPropertyChanged(nameof(Name)); }
        }

        public bool AddJsonPropertyName
        {
            get => _AddJsonPropertyName;
            set { _AddJsonPropertyName = value; OnPropertyChanged(nameof(AddJsonPropertyName)); }
        }

        public bool AddJsonProperty
        {
            get => _AddJsonProperty;
            set { _AddJsonProperty = value; OnPropertyChanged(nameof(AddJsonProperty)); }
        }

        public bool AddDTOSuffix
        {
            get => _AddDTOSuffix;
            set { _AddDTOSuffix = value; OnPropertyChanged(nameof(AddDTOSuffix)); }
        }

        public string ClassName
        {
            get => _ClassName;
            set { _ClassName = value; OnPropertyChanged(nameof(ClassName)); }
        }

        public bool AddTableTypeConstraint
        {
            get => _AddTableTypeConstraint;
            set { _AddTableTypeConstraint = value; OnPropertyChanged(nameof(AddTableTypeConstraint)); }
        }

        public bool IsSortProperties
        {
            get => _IsSortProperties;
            set { _IsSortProperties = value; OnPropertyChanged(nameof(IsSortProperties)); }
        }

        public bool AskOnProperties
        {
            get => _AskOnProperties;
            set { _AskOnProperties = value; OnPropertyChanged(nameof(AskOnProperties)); }
        }

        #endregion

        #region Construction/Initialization

        #endregion

        #region Methods
        /// <summary>
        /// Event raised when a property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion
    }
}
