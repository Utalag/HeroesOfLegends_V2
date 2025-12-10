using HoL.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace HoL.Domain.ValueObjects.Anatomi
{
    /// <summary>
    /// Profil anatomie rasy/postavy s hierarchickou strukturou tělesných částí.
    /// </summary>

    [Owned]
    public class AnatomyProfile
    {
        // Základní tělesné rozměry
        public RaceSize RaceSize { get; set; }
        public int WeightMin { get; set; }
        public int WeightMax { get; set; }
        public int LengthMin { get; set; }
        public int LengthMax { get; set; }
        public int HeihtMin { get; set; }
        public int HeihtMax { get; set; }
        public int MaxAge { get; set; }

        

    }
}

