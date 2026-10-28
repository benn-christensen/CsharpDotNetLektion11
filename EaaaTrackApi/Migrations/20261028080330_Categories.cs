using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EaaaTrackApi.Migrations
{
    /// <inheritdoc />
    public partial class Categories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubScriptions_SubscriptionCategory_SubscriptionCategoryId",
                table: "SubScriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubscriptionCategory",
                table: "SubscriptionCategory");

            migrationBuilder.RenameTable(
                name: "SubscriptionCategory",
                newName: "Categories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubScriptions_Categories_SubscriptionCategoryId",
                table: "SubScriptions",
                column: "SubscriptionCategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubScriptions_Categories_SubscriptionCategoryId",
                table: "SubScriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "SubscriptionCategory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubscriptionCategory",
                table: "SubscriptionCategory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubScriptions_SubscriptionCategory_SubscriptionCategoryId",
                table: "SubScriptions",
                column: "SubscriptionCategoryId",
                principalTable: "SubscriptionCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
