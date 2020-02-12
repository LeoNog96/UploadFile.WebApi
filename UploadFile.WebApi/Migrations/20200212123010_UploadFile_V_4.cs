using Microsoft.EntityFrameworkCore.Migrations;

namespace UploadFile.WebApi.Migrations
{
    public partial class UploadFile_V_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "removed",
                table: "users",
                nullable: false,
                defaultValueSql: "false");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "removed",
                table: "users");
        }
    }
}
