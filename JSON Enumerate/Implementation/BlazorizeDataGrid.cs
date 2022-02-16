using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using JSON_Enumerate.Operation;

namespace JSON_Enumerate.Implementation;

public class BlazorizeDataGrid : JsonOperationBase
{
    private string _Schema;

    public int IdentityStart { get; set;  }

  

    public BlazorizeDataGrid() {}

    public BlazorizeDataGrid(IJsonSettings settings, ICommand overrideProperty = null) : base(settings, overrideProperty) { }


    public override string ToString()
    {
        var sb = new StringBuilder();

        if (IsNameUndefined)
            Name = Settings.Name;

        var dto = $"{Name}DTO";

        sb.AppendLine($"<DataGrid TItem=\x22{dto}\x22 Field=\x22@nameof(ListOf{Name}s)\x22>");
        
        var props = Properties.Cast<BlazoriseDataGridProperty>();

        if (SettingsSingleton.Settings.IsSortProperties)
            props = props.OrderBy(nm => nm.Name).ThenBy(prp => prp.IsId());

        sb.AppendJoin(Environment.NewLine, props.Select(prp => $"\t<DataGridColumn TItem=\x22{dto}\x22 Field=\x22nameof({dto}.{prp})\x22 Caption=\x22{prp}\x22 />"));

        sb.AppendLine($"{Environment.NewLine}</DataGrid>");

        return sb.ToString();
    }
}
