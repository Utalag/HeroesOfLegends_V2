using System;
using HoL.Domain.Entities;
using HoL.Domain.Enums;
using HoL.Domain.ValueObjects;
using HoL.Domain.ValueObjects.Anatomi.Body;
using HoL.Domain.ValueObjects.Anatomi.Stat;

namespace HoL.Domain.Builders
{
    /// <summary>
    /// Builder pro vytváření Race agregátů s fluent API.
    /// Umožňuje snadné a čitelné vytváření komplexních objektů.
    /// </summary>
    /// <remarks>
    /// <para>
    /// RaceBuilder implementuje Builder pattern pro vytváření Race agregátů.
    /// Separuje komplexní logiku konstruování Race od její reprezentace.
    /// </para>
    /// <para>
    /// Povinné vlastnosti (musí být nastaveny):
    /// <list type="bullet">
    /// <item><description><see cref="WithName(string)"/> - Název rasy</description></item>
    /// <item><description><see cref="WithCategory(RaceCategory)"/> - Kategorie rasy</description></item>
    /// <item><description><see cref="WithBodyDimensions(BodyDimension)"/> - Tělesné rozměry</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// Volitelné vlastnosti:
    /// <list type="bullet">
    /// <item><description><see cref="AddStat(BodyStat, ValueRange)"/> - Přidat statistiku</description></item>
    /// <item><description><see cref="WithVulnerability(VulnerabilityType, double)"/> - Nastavit zranitelnost</description></item>
    /// <item><description><see cref="AddMobility(MobilityType, int)"/> - Nastavit mobilitu</description></item>
    /// <item><description><see cref="WithDescription(string, string)"/> - Nastavit popis a historii</description></item>
    /// <item><description><see cref="WithBaseXP(int)"/> - Nastavit BaseXP</description></item>
    /// <item><description><see cref="WithFightingSpirit(int)"/> - Nastavit Fighting Spirit Number</description></item>
    /// <item><description><see cref="WithBaseInitiative(int)"/> - Nastavit iniciativu</description></item>
    /// <item><description><see cref="WithTreasure(Treasure)"/> - Přidat poklad</description></item>
    /// <item><description><see cref="AddBodyPart(BodyPart)"/> - Přidat část těla</description></item>
    /// <item><description><see cref="AddSpecialAbility(SpecialAbilities)"/> - Přidat speciální schopnost</description></item>
    /// <item><description><see cref="WithConviction(ConvictionType)"/> - Nastavit přesvědčení</description></item>
    /// <item><description><see cref="WithZSM(int)"/> - Nastavit Zone Spawn Modifier</description></item>
    /// <item><description><see cref="WithDomesticationValue(int)"/> - Nastavit domestikaci</description></item>
    /// <item><description><see cref="AddRaceHierarchy(string)"/> - Přidat hierarchii</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <example>
    /// Příklad použití RaceBuilder:
    /// <code>
    /// var race = new RaceBuilder()
    ///     .WithName("Elf")
    ///     .WithCategory(RaceCategory.Humanoid)
    ///     .WithBodyDimensions(new BodyDimension(RaceSize.B))
    ///     .AddStat(BodyStat.Intelligence, new ValueRange(10, 2, DiceType.D4))
    ///     .AddStat(BodyStat.Charisma, new ValueRange(12, 2, DiceType.D6))
    ///     .WithDescription("Elegantní a dlouhověcí elfové", "Starobilá rasa...")
    ///     .WithBaseInitiative(5)
    ///     .WithBaseXP(100)
    ///     .AddBodyPart(new BodyPart("hlava", BodyPartType.Head, 1))
    ///     .AddBodyPart(new BodyPart("paže", BodyPartType.Arm, 2))
    ///     .Build();
    /// </code>
    /// </example>
    public class RaceBuilder
    {
        // Povinná pole
        private int _id;
        private string _raceName = string.Empty;
        private RaceCategory _raceCategory = RaceCategory.Humanoid;
        private BodyDimension? _bodyDimensions;
        private readonly Dictionary<BodyStat, ValueRange> _stats = new();

        // Volitelná pole
        private string _description = string.Empty;
        private string _history = string.Empty;
        private ConvictionType _conviction = ConvictionType.Neutral;
        private int _zsm = 0;
        private int _domesticationValue = 0;
        private int _baseInitiative = 0;
        private int _baseXP = 0;
        private int _fightingSpiritNumber = 0;

