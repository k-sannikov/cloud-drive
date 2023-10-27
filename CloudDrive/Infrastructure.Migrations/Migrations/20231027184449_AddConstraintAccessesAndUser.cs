using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddConstraintAccessesAndUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_access_user_id",
                table: "access",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_access_user_user_id",
                table: "access",
                column: "user_id",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_access_user_user_id",
                table: "access");

            migrationBuilder.DropIndex(
                name: "IX_access_user_id",
                table: "access");
        }
    }
}
