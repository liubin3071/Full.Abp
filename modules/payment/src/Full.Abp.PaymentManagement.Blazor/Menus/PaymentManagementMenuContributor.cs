﻿using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;

namespace Full.Abp.PaymentManagement.Blazor.Menus;

public class PaymentManagementMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        //Add main menu items.
        context.Menu.AddItem(new ApplicationMenuItem(PaymentManagementMenus.Prefix, displayName: "PaymentManagement", "/PaymentManagement", icon: "fa fa-globe"));

        return Task.CompletedTask;
    }
}
