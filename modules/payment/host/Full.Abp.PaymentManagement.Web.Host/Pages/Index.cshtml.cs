using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace Full.Abp.PaymentManagement.Pages;

public class IndexModel : PaymentManagementPageModel
{
    public void OnGet()
    {

    }

    public async Task OnPostLoginAsync()
    {
        await HttpContext.ChallengeAsync("oidc");
    }
}
