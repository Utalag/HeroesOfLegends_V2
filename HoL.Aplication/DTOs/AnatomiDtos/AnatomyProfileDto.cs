using HoL.Domain.Enums;

namespace HoL.Aplication.DTOs.AnatomiDtos
{
    /// <summary>
    /// Data Transfer Object pro reprezentaci anatomického profilu rasy/entity.
    /// Obsahuje tělesné rozměry, věkové rozsahy a definice částí těla.
    /// </summary>
    /// <remarks>
    /// <para>
    /// AnatomyProfileDto se používá pro definici fyzických charakteristik entit v herním světě.
    /// Zahrnuje:
    /// <list type="bullet">
    /// <item>
    ///     <description>Kategorie velikosti (<see cref="RaceSize"/>)</description>
    /// </item>
    /// <item>
    ///     <description>Váhové rozsahy (min/max v kg)</description>
    /// </item>
    /// <item>
    ///     <description>Výškové rozsahy těla a celkové výšky (v cm)</description>
    /// </item>
    /// <item>
    ///     <description>Maximální věk (v letech)</description>
    /// </item>
    /// <item>
    ///     <description>Seznam částí těla s jejich vlastnostmi</description>
    /// </item>
    /// </list>
    /// </para>
    /// <para>
    /// Používá se v:
    /// <list type="bullet">
    /// <item><description><c>RaceDto.BodyDimensins</c></description></item>
    /// <item><description><c>MonsterDto.Anatomy</c></description></item>
    /// <item><description><c>NPCDto.PhysicalProfile</c></description></item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <example>
    /// Příklad anatomického profilu pro elfa:
    /// <code>
    /// var elfAnatomy = new AnatomyProfileDto
    /// {
    ///     RaceSize = RaceSize.B,          // 1,5m - 2m
    ///     WeightMin = 50,
    ///     WeightMax = 80,
    ///     BodyHeightMin = 160,
    ///     BodyHeightMax = 190,
    ///     HeightMin = 160,
    ///     HeightMax = 190,
    ///     MaxAge = 750,
    ///     BodyParts = new List&lt;BodyPartDto&gt;
    ///     {
    ///         new BodyPartDto { Name = "Hlava", BodyPartCategory = BodyPartType.Head, Quantity = 1 },
    ///         new BodyPartDto { Name = "Ruce", BodyPartCategory = BodyPartType.Arm, Quantity = 2 },
    ///         new BodyPartDto { Name = "Nohy", BodyPartCategory = BodyPartType.Leg, Quantity = 2 }
    ///     }
    /// };
    /// </code>
    /// </example>
    /// <seealso cref="RaceSize"/>
    /// <seealso cref="BodyPartDto"/>
    /// <seealso cref="AnatomyProfileDtoValidator"/>
    public class AnatomyProfileDto
    {
        /// <summary>
        /// Kategorie velikosti entity určující základní rozměrové rozmezí.
        /// </summary>
        /// <value>
        /// Hodnota z <see cref="RaceSize"/> enumu:
        /// <list type="bullet">
        /// <item><description><c>A0</c> - do 0,5m (víly, pixies)</description></item>
        /// <item><description><c>A</c> - 0,5m až 1,5m (goblini, trpaslíci)</description></item>
        /// <item><description><c>B</c> - 1,5m až 2m (lidé, elfové)</description></item>
        /// <item><description><c>C</c> - 2m až 3m (orkové, trolové)</description></item>
        /// <item><description><c>D</c> - 3m až 5m (obři)</description></item>
        /// <item><description><c>E</c> - 5m až 10m (draci)</description></item>
        /// <item><description><c>F</c> - nad 10m (titáni)</description></item>
        /// </list>
        /// </value>
        public RaceSize RaceSize { get; set; }

        #region Weight
        /// <summary>
        /// Minimální váha entity v kilogramech.
        /// </summary>
        /// <value>
        /// Musí být >= 0 a menší než <see cref="WeightMax"/>.
        /// Typicky nejlehčí možná váha pro danou rasu (mladí jedinci, subtilnější stavba).
        /// </value>
        public int WeightMin { get; set; }

        /// <summary>
        /// Maximální váha entity v kilogramech.
        /// </summary>
        /// <value>
        /// Musí být větší než <see cref="WeightMin"/>.
        /// Typicky nejtěžší možná váha pro danou rasu (starší jedinci, robustnější stavba).
        /// </value>
        public int WeightMax { get; set; }
        #endregion

        #region BodyHeight
        /// <summary>
        /// Minimální výška těla (měřeno po ramena/kohoutek) v centimetrech.
        /// </summary>
        /// <value>
        /// Musí být >= 0 a menší než <see cref="BodyHeightMax"/>.
        /// Pro čtyřnohá zvířata odpovídá výšce v kohoutku, pro humanoidy výška po ramena.
        /// </value>
        public int BodyHeightMin { get; set; }

        /// <summary>
        /// Maximální výška těla (měřeno po ramena/kohoutek) v centimetrech.
        /// </summary>
        /// <value>
        /// Musí být větší než <see cref="BodyHeightMin"/>.
        /// Pro čtyřnohá zvířata odpovídá výšce v kohoutku, pro humanoidy výška po ramena.
        /// </value>
        public int BodyHeightMax { get; set; }
        #endregion

        #region TotalHeight
        /// <summary>
        /// Minimální celková výška entity v centimetrech (včetně hlavy).
        /// </summary>
        /// <value>
        /// Musí být >= 0 a menší než <see cref="HeightMax"/>.
        /// Pro humanoidy to je výška od hlavy k patě, pro zvířata od země po nejvyšší bod těla.
        /// </value>
        public int HeightMin { get; set; }

        /// <summary>
        /// Maximální celková výška entity v centimetrech (včetně hlavy).
        /// </summary>
        /// <value>
        /// Musí být větší než <see cref="HeightMin"/>.
        /// Pro humanoidy to je výška od hlavy k patě, pro zvířata od země po nejvyšší bod těla.
        /// </value>
        public int HeightMax { get; set; }
        #endregion

        /// <summary>
        /// Maximální věk entity v letech.
        /// </summary>
        /// <value>
        /// Musí být > 0 a <= 10000.
        /// Představuje typickou délku života pro danou rasu při přirozeném stárnutí.
        /// Příklady:
        /// <list type="bullet">
        /// <item><description>Lidé: ~80 let</description></item>
        /// <item><description>Elfové: ~750 let</description></item>
        /// <item><description>Trpaslíci: ~350 let</description></item>
        /// <item><description>Draci: ~5000 let</description></item>
        /// </list>
        /// </value>
        public int MaxAge { get; set; }

        /// <summary>
        /// Kolekce částí těla entity s jejich vlastnostmi.
        /// </summary>
        /// <value>
        /// Nullable seznam <see cref="BodyPartDto"/> objektů.
        /// Každá část těla může obsahovat:
        /// <list type="bullet">
        /// <item><description>Název a typ (hlava, končetiny, křídla, ocas, atd.)</description></item>
        /// <item><description>Počet (např. 2 ruce, 4 nohy)</description></item>
        /// <item><description>Útočné schopnosti (<see cref="BodyPartAttackDto"/>)</description></item>
        /// <item><description>Obranné vlastnosti (<see cref="BodyPartDefenseDto"/>)</description></item>
        /// <item><description>Funkce a vzhled</description></item>
        /// </list>
        /// Pokud je <c>null</c> nebo prázdný, entita nemá definované specifické části těla.
        /// </value>
        public List<BodyPartDto>? BodyParts { get; set; }
    }
}
