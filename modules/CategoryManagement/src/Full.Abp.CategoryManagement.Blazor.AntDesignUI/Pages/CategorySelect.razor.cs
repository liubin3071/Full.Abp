using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Full.Abp.Categories;
using Microsoft.AspNetCore.Components;

namespace Full.Abp.CategoryManagement.Blazor.AntDesignUI.Pages;

public partial class CategorySelect
{
    [Parameter]
    public Guid? SelectedId
    {
        get
        {
            if (Guid.TryParse(_value, out var guid))
            {
                return guid;
            }
            return null;
        }
        set => _value = value.ToString();
    }

    [Parameter]
    public EventCallback<Guid?> SelectedIdChanged { get; set; }

    [Inject]
    public ICategoryAppService CategoryAppService { get; set; }

    private string? _value;

    private void ValueChanged(string value)
    {
        if (Guid.TryParse(value, out var guid))
        {
            SelectedIdChanged.InvokeAsync(guid);
        }
        else
        {
            SelectedIdChanged.InvokeAsync(null);
        }
    }


    [Parameter]
    public string DefinitionName { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Roots = await CategoryAppService.GetTreeAsync(DefinitionName);
    }

    public IEnumerable<CategoryDto> Roots { get; set; }
}