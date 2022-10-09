using AutoMapper;
using Full.Abp.Categories;

namespace Full.Abp.CategoryManagement;

public class CategoryManagementDomainAutoMapperProfile : Profile
{
    public CategoryManagementDomainAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<CategoryInfo, Category>(MemberList.Source);
        CreateMap<Category, CategoryInfo>(MemberList.Destination);
    }
}
