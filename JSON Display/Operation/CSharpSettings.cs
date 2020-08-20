using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using JSON_Enumerate.Operation;

namespace JSON_Display.Operation
{
    public class CSharpSettings : INotifyPropertyChanged, IJsonSettings
    {

        #region Variables
        private bool _AddDTOSuffix;
        private string _ClassName;
        private bool _AddTableTypeConstraint;
        private bool _IsSortProperties;

        #endregion

        #region Properties

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
