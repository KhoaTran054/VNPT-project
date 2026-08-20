using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductWebApi.Migrations
{
    /// <inheritdoc />
    public partial class VNPTmigration_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PRODUCT_CATEGORY_CATEGORY_ID",
                table: "PRODUCT");

            migrationBuilder.CreateTable(
                name: "USERS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    USERNAME = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PASSWORD_HASH = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ROLE = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USERS", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_PRODUCT_CATEGORY_CATEGORY_ID",
                table: "PRODUCT",
                column: "CATEGORY_ID",
                principalTable: "CATEGORY",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PRODUCT_CATEGORY_CATEGORY_ID",
                table: "PRODUCT");

            migrationBuilder.DropTable(
                name: "USERS");

            migrationBuilder.AddForeignKey(
                name: "FK_PRODUCT_CATEGORY_CATEGORY_ID",
                table: "PRODUCT",
                column: "CATEGORY_ID",
                principalTable: "CATEGORY",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
