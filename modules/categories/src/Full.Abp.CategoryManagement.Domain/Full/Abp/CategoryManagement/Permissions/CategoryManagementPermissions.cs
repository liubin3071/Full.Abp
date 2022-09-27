using Volo.Abp.Reflection;

namespace Full.Abp.CategoryManagement.Permissions;

public class CategoryManagementPermissions
{
    public const string GroupName = "CategoryManagement";

    public static CategoryPermission Get(string categoryDefinitionName)
    {
        return new CategoryPermission($"{GroupName}.{categoryDefinitionName}");
    }

    public class CategoryPermission
    {
        public CategoryPermission(string defaultPermission)
        {
            Default = defaultPermission;
            Create = defaultPermission + ".Create";
            Update = defaultPermission + ".Update";
            Delete = defaultPermission + ".Delete";
        }

        public string Default { get; }
        public string Create { get; }
        public string Update { get; }
        public string Delete { get; }
    }


    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(CategoryManagementPermissions));
    }
}