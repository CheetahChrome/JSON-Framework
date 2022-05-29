using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Json.Common.Extensions;
using JSON_Enumerate.Operation;

namespace JSON_Enumerate.Implementation
{
    public class SqlTableType : JsonOperationBase
    {
        private string _Schema;


        public int IdentityStart { get; set;  }

        public string Schema
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_Schema))
                    _Schema = "dbo";

                return _Schema;
            }
            set => _Schema = value;
        }


        public SqlTableType(IJsonSettings settings, ICommand overrideProperty = null) : base(settings, overrideProperty) { }

        public SqlTableType() : base() { }


        public override string ToString()
        {
            var sb = new StringBuilder();
            List<SQLProperty> propsList;

            // var id = $"{Name}Id";
            if (IsNameUndefined)
            {
                if (Settings.IsNameUndefined == false)
                    Name = Settings.Name;
            }

            var name = Name; //.ToPascalCase();

            sb.AppendLine($"CREATE TYPE [{Schema}].[{name}TT] AS TABLE");
            sb.AppendLine($"({Environment.NewLine}");
            // sb.AppendLine($"   [{id}][int] NULL,{Environment.NewLine}");

            var props = Properties.Cast<SQLProperty>();

            if (SettingsSingleton.Settings.IsSortProperties)
                props = props.OrderBy(nm => nm.Name).ThenBy(prp => prp.IsId());

            propsList = props.ToList();

            sb.AppendLine(string.Join(Environment.NewLine, propsList.Select(prp => prp.ToString())));

            sb.AppendLine(")");
            sb.AppendLine("go");

            return sb.ToString();
        }
    }
}
