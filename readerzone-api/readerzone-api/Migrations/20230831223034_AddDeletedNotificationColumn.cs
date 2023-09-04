using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace readerzone_api.Migrations
{
    /// <inheritdoc />
    public partial class AddDeletedNotificationColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Notifications");
        }
    }
}
