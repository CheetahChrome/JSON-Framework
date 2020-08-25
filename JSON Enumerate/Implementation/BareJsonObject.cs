using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using JSON_Enumerate.Operation;

namespace JSON_Enumerate.Implementation
{
    public class BareJsonObject : JsonOperationBase
    {
        public BareJsonObject(IJsonSettings settings, ICommand overrideProperty = null) : base(settings, overrideProperty)
        {
        }
    }
}
