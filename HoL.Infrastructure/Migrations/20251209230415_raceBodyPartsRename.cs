using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoL.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class raceBodyPartsRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BodyParts");

            migrationBuilder.CreateTable(
                name: "RaceBodyParts",
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
                    table.PrimaryKey("PK_RaceBodyParts", x => new { x.RaceId, x.Id });
                    table.ForeignKey(
                        name: "FK_RaceBodyParts_Races_RaceId",
                        column: x => x.RaceId,
                        principalTable: "Races",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RaceBodyParts");

            migrationBuilder.CreateTable(
                name: "BodyParts",
                columns: table => new
                {
                    RaceId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Appearance = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Function = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsMagical = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Attack_CanBeUsedWithOtherAttacks = table.Column<bool>(type: "bit", nullable: true),
                    Attack_DamageType = table.Column<int>(type: "int", nullable: true),
                    Attack_Initiative = table.Column<int>(type: "int", nullable: true),
                    Attack_DamageDice_Bonus = table.Column<int>(type: "int", nullable: true),
                    Attack_DamageDice_Count = table.Column<int>(type: "int", nullable: true),
                    Attack_DamageDice_Sides = table.Column<int>(type: "int", nullable: true),
                    Defense_ArmorValue = table.Column<int>(type: "int", nullable: true),
                    Defense_IsProtected = table.Column<bool>(type: "bit", nullable: true),
                    Defense_IsVital = table.Column<bool>(type: "bit", nullable: true)
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
        }
    }
}
