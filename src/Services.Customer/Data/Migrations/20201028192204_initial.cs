using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Services.Customer.Data.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "customer");

            migrationBuilder.CreateTable(
                name: "customers",
                schema: "customer",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    email = table.Column<string>(nullable: true),
                    first_name = table.Column<string>(nullable: true),
                    last_name = table.Column<string>(nullable: true),
                    address = table.Column<string>(nullable: true),
                    phone_number = table.Column<string>(nullable: true),
                    birth_date = table.Column<DateTime>(nullable: true),
                    gender = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "outbox_events",
                schema: "customer",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    aggregate_type = table.Column<string>(nullable: true),
                    aggregate_id = table.Column<Guid>(nullable: false),
                    type = table.Column<string>(nullable: true),
                    payload = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_outbox_events", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "customers",
                schema: "customer");

            migrationBuilder.DropTable(
                name: "outbox_events",
                schema: "customer");
        }
    }
}
