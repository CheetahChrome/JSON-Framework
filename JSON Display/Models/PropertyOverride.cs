using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace JSON_Display.Models
{
    public class PropertyOverride : INotifyPropertyChanged
    {

        private string _PropertyName;

        public string PropertyName
        {
            get => _PropertyName;
            set { _PropertyName = value; OnPropertyChanged(nameof(PropertyName)); }
        }

        public string JSONPropertyType { get; set; }


        private string _CSharp;

        public string CSharp
        {
            get => _CSharp;
            set { _CSharp = value; OnPropertyChanged(nameof(CSharp)); }
        }


        private string _SQL;

        public string SQL
        {
            get => _SQL;
            set { _SQL = value; OnPropertyChanged(nameof(SQL)); }
        }

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


    }
}
