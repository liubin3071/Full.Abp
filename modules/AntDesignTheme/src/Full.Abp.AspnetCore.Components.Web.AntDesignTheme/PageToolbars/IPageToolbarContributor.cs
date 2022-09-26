using System.Threading.Tasks;
using Full.Abp.AspnetCore.Components.Web.AntDesignTheme.PageToolbars;

namespace Full.Abp.AspnetCore.Components.Web.AntDesignTheme.PageToolbars;

public interface IPageToolbarContributor
{
    Task ContributeAsync(PageToolbarContributionContext context);
}
