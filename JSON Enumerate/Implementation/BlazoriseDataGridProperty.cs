using System;
using System.Collections.Generic;
using System.Text;
using JSON_Enumerate.Operation;

namespace JSON_Enumerate.Implementation
{
    public class BlazoriseDataGridProperty : BareJsonProperty
    {

        public override string ToString() => Name;
        public  string ToDataGridColumn(string dto)
        {
            //            return Name;
            var sb = new StringBuilder();

            // $"\t<DataGridColumn TItem=\x22{dto}\x22 Field=\x22nameof({dto}.{prp})\x22 Caption=\x22{prp}\x22 />"));

            sb.Append($"\t<DataGrid");

            switch (JsonType)
            {
                case JsonPropType.Undefined:
                case JsonPropType.StrType:
                    if (IsDateTime)
                        sb.Append("Date");
                    break;

                case JsonPropType.NumberType:
                    sb.Append("Numeric");
                    break;

                case JsonPropType.BoolType:
                    sb.Append("Check");
                    break;

                case JsonPropType.UserType:
                    sb.Append($"Select");
                    break;

            }

            sb.Append($"Column TItem=\x22{dto}\x22 Editable=\x22true\x22 Caption=\x22{Name}\x22 Field=\x22@nameof({dto}.{Name})\x22  />");

            return sb.ToString();
        }

        public static string ToDrop(string targetTable)
            => $"-- DROP TABLE IF EXISTS {targetTable};{Environment.NewLine}GO";
    }
}
