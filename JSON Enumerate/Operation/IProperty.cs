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

        /// <summary>
        /// Will hold a quoted value in the string. See ValueText for Non quoted value.
        /// </summary>
        string RawValueText { get; set; }

        /// <summary>
        /// Holds the non quoted string value. 
        /// </summary>
        string ValueText { get; set; }

        DateTimeOffset Date { get; set; }

        int Size { get; set; }

        bool IsId();

        bool IsUserType();
        
        bool IsUndefined();

        bool IsDateTime { get; set; }

        JsonPropType JsonType { get; set; }

        ICommand OverrideProperty { get; set; }

    }
}