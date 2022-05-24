using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Json.Common.Extensions;
using JSON_Enumerate.Operation;

namespace JSON_Enumerate.Implementation
{
    public class SqlMerge : JsonOperationBase
    {
        private string _Schema;


        public int IdentityStart { get; set; }

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


        public SqlMerge(IJsonSettings settings, ICommand overrideProperty = null) : base(settings, overrideProperty) { }

        public SqlMerge() : base() { }


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

            var name = Name.ToPascalCase();

            //sb.AppendLine($"CREATE TYPE [{Schema}].[{name}TT] AS TABLE");
            //sb.AppendLine($"({Environment.NewLine}");
            //// sb.AppendLine($"   [{id}][int] NULL,{Environment.NewLine}");
            ///
            SprocHeader(sb, name);

            var props = Properties.Cast<SQLProperty>();

            if (SettingsSingleton.Settings.IsSortProperties)
                props = props.OrderBy(nm => nm.Name).ThenBy(prp => prp.IsId());
            
            sb.AppendLine(WhenMatched());

            GenerateAssignments(sb, props);
            
            // Insert

            sb.AppendLine(WhenNotMatched());

            GenerateColumns(sb, props);
            
            sb.AppendLine(WhenNotMatchedSource());
            
            GenerateSourceColumns(sb, props);

            sb.AppendLine(Fini());

            //propsList = props.ToList();

            //sb.AppendLine(string.Join(Environment.NewLine, propsList.Select(prp => prp.ToString())));

            //sb.AppendLine(")");

            return sb.ToString();
        }

        public void GenerateAssignments(StringBuilder sb, IEnumerable<SQLProperty> props)
        { // TARGET.PhaseGateDate  = SOURCE.PhaseGateDate
            // 12 adds "     target." length
            var format = string.Format("{{0,-{0}}} = {{1}}", (props.Max(prp => prp.NamePascalCase.Length) + 12));

            sb.AppendLine(string.Join($",{Environment.NewLine}", props.Select(prp => string.Format(format, $"     Target.{prp.NamePascalCase}", $"coalesce(SOURCE.{prp.NamePascalCase}, TARGET.{prp.NamePascalCase}) ")).ToList()));
            // sb.AppendLine(string.Join($",{Environment.NewLine}", props.Select(prp => $"\tTarget.{prp.NamePascalCase} = SOURCE.{prp.NamePascalCase}").ToList()));
        }

        public void GenerateColumns(StringBuilder sb, IEnumerable<SQLProperty> props)
        { 
            sb.AppendLine(string.Join($",{Environment.NewLine}", props.Select(prp => $"     {prp.NamePascalCase}" ).ToList()));
        }

        public void GenerateSourceColumns(StringBuilder sb, IEnumerable<SQLProperty> props)
        { 
            sb.AppendLine(string.Join($",{Environment.NewLine}", props.Select(prp => $"     SOURCE.{prp.NamePascalCase}" ).ToList()));
        }

        public void SprocHeader(StringBuilder sb, string name)
        {
            var lowerStartName = name.ToFirstCharLower();

            var tableType = $"{lowerStartName}TT";

            var header =
@$"create or alter PROCEDURE post.{name}
    @{tableType} tt.{name} readonly,
    @FK***Id int = null
as
begin

-- Get the Id from an existing source if not passed in; Passed in during creation generally.
declare @FK***Id int = coalesce(@FK***Id, (select top(1) PrimaryId from @{lowerStartName}TT)) ;

merge {Schema}.{name} as TARGET
using @{tableType} as SOURCE
on @FK***Id     = TARGET.XXXXXId,";

            sb.AppendLine(header);
        }

        public string WhenMatched()
        {
            return  
@"when matched then
update set
     Target.***Id               = Source.***Id,";
        }

        public string WhenNotMatched()
        {
            return
@"when not matched then
  insert  
  (
     ****Id";
        }

        public string WhenNotMatchedSource()
        {
            return
@" )
values
   (   
     @FK***Id,";

        }

        public string Fini()
            => @");

    return 200;
end
go";

    }
}
