using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace JSON_Enumerate.Operation
{
    public class JsonOperationBase : IJsonOperation
    {

        public List<IProperty> Properties { get; set; }

        public List<IJsonOperation> SubClasses { get; set; }

        public ICommand OverrideProperty { get; set; }

        public IJsonSettings Settings { get; set; }
        public string Name
        {
            get => Settings?.Name ?? "Undefined";
            set => throw new NotImplementedException();
        }

        public JsonOperationBase(IJsonSettings settings, ICommand overrideProperty = null) 
        {
            OverrideProperty = overrideProperty;
            Settings = settings;
            Properties = new List<IProperty>();
            SubClasses = new List<IJsonOperation>();
        }

        public JsonOperationBase() { }


        /// <summary>
        /// This must be overriden (`new`) on the derived to export the results.
        /// </summary>
        /// <param name="asProperty"></param>
        /// <returns></returns>
        public virtual string ToString(bool asProperty)
        {
            throw new NotImplementedException();
        }
    }
}