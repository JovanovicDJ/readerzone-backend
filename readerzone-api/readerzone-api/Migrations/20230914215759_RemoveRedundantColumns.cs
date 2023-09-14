using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace readerzone_api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRedundantColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnnualChallenge",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AnnualChallengeProgress",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FinalPrice",
                table: "PurchasedBooks");

            migrationBuilder.DropColumn(
                name: "Likes",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Likes",
                table: "Comments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnnualChallenge",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AnnualChallengeProgress",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FinalPrice",
                table: "PurchasedBooks",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "Posts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
