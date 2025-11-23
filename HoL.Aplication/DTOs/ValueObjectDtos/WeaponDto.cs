namespace HoL.Aplication.DTOs.ValueObjectDtos
{
    /// <summary>
    /// Data Transfer Object pro reprezentaci zbraně v aplikaci.
    /// </summary>
    public class WeaponDto
    {
        /// <summary>
        /// Název zbraně.
        /// </summary>
        /// <value>
        /// Název musí být neprázdný string o maximální délce 100 znaků.
        /// Měl by být unikátní a popisný (např. "Dlouhý meč", "Elfský luk", "Ohnivá hůl").
        /// Výchozí hodnota je prázdný string.
        /// </value>
        public string WeaponName { get; set; } = string.Empty;
    }
}
