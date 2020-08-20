using System;
using System.Collections.Generic;
using System.Text;
using JSON_Enumerate.Operation;

namespace JSON_Enumerate.Implementation
{
    public class BareJsonProperty : IProperty
    {

        public string Name { get; set; }
        public int Size { get; set; }

        public string RawValueText { get; set; }

        public JsonPropType JsonType { get; set; }

        public bool IsUserType() => JsonType == JsonPropType.UserType;
        public bool IsUndefined() => JsonType == JsonPropType.Undefined;

        public bool IsId() =>
            !IsUserType() && !IsUndefined() && Name.EndsWith("id", StringComparison.OrdinalIgnoreCase);


        public override string ToString()
        {
            return $@"\x22{Name} : {RawValueText}";
        }
    }
}
