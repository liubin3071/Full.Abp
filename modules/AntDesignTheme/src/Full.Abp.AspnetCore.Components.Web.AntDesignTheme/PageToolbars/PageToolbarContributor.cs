using System.Threading.Tasks;
using Full.Abp.AspnetCore.Components.Web.AntDesignTheme.PageToolbars;

namespace Full.Abp.AspnetCore.Components.Web.AntDesignTheme.PageToolbars;

public abstract class PageToolbarContributor : IPageToolbarContributor
{
    public abstract Task ContributeAsync(PageToolbarContributionContext context);
}
