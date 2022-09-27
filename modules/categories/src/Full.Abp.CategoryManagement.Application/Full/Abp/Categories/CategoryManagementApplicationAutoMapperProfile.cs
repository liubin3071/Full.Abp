using AutoMapper;
using Full.Abp.Categories;
using Volo.Abp.AutoMapper;

namespace Full.Abp.CategoryManagement.Full.Abp.Categories;

public class CategoryManagementApplicationAutoMapperProfile : Profile
{
    public CategoryManagementApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<CategoryCreateInput, Category>(MemberList.Source)
            .ForSourceMember(input => input.ParentId, expression => expression.DoNotValidate());
            ;
        CreateMap<CategoryUpdateInput, Category>(MemberList.Source)
            .ForSourceMember(input => input.ParentId, expression => expression.DoNotValidate());
            // .Ignore(category => category.TenantId)
            // .Ignore(category => category.ConcurrencyStamp)
            // ;
        CreateMap<Category, CategoryDto>(MemberList.Destination)
            .Ignore(c=>c.ParentId)
            .Ignore(c => c.Children);
    }
}
