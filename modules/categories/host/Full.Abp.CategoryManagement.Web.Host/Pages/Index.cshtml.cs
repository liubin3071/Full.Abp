using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace Full.Abp.CategoryManagement.Pages;

public class IndexModel : CategoryManagementPageModel
{
    public void OnGet()
    {

    }

    public async Task OnPostLoginAsync()
    {
        await HttpContext.ChallengeAsync("oidc");
    }
}
