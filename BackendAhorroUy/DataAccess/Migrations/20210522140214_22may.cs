using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

namespace DataAccess.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class _22may : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Market");

            migrationBuilder.AddColumn<float>(
                name: "Latitude",
                table: "Market",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Longitude",
                table: "Market",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Market");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Market");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Market",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
