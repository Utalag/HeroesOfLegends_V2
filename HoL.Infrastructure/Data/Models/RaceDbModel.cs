using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using HoL.Domain.Enums;
using HoL.Domain.ValueObjects;
using HoL.Domain.ValueObjects.Anatomi.Body;
using HoL.Domain.ValueObjects.Anatomi.Stat;

namespace HoL.Infrastructure.Data.Models
{
    /// <summary>
    /// Databázový model pro rasu - datová reprezentace bez domén logiky.
    /// </summary>
    /// <remarks>
    /// <para>
    /// RaceDbModel reprezentuje herní rasu v databázi. Model je optimalizován pro jednoduchost a výkon:
    /// </para>
    /// <para>
    /// <strong>Scalar Properties:</strong>
    /// Všechny základní vlastnosti (jméno, kategorie, iniciativa, atd.) jsou ukládány přímo jako sloupce.
    /// Enumerační hodnoty jsou konvertovány na string.
    /// </para>
    /// <para>
    /// <strong>Owned Value Objects:</strong>
    /// <see cref="BodyDimensionDbModel"/> a <see cref="TreasureDbModel"/> jsou začleněny do tabulky (bez vlastní tabulky).
    /// </para>
    /// <para>
    /// <strong>Serializované Kolekce:</strong>
    /// Komplexní kolekce (seznamy, slovníky) jsou serializovány do JSON stringů pro jednoduchost a výkon.
    /// Deserializace probíhá v mapovacích třídách při konverzi na Domain model nebo DTO.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var race = new RaceDbModel
    /// {
    ///     RaceName = "Elf",
    ///     RaceCategory = RaceCategory.Humanoid,
    ///     BaseInitiative = 5,
    ///     BodyDimensions = new BodyDimensionDbModel { RaceSize = "B" },
    ///     JsonBodyParts = "[{...}]", // JSON string
    ///     JsonStats = "{...}"        // JSON string
    /// };
    /// </code>
    /// </example>
    public class RaceDbModel
    {
        #region Atributy třídy
        /// <summary>
        /// Primární klíč - unikátní identifikátor rasy v databázi.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Unikátní název rasy (např. "Elf", "Skřet", "Drak").
        /// </summary>
        /// <remarks>Povinné pole, nesmí být prázdné.</remarks>
        public string RaceName { get; set; } = string.Empty;

        /// <summary>
        /// Kategorie rasy (Humanoid, Beast, Dragon, atd.).
        /// </summary>
        /// <remarks>Enum hodnota <see cref="RaceCategory"/> určující typ rasy.</remarks>
        public RaceCategory RaceCategory { get; set; }

        /// <summary>
        /// Základní iniciativa všech jedinců rasy.
        /// </summary>
        /// <remarks>
        /// Iniciativa ovlivňuje pořadí v combat systému. Vyšší hodnota = rychlejší reagování.
        /// Výchozí: 0
        /// </remarks>
        public int BaseInitiative { get; set; } = 0;

        /// <summary>
        /// Detailný popis rasy a její charakteru.
        /// </summary>
        /// <remarks>Textový popis určený pro flavor text a herní kontext.</remarks>
        public string RaceDescription { get; set; } = string.Empty;

        /// <summary>
        /// Historie a původ rasy.
        /// </summary>
        /// <remarks>Narativní popis vzniku a vývoje rasy v herním světě.</remarks>
        public string RaceHistory { get; set; } = string.Empty;

        /// <summary>
        /// Ideální přesvědčení rasy (Good, Neutral, Evil).
        /// </summary>
        /// <remarks>
        /// Enum hodnota <see cref="ConvictionType"/> určující tendenci rasy k určitému typu chování.
        /// Výchozí: <see cref="ConvictionType.Neutral"/>
        /// </remarks>
        public ConvictionType Conviction { get; set; } = ConvictionType.Neutral;

        /// <summary>
        /// Základní cena v XP za porážku jedince rasy.
        /// </summary>
        /// <remarks>Vyšší hodnota = silnější rasa. Používá se pro výpočet odměny hráčů.</remarks>
        public int BaseXP { get; set; }

