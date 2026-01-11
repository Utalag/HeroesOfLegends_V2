using Microsoft.EntityFrameworkCore;

namespace HoL.Infrastructure.Data.Models
{
    /// <summary>
    /// Databázový model pro tělesné rozměry - datová reprezentace bez domén logiky.
    /// </summary>
    [Owned]
    public class BodyDimensionDbModel
    {
        /// <summary>
        /// Velikost rasy (A0, A, B, C, D, E, F) - stored as string enum.
        /// </summary>
        public string RaceSize { get; set; } = string.Empty;

        /// <summary>
        /// Minimální váha v kg.
        /// </summary>
        public int WeightMin { get; set; }

        /// <summary>
        /// Maximální váha v kg.
        /// </summary>
        public int WeightMax { get; set; }

        /// <summary>
        /// Minimální délka v cm.
        /// </summary>
        public int LengthMin { get; set; }

        /// <summary>
        /// Maximální délka v cm.
        /// </summary>
        public int LengthMax { get; set; }

        /// <summary>
        /// Minimální výška v cm.
        /// </summary>
        public int HeightMin { get; set; }

        /// <summary>
        /// Maximální výška v cm.
        /// </summary>
        public int HeightMax { get; set; }

        /// <summary>
        /// Maximální věk v letech.
        /// </summary>
        public int MaxAge { get; set; }
    }
}