        // Kolekce
        private Treasure? _treasure;
        private readonly List<BodyPart> _bodyParts = new();
        private readonly List<string> _raceHierarchySystem = new();
        private readonly List<SpecialAbilities> _specialAbilities = new();
        private readonly Dictionary<VulnerabilityType, double> _vulnerabilities = new();
        private readonly Dictionary<MobilityType, int> _mobility = new();

        // Editace existující rasy
        private Race? _originalRace;


        #region Konstruktory a Factory metody

        /// <inheritdoc cref="RaceBuilder" path="/summary"/>
        /// <remarks>
        /// <para>
        /// Používá se pro vytváření zcela nové rasy od začátku.
        /// Pro editaci existující rasy použijte <see cref="FromRace(Race)"/>.
        /// </para>
        /// <para>
        /// Povinné vlastnosti musí být nastaveny:
        /// <list type="bullet">
        /// <item><description><see cref="WithName(string)"/> - Název rasy</description></item>
        /// <item><description><see cref="WithCategory(RaceCategory)"/> - Kategorie rasy</description></item>
        /// <item><description><see cref="WithBodyDimensions(BodyDimension)"/> - Tělesné rozměry</description></item>
        /// </list>
        /// </para>
        /// <para>
        /// Typický workflow:
        /// <list type="number">
        /// <item><description>Vytvořit nový builder</description></item>
        /// <item><description>Nastavit povinné vlastnosti (Name, Category, BodyDimensions)</description></item>
        /// <item><description>Volitelně nastavit další vlastnosti (popis, statistiky, atd.)</description></item>
        /// <item><description>Zavolat Build() pro vytvoření finální Race instance</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <example>
        /// Vytvoření nové rasy:
        /// <code>
        /// var race = new RaceBuilder()
        ///     .WithName("Elf")
        ///     .WithCategory(RaceCategory.Humanoid)
        ///     .WithBodyDimensions(new BodyDimension(RaceSize.B))
        ///     .WithDescription("Elegantní a dlouhověcí elfové", "Starobylá rasa...")
        ///     .WithBaseInitiative(5)
        ///     .WithBaseXP(100)
        ///     .Build();
        /// </code>
        /// </example>
        /// <seealso cref="FromRace(Race)"/>
        /// <seealso cref="Build"/>
        public RaceBuilder()
        {
        }

        /// <summary>
        /// Vytvoří builder založený na existující rase pro editaci.
        /// </summary>
        /// <param name="race">Existující rasa k editaci.</param>
        /// <returns>Builder s přednastavenými vlastnostmi z existující rasy.</returns>
        /// <exception cref="ArgumentNullException">Vyvolá se, když je race null.</exception>
        /// <remarks>
        /// <para>
        /// Tato metoda umožňuje načíst existující rasu a upravit ji přes fluent API.
        /// Všechny vlastnosti se zkopírují z původní rasy a lze je modifikovat.
        /// Při volání Build() se vrátí původní instance se všemi změnami.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// // Načíst existující rasu a editovat
        /// var updatedRace = RaceBuilder.FromRace(existingRace)
        ///     .WithDescription("Nový popis", "Nová historie")
        ///     .WithBaseInitiative(7)
        ///     .AddBodyPart(new BodyPart("křídla", BodyPartType.Wing, 2))
        ///     .Build();
        /// </code>
        /// </example>
        public static RaceBuilder FromRace(Race race)
        {
            if (race == null)
                throw new ArgumentNullException(nameof(race), "Rasa nesmí být null.");

            var builder = new RaceBuilder
            {
                _originalRace = race,
                _raceName = race.RaceName,
                _raceCategory = race.RaceCategory,
                _bodyDimensions = race.BodyDimensins,
                _description = race.RaceDescription,
                _history = race.RaceHistory,
                _conviction = race.Conviction,
                _zsm = race.ZSM,
                _domesticationValue = race.DomesticationValue,
                _baseInitiative = race.BaseInitiative,
                _baseXP = race.BaseXP,
                _fightingSpiritNumber = race.FightingSpiritNumber,
                _treasure = race.Treasure
            };

            // Kopíruj statistiky
            if (race.StatsPrimar != null)
            {
                foreach (var stat in race.StatsPrimar)
                {
                    builder._stats[stat.Key] = stat.Value;
                }
            }

            // Kopíruj tělesné části
            if (race.BodyParts != null)
            {
                builder._bodyParts.AddRange(race.BodyParts);
            }

            // Kopíruj hierarchii
            if (race.RaceHierarchySystem != null)
            {
                builder._raceHierarchySystem.AddRange(race.RaceHierarchySystem);
            }

            // Kopíruj speciální schopnosti
            if (race.SpecialAbilities != null)
            {
                builder._specialAbilities.AddRange(race.SpecialAbilities);
            }

            // Kopíruj zranitelnosti
            if (race.Vulnerabilities != null)
            {
                foreach (var vuln in race.Vulnerabilities)
                {
                    builder._vulnerabilities[vuln.Key] = vuln.Value;
                }
            }

            // Kopíruj mobility
            if (race.Mobility != null)
            {
                foreach (var mob in race.Mobility)
                {
                    builder._mobility[mob.Key] = mob.Value;
                }
            }

            return builder;
        }

