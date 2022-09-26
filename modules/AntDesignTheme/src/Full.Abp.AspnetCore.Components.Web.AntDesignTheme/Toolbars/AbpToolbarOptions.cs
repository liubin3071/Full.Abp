using System.Collections.Generic;
using JetBrains.Annotations;
using Full.Abp.AspnetCore.Components.Web.AntDesignTheme.Toolbars;

namespace Full.Abp.AspnetCore.Components.Web.AntDesignTheme.Toolbars;

public class AbpToolbarOptions
{
    [NotNull]
    public List<IToolbarContributor> Contributors { get; }

    public AbpToolbarOptions()
    {
        Contributors = new List<IToolbarContributor>();
    }
}
