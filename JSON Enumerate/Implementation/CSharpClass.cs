using JSON_Enumerate.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
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
                sb.AppendLine($"public class {name}");
                sb.AppendLine("{");
                sb.AppendLine(string.Join(Environment.NewLine, Properties.Select(prp => prp.ToString())));

                sb.AppendLine($"}}{Environment.NewLine}");

                sb.Append( string.Join($"{Environment.NewLine}", SubClasses.Select(sbc => sbc.ToString())) );
            }

            return sb.ToString();
        }
    }
}
