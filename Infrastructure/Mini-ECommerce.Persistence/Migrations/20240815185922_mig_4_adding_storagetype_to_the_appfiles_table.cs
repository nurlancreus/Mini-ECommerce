using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mini_ECommerce.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_4_adding_storagetype_to_the_appfiles_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "ApplicationFiles",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "ApplicationFiles",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StorageType",
                table: "ApplicationFiles",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "ApplicationFiles");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "ApplicationFiles");

            migrationBuilder.DropColumn(
                name: "StorageType",
                table: "ApplicationFiles");
        }
    }
}