        #endregion


        #region Povinné vlastnosti
        /// <summary>
        /// Nastaví povinné pole - název rasy.
        /// </summary>
        /// <param name="raceName">Název rasy (nesmí být prázdný).</param>
        /// <returns>Vrátí builder pro fluent API.</returns>
        /// <exception cref="ArgumentException">Vyvolá se, když je název prázdný nebo null.</exception>
        public RaceBuilder WithName(string raceName)
        {
            if (string.IsNullOrWhiteSpace(raceName))
                throw new ArgumentException("Název rasy nesmí být prázdný", nameof(raceName));

            _raceName = raceName;
            return this;
        }

        /// <summary>
        /// Nastaví kategorii rasy.
        /// </summary>
        /// <param name="category">Kategorie rasy (Humanoid, Beast, atd.).</param>
        /// <returns>Vrátí builder pro fluent API.</returns>
        public RaceBuilder WithCategory(RaceCategory category)
        {
            _raceCategory = category;
            return this;
        }

        /// <summary>
        /// Nastaví anatomii rasy.
        /// </summary>
        /// <param name="bodyDimensions">Tělesné rozměry.</param>
        /// <returns>Vrátí builder pro fluent API.</returns>
        /// <exception cref="ArgumentNullException">Vyvolá se, když je bodyDimensions null.</exception>
        public RaceBuilder WithBodyDimensions(BodyDimension bodyDimensions)
        {
            if (bodyDimensions == null)
                throw new ArgumentNullException(nameof(bodyDimensions));

            _bodyDimensions = bodyDimensions;
            return this;
        }

        /// <summary>
        /// Nastaví staty rasy.
        /// </summary>
        /// <param name="bodyStat">Typ statistiky (Síla, Obratnost, atd.).</param>
        /// <param name="range">Rozsah hodnot pro tuto statistiku.</param>
        /// <returns>Vrátí builder pro fluent API.</returns>
        /// <exception cref="ArgumentNullException">Vyvolá se, když je range null.</exception>
        public RaceBuilder AddStat(Dictionary<BodyStat, ValueRange> stats)
        {
            if (stats == null)
                throw new ArgumentNullException(nameof(stats));

            foreach (var stat in stats)
            {
                _stats[stat.Key] = stat.Value;
            }
            return this;
        }
        /// <summary>
        /// Nastaví zranitelnost rasy.
        /// </summary>
        /// <param name="type">Typ zranitelnosti.</param>
        /// <param name="modifier">Multiplikátor poškození (1.0 = normální, 0.5 = odolnost, 2.0 = slabost).</param>
        /// <returns>Vrátí builder pro fluent API.</returns>
        public RaceBuilder AddVulnerabilities(Dictionary<VulnerabilityType,double> vulnerabilities)
        {
           
            foreach (var vulnerability in vulnerabilities)
            {
                _vulnerabilities[vulnerability.Key] = vulnerability.Value;
            }
            
            return this;
        }
        /// <summary>
        /// Nastaví mobilitu rasy.
        /// </summary>
        /// <param name="mobilityType">Typ mobility (chůze, let, plavání, atd.).</param>
        /// <param name="speed">Rychlost v daném typu mobility.</param>
        /// <returns>Vrátí builder pro fluent API.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Vyvolá se, když je rychlost záporná.</exception>
        public RaceBuilder AddMobility(Dictionary<MobilityType, int> mobility)
        {
            if(mobility == null)
                throw new ArgumentNullException(nameof(mobility));

            foreach (var item in mobility)
            {
                _mobility[item.Key] = item.Value;
            }
            return this;
        }

