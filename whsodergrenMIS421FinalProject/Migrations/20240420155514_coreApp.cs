using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace whsodergrenMIS421FinalProject.Migrations
{
    /// <inheritdoc />
    public partial class coreApp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "Transaction",
                type: "nvarchar(75)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "Transaction",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(75)",
                oldNullable: true);
        }
    }
}
