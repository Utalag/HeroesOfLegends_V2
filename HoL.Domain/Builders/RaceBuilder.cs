using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoL.Domain.Entities;
using HoL.Domain.Enums;
using HoL.Domain.ValueObjects;
using HoL.Domain.ValueObjects.Anatomi;
using HoL.Domain.ValueObjects.Anatomi.Stat;

namespace HoL.Domain.Builders
{
    /// <summary>
    /// Builder pro vytváření Race agregátů s fluent API.
    /// Umožňuje snadné a čitelné vytváření komplexních objektů.
    /// </summary>
    public class RaceBuilder
    {
        // Povinná pole
        private string _raceName = string.Empty;
        private RaceCategory _raceCategory = RaceCategory.Humanoid;
        private BodyDimension _bodyDimensions = new();
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


    #region Povinné vlastnosti
        /// <summary>
        /// Nastaví povinné pole - název rasy.
        /// </summary>
        public RaceBuilder WithName(string raceName)
        {
            if (string.IsNullOrWhiteSpace(raceName))
                throw new ArgumentException("Název rasy nesmí být prázdný", nameof(raceName));

            _raceName = raceName;
            return this;
        } //✅ 

        /// <summary>
        /// Nastaví kategorii rasy.
        /// </summary>
        public RaceBuilder WithCategory(RaceCategory category)
        {
            _raceCategory = category;
            return this;
        } // ✅ 

        /// <summary>
        /// Nastaví anatomii rasy.
        /// </summary>
        public RaceBuilder WithBodyDimensions(BodyDimension bodyDimensions)
        {
            if (bodyDimensions == null)
                throw new ArgumentNullException(nameof(bodyDimensions));

            _bodyDimensions = bodyDimensions;
            return this;
        } //✅ 

        /// <summary>
        /// Nastaví staty rasy.
        /// </summary>
        public RaceBuilder AddStat(BodyStat bodyStat, ValueRange range)
        {
            if (range == null)
                throw new ArgumentNullException(nameof(range));

            _stats[bodyStat] = range;
            return this;
        } //✅ 
        /// <summary>
        /// Nastaví zranitelnost.
        /// </summary>
        public RaceBuilder WithVulnerability(VulnerabilityType type, double modifier)
        {
            _vulnerabilities[type] = modifier;
            return this;
        } //✅ 
        /// <summary>
        /// Nastaví mobilitu.
        /// </summary>
        public RaceBuilder WithMobility(MobilityType mobilityType, int speed)
        {
            if (speed < 0)
                throw new ArgumentOutOfRangeException(nameof(speed), "Rychlost nemůže být záporná");

            _mobility[mobilityType] = speed;
            return this;
        } //✅

        #endregion

        #region Volitené vlastnosti

        /// <summary>
        /// Nastaví popis a historii rasy.
        /// </summary>
        public RaceBuilder WithDescription(string description, string history)
        {
            _description = description ?? string.Empty;
            _history = history ?? string.Empty;
            return this;
        } //✅

        /// <summary>
        /// Nastaví BaseXP hodnotu.
        /// </summary>
        public RaceBuilder WithBaseXP(int baseXP)
        {
            if (baseXP < 0)
                throw new ArgumentOutOfRangeException(nameof(baseXP), "BaseXP nemůže být záporné");

            _baseXP = baseXP;
            return this;
        } //✅

        /// <summary>
        /// Nastaví Fighting Spirit Number.
        /// </summary>
        public RaceBuilder WithFightingSpirit(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Fighting Spirit nemůže být záporný");

            _fightingSpiritNumber = value;
            return this;
        } //✅

        /// <summary>
        /// Nastaví základní iniciativu.
        /// </summary>
        public RaceBuilder WithBaseInitiative(int initiative)
        {
            if (initiative < 0)
                throw new ArgumentOutOfRangeException(nameof(initiative), "Iniciativa nemůže být záporná");

            _baseInitiative = initiative;
            return this;
        } //✅

        /// <summary>
        /// Přidá poklad.
        /// </summary>
        public RaceBuilder WithTreasure(TreasureBuilder treasure)
        {
            if (treasure == null)
                throw new ArgumentNullException(nameof(treasure));

            _treasure = treasure.Build();
            return this;
        } //✅

        /// <summary>
        /// Přidá část těla.
        /// </summary>
        public RaceBuilder AddBodyPart(BodyPart bodyPart)
        {
            if (bodyPart == null)
                throw new ArgumentNullException(nameof(bodyPart));

            _bodyParts.Add(bodyPart);
            return this;
        } //✅

        /// <summary>
        /// Přidá speciální schopnost.
        /// </summary>
        public RaceBuilder AddSpecialAbility(SpecialAbilities ability)
        {
            if (ability == null)
                throw new ArgumentNullException(nameof(ability));

            _specialAbilities.Add(ability);
            return this;
        } //✅

        #endregion


        /// <summary>
        /// Vytvoří finální Race agregát.
        /// </summary>
        public Race Build()
        {
            // Validace povinných polí
            if (string.IsNullOrWhiteSpace(_raceName))
                throw new InvalidOperationException("Název rasy je povinný");

            // Vytvoř Race
            var race = new Race(_raceName, _raceCategory, _bodyDimensions);

            // Nastav volitelná pole
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
