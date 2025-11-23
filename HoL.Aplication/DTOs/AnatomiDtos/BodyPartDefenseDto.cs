namespace HoL.Aplication.DTOs.AnatomiDtos
{
    /// <summary>
    /// Data Transfer Object pro reprezentaci obranných vlastností části těla.
    /// </summary>
    /// <remarks>
    /// <para>
    /// BodyPartDefenseDto definuje jak dobře je část těla chráněna proti útokům.
    /// <see cref="ArmorValue"/> reprezentuje přirozenou nebo magickou ochranu jako krunýř, šupiny, tlustá kůže nebo magické štíty.
    /// </para>
    /// </remarks>
    /// <example>
    /// Příklady obranných vlastností:
    /// <code>
    /// // Magický štít - nadpřirozená ochrana
    /// var magicShield = new BodyPartDefenseDto
    /// {
    ///     ArmorValue = 30,
    ///     IsVital = false,
    ///     IsMagical = true
    /// };
    /// </code>
    /// </example>
    /// <seealso cref="BodyPartDto"/>
    /// <seealso cref="BodyPartDefenseDtoValidator"/>
    public class BodyPartDefenseDto
    {
        /// <summary>
        /// Hodnota brnění/ochrany této části těla.
        /// </summary>
        public int ArmorValue { get; set; } = 0;

        /// <summary>
        /// Indikuje zda je tato část těla vitální (zásah může být smrtelný).
        /// </summary>
        public bool IsVital { get; set; } = false;

        /// <summary>
        /// Indikuje zda je ochrana magická povahy.
        /// </summary>
        public bool IsMagical { get; set; } = false;
    }
}
