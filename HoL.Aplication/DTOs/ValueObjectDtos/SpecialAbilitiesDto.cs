namespace HoL.Aplication.DTOs.ValueObjectDtos
{
    /// <summary>
    /// Data Transfer Object pro reprezentaci speciální schopnosti entity.
    /// </summary>
    /// <remarks>
    /// <para>
    /// SpecialAbilitiesDto reprezentuje unikátní schopnost, kterou může mít rasa, postava,
    /// předmět nebo kouzlo. Schopnosti mohou být rasové (např. darkvision elfů),
    /// třídní (např. rage barbara), magické (např. teleportace) nebo jiné.

    /// Každá schopnost by měla být:
    /// <list type="bullet">
    /// <item><description>Jasně pojmenovaná (výstižný název)</description></item>
    /// <item><description>Detailně popsaná (co přesně dělá, jak se aktivuje)</description></item>
    /// <item><description>Herně vyvážená (bonus/penalizace)</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <example>
    /// Příklady různých typů schopností:
    /// <code>
    /// // Rasová schopnost elfů
    /// var darkvision = new SpecialAbilitiesDto
    /// {
    ///     AbilityName = "Darkvision",
    ///     AbilityDescription = "Může vidět ve tmě do vzdálenosti 60 stop jako by bylo šero. Ve tmě vidí černobíle."
    /// };
    /// 
    /// // Třídní schopnost barbara
    /// var rage = new SpecialAbilitiesDto
    /// {
    ///     AbilityName = "Rage",
    ///     AbilityDescription = "Může vstoupit do zběsilosti na 1 minutu. Získá +2 damage, výhodu na sílu a odolnost vůči fyzickému poškození."
    /// };
    /// 
    /// // Magická schopnost předmětu
    /// var teleport = new SpecialAbilitiesDto
    /// {
    ///     AbilityName = "Teleportace",
    ///     AbilityDescription = "1x denně může uživatel teleportovat až 100 stop na viditelné místo."
    /// };
    /// </code>
    /// </example>
    /// <seealso cref="SpecialAbilitiesDtoValidator"/>
    public class SpecialAbilitiesDto
    {
        /// <summary>
        /// Název speciální schopnosti.
        /// </summary>
        public string AbilityName { get; set; } = string.Empty;

        /// <summary>
        /// Detailní popis schopnosti včetně mechaniky a efektů.
        /// </summary>
        public string AbilityDescription { get; set; } = string.Empty;
    }
}
