using JSON_Enumerate.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace JSON_Enumerate.Implementation
{
    public class CSharpClass : JsonOperationBase
    {
        public CSharpClass(IJsonSettings settings, ICommand overrideProperty = null) : base(settings, overrideProperty)
        {
        }

        public CSharpClass() {}


        public override string ToString()
        {

            var props = Properties?.Cast<CSharpProperty>();

            var sb = new StringBuilder();
            var className = string.IsNullOrWhiteSpace(Name)
                          ? ExtractNameFromFirstProperty(props)
                          : Name;

            // Top level can get and set override, but subclasses need their given name.
            if (!IsSubClass && Settings != null) 
            {
                if (!string.IsNullOrWhiteSpace(Settings.ClassName))
                    className = Settings.ClassName;
                else
                {
                    Settings.ClassName = className;
                    Settings.Name = className;
                }
            }
          
            // Set our class name.
            Name = className;

            if ((Settings != null) && string.IsNullOrWhiteSpace(Settings?.ClassName))
                Settings.ClassName = Name;

           var name = SettingsSingleton.Settings.AddDTOSuffix ? $"{Name}DTO" : Name;

            var tableTypeNumber = 0;
            sb.AppendLine($"public class {name}");

            if (props != null)
            {
                sb.AppendLine("{");
                if (SettingsSingleton.Settings.IsSortProperties)
                    props = props.OrderBy(nm => nm.Name).ThenBy(prp => prp.IsId());

                var propsList = props.ToList();

                if (SettingsSingleton.Settings.AddTableTypeConstraint)
                    propsList.ForEach(prp => prp.TableTypeNumber = ++tableTypeNumber);

                //      [TableType(1)]
                sb.AppendLine(string.Join(Environment.NewLine, propsList.Select(prp => prp.ToString())));

                sb.AppendLine($"}}{Environment.NewLine}");

                if (SubClasses?.Any() ?? false)
                    sb.Append(string.Join($"{Environment.NewLine}", SubClasses.Select(sbc => sbc.ToString())));
            }
            else // Empty Array at source, unknown objects.
                sb.AppendLine("{ /* Empty Array Encountered in Source */ }");

            return sb.ToString();
        }

        private static string ExtractNameFromFirstProperty(IEnumerable<CSharpProperty> props)
        {
            if (!(props?.Any() ?? false))
                return "NotOnProp";

            string className;
            var firstPropName = props.First().Name;

            className = Regex.Replace(firstPropName, "id$", string.Empty, RegexOptions.IgnoreCase);
            return className;
        }
    }
}