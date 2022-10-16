namespace Full.Abp.FinancialManagement;

public static class FinancialManagementDbProperties
{
    public static string DbTablePrefix { get; set; } = "FinancialManagement";

    public static string DbSchema { get; set; } = null;

    public const string ConnectionStringName = "FinancialManagement";
}