        /// <summary>
        /// Číslo bojového ducha (speciální efekt).
        /// </summary>
        /// <remarks>Speciální mechanika ovlivňující bojový systém.</remarks>
        public int FightingSpiritNumber { get; set; }

        /// <summary>
        /// Zone Spawn Modifier - modifikátor pro generování tvorů v zónách.
        /// </summary>
        /// <remarks>Výchozí: 0</remarks>
        public int ZSM { get; set; } = 0;

        /// <summary>
        /// Schopnost domestikace (přidělení).
        /// </summary>
        /// <remarks>
        /// Určuje, zda mohou být jedinci rasy zdomácněni nebo kontrolováni.
        /// Vyšší hodnota = lépe se dá s nimi manipulovat.
        /// Výchozí: 0
        /// </remarks>
        public int DomesticationValue { get; set; } = 0;

        #endregion

        #region Owned Value Objects

        /// <summary>
        /// Navigační vlastnost na tělesné rozměry rasy.
        /// </summary>
        /// <remarks>
        /// <see cref="BodyDimensionDbModel"/> je owned entity - je začlena do tabulky Races bez vlastní tabulky.
        /// Obsahuje informace: RaceSize, váhu, délku, výšku, maximální věk.
        /// </remarks>
        public BodyDimensionDbModel BodyDimensins { get; set; } = new();

        /// <summary>
        /// Navigační vlastnost na poklad, který padá z rasy.
        /// </summary>
        /// <remarks>
        /// Volitelné pole. Pokud je null, rasa nepodporuje poklad.
        /// </remarks>
        public TreasureDbModel? Treasure { get; set; }

        #endregion

        #region Serializované Kolekce

        /// <summary>
        /// Serializovaný seznam tělesných částí rasy jako JSON string.
        /// </summary>
        /// <remarks>
        /// Serializovaný typ: <see cref="List{BodyPart}"/> z <see cref="BodyPart"/>
        /// </remarks>

        public string JsonBodyParts { get; set; } = string.Empty;

        /// <summary>
        /// Serializovaný slovník primárních statistik rasy jako JSON string.
        /// </summary>
        /// <remarks>
        /// Serializovaný typ: <see cref="Dictionary{BodyStat, ValueRange}"/> kde Key = <see cref="BodyStat"/>, Value = <see cref="ValueRange"/>
        /// </remarks>
        public string JsonBodyStats { get; set; } = string.Empty;

        /// <summary>
        /// Serializovaný slovník zranitelností rasy jako JSON string.
        /// </summary>
        /// <remarks>
        /// Serializovaný typ: <see cref="Dictionary{VulnerabilityType, double}"/> kde Key = <see cref="VulnerabilityType"/>, Value = <see cref="double"/>
        /// </remarks>
        public string JsonVulnerabilities { get; set; } = string.Empty;

        /// <summary>
        /// Serializovaný slovník mobility rasy v různých prostředích jako JSON string.
        /// </summary>
        /// <remarks>
        /// Serializovaný typ: <see cref="Dictionary{MobilityType, int}"/> kde Key = <see cref="MobilityType"/>, Value = <see cref="int"/> 
        /// </remarks>
        public string JsonMobility { get; set; } = string.Empty;

        /// <summary>
        /// Serializovaný seznam hierarchických systémů rasy jako JSON string.
        /// </summary>
        /// <remarks>
        /// Serializovaný typ: <see cref="List{string}"/> z <see cref="string"/>
        /// </remarks>
        public string JsonHierarchySystem { get; set; } = string.Empty;

        /// <summary>
        /// Serializovaný seznam speciálních schopností rasy jako JSON string.
        /// </summary>
        /// <remarks>
        /// Serializovaný typ: <see cref="List{SpecialAbilities}"/> z <see cref="SpecialAbilities"/>
        /// </remarks>
        public string JsonSpeciualAbilities { get; set; } = string.Empty;

        #endregion

    }
}
