using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    [ExcludeFromCodeCoverage]

    public partial class CouponsV03 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Market",
                table: "Coupons");

            migrationBuilder.AddColumn<string>(
                name: "LogoCoupon",
                table: "Market",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MarketId",
                table: "Coupons",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Value",
                table: "Coupons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_MarketId",
                table: "Coupons",
                column: "MarketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Coupons_Market_MarketId",
                table: "Coupons",
                column: "MarketId",
                principalTable: "Market",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coupons_Market_MarketId",
                table: "Coupons");

            migrationBuilder.DropIndex(
                name: "IX_Coupons_MarketId",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "LogoCoupon",
                table: "Market");

            migrationBuilder.DropColumn(
                name: "MarketId",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Coupons");

            migrationBuilder.AddColumn<Guid>(
                name: "Market",
                table: "Coupons",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
