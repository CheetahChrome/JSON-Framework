using System.Collections.Generic;
using System.Windows.Input;

namespace JSON_Enumerate.Operation
{
    /// <summary>
    /// Each toplevel operation has to conform to this structure. 
    /// </summary>
    public interface IJsonOperation
    {
        string Name { get; set; }

        bool IsNameUndefined { get; set; }

        List<IProperty> Properties { get; set; }

        List<IJsonOperation> SubClasses { get; set; }

        public string ToString(bool asProperty);

        ICommand OverrideProperty { get; set; }
    }
}