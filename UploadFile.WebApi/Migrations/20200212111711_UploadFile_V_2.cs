using Microsoft.EntityFrameworkCore.Migrations;

namespace UploadFile.WebApi.Migrations
{
    public partial class UploadFile_V_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "file_name",
                table: "documents",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "file_name",
                table: "documents");
        }
    }
}
