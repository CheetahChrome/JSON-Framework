using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace JSON_Models;

public class PropertyOverride : INotifyPropertyChanged
{


    private bool _IsFKUsed;

    public bool IsFKUsed
    {
        get { return _IsFKUsed; }
        set { _IsFKUsed = value; OnPropertyChanged(nameof(IsFKUsed)); }
    }

    private string _PropertyName;

    public string PropertyName
    {
        get => _PropertyName;
        set { _PropertyName = value; OnPropertyChanged(nameof(PropertyName)); }
    }

    public string JSONPropertyType { get; set; }

    public string NameAsVariable => string.IsNullOrWhiteSpace(PropertyName) ? string.Empty : $"{char.ToLower(PropertyName[0])}{PropertyName.Substring(1)}";

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
