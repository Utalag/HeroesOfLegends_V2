using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoL.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class raceMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Currency1Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Currency2Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Currency3Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Currency4Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Currency5Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Races",
                columns: table => new
                {
                    RaceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RaceName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RaceDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    RaceHistory = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    RaceCategory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Conviction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZSM = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    DomesticationValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    BaseInitiative = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    BaseXP = table.Column<int>(type: "int", nullable: false),
                    FightingSpiritNumber = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Treasure_GlobalCurrencyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Treasure_Currency1 = table.Column<int>(type: "int", nullable: true),
                    Treasure_Currency2 = table.Column<int>(type: "int", nullable: true),
                    Treasure_Currency3 = table.Column<int>(type: "int", nullable: true),
                    Treasure_Currency4 = table.Column<int>(type: "int", nullable: true),
                    Treasure_Currency5 = table.Column<int>(type: "int", nullable: true),
                    Treasure_CurrencyId = table.Column<int>(type: "int", nullable: true),
                    BodyDimensins_RaceSize = table.Column<int>(type: "int", nullable: false),
                    BodyDimensins_WeightMin = table.Column<int>(type: "int", nullable: false),
                    BodyDimensins_WeightMax = table.Column<int>(type: "int", nullable: false),
                    BodyDimensins_LengthMin = table.Column<int>(type: "int", nullable: false),
                    BodyDimensins_LengthMax = table.Column<int>(type: "int", nullable: false),
                    BodyDimensins_HeihtMin = table.Column<int>(type: "int", nullable: false),
                    BodyDimensins_HeihtMax = table.Column<int>(type: "int", nullable: false),
                    BodyDimensins_MaxAge = table.Column<int>(type: "int", nullable: false),
                    RaceHierarchySystem = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    StatsPrimar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Vulnerabilities = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mobility = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpecialAbilities = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Races", x => x.RaceId);
                    table.ForeignKey(
                        name: "FK_Races_Currencies_Treasure_CurrencyId",
                        column: x => x.Treasure_CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BodyParts",
                columns: table => new
                {
                    RaceId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Function = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Attack_DamageDice_Count = table.Column<int>(type: "int", nullable: true),
                    Attack_DamageDice_Sides = table.Column<int>(type: "int", nullable: true),
                    Attack_DamageDice_Bonus = table.Column<int>(type: "int", nullable: true),
                    Attack_DamageType = table.Column<int>(type: "int", nullable: true),
                    Attack_Initiative = table.Column<int>(type: "int", nullable: true),
                    Attack_CanBeUsedWithOtherAttacks = table.Column<bool>(type: "bit", nullable: true),
                    Defense_ArmorValue = table.Column<int>(type: "int", nullable: true),
                    Defense_IsVital = table.Column<bool>(type: "bit", nullable: true),
                    Defense_IsProtected = table.Column<bool>(type: "bit", nullable: true),
                    Appearance = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsMagical = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BodyParts", x => new { x.RaceId, x.Id });
                    table.ForeignKey(
                        name: "FK_BodyParts_Races_RaceId",
                        column: x => x.RaceId,
                        principalTable: "Races",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Races_Treasure_CurrencyId",
                table: "Races",
                column: "Treasure_CurrencyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BodyParts");

            migrationBuilder.DropTable(
                name: "Races");

            migrationBuilder.DropTable(
                name: "Currencies");
        }
    }
}
