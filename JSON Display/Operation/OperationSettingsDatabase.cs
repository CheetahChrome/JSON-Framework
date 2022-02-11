using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using JSON_Enumerate.Operation;

namespace JSON_Display.Operation
{
    public class OperationSettingsDatabase : INotifyPropertyChanged, IDatabaseSettings
    {

        #region Variables
        #endregion

        #region Properties


        private string _DatabaseConnectionString;


        public string DatabaseConnectionString
        {
            get => _DatabaseConnectionString;
            set { _DatabaseConnectionString = value; OnPropertyChanged(nameof(DatabaseConnectionString)); }
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
        /// <param name="propertyDatabaseConnectionString">The name of the property that has changed.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyDatabaseConnectionString = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyDatabaseConnectionString));

        #endregion
    }
}
