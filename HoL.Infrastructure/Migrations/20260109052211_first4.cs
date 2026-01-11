using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoL.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class first4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SingleCurrencies_CurrencyGroups_CurrencyGroupId",
                table: "SingleCurrencies");

            migrationBuilder.DropIndex(
                name: "IX_SingleCurrencies_CurrencyGroupId",
                table: "SingleCurrencies");

            migrationBuilder.DropColumn(
                name: "CurrencyGroupId",
                table: "SingleCurrencies");

            migrationBuilder.CreateTable(
                name: "CurrencyGroupSingleCurrency",
                columns: table => new
                {
                    CurrencyGroupId = table.Column<int>(type: "int", nullable: false),
                    SingleCurrencyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyGroupSingleCurrency", x => new { x.CurrencyGroupId, x.SingleCurrencyId });
                    table.ForeignKey(
                        name: "FK_CurrencyGroupSingleCurrency_CurrencyGroups_CurrencyGroupId",
                        column: x => x.CurrencyGroupId,
                        principalTable: "CurrencyGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CurrencyGroupSingleCurrency_SingleCurrencies_SingleCurrencyId",
                        column: x => x.SingleCurrencyId,
                        principalTable: "SingleCurrencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SingleCurrencies_HierarchyLevel",
                table: "SingleCurrencies",
                column: "HierarchyLevel");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyGroups_GroupName",
                table: "CurrencyGroups",
                column: "GroupName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyGroupSingleCurrency_SingleCurrencyId",
                table: "CurrencyGroupSingleCurrency",
                column: "SingleCurrencyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrencyGroupSingleCurrency");

            migrationBuilder.DropIndex(
                name: "IX_SingleCurrencies_HierarchyLevel",
                table: "SingleCurrencies");

            migrationBuilder.DropIndex(
                name: "IX_CurrencyGroups_GroupName",
                table: "CurrencyGroups");

            migrationBuilder.AddColumn<int>(
                name: "CurrencyGroupId",
                table: "SingleCurrencies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SingleCurrencies_CurrencyGroupId",
                table: "SingleCurrencies",
                column: "CurrencyGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_SingleCurrencies_CurrencyGroups_CurrencyGroupId",
                table: "SingleCurrencies",
                column: "CurrencyGroupId",
                principalTable: "CurrencyGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
