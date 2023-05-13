using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WellnessSite.Migrations
{
    public partial class six : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageAltText",
                table: "Service",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "Service",
                type: "varbinary(max)",
                nullable: true,
                defaultValue: new byte[0]);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageAltText",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "Service");
        }
    }
}
