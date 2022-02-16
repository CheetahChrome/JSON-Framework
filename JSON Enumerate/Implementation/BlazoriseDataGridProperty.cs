using System;
using System.Collections.Generic;
using System.Text;
using JSON_Enumerate.Operation;

namespace JSON_Enumerate.Implementation
{
    public class BlazoriseDataGridProperty : BareJsonProperty
    {
        public override string ToString()
        {
            return Name;
            //var sb = new StringBuilder();


            //sb.Append($"<DataGridColumn TItem =\x22 )

            //switch (JsonType)
            //{
            //    case JsonPropType.Undefined:
            //    case JsonPropType.StrType:
            //        sb.Append("[nvarchar](128) null");
            //        break;

            //    case JsonPropType.NumberType:
            //        sb.Append("[int] null");
            //        break;

            //    case JsonPropType.BoolType:
            //        sb.Append("[bit] null");
            //        break;

            //    // TODO Create sub table with FKs
            //    case JsonPropType.UserType:
            //        sb.Append($"   {Name}Id [int]  null");
            //        break;

            //    default:
            //        throw new ArgumentOutOfRangeException(JsonType.ToString());
            //}

            //sb.AppendLine(",");

            //return sb.ToString();
        }

        public static string ToDrop(string targetTable)
            => $"-- DROP TABLE IF EXISTS {targetTable};{Environment.NewLine}GO";
    }
}
