using System;
using System.Windows.Input;

namespace JSON_Enumerate.Operation
{


    /// <summary>
    /// Within each IJsonOperation instance, it will have properties. This
    /// contract defines how the properties operate. 
    /// </summary>
    public interface IProperty
    {
        string Name { get; set; }

        string RawValueText { get; set; }

        int Size { get; set; }

        bool IsId();

        bool IsUserType();
        
        bool IsUndefined();

        JsonPropType JsonType { get; set; }

        ICommand OverrideProperty { get; set; }

    }
}