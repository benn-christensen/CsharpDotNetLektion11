using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EaaaTrackApi.Migrations
{
    /// <inheritdoc />
    public partial class BillingCycle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubScriptions_Categories_SubscriptionCategoryId",
                table: "SubScriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_SubScriptions_PaymentMethods_PaymentMethodId",
                table: "SubScriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubScriptions",
                table: "SubScriptions");

            migrationBuilder.RenameTable(
                name: "SubScriptions",
                newName: "Subscriptions");

            migrationBuilder.RenameIndex(
                name: "IX_SubScriptions_SubscriptionCategoryId",
                table: "Subscriptions",
                newName: "IX_Subscriptions_SubscriptionCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_SubScriptions_PaymentMethodId",
                table: "Subscriptions",
                newName: "IX_Subscriptions_PaymentMethodId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subscriptions",
                table: "Subscriptions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Categories_SubscriptionCategoryId",
                table: "Subscriptions",
                column: "SubscriptionCategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_PaymentMethods_PaymentMethodId",
                table: "Subscriptions",
                column: "PaymentMethodId",
                principalTable: "PaymentMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Categories_SubscriptionCategoryId",
                table: "Subscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_PaymentMethods_PaymentMethodId",
                table: "Subscriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subscriptions",
                table: "Subscriptions");

            migrationBuilder.RenameTable(
                name: "Subscriptions",
                newName: "SubScriptions");

            migrationBuilder.RenameIndex(
                name: "IX_Subscriptions_SubscriptionCategoryId",
                table: "SubScriptions",
                newName: "IX_SubScriptions_SubscriptionCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscriptions_PaymentMethodId",
                table: "SubScriptions",
                newName: "IX_SubScriptions_PaymentMethodId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubScriptions",
                table: "SubScriptions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubScriptions_Categories_SubscriptionCategoryId",
                table: "SubScriptions",
                column: "SubscriptionCategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubScriptions_PaymentMethods_PaymentMethodId",
                table: "SubScriptions",
                column: "PaymentMethodId",
                principalTable: "PaymentMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
