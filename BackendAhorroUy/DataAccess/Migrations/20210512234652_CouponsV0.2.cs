using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    [ExcludeFromCodeCoverage]

    public partial class CouponsV02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Coupons");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "UserSessions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserSessions");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Coupons",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Coupons",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
