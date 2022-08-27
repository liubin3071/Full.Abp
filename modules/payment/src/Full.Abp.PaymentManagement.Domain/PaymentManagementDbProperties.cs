namespace Full.Abp.PaymentManagement;

public static class PaymentManagementDbProperties
{
    public static string DbTablePrefix { get; set; } = "PaymentManagement";

    public static string DbSchema { get; set; } = null;

    public const string ConnectionStringName = "PaymentManagement";
}