        #endregion

        #region Volitené vlastnosti
        public RaceBuilder WithId(int id)
        {
            if (id > 0 && _originalRace != null)
            {
                _originalRace.Id = id;
            }
            else
                _id = id;

                return this;
        }

        /// <summary>
        /// Nastaví popis a historii rasy.
        /// </summary>
        /// <param name="description">Popis rasy.</param>
        /// <param name="history">Historie a původ rasy.</param>
        /// <returns>Vrátí builder pro fluent API.</returns>
        public RaceBuilder WithDescription(string description, string history)
        {
            _description = description ?? string.Empty;
            _history = history ?? string.Empty;
            return this;
        }

        /// <summary>
        /// Nastaví přesvědčení rasy.
        /// </summary>
        /// <param name="conviction">Typ přesvědčení (Good, Neutral, Evil).</param>
        /// <returns>Vrátí builder pro fluent API.</returns>
        public RaceBuilder WithConviction(ConvictionType conviction)
        {
            _conviction = conviction;
            return this;
        }

        /// <summary>
        /// Nastaví Zone Spawn Modifier.
        /// </summary>
        /// <param name="zsm">Modifikátor pro generování tvorů v zónách.</param>
        /// <returns>Vrátí builder pro fluent API.</returns>
        public RaceBuilder WithZSM(int zsm)
        {
            _zsm = zsm;
            return this;
        }

        /// <summary>
        /// Nastaví schopnost domestikace (přidělení).
        /// </summary>
        /// <param name="domesticationValue">Hodnota domestikace.</param>
        /// <returns>Vrátí builder pro fluent API.</returns>
        public RaceBuilder WithDomesticationValue(int domesticationValue)
        {
            _domesticationValue = domesticationValue;
            return this;
        }

        /// <summary>
        /// Nastaví BaseXP hodnotu.
        /// </summary>
        /// <param name="baseXP">Počet XP za porážku jedince rasy.</param>
        /// <returns>Vrátí builder pro fluent API.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Vyvolá se, když je BaseXP záporné.</exception>
        public RaceBuilder WithBaseXP(int baseXP)
        {
            if (baseXP < 0)
                throw new ArgumentOutOfRangeException(nameof(baseXP), "BaseXP nemůže být záporné");

            _baseXP = baseXP;
            return this;
        }

        /// <summary>
        /// Nastaví Fighting Spirit Number.
        /// </summary>
        /// <param name="value">Číslo bojového ducha.</param>
        /// <returns>Vrátí builder pro fluent API.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Vyvolá se, když je Fighting Spirit záporný.</exception>
        public RaceBuilder WithFightingSpirit(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Fighting Spirit nemůže být záporný");

            _fightingSpiritNumber = value;
            return this;
        }

        /// <summary>
        /// Nastaví základní iniciativu.
        /// </summary>
        /// <param name="initiative">Iniciativa rasy.</param>
        /// <returns>Vrátí builder pro fluent API.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Vyvolá se, když je iniciativa záporná.</exception>
        public RaceBuilder WithBaseInitiative(int initiative)
        {
            if (initiative < 0)
                throw new ArgumentOutOfRangeException(nameof(initiative), "Iniciativa nemůže být záporná");

            _baseInitiative = initiative;
            return this;
        }

        /// <summary>
        /// Přidá poklad do rasy.
        /// </summary>
        /// <param name="treasure">Poklad, který padá z tvora.</param>
        /// <returns>Vrátí builder pro fluent API.</returns>
        /// <exception cref="ArgumentNullException">Vyvolá se, když je treasure null.</exception>
        public RaceBuilder WithTreasure(Treasure treasure)
        {
            if (treasure == null)
                throw new ArgumentNullException(nameof(treasure));

            _treasure = treasure;
            return this;
        }

        /// <summary>
        /// Přidá část těla rasy.
        /// </summary>
        /// <param name="bodyPart">Část těla k přidání.</param>
        /// <returns>Vrátí builder pro fluent API.</returns>
        /// <exception cref="ArgumentNullException">Vyvolá se, když je bodyPart null.</exception>
        public RaceBuilder AddBodyPart(BodyPart bodyPart)
        {
            if (bodyPart == null)
                throw new ArgumentNullException(nameof(bodyPart));

            _bodyParts.Add(bodyPart);
            return this;
        }

