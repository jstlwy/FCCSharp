using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FCCSharp.Migrations.ShortUrlDb
{
    /// <inheritdoc />
    public partial class SimplifyShortUrlEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortUrl",
                table: "ShortUrls");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShortUrl",
                table: "ShortUrls",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
