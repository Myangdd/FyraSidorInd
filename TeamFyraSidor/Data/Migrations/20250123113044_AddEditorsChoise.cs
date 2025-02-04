using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamFyraSidor.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEditorsChoise : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EditorsChoise",
                table: "Articles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EditorsChoise",
                table: "Articles");
        }
    }
}
