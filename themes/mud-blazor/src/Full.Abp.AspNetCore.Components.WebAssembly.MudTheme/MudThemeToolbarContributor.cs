using Full.Abp.AspNetCore.Components.WebAssembly.MudTheme.Themes.Mud;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars;

namespace Full.Abp.AspNetCore.Components.WebAssembly.MudTheme;

public class MudThemeToolbarContributor : IToolbarContributor
{
    public Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
    {
        if (context.Toolbar.Name == StandardToolbars.Main)
        {
            context.Toolbar.Items.Add(new ToolbarItem(typeof(LanguageSwitch)));

            //TODO: Can we find a different way to understand if authentication was configured or not?
            var authenticationStateProvider = context.ServiceProvider
                .GetService<AuthenticationStateProvider>();

            if (authenticationStateProvider != null)
            {
                context.Toolbar.Items.Add(new ToolbarItem(typeof(LoginDisplay)));
            }
        }

        return Task.CompletedTask;
    }
}