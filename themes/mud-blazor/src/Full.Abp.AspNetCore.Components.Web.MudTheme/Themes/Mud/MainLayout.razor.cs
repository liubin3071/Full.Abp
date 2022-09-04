﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Full.Abp.AspNetCore.Components.Web.MudTheme.Themes.Mud;

public partial class MainLayout : IDisposable
{
    [Inject] private NavigationManager NavigationManager { get; set; }

    private bool IsCollapseShown { get; set; }

    protected override void OnInitialized()
    {
        NavigationManager.LocationChanged += OnLocationChanged;
    }

    private void ToggleCollapse()
    {
        IsCollapseShown = !IsCollapseShown;
    }

    public void Dispose()
    {
        NavigationManager.LocationChanged -= OnLocationChanged;
    }

    private void OnLocationChanged(object sender, LocationChangedEventArgs e)
    {
        IsCollapseShown = false;
        InvokeAsync(StateHasChanged);
    }
}
