﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Full.Abp.Categories;
using Microsoft.AspNetCore.Components;

namespace Full.Abp.CategoryManagement.Blazor.AntDesignUI.Pages;

public partial class StaticCategorySelect
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

    public List<CategoryDto> Roots { get; set; }

    CategoryDto[] treeData = new CategoryDto[]
    {
        new()
        {
            Name = "Node1",
            Id = Guid.NewGuid(),
            Children = new CategoryDto[]
            {
                new()
                {
                    Name = "Child Node1",
                    Id = Guid.NewGuid(),
                },
                new()
                {
                    Name = "Child Node2",
                    Id = Guid.NewGuid(),
                }
            }
        },
        new()
        {
            Name = "Node2",
            Id = Guid.NewGuid(),
        }
    };

}