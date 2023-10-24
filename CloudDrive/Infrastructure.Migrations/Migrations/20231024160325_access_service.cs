using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class access_service : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_access_NodeForAccess_node_id",
                table: "user_access");

            migrationBuilder.DropForeignKey(
                name: "FK_user_access_UserForAccess_user_id",
                table: "user_access");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserForAccess",
                table: "UserForAccess");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NodeForAccess",
                table: "NodeForAccess");

            migrationBuilder.RenameTable(
                name: "UserForAccess",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "NodeForAccess",
                newName: "Node");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Node",
                table: "Node",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_user_access_Node_node_id",
                table: "user_access",
                column: "node_id",
                principalTable: "Node",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_access_User_user_id",
                table: "user_access",
                column: "user_id",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_access_Node_node_id",
                table: "user_access");

            migrationBuilder.DropForeignKey(
                name: "FK_user_access_User_user_id",
                table: "user_access");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Node",
                table: "Node");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "UserForAccess");

            migrationBuilder.RenameTable(
                name: "Node",
                newName: "NodeForAccess");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserForAccess",
                table: "UserForAccess",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NodeForAccess",
                table: "NodeForAccess",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_user_access_NodeForAccess_node_id",
                table: "user_access",
                column: "node_id",
                principalTable: "NodeForAccess",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_access_UserForAccess_user_id",
                table: "user_access",
                column: "user_id",
                principalTable: "UserForAccess",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
