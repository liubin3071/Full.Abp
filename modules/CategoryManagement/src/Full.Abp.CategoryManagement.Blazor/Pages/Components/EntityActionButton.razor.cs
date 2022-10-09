using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Messages;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;

namespace Full.Abp.CategoryManagement.Blazor.Pages.Components;

public partial class EntityActionButton<TItem> : ComponentBase
{
    [Parameter] public EntityAction EntityAction { get; set; }
    [Parameter] public TItem Entity { get; set; }
    
    [Inject]
    protected IAuthorizationService AuthorizationService { get; set; }

    [Inject]
    protected IUiMessageService UiMessageService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        // await SetDefaultValuesAsync();
    }

    protected virtual async Task ActionClickedAsync()
    {
        if (EntityAction.ConfirmationMessage != null)
        {
            if (await UiMessageService.Confirm(EntityAction.ConfirmationMessage(Entity)))
            {
                await InvokeAsync(async () => await EntityAction.Clicked(Entity));
            }
        }
        else
        {
            await EntityAction.Clicked(Entity);
        }
    }

    // protected virtual ValueTask SetDefaultValuesAsync()
    // {
    //     Color = Color.Primary;
    //     return ValueTask.CompletedTask;
    // }
}
