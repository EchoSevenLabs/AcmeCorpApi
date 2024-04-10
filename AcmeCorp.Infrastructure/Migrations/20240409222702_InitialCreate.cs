using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcmeCorp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "customer",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    middle_name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    last_name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    archive = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    modified_by = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    modified_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "address",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    customer_id = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<int>(type: "int", nullable: false),
                    street_1 = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    street_2 = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    city = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    state_province = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    postal_code = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    country = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    archive = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    modified_by = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    modified_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_address", x => x.id);
                    table.ForeignKey(
                        name: "FK_address_customer",
                        column: x => x.customer_id,
                        principalTable: "customer",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "order",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    customer_id = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    ship_method = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    date_shipped = table.Column<DateTime>(type: "datetime", nullable: true),
                    archive = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    modified_by = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    modified_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order", x => x.id);
                    table.ForeignKey(
                        name: "FK_order_customer",
                        column: x => x.customer_id,
                        principalTable: "customer",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_address_customer_id",
                table: "address",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_customer_id",
                table: "order",
                column: "customer_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "address");

            migrationBuilder.DropTable(
                name: "order");

            migrationBuilder.DropTable(
                name: "customer");
        }
    }
}
