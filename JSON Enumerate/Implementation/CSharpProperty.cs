using JSON_Enumerate.Operation;
using System;
using System.Collections.Generic;
using System.Text;

namespace JSON_Enumerate.Implementation
{
    public class CSharpProperty : BareJsonProperty
    {

        public int TableTypeNumber { get; set; }

        public string ToString(int tableTypeNumber)
        {
            TableTypeNumber = tableTypeNumber;
            return ToString();
        }

        public override string ToString()
        {
            var prefix = "  ";

            var sb = new StringBuilder();

            if (TableTypeNumber > 0)
                sb.AppendLine($"{prefix}[TableType({TableTypeNumber})]");

            sb.AppendLine(@$"{prefix}[JsonProperty(""{Name}"")]");

            sb.Append($"{prefix}public ");

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
