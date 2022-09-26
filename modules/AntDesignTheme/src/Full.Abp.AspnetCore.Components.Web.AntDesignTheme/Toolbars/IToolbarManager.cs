using System.Threading.Tasks;

namespace Full.Abp.AspnetCore.Components.Web.AntDesignTheme.Toolbars;

public interface IToolbarManager
{
    Task<Toolbar> GetAsync(string name);
}
