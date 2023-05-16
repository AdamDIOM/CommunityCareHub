using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WellnessSite.Migrations
{
    public partial class fourteen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.RenameColumn(
                name: "TextSize",
                table: "Preferences",
                newName: "FontSize");

            migrationBuilder.RenameColumn(
                name: "Hex1",
                table: "Preferences",
                newName: "HexTextColour");

            migrationBuilder.RenameColumn(
                name: "Hex2",
                table: "Preferences",
                newName: "HexColour2");

            migrationBuilder.RenameColumn(
                name: "UID",
                table: "Bookmarks",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "SID",
                table: "Bookmarks",
                newName: "ServiceID");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Bookmarks",
                newName: "BookmarkID");

            migrationBuilder.AlterColumn<string>(
                name: "Question2",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Question1",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Answer2",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Answer1",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HexTextColour",
                table: "Preferences",
                newName: "Hex1");

            migrationBuilder.RenameColumn(
                name: "HexColour2",
                table: "Preferences",
                newName: "Hex2");

            migrationBuilder.RenameColumn(
                name: "FontSize",
                table: "Preferences",
                newName: "TextSize");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Bookmarks",
                newName: "UID");

            migrationBuilder.RenameColumn(
                name: "ServiceID",
                table: "Bookmarks",
                newName: "SID");

            migrationBuilder.RenameColumn(
                name: "BookmarkID",
                table: "Bookmarks",
                newName: "ID");

            migrationBuilder.AlterColumn<string>(
                name: "Question2",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Question1",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Answer2",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Answer1",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            
        }
    }
}
