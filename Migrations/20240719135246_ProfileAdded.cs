using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IliaDabirkhane.Migrations
{
    /// <inheritdoc />
    public partial class ProfileAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Profile",
                table: "Users_tbl",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Profile",
                table: "Users_tbl");
        }
    }
}
