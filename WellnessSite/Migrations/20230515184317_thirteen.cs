using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WellnessSite.Migrations
{
    public partial class thirteen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Answer1",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Answer2",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Question1",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Question2",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Answer1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Answer2",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Question1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Question2",
                table: "AspNetUsers");
        }
    }
}
