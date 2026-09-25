using Microsoft.EntityFrameworkCore.Migrations;

namespace GiftOfTheGivers.Data.Migrations
{
    public partial class RenameProjectUpdateId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Rename column if it exists; RenameColumn will work for many providers.
            migrationBuilder.RenameColumn(
                name: "UpdateID",
                table: "ProjectUpdates",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ProjectUpdates",
                newName: "UpdateID");
        }
    }
}
