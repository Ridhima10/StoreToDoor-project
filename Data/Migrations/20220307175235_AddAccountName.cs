using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreToDoor.Data.Migrations
{
    public partial class AddAccountName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountName",
                table: "AspNetUsers");
        }
    }
}
