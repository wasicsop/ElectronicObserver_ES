using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronicObserver.Database
{
    public partial class AddEventPhases : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Phases",
                table: "EventLockPlans",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phases",
                table: "EventLockPlans");
        }
    }
}
