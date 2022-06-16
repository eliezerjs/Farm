using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projeto.Avaliacao.API.Migrations
{
    public partial class AddDataToTelemetria : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Data",
                table: "Telemetrias",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                table: "Telemetrias");
        }
    }
}
