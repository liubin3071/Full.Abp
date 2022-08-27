using Volo.Abp.Reflection;

namespace Full.Abp.PaymentManagement.Permissions;

public class PaymentManagementPermissions
{
    public const string GroupName = "PaymentManagement";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(PaymentManagementPermissions));
    }
}
