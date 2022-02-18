using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using JSON_Enumerate.Operation;

namespace JSON_Enumerate.Implementation
{
    public class BareJsonProperty : IProperty
    {

        public string Name { get; set; }
        public int Size { get; set; }

        public string RawValueText { get; set; }

        public string ValueText { get; set; }

        public JsonPropType JsonType { get; set; }

        public bool IsUserType() => JsonType == JsonPropType.UserType;
        public bool IsUndefined() => JsonType == JsonPropType.Undefined;

        public bool IsDateTime { get; set; }

        public DateTimeOffset Date { get; set; }
        public bool IsId() =>
            !IsUserType() && !IsUndefined() && Name.EndsWith("id", StringComparison.OrdinalIgnoreCase);

        public ICommand OverrideProperty { get; set; }

        public override string ToString() => $@"\x22{Name} : {RawValueText}";
    }
}
