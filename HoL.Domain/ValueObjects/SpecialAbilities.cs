using System.Text.Json.Serialization;

namespace HoL.Domain.ValueObjects
{
    public class SpecialAbilities
    {

        /// <summary>
        /// Název speciální schopnosti.
        /// </summary>
        [JsonPropertyName("AbilityName")]
        public string AbilityName { get; internal set; } = string.Empty;

        /// <summary>
        /// Popis speciální schopnosti.
        /// </summary>
        [JsonPropertyName("AbilityDescription")]
        public string AbilityDescription { get; internal set; } = string.Empty;

        [JsonConstructor]
        public SpecialAbilities(
            string abilityName,
            string abilityDescription)
        {
            if (string.IsNullOrWhiteSpace(abilityName))
                throw new ArgumentException("Název schopnosti nesmí být prázdný", nameof(abilityName));

            AbilityName = abilityName;
            AbilityDescription = abilityDescription ?? string.Empty;
        }

        /// <summary>
        /// Nastaví název speciální schopnosti.
        /// </summary>
        public SpecialAbilities WithName(string abilityName)
        {
            if (string.IsNullOrWhiteSpace(abilityName))
                throw new ArgumentException("Název schopnosti nesmí být prázdný", nameof(abilityName));

            AbilityName = abilityName;
            return this;
        }

        /// <summary>
        /// Nastaví popis speciální schopnosti.
        /// </summary>
        public SpecialAbilities WithDescription(string description)
        {
            AbilityDescription = description ?? string.Empty;
            return this;
        }
    }
}
