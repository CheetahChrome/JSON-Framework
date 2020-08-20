using JSON_Enumerate.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace JSON_Enumerate.Implementation
{
    public class CSharpClass : JsonOperationBase
    {
        public CSharpClass(string name) : base(name) { }

        public CSharpClass() : base() { }

        public override string ToString()
        {
            return this.ToString(false);
        }

        public override string ToString(bool asProperty)
        {
            var sb = new StringBuilder();

            var name = SettingsSingleton.Settings.AddDTOSuffix ? $"{Name}DTO" : Name;

            if (asProperty) // This may not be used...investigate
            {
                sb.AppendLine($"public {name} {Name} {{ get; set; }}");
            }
            else
            {
                int tableTypeNumber = 0;
                sb.AppendLine($"public class {name}");
                sb.AppendLine("{");

                var props = Properties.Cast<CSharpProperty>();

                if (SettingsSingleton.Settings.IsSortProperties)
                    props = props.OrderBy(nm => nm.Name).ThenBy(prp => prp.IsId());

                var propsList = props.ToList();

                if (SettingsSingleton.Settings.AddTableTypeConstraint)
                    propsList.ForEach(prp => prp.TableTypeNumber = ++tableTypeNumber);  
                        
                //      [TableType(1)]
                sb.AppendLine(string.Join(Environment.NewLine, propsList.Select(prp => prp.ToString())));

                sb.AppendLine($"}}{Environment.NewLine}");

                sb.Append( string.Join($"{Environment.NewLine}", SubClasses.Select(sbc => sbc.ToString())) );
            }

            return sb.ToString();
        }
    }
}
