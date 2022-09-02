using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Full.Abp.AspNetCore.Components.Web.BasicTheme.Themes.Basic;

public partial class MainLayout : IDisposable
{
    [CascadingParameter]
    protected Theme Theme { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    protected override void OnInitialized()
    {
        NavigationManager.LocationChanged += OnLocationChanged;
    }

    public void Dispose()
    {
        NavigationManager.LocationChanged -= OnLocationChanged;
    }

    private void OnLocationChanged(object sender, LocationChangedEventArgs e)
    {
        InvokeAsync(StateHasChanged);
    }
}