using System;
using System.Collections.Generic;
using System.Text;
using JSON_Enumerate.Operation;

namespace JSON_Enumerate.Implementation
{
    public class SQLProperty : BareJsonProperty
    {
        public override string ToString()
        {
            var sb = new StringBuilder();

            //    [PhoneNbr] [nvarchar](10) NOT NULL,
            //    [VoiceOptOutFlag] [bit] NOT NULL,
            //    [TextOptOutFlag] [bit] NOT NULL,
            //    [CreateDttm] [datetime] NOT NULL,
            //    [UpdateDttm] [datetime] NOT NULL,
            //    [PhoneGUId] [nvarchar](40) NULL,

            if (JsonType != JsonPropType.UserType)
                sb.Append($"   [{Name}] ");

            switch (JsonType)
            {
                case JsonPropType.Undefined:
                case JsonPropType.StrType:
                    sb.Append("[nvarchar](128)");
                    break;

                case JsonPropType.NumberType:
                    sb.Append("[int]");
                    break;

                case JsonPropType.BoolType:
                    sb.Append("[bit]");
                    break;

                // TODO Create sub table with FKs
                case JsonPropType.UserType:
                    sb.Append($"   [int] {Name}Id");
                    break;

                default:
                    throw new ArgumentOutOfRangeException(JsonType.ToString());
            }

            sb.AppendLine(",");

            return sb.ToString();
        }
    }
}
