using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreToDoor.Data.Migrations
{
    public partial class UpdatePinField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "pin",
                table: "AspNetUsers",
                newName: "PinCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PinCode",
                table: "AspNetUsers",
                newName: "pin");
        }
    }
}
