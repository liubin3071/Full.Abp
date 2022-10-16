using Volo.Abp.Reflection;

namespace Full.Abp.FinancialManagement.Permissions;

public class FinancialManagementPermissions
{
    public const string GroupName = "FinancialManagement";

    public static class SystemAccounts
    {
        public const string Default = GroupName + ".SystemAccounts";
        public const string Increase = Default + "Increase";
        public const string Decrease = Default + "Decrease";
    }
    
    public static AccountPermission GetAccountManagementPermissions(string providerName, string name)
    {
        return new AccountPermission($"{GroupName}.Accounts.{providerName}.{name}");
    }

    public class AccountPermission
    {
        public AccountPermission(string defaultPermission)
        {
            Default = defaultPermission;
            Create = defaultPermission + ".Create";
            Update = defaultPermission + ".Update";
            Increase = defaultPermission + ".Increase";
            Decrease = defaultPermission + ".Decrease";
        }

        public string Default { get; }
        public string Create { get; }
        public string Update { get; }
        public string Increase { get; set; }
        public string Decrease { get; set; }
    }

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(FinancialManagementPermissions));
    }
}