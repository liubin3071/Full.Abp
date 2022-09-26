using System.Threading.Tasks;

namespace Full.Abp.AspnetCore.Components.Web.AntDesignTheme.Toolbars;

public interface IToolbarContributor
{
    Task ConfigureToolbarAsync(IToolbarConfigurationContext context);
}
