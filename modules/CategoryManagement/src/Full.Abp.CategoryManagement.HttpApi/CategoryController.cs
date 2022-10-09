using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Full.Abp.Categories;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Full.Abp.CategoryManagement;

[RemoteService(Name = CategoryManagementRemoteServiceConsts.RemoteServiceName)]
[Area(CategoryManagementRemoteServiceConsts.ModuleName)]
[ControllerName("Category")]
[Route("api/CategoryManagement/Categories")]
public class CategoryController : CategoryManagementController, ICategoryAppService
{
    private readonly ICategoryAppService _categoryAppServiceImplementation;

    public CategoryController(ICategoryAppService categoryAppServiceImplementation)
    {
        _categoryAppServiceImplementation = categoryAppServiceImplementation;
    }

    [HttpGet]
    [Route("{definitionName}/{id:guid}")]
    public Task<CategoryDto> GetAsync(string definitionName, Guid id)
    {
        return _categoryAppServiceImplementation.GetAsync(definitionName, id);
    }

    [HttpGet]
    [Route("Ancestors/{definitionName}/{id:guid}")]
    public Task<ListResultDto<CategoryDto>> GetAncestorsAsync(string definitionName, Guid id)
    {
        return _categoryAppServiceImplementation.GetAncestorsAsync(definitionName, id);
    }

    [HttpGet]
    [Route("Parent/{definitionName}/{id:guid}")]
    public Task<CategoryDto?> GetParentAsync(string definitionName, Guid id)
    {
        return _categoryAppServiceImplementation.GetParentAsync(definitionName, id);
    }

    [HttpGet]
    [Route("Descendants/{definitionName}")]
    public Task<ListResultDto<CategoryDto>> GetDescendantsAsync(string definitionName, Guid? id = default,
        int? maxDistance = null)
    {
        return _categoryAppServiceImplementation.GetDescendantsAsync(definitionName, id, maxDistance);
    }

    [HttpGet]
    [Route("Tree/{definitionName}")]
    public Task<IEnumerable<CategoryDto>> GetTreeAsync(string definitionName)
    {
        return _categoryAppServiceImplementation.GetTreeAsync(definitionName);
    }

    [HttpGet]
    [Route("HasChildren/{definitionName}")]
    public Task<bool> HasChildrenAsync(string definitionName, Guid? id= default)
    {
        return _categoryAppServiceImplementation.HasChildrenAsync(definitionName, id);
    }

    [HttpGet]
    [Route("Children/Count/{definitionName}")]
    public Task<int> GetChildrenCountAsync(string definitionName, Guid? id = default)
    {
        return _categoryAppServiceImplementation.GetChildrenCountAsync(definitionName, id);
    }

    [HttpGet]
    [Route("Children/{definitionName}")]
    public Task<ListResultDto<CategoryDto>> GetChildrenAsync(string definitionName, Guid? id = default)
    {
        return _categoryAppServiceImplementation.GetChildrenAsync(definitionName, id);
    }

    [HttpGet]
    [Route("Children/Page/{definitionName}")]
    public Task<PagedResultDto<CategoryDto>> GetPagedChildrenAsync(string definitionName, Guid? id,
        CategoryGetChildrenInput input)
    {
        return _categoryAppServiceImplementation.GetPagedChildrenAsync(definitionName, id, input);
    }

    [HttpPost]
    [Route("{definitionName}")]
    public Task<CategoryDto> CreateAsync(string definitionName, CategoryCreateOrUpdateInput input)
    {
        return _categoryAppServiceImplementation.CreateAsync(definitionName, input);
    }

    [HttpPut]
    [Route("{definitionName}/{id:guid}")]
    public Task<CategoryDto> UpdateAsync(string definitionName, Guid id, CategoryCreateOrUpdateInput input)
    {
        return _categoryAppServiceImplementation.UpdateAsync(definitionName, id, input);
    }

    [HttpDelete]
    [Route("{definitionName}/{id:guid}")]
    public Task DeleteAsync(string definitionName, Guid id)
    {
        return _categoryAppServiceImplementation.DeleteAsync(definitionName, id);
    }

    [HttpDelete]
    [Route("Batch/{definitionName}")]
    public Task DeleteManyAsync(string definitionName, IEnumerable<Guid> ids)
    {
        return _categoryAppServiceImplementation.DeleteManyAsync(definitionName, ids);
    }
}