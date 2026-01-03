using HoL.Domain.Builders;
using HoL.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace HoL.Domain.ValueObjects.Anatomi
{
    /// <summary>
    /// Profil anatomie rasy/postavy s hierarchickou strukturou tělesných částí.
    /// </summary>


    public class BodyDimension
    {
        // Základní tělesné rozměry
        public RaceSize RaceSize    { get; private set; }
        public int WeightMin        { get; private set; }
        public int WeightMax        { get; private set; }
        public int LengthMin        { get; private set; }
        public int LengthMax        { get; private set; }
        public int HeihtMin         { get; private set; }
        public int HeihtMax         { get; private set; }
        public int MaxAge           { get; private set; }

        public BodyDimension WithRaceSize(RaceSize raceSize)
        {
            RaceSize = raceSize;
            return this;
        }
        public BodyDimension WithWeightRange(int min, int max)
        {
            if (min < 0 || max < 0 || min > max)
            {
                throw new ArgumentException("Invalid weight range");
            }
            WeightMin = min;
            WeightMax = max;
            return this;
        }
        public BodyDimension WithLengthRange(int min, int max)
        {
            if (min < 0 || max < 0 || min > max)
            {
                throw new ArgumentException("Invalid length range");
            }
            LengthMin = min;
            LengthMax = max;
            return this;
        }
        public BodyDimension WithHeightRange(int min, int max)
        {
            if (min < 0 || max < 0 || min > max)
            {
                throw new ArgumentException("Invalid height range");
            }
            HeihtMin = min;
            HeihtMax = max;
            return this;
        }

    }
}

