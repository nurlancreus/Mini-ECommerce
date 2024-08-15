using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mini_ECommerce.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_5_adding_storagetype_as_string : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StorageType",
                table: "ApplicationFiles");

            migrationBuilder.AddColumn<string>(
                name: "Storage",
                table: "ApplicationFiles",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Storage",
                table: "ApplicationFiles");

            migrationBuilder.AddColumn<int>(
                name: "StorageType",
                table: "ApplicationFiles",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
