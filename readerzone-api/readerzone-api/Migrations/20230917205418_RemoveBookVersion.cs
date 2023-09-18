using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace readerzone_api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBookVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "Books");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "Books",
                type: "bigint",
                rowVersion: true,
                nullable: false,
                defaultValue: 0L);
        }
    }
}
