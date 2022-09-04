using Full.Abp.AspNetCore.Components.Server.MudTheme.Themes.Mud;
using Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars;

namespace Full.Abp.AspNetCore.Components.Server.MudTheme;

public class MudThemeToolbarContributor : IToolbarContributor
{
    public Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
    {
        if (context.Toolbar.Name == StandardToolbars.Main)
        {
            context.Toolbar.Items.Add(new ToolbarItem(typeof(LanguageSwitch)));
            context.Toolbar.Items.Add(new ToolbarItem(typeof(LoginDisplay)));
        }

        return Task.CompletedTask;
    }
}