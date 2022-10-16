using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Full.Abp.FinancialManagement.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FinancialManagementAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProviderName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Balance = table.Column<decimal>(type: "DECIMAL(19,4)", nullable: false),
                    LatestEntryIndex = table.Column<int>(type: "int", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialManagementAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FinancialManagementAccountEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "DECIMAL(19,4)", nullable: false),
                    PostBalance = table.Column<decimal>(type: "DECIMAL(19,4)", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialManagementAccountEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancialManagementAccountEntries_FinancialManagementAccounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "FinancialManagementAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinancialManagementAccountEntries_AccountId_Index",
                table: "FinancialManagementAccountEntries",
                columns: new[] { "AccountId", "Index" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FinancialManagementAccounts_TenantId_ProviderName_ProviderKey_Name",
                table: "FinancialManagementAccounts",
                columns: new[] { "TenantId", "ProviderName", "ProviderKey", "Name" },
                unique: true,
                filter: "[TenantId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinancialManagementAccountEntries");

            migrationBuilder.DropTable(
                name: "FinancialManagementAccounts");
        }
    }
}
