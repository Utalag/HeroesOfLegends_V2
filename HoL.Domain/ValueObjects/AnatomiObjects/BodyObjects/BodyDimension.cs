using HoL.Domain.Enums;

namespace HoL.Domain.ValueObjects.Anatomi.Body
{
    /// <summary>
    /// Profil anatomie rasy/postavy s hierarchickou strukturou tělesných částí.
    /// </summary>
    /// <remarks>
    /// <para>
    /// BodyDimension reprezentuje fyzické rozměry a velikost bytosti.
    /// RaceSize je povinný parametr inicializace a určuje velikostní kategorii.
    /// </para>
    /// <para>
    /// Velikostní kategorie (RaceSize):
    /// <list type="bullet">
    /// <item><description>A0 - Velmi malé (až 0,5m) - mravenci, malí goblinové</description></item>
    /// <item><description>A - Malé (0,5m-1,5m) - haflinské, koboldové</description></item>
    /// <item><description>B - Střední (1,5m-2m) - lidé, elfové, skřeti</description></item>
    /// <item><description>C - Velké (2m-3m) - trpaslici, trolové</description></item>
    /// <item><description>D - Velmi velké (3m-5m) - obři, masivní tvory</description></item>
    /// <item><description>E - Obrovské (5m-10m) - draci, velmi velké bestie</description></item>
    /// <item><description>F - Kolosální (nad 10m) - legendární tvory</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // Vytvoření rozměrů pro elfskou rasu
    /// var elfDimensions = new BodyDimension(RaceSize.B)
    ///     .SetHeightRange(150, 180)
    ///     .SetWeightRange(45, 75)
    ///     .SetLengthRange(10, 20);
    /// </code>
    /// </example>
    public class BodyDimension
    {
        /// <summary>
        /// Získá nebo nastaví velikost rasy.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Povinná vlastnost, musí být nastavena v konstruktoru.
        /// Určuje kategorii velikosti těla.
        /// </para>
        /// </remarks>
        public RaceSize RaceSize { get; private set; }
        
        /// <summary>
        /// Minimální váha v kg.
        /// </summary>
        public int WeightMin { get; private set; }
        
        /// <summary>
        /// Maximální váha v kg.
        /// </summary>
        public int WeightMax { get; private set; }
        
        /// <summary>
        /// Minimální délka v cm.
        /// </summary>
        public int LengthMin { get; private set; }
        
        /// <summary>
        /// Maximální délka v cm.
        /// </summary>
        public int LengthMax { get; private set; }
        
        /// <summary>
        /// Minimální výška v cm.
        /// </summary>
        public int HeihtMin { get; private set; }
        
        /// <summary>
        /// Maximální výška v cm.
        /// </summary>
        public int HeihtMax { get; private set; }
        
        /// <summary>
        /// Maximální věk v letech.
        /// </summary>
        public int MaxAge { get; private set; }

        /// <summary>
        /// Inicializuje novou instanci třídy <see cref="BodyDimension"/> s povinným RaceSize.
        /// </summary>
        /// <param name="raceSize">Velikost rasy (povinný parametr).</param>
        /// <remarks>
        /// <para>
        /// Konstruktor vyžaduje RaceSize jako povinný parametr.
        /// Ostatní vlastnosti (váha, délka, výška, věk) lze nastavit později pomocí fluent API.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var bodyDims = new BodyDimension(RaceSize.B);
        /// var configured = bodyDims
        ///     .SetHeightRange(160, 190)
        ///     .SetWeightRange(60, 100);
        /// </code>
        /// </example>
        public BodyDimension(RaceSize raceSize)
        {
            RaceSize = raceSize;
        }

        /// <summary>
        /// Nastaví velikost rasy.
        /// </summary>
        /// <param name="raceSize">Velikost rasy.</param>
        /// <returns>Aktuální instanci pro řetězení metod.</returns>
        public BodyDimension SetRaceSize(RaceSize raceSize)
        {
            RaceSize = raceSize;
            return this;
        }
        
        /// <summary>
        /// Nastaví rozsah váhy.
        /// </summary>
        /// <param name="min">Minimální váha v kg.</param>
        /// <param name="max">Maximální váha v kg.</param>
        /// <returns>Aktuální instanci pro řetězení metod.</returns>
        /// <exception cref="ArgumentException">Vyvolána při neplatném rozsahu hodnot.</exception>
        public BodyDimension SetWeightRange(int min, int max)
        {
            if (min < 0 || max < 0 || min > max)
            {
                throw new ArgumentException("Invalid weight range");
            }
            WeightMin = min;
            WeightMax = max;
            return this;
        }
        
        /// <summary>
        /// Nastaví rozsah délky.
        /// </summary>
        /// <param name="min">Minimální délka v cm.</param>
        /// <param name="max">Maximální délka v cm.</param>
        /// <returns>Aktuální instanci pro řetězení metod.</returns>
        /// <exception cref="ArgumentException">Vyvolána při neplatném rozsahu hodnot.</exception>
        public BodyDimension SetLengthRange(int min, int max)
        {
            if (min < 0 || max < 0 || min > max)
            {
                throw new ArgumentException("Invalid length range");
            }
            LengthMin = min;
            LengthMax = max;
            return this;
        }
        
        /// <summary>
        /// Nastaví rozsah výšky.
        /// </summary>
        /// <param name="min">Minimální výška v cm.</param>
        /// <param name="max">Maximální výška v cm.</param>
        /// <returns>Aktuální instanci pro řetězení metod.</returns>
        /// <exception cref="ArgumentException">Vyvolána při neplatném rozsahu hodnot.</exception>
        public BodyDimension SetHeightRange(int min, int max)
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

