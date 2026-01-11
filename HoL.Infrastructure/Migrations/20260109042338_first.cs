using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoL.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CurrencyGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Races",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RaceName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RaceCategory = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BaseInitiative = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    RaceDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    RaceHistory = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Conviction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BaseXP = table.Column<int>(type: "int", nullable: false),
                    FightingSpiritNumber = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ZSM = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    DomesticationValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    BodyDimensins_RaceSize = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BodyDimensins_WeightMin = table.Column<int>(type: "int", nullable: false),
                    BodyDimensins_WeightMax = table.Column<int>(type: "int", nullable: false),
                    BodyDimensins_LengthMin = table.Column<int>(type: "int", nullable: false),
                    BodyDimensins_LengthMax = table.Column<int>(type: "int", nullable: false),
                    BodyDimensins_HeightMin = table.Column<int>(type: "int", nullable: false),
                    BodyDimensins_HeightMax = table.Column<int>(type: "int", nullable: false),
                    BodyDimensins_MaxAge = table.Column<int>(type: "int", nullable: false),
                    treasure_CoinQuantities = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    treasure_CurrencyId = table.Column<int>(type: "int", nullable: true),
                    bodyParts = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bodyStats = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vulnerabilities = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    mobility = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    hierarchySystem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    speciualAbilities = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Races", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SingleCurrencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ShotName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    HierarchyLevel = table.Column<int>(type: "int", nullable: false),
                    ExchangeRate = table.Column<int>(type: "int", nullable: false),
                    CurrencyGroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SingleCurrencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SingleCurrencies_CurrencyGroups_CurrencyGroupId",
                        column: x => x.CurrencyGroupId,
                        principalTable: "CurrencyGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Races_RaceCategory",
                table: "Races",
                column: "RaceCategory");

            migrationBuilder.CreateIndex(
                name: "IX_Races_RaceName",
                table: "Races",
                column: "RaceName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SingleCurrencies_CurrencyGroupId",
                table: "SingleCurrencies",
                column: "CurrencyGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Races");

            migrationBuilder.DropTable(
                name: "SingleCurrencies");

            migrationBuilder.DropTable(
                name: "CurrencyGroups");
        }
    }
}
