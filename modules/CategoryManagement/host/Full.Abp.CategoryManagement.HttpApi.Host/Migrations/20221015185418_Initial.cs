using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Full.Abp.CategoryManagement.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoryManagementCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryManagementCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoryManagementCategoryRelations",
                columns: table => new
                {
                    Ancestor = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Descendant = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProviderType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Distance = table.Column<int>(type: "int", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    IsRoot = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryManagementCategoryRelations", x => new { x.ProviderType, x.ProviderName, x.ProviderKey, x.Ancestor, x.Descendant });
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryManagementCategoryRelations_Distance",
                table: "CategoryManagementCategoryRelations",
                column: "Distance");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryManagementCategoryRelations_ProviderType_ProviderName_ProviderKey_Descendant",
                table: "CategoryManagementCategoryRelations",
                columns: new[] { "ProviderType", "ProviderName", "ProviderKey", "Descendant" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryManagementCategories");

            migrationBuilder.DropTable(
                name: "CategoryManagementCategoryRelations");
        }
    }
}
