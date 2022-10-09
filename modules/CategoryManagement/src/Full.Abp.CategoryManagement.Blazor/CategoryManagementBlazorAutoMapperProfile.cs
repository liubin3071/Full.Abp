using AutoMapper;
using Full.Abp.Categories;

namespace Full.Abp.CategoryManagement.Blazor;

public class CategoryManagementBlazorAutoMapperProfile : Profile
{
    public CategoryManagementBlazorAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<CategoryDto, CategoryCreateOrUpdateInput>(MemberList.Destination);
    }
}
