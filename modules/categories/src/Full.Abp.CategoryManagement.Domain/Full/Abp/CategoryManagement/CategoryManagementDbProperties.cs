namespace Full.Abp.CategoryManagement;

public static class CategoryManagementDbProperties
{
    public static string DbTablePrefix { get; set; } = "CategoryManagement";

    public static string DbSchema { get; set; } = null;

    public const string ConnectionStringName = "CategoryManagement";
}