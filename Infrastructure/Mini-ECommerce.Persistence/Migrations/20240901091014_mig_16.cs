using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mini_ECommerce.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_16 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppEndpoints",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Menu = table.Column<string>(type: "text", nullable: false),
                    Action = table.Column<string>(type: "text", nullable: false),
                    HttpMethod = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Definition = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppEndpoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppEndpointAppRole",
                columns: table => new
                {
                    EndpointsId = table.Column<Guid>(type: "uuid", nullable: false),
                    RolesId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppEndpointAppRole", x => new { x.EndpointsId, x.RolesId });
                    table.ForeignKey(
                        name: "FK_AppEndpointAppRole_AppEndpoints_EndpointsId",
                        column: x => x.EndpointsId,
                        principalTable: "AppEndpoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppEndpointAppRole_AspNetRoles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppEndpointAppRole_RolesId",
                table: "AppEndpointAppRole",
                column: "RolesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppEndpointAppRole");

            migrationBuilder.DropTable(
                name: "AppEndpoints");
        }
    }
}
