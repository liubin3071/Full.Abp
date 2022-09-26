using AntDesign;
using Full.Abp.AspnetCore.Components.Web.AntDesignTheme.Settings;

namespace Full.Abp.AspnetCore.Components.Web.AntDesignTheme;

public class AbpAntDesignThemeOptions
{
    public MenuOptions Menu { get; set; }

    public AbpAntDesignThemeOptions()
    {
        Menu = new MenuOptions();
    }
}

public class MenuOptions
{
    public MenuTheme Theme { get; set; }

    public MenuPlacement Placement { get; set; }

    public MenuOptions()
    {
        Theme = MenuTheme.Dark;
        Placement = MenuPlacement.Left;
    }
}
