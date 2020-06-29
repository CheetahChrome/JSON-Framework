using JSON_Enumerate.Operation;
using System;
using System.Collections.Generic;
using System.Text;

namespace JSON_Enumerate.Implementation
{
    public class CSharpProperty : IProperty
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public JsonPropType JsonType { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder(@$"   [JsonProperty(""{Name}"")]");
            sb.Append(Environment.NewLine);
            sb.Append("   public ");

            switch (JsonType)
            {
                case JsonPropType.Undefined:
                case JsonPropType.StrType:
                    sb.Append("string ");
                    break;
                case JsonPropType.NumberType:
                    sb.Append("int ");
                    break;

                case JsonPropType.BoolType:
                    sb.Append("bool ");
                    break;

                case JsonPropType.UserType:
                    sb.Append($"{Name} ");
                    break;

            }

            sb.Append($" {Name} {{ get; set; }}");
            sb.Append(Environment.NewLine);

            return sb.ToString();
        }
    }
}
