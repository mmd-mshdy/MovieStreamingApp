using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieStreaming.Migrations
{
    /// <inheritdoc />
    public partial class Beforeseeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_WatchList_WatchListId",
                table: "Movies");

            migrationBuilder.DropForeignKey(
                name: "FK_WatchList_Users_UserId",
                table: "WatchList");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WatchList",
                table: "WatchList");

            migrationBuilder.RenameTable(
                name: "WatchList",
                newName: "WatchLists");

            migrationBuilder.RenameIndex(
                name: "IX_WatchList_UserId",
                table: "WatchLists",
                newName: "IX_WatchLists_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WatchLists",
                table: "WatchLists",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_WatchLists_WatchListId",
                table: "Movies",
                column: "WatchListId",
                principalTable: "WatchLists",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WatchLists_Users_UserId",
                table: "WatchLists",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_WatchLists_WatchListId",
                table: "Movies");

            migrationBuilder.DropForeignKey(
                name: "FK_WatchLists_Users_UserId",
                table: "WatchLists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WatchLists",
                table: "WatchLists");

            migrationBuilder.RenameTable(
                name: "WatchLists",
                newName: "WatchList");

            migrationBuilder.RenameIndex(
                name: "IX_WatchLists_UserId",
                table: "WatchList",
                newName: "IX_WatchList_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WatchList",
                table: "WatchList",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_WatchList_WatchListId",
                table: "Movies",
                column: "WatchListId",
                principalTable: "WatchList",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WatchList_Users_UserId",
                table: "WatchList",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
