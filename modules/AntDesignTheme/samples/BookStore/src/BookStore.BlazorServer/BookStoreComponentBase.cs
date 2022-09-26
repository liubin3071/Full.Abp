using BookStore.Localization;
using Volo.Abp.AspNetCore.Components;

namespace BookStore.BlazorServer;

public abstract class BookStoreComponentBase : AbpComponentBase
{
    protected BookStoreComponentBase()
    {
        LocalizationResource = typeof(BookStoreResource);
    }
}
