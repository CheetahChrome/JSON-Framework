using Json.Common.Extensions;
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
            const string prefix = "  ";
            string name = string.Empty;

            var sb = new StringBuilder();

            if (TableTypeNumber > 0)
                sb.AppendLine($"{prefix}[TableType({TableTypeNumber})]");

            if (Name != null) // Some arrays are just arrays and not properties. TODO Fix to jump out?
                if (Name.Length > 3)
                {
                    char c2 = Name[2];
                    name = Char.IsLower(c2) ? Name : Name.ToPascalCase();
                }
                else
                {
                    name = Name;
                }
            else
                name = string.Empty;

            // [JsonProperty("Wind")]
            if (SettingsSingleton.Settings.AddJsonProperty)
                sb.AppendLine(@$"{prefix}[JsonProperty(""{name}"")]");

            // [JsonPropertyName("Wind")]
            if (SettingsSingleton.Settings.AddJsonProperty)
                sb.AppendLine(@$"{prefix}[JsonPropertyName(""{name}"")]");

            sb.Append($"{prefix}public ");

            switch (JsonType)
            {
                case JsonPropType.Undefined:
                case JsonPropType.StrType:
                    sb.Append(IsDateTime? "DateTime " : "string ");
                    break;
                case JsonPropType.NumberType:
                //    OverrideProperty?.Execute(null);
                    sb.Append("int ");
                    break;

                case JsonPropType.BoolType:
                    sb.Append("bool ");
                    break;

                case JsonPropType.UserType:
                    sb.Append($"{name} ");
                    break;

            }

            sb.AppendLine($" {name} {{ get; set; }}");

            // For the mimic property which has the actual string value...only used rarely.
            //if (Name.EndsWith("Id"))
            //{ 
            //    var shortName = name.Substring(0, name.Length - 2);
            //    sb.AppendLine($"{prefix}public string {shortName}Str {{ get; set; }}");
            //}

            return sb.ToString();
        }
    }
}