        /// <summary>
        /// Přidá speciální schopnost rasy.
        /// </summary>
        /// <param name="ability">Speciální schopnost k přidání.</param>
        /// <returns>Vrátí builder pro fluent API.</returns>
        /// <exception cref="ArgumentNullException">Vyvolá se, když je ability null.</exception>
        public RaceBuilder AddSpecialAbility(SpecialAbilities ability)
        {
            if (ability == null)
                throw new ArgumentNullException(nameof(ability));

            _specialAbilities.Add(ability);
            return this;
        }

        /// <summary>
        /// Přidá element do hierarchie rasy.
        /// </summary>
        /// <param name="hierarchy">Hierarchický element (kastovní systém, vojenská hierarchie, atd.).</param>
        /// <returns>Vrátí builder pro fluent API.</returns>
        /// <exception cref="ArgumentException">Vyvolá se, když je hierarchy prázdný nebo null.</exception>
        public RaceBuilder AddRaceHierarchy(List<string> raceHierarchies)
        {
            if (raceHierarchies == null)
                throw new ArgumentNullException("?usí být alespoň jeden záznam");

            _raceHierarchySystem.AddRange(raceHierarchies);
            return this;
        }



        #endregion


        /// <summary>
        /// Vytvoří finální Race agregát.
        /// </summary>
        /// <returns>Nová instance <see cref="Race"/> s nastavenými vlastnostmi, nebo upravenou původní instanci.</returns>
        /// <exception cref="InvalidOperationException">Vyvolá se, když nejsou nastaveny povinné vlastnosti.</exception>
        /// <remarks>
        /// <para>
        /// Metoda Build() kontroluje, zda jsou nastaveny povinné vlastnosti (jméno)
        /// a sestaví finální Race objekt se všemi nastaveními.
        /// </para>
        /// <para>
        /// Pokud byl builder vytvořen metodou <see cref="FromRace(Race)"/>, vrátí se upravená
        /// původní instance. V opačném případě se vytvoří nová instance.
        /// </para>
        /// <para>
        /// Volitelné vlastnosti jsou nastavovány pouze pokud byly poskytnuty.
        /// </para>
        /// </remarks>
        public Race Build()
        {
            // Validace povinných polí
            if (string.IsNullOrWhiteSpace(_raceName))
                throw new InvalidOperationException("Název rasy je povinný");

            if (_bodyDimensions == null)
                throw new InvalidOperationException("Tělesné rozměry (BodyDimensions) jsou povinné");

            Race race;

            // Pokud existuje originální rasa, edituj ji
            if (_originalRace != null)
            {
                race = _originalRace;
            }
            else
            {
                // Vytvoř novou Race
                race = new Race(_raceName, _raceCategory, _bodyDimensions);
            }

            // Nastav volitelná pole
            race.Id = _id;
            race.RaceDescription = _description;
            race.RaceHistory = _history;
            race.Conviction = _conviction;
            race.ZSM = _zsm;
            race.DomesticationValue = _domesticationValue;
            race.BaseInitiative = _baseInitiative;
            race.BaseXP = _baseXP;
            race.FightingSpiritNumber = _fightingSpiritNumber;

            // Přidej poklad
            if (_treasure != null)
            {
                race.Treasure = _treasure;
            }

            // Přidej body parts
            if (_bodyParts.Count > 0)
            {
                race.BodyParts = _bodyParts;
            }

            // Přidej race hierarchy
            if (_raceHierarchySystem.Count > 0)
            {
                race.RaceHierarchySystem = _raceHierarchySystem;
            }

            // Přidej special abilities
            if (_specialAbilities.Count > 0)
            {
                race.SpecialAbilities = _specialAbilities;
            }

            // Nastav statistiky
            if (_stats.Count > 0)
            {
                race.StatsPrimar = _stats;
            }

            // Nastav mobility
            if (_mobility.Count > 0)
            {
                race.Mobility = _mobility;
            }

            // Nastav vulnerabilities
            if (_vulnerabilities.Count > 0)
            {
                race.Vulnerabilities = _vulnerabilities;
            }

            return race;
        }
    }
}
