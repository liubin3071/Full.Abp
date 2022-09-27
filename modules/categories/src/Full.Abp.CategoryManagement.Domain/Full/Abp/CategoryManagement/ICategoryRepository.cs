using System;
using System.Threading.Tasks;
using Full.Abp.Trees;

namespace Full.Abp.CategoryManagement;

public interface ICategoryRepository:ITreeRepository<Category,Guid>
{
}