using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using JSON_Enumerate.Operation;

namespace JSON_Enumerate.Implementation
{
    public class SQLTable : JsonOperationBase
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


        public SQLTable(IJsonSettings settings, ICommand overrideProperty = null) : base(settings, overrideProperty) { }

        public SQLTable() {}

        public override string ToString()
        {
            var sb = new StringBuilder();

            //// var name = SettingsSingleton.Settings.AddDTOSuffix ? $"{Name}DTO" : Name;


            //int tableTypeNumber = 0;
            //sb.AppendLine($"public class {name}");
            //sb.AppendLine("{");

            //var props = Properties.Cast<CSharpProperty>();

            //if (SettingsSingleton.Settings.IsSortProperties)
            //    props = props.OrderBy(nm => nm.Name).ThenBy(prp => prp.IsId());

            //var propsList = props.ToList();

            //if (SettingsSingleton.Settings.AddTableTypeConstraint)
            //    propsList.ForEach(prp => prp.TableTypeNumber = ++tableTypeNumber);

            ////      [TableType(1)]
            //sb.AppendLine(string.Join(Environment.NewLine, propsList.Select(prp => prp.ToString())));

            //sb.AppendLine($"}}{Environment.NewLine}");

            //sb.Append(string.Join($"{Environment.NewLine}", SubClasses.Select(sbc => sbc.ToString())));

            List<SQLProperty> propsList;

            if (IsNameUndefined)
            {
                Name = Settings.Name;
            }

            var id = $"{Name}Id";
            sb.AppendLine($"CREATE TABLE [{Schema}].[{Name}]");
            sb.AppendLine($"({Environment.NewLine}");
            
            sb.AppendLine($"   [{id}][int] IDENTITY(10000, 1) NOT NULL,{Environment.NewLine}");

            var props = Properties.Cast<SQLProperty>();

            if (SettingsSingleton.Settings.IsSortProperties)
                props = props.OrderBy(nm => nm.Name).ThenBy(prp => prp.IsId());

            if (props.First().Name == id)
                propsList = props.Skip(1).ToList();
            else
                propsList = props.ToList();
            
            //sb.AppendLine(string.Join(Environment.NewLine, propsList.Select(prp => prp.ToString())));
            sb.AppendJoin(Environment.NewLine, propsList.Select(prp => prp.ToString()));

            sb.AppendLine(string.Empty);
            sb.AppendLine($"   CONSTRAINT[PK__{Name}__1] PRIMARY KEY CLUSTERED ([{id}] ASC){Environment.NewLine}");
            sb.AppendLine($"   WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]{Environment.NewLine}");
            sb.AppendLine($") ON[PRIMARY]{Environment.NewLine}");

            // FK Tables
            if (SubClasses?.Any() ?? false)
                sb.Append(string.Join($"{Environment.NewLine}", SubClasses.Select(sbc => sbc.ToString())));


            return sb.ToString();
        }
    }
}
