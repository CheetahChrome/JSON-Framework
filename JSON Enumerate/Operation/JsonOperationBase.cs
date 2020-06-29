using System;
using System.Collections.Generic;

namespace JSON_Enumerate.Operation
{
    public class JsonOperationBase : IJsonOperation
    {
        public string Name { get; set; }

        public List<IProperty> Properties { get; set; }

        public List<IJsonOperation> SubClasses { get; set; }

        public JsonOperationBase(string seedName) : this() => Name = seedName;

        public JsonOperationBase()
        {
            Properties = new List<IProperty>();
            SubClasses = new List<IJsonOperation>();
        }

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