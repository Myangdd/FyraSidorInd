using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamFyraSidor.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNewsletterBoolToUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Newsletter",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Newsletter",
                table: "AspNetUsers");
        }
    }
}
