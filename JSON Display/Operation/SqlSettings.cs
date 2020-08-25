using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using JSON_Enumerate.Operation;

namespace JSON_Display.Operation
{
    public class SqlSettings : INotifyPropertyChanged
    {
        private string _TableName;

        public string TableName
        {
            get => _TableName;
            set { _TableName = value; OnPropertyChanged(nameof(TableName)); }
        }

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
