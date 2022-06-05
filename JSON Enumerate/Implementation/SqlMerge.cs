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
          // List<SQLProperty> propsList;

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
            SprocHeader(sb, name, GetFKoverride(), GetValidFKs());

            var props = Properties.Cast<SQLProperty>();

            if (SettingsSingleton.Settings.IsSortProperties)
                props = props.OrderBy(nm => nm.Name).ThenBy(prp => prp.IsId());
            
            sb.AppendLine(WhenMatched());

            GenerateAssignments(sb, props);
            
            // Insert

            sb.Append(WhenNotMatched(GetFKoverride(), GetValidFKs()));

            GenerateColumns(sb, props);
            
            sb.Append(WhenNotMatchedSource(GetFKoverride(), GetValidFKs()));
            
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

            
            sb.AppendLine(string.Join($",{Environment.NewLine}", props.Skip(1).Select(prp => string.Format(format, $"\tTarget.{prp.NamePascalCase}", $"coalesce(SOURCE.{prp.NamePascalCase}, TARGET.{prp.NamePascalCase}) ")).ToList()));
            // sb.AppendLine(string.Join($",{Environment.NewLine}", props.Select(prp => $"\tTarget.{prp.NamePascalCase} = SOURCE.{prp.NamePascalCase}").ToList()));
        }

        public void GenerateColumns(StringBuilder sb, IEnumerable<SQLProperty> props)
        { 
            sb.AppendLine(string.Join($",{Environment.NewLine}", props.Skip(1).Select(prp => $"\t{prp.NamePascalCase}" ).ToList()));
        }

        public void GenerateSourceColumns(StringBuilder sb, IEnumerable<SQLProperty> props)
        { 
            sb.AppendLine(string.Join($",{Environment.NewLine}", props.Skip(1).Select(prp => $"\tSOURCE.{prp.NamePascalCase}" ).ToList()));
        }

        public bool GetFKoverride() => Settings.Overrides.Any(ov => ov.IsFKUsed);

        public List<JSON_Models.PropertyOverride> GetValidFKs() => Settings.Overrides.Where(ov => ov.IsFKUsed).ToList();

        public void SprocHeader(StringBuilder sb, string name, bool fKoverride, List<JSON_Models.PropertyOverride> validFKs)
        {
            var lowerStartName = name.ToFirstCharLower();
            var tableType = $"@{lowerStartName}TT";

            sb.AppendLine($"create or alter PROCEDURE post.{name}");
            sb.AppendLine($"{tableType} tt.{name} readonly");
//            sb.AppendLine($"@{lowerStartName} int = null");

            if (fKoverride)
                validFKs.ForEach(fk => sb.AppendLine($", @{fk.NameAsVariable} {fk.SQL} = null"));

            sb.AppendLine("as");
            sb.AppendLine("begin");

            if (fKoverride)
            {
                sb.AppendLine("-- Get the Id from an existing source if not passed in; Passed in during creation generally.");
                validFKs.ForEach(fk => sb.AppendLine($"set @{fk.NameAsVariable} = coalesce(@{fk.NameAsVariable}, (select top(1) {fk.PropertyName} from {tableType}));"));
            }

//            sb.AppendLine($"\tselect top(1) @{lowerStartName}Id = {name}Id from {tableType} order by {name}");
            var merge = @$"
merge {Schema}.{name} as TARGET
using {tableType} as SOURCE
on SOURCE.{Name}Id = TARGET.{Name}Id";
           sb.AppendLine(merge);

            if (fKoverride)
                validFKs.ForEach(fk => sb.AppendLine($"AND @{fk.NameAsVariable}  = TARGET.{fk.PropertyName}"));

        }

        public string WhenMatched()
        {
            return
@"when matched then
update set";
        }

        public string WhenNotMatched(bool fKoverride, List<JSON_Models.PropertyOverride> validFKs)
        {
            var sbNM = new StringBuilder();

            sbNM.AppendLine(
@"when not matched then
  insert  
  (");

            if (fKoverride)
                validFKs.ForEach(fk => sbNM.AppendLine($"\t{fk.PropertyName},"));

            return sbNM.ToString();

        }

        public string WhenNotMatchedSource(bool fKoverride, List<JSON_Models.PropertyOverride> validFKs)
        {
            var sbNM = new StringBuilder(@" )
values
   (");


            if (fKoverride)
                validFKs.ForEach(fk => sbNM.AppendLine($"\t@{fk.NameAsVariable},"));

            return sbNM.ToString();

        }

        public string Fini()
        {
             
            return @$");

    return 200;
end
go


CREATE or Alter PROCEDURE get.{Name}
 @projectId int
AS
    
select IsNull((select * from info.{Name} where ProjectId = @projectId for json auto), '[]');

RETURN 200 
go






";
        }
    }
}
