using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WellnessSite.Migrations
{
    public partial class five : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Times",
                table: "Service",
                newName: "Town");

            migrationBuilder.RenameColumn(
                name: "Referral",
                table: "Service",
                newName: "Postcode");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "Service",
                newName: "Other");

            migrationBuilder.RenameColumn(
                name: "Aim",
                table: "Service",
                newName: "Category");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Service",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Service");

            migrationBuilder.RenameColumn(
                name: "Town",
                table: "Service",
                newName: "Times");

            migrationBuilder.RenameColumn(
                name: "Postcode",
                table: "Service",
                newName: "Referral");

            migrationBuilder.RenameColumn(
                name: "Other",
                table: "Service",
                newName: "Location");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Service",
                newName: "Aim");
        }
    }
}
