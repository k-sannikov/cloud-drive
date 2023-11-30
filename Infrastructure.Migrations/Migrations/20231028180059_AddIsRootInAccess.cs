using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddIsRootInAccess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "is_owner",
                table: "access",
                newName: "is_root");

            migrationBuilder.AddColumn<bool>(
                name: "IsRoot",
                table: "access",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRoot",
                table: "access");

            migrationBuilder.RenameColumn(
                name: "is_root",
                table: "access",
                newName: "is_owner");
        }
    }
}
