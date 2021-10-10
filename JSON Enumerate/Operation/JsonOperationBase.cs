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

        public bool IsNameUndefined { get; set; }

        public string _Name;
        public string Name
        {
            get
            {
                //if (string.IsNullOrWhiteSpace(_Name))
                //{
                //    if (Settings != null && !string.IsNullOrWhiteSpace(Settings.Name))
                //    {
                //        _Name = Settings.Name;
                //    }
                //    else
                //    {
                //        _Name = "Jabberwocky";
                //        IsNameUndefined = true;
                //    }
                //}

                return _Name;
            }
            set
            {
                _Name = value;
                IsNameUndefined = false;
            }
        }

        public JsonOperationBase(IJsonSettings settings, ICommand overrideProperty = null) 
        {
            OverrideProperty = overrideProperty;
            Settings = settings;
            Properties = new List<IProperty>();
            SubClasses = new List<IJsonOperation>();
            IsNameUndefined = true;
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