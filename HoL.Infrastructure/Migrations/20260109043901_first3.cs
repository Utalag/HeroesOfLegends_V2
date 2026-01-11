using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoL.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class first3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CurrencyGroupId",
                table: "SingleCurrencies",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_SingleCurrencies_Name",
                table: "SingleCurrencies",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SingleCurrencies_Name",
                table: "SingleCurrencies");

            migrationBuilder.AlterColumn<int>(
                name: "CurrencyGroupId",
                table: "SingleCurrencies",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
