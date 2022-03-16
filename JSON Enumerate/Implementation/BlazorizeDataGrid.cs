using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Json.Common.Extensions;
using JSON_Enumerate.Operation;

namespace JSON_Enumerate.Implementation;

public class BlazorizeDataGrid : JsonOperationBase
{
    public int IdentityStart { get; set;  }

    public BlazorizeDataGrid() {}

    public BlazorizeDataGrid(IJsonSettings settings, ICommand overrideProperty=null) : base(settings, overrideProperty) { }


    public override string ToString()
    {
        var sb=new StringBuilder();

        if (IsNameUndefined)
            Name=(Settings.Name).ToPascalCase();

        var dto=$"{Name}DTO";

        //sb.AppendLine($"<DataGrid TItem=\x22{dto}\x22 Field=\x22@nameof(ListOf{Name}s)\x22>");
        sb.AppendLine(GridNodeStart());
        
        var props=Properties.Cast<BlazoriseDataGridProperty>();

        if (SettingsSingleton.Settings.IsSortProperties)
            props=props.OrderBy(nm => nm.Name).ThenBy(prp => prp.IsId());

        sb.AppendJoin($",{Environment.NewLine}", props.Select(prp => prp.ToDataGridColumn(dto)));

        //sb.AppendLine($"{Environment.NewLine}</DataGrid>");
        sb.AppendLine(GridNodeEnd());

        sb.AppendLine(Code());

        return sb.ToString();
    }

    public string GridNodeStart() => @$"
<DataGrid TItem=""{Name}DTO"" 
          Data=""{Name}List""
          @bind-SelectedRow =""@selected{Name}""
          Editable           
          Filterable
          ShowPager
          ShowPageSizes
          EditMode=""Blazorise.DataGrid.DataGridEditMode.Inline""
          CommandMode=""DataGridCommandMode.ButtonRow"">
    <DataGridColumns>
        <DataGridCommandColumn TItem=""{Name}DTO"" NewCommandAllowed=""false"" EditCommandAllowed=""false"" DeleteCommandAllowed=""false"">
           <SaveCommandTemplate>
                <Button ElementId=""btnSave"" Type=""ButtonType.Submit"" PreventDefaultOnSubmit Color=""Color.Primary"" Clicked=""@context.Clicked""> @context.LocalizationString </Button>
           </SaveCommandTemplate>
           <CancelCommandTemplate>
                <Button ElementId=""btnCancel"" Color=""Color.Secondary"" Clicked=""@context.Clicked""> @context.LocalizationString </Button>
           </CancelCommandTemplate>
        </DataGridCommandColumn>";

   public string GridNodeEnd() => @$"
    </DataGridColumns>
    <ButtonRowTemplate>
         <Button Color=""Color.Success"" Clicked=""@context.NewCommand.Clicked"">@context.NewCommand.LocalizationString</Button>
         <Button Color=""Color.Primary"" Clicked=""@context.EditCommand.Clicked"" Disabled=""@(selectedAccount is null)"">@context.EditCommand.LocalizationString</Button>
         <Button Color=""Color.Danger"" Clicked=""@context.DeleteCommand.Clicked"" Disabled=""@(selectedAccount is null)"">@context.DeleteCommand.LocalizationString</Button>
         <Button Color=""Color.Warning"" Clicked=""@context.ClearFilterCommand.Clicked"">@context.ClearFilterCommand.LocalizationString</Button>
    </ButtonRowTemplate>
</DataGrid>
";

    public string Code() => @$"
@code {{
    private List<{Name}DTO>? {Name}sList {{ get; set; }}
    private {Name}DTO selected{Name};

    protected override async Task OnInitializedAsync()
    {{
        {Name}sList = (await {Name}Service.GetAll{Name}s()).ToList() ?? null;

        await base.OnInitializedAsync();
    }}

    async Task OnRowInserted(SavedRowItem<{Name}DTO, Dictionary<string, object>> e){{
        Console.WriteLine($""{{e.Item.Name}} Created"");
    }}
    
    async Task OnRowUpdated(SavedRowItem<{Name}DTO, Dictionary<string, object>> e){{
        Console.WriteLine($""{{e.Item.Name}} Saved"");
    }}

}}";
}
