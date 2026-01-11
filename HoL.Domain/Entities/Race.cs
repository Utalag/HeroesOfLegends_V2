using HoL.Domain.Enums;
using HoL.Domain.ValueObjects;
using HoL.Domain.ValueObjects.Anatomi.Body;
using HoL.Domain.ValueObjects.Anatomi.Stat;

namespace HoL.Domain.Entities
{
    /// <summary>
    /// Reprezentuje herní rasu s kompletní definicí anatomických, statistických a mechanických vlastností.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="Race"/> je agregační entita, která definuje veškeré charakteristiky určité rasy v herním světě.
    /// Zahrnuje anatomické informace (tělesné části, rozměry), statistické vlastnosti, mobility, 
    /// zranitelnosti, schopnosti a další herní mechaniky.
    /// </para>
    /// <para>
    /// Povinné vlastnosti (nastavují se v konstruktoru):
    /// <list type="bullet">
    /// <item><description><see cref="RaceName"/> - Unikátní název rasy (např. "Elf", "Skřet", "Drak")</description></item>
    /// <item><description><see cref="RaceCategory"/> - Kategorie rasy (Humanoid, Beast, Dragon, atd.)</description></item>
    /// <item><description><see cref="BodyDimensins"/> - Rozměry těla (výška, šířka, hloubka)</description></item>
    /// <item><description><see cref="StatsPrimar"/> - Primární statistika (Síla, Obratnost, Odolnost, atd.)</description></item>
    /// <item><description><see cref="Vulnerabilities"/> - Zranitelnost vůči různým typům poškození</description></item>
    /// <item><description><see cref="Mobility"/> - Mobility v různých prostředích (chůze, let, plavání, atd.)</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// Volitelné vlastnosti (jsou inicializovány na výchozí hodnoty):
    /// <list type="bullet">
    /// <item><description><see cref="RaceDescription"/> - Detailný popis rasy a její charakteru</description></item>
    /// <item><description><see cref="RaceHistory"/> - Historie a původ rasy</description></item>
    /// <item><description><see cref="Conviction"/> - Ideální přístup rasy (Dobrý, Neutrální, Zlý)</description></item>
    /// <item><description><see cref="ZSM"/> - Zone Spawn Modifier (modifikátor pro generování)</description></item>
    /// <item><description><see cref="DomesticationValue"/> - Schopnost přidělení (domestikace)</description></item>
    /// <item><description><see cref="BaseInitiative"/> - Základní iniciativa všech jedinců rasy</description></item>
    /// <item><description><see cref="BaseXP"/> - Základní cena v XP za porážku</description></item>
    /// <item><description><see cref="FightingSpiritNumber"/> - Číslo bojového ducha (speciální efekt)</description></item>
    /// <item><description><see cref="Treasure"/> - Schopnost drop kořisti</description></item>
    /// <item><description><see cref="BodyParts"/> - Seznam tělesných částí rasy</description></item>
    /// <item><description><see cref="RaceHierarchySystem"/> - Hierarchické systémy v rase</description></item>
    /// <item><description><see cref="SpecialAbilities"/> - Speciální schopnosti rasy</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <example>
    /// Vytváření definice nové rasy:
    /// <code>
    /// // Vytvoření základní definition rasy
    /// var bodyDims = new BodyDimension(180, 50, 30); // výška, šířka, hloubka
    /// 
    /// var race = new Race("Elf", RaceCategory.Humanoid, bodyDims);
    /// race.RaceDescription = "Elegantní elfové jsou známí svou obratností a magií";
    /// race.RaceHistory = "Starobylá rasa, která obývá lesy po tisíciletí";
    /// race.BaseInitiative = 5;
    /// race.Conviction = ConvictionType.Good;
    /// 
    /// // Přidání tělesných částí
    /// race.BodyParts = new List&lt;BodyPart&gt;
    /// {
    ///     new BodyPart("hlava", BodyPartType.Head, 1).SetFunction("vidění"),
    ///     new BodyPart("paže", BodyPartType.Arm, 2).SetFunction("manipulace"),
    ///     new BodyPart("nohy", BodyPartType.Leg, 2).SetFunction("pohyb")
    /// };
    /// 
    /// // Přidání speciálních schopností
    /// race.SpecialAbilities = new List&lt;SpecialAbilities&gt;
    /// {
    ///     new SpecialAbilities("Nočné vidění", "Vidění ve tmě"),
    ///     new SpecialAbilities("Magická otpornost", "Odolnost vůči magii")
    /// };
    /// </code>
    /// </example>
    /// <seealso cref="BodyDimension"/>
    /// <seealso cref="BodyPart"/>
    /// <seealso cref="Treasure"/>
    /// <seealso cref="SpecialAbilities"/>
    /// <seealso cref="RaceCategory"/>
    /// <seealso cref="VulnerabilityType"/>
    /// <seealso cref="MobilityType"/>
    /// <seealso cref="ConvictionType"/>
    public class Race
    {
        #region Identifikace a Kategorizace

        /// <summary>
        /// Získá nebo nastaví unikátní identifikátor rasy v databázi.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Primární klíč entity, generovaný databází.
        /// Používá se pro referencování rasy v ostatních entitách.
        /// </para>
        /// </remarks>
        /// <value>Celočíselný identifikátor (0 = neuloženo v databázi).</value>
        public int Id { get; internal set; }

        /// <summary>
        /// Získá nebo nastaví unikátní název rasy.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Identifikátor rasy v herním světě. Měl by být unikátní a snadno rozpoznatelný.
        /// </para>
        /// <para>
        /// Příklady:
        /// <list type="bullet">
        /// <item><description>Elf</description></item>
        /// <item><description>Skřet</description></item>
        /// <item><description>Drak Červený</description></item>
        /// <item><description>Goblin</description></item>
        /// <item><description>Troll</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <value>String reprezentující název (povinný, nesmí být prázdný).</value>
        public string RaceName { get; internal set; } = string.Empty;

        /// <summary>
        /// Získá nebo nastaví kategorii rasy.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Klasifikuje rasu do určité kategorie, která ovlivňuje její herní chování.
        /// </para>
        /// <para>
        /// Typické kategorie:
        /// <list type="bullet">
        /// <item><description>Humanoid - Lidské rasy a humanoidní bytosti</description></item>
        /// <item><description>Beast - Zvířecí tvory</description></item>
        /// <item><description>Dragon - Drakové a drakobytnosti</description></item>
        /// <item><description>Undead - Nemrtvé tvory</description></item>
        /// <item><description>Magical - Čistě magické bytosti</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <value>Enum hodnota reprezentující kategorii.</value>
        public RaceCategory RaceCategory { get; internal set; }

        #endregion

        #region Anatomie a Fyzické Vlastnosti

        /// <summary>
        /// Získá nebo nastaví tělesné rozměry rasy.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Obsahuje informace o průměrných tělesných rozměrech jedinců rasy.
        /// Ovlivňuje fyzické interakce, schránky pohybu a herní balance.
        /// </para>
        /// </remarks>
        /// <value>Objekt <see cref="BodyDimension"/> s rozměry (povinný).</value>
        public BodyDimension BodyDimensins { get; internal set; }

        /// <summary>
        /// Získá nebo nastaví seznam tělesných částí typických pro tuto rasu.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Definuje anatomii rasy - jaké tělesné části má a jejich vlastnosti.
        /// Zahrnuje informace o útoku, obraně a funkci každé části.
        /// </para>
        /// <para>
        /// Příklady:
        /// <list type="bullet">
        /// <item><description>Hlava - vidění, smyslové vnímání</description></item>
        /// <item><description>Paže - manipulace, útok</description></item>
        /// <item><description>Nohy - pohyb, chůze</description></item>
        /// <item><description>Křídla - let (u létajících ras)</description></item>
        /// <item><description>Ocas - rovnováha, útok (u zvířecích ras)</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <value>List objektů <see cref="BodyPart"/> nebo null (volitelné).</value>
        public List<BodyPart>? BodyParts { get; internal set; }

        #endregion

        #region Statistika a Vlastnosti

        /// <summary>
        /// Získá nebo nastaví primární statistiky rasy.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Mapuje primární body charakteru (Síla, Obratnost, Odolnost, atd.)
        /// na jejich rozsahy hodnot pro tuto rasu.
        /// </para>
        /// <para>
        /// Typické statistiky:
        /// <list type="bullet">
        /// <item><description>Síla (STR) - Fyzická síla a schopnost zdvihat</description></item>
        /// <item><description>Obratnost (DEX) - Rychlost a koordinace</description></item>
        /// <item><description>Odolnost (CON) - Zdraví a fyzická vytrvalost</description></item>
        /// <item><description>Inteligence (INT) - Mentální schopnost</description></item>
        /// <item><description>Moudrost (WIS) - Vnímavost a instinkt</description></item>
        /// <item><description>Charisma (CHA) - Sociální dovednosti</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <value>Dictionary mapující <see cref="BodyStat"/> na <see cref="ValueRange"/> (povinný).</value>
        public Dictionary<BodyStat, ValueRange> StatsPrimar { get; internal set; } = new();

        /// <summary>
        /// Získá nebo nastaví iniciativu všech jedinců rasy.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Základní hodnota iniciativy, která se používá v combat systému.
        /// Vyšší hodnota znamená rychlejší reagování na útoky.
        /// </para>
        /// </remarks>
        /// <value>Celočíselná hodnota iniciativy (výchozí: 0).</value>
        public int BaseInitiative { get; internal set; } = 0;

        #endregion

        #region Combat a Zranitelnosti

        /// <summary>
        /// Získá nebo nastaví zranitelnosti rasy vůči různým typům poškození.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Mapuje typy poškození na multiplikátor, který ovlivňuje, jak moc poškození je účinné.
        /// Výchozí hodnota 1.0 znamená normální poškození.
        /// </para>
        /// <para>
        /// Příklady:
        /// <list type="bullet">
        /// <item><description>1.0 - Normální zranitelnost</description></item>
        /// <item><description>0.5 - Poloviční poškození (odolnost)</description></item>
        /// <item><description>2.0 - Dvojité poškození (slabost)</description></item>
        /// <item><description>0.0 - Imunita</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <value>Dictionary mapující <see cref="VulnerabilityType"/> na double multiplikátor (povinný).</value>
        public Dictionary<VulnerabilityType, double> Vulnerabilities { get; internal set; } = new();
        #endregion

        #region Mobilita a Pohyb

        /// <summary>
        /// Získá nebo nastaví mobility rasy v různých prostředích.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Mapuje typy mobility (chůze, běh, let, plavání, atd.) na jejich hodnoty.
        /// Určuje, jak se rasa pohybuje v různých prostředích.
        /// </para>
        /// <para>
        /// Typické mobility:
        /// <list type="bullet">
        /// <item><description>Walk - Chůze (10 = normální)</description></item>
        /// <item><description>Fly - Let (závisí na typu)</description></item>
        /// <item><description>Swim - Plavání (závisí na typu)</description></item>
        /// <item><description>Climb - Lezení (závisí na typu)</description></item>
        /// <item><description>Burrow - Kopání (pouze některé rasy)</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <value>Dictionary mapující <see cref="MobilityType"/> na celočíselné hodnoty (povinný).</value>
        public Dictionary<MobilityType, int> Mobility { get; internal set; } = new();

        #endregion

        #region Lore a Flavor

        /// <summary>
        /// Získá nebo nastaví detailný popis rasy.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Textový popis charakteru, chování a zvláštností rasy.
        /// Slouží pro flavor text a herní kontext.
        /// </para>
        /// </remarks>
        /// <value>String s popisem (výchozí: prázdný string).</value>
        public string RaceDescription { get; internal set; } = string.Empty;

        /// <summary>
        /// Získá nebo nastaví historii a původ rasy.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Narativní popis vzniku a vývoje rasy v herním světě.
        /// Může ovlivňovat dostupné questy a sociální interakce.
        /// </para>
        /// </remarks>
        /// <value>String s historií (výchozí: prázdný string).</value>
        public string RaceHistory { get; internal set; } = string.Empty;

        /// <summary>
        /// Získá nebo nastaví ideální přesvědčení rasy.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Udává tendenci rasy k určitému typu chování a morálky.
        /// </para>
        /// <para>
        /// Možné hodnoty:
        /// <list type="bullet">
        /// <item><description>Good - Dobrá přesvědčení</description></item>
        /// <item><description>Neutral - Neutrální přesvědčení</description></item>
        /// <item><description>Evil - Zlá přesvědčení</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <value>Enum hodnota <see cref="ConvictionType"/> (výchozí: Neutral).</value>
        public ConvictionType Conviction { get; internal set; } = ConvictionType.Neutral;

        #endregion

        #region Mechanika a Balanc

        /// <summary>
        /// Získá nebo nastaví základní XP hodnotu za porážku jedince rasy.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Určuje, kolik zkušenostních bodů hráč dostane za porážku typického jedince rasy.
        /// Vyšší hodnota = silnější rasa.
        /// </para>
        /// </remarks>
        /// <value>Celočíselná XP hodnota (výchozí: 0).</value>
        public int BaseXP { get; internal set; }

        /// <summary>
        /// Získá nebo nastaví číslo bojového ducha rasy.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Speciální mechanika ovlivňující bojový systém.
        /// Vyšší hodnota znamená více bojového ducha a odolnosti.
        /// </para>
        /// </remarks>
        /// <value>Celočíselná hodnota (výchozí: 0).</value>
        public int FightingSpiritNumber { get; internal set; }

        /// <summary>
        /// Získá nebo nastaví schopnost drop kořisti pro tuto rasu.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Definuje, jakou kořist (poklady) mohou jedinci rasy drop při porážce.
        /// </para>
        /// </remarks>
        /// <value>Objekt <see cref="Treasure"/> nebo null (volitelné).</value>
        public Treasure? Treasure { get; internal set; }

        /// <summary>
        /// Získá nebo nastaví Zone Spawn Modifier.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Modifikátor, který ovlivňuje generování tvorů rasy v určitých zónách.
        /// </para>
        /// </remarks>
        /// <value>Celočíselný modifikátor (výchozí: 0).</value>
        public int ZSM { get; internal set; } = 0;

        /// <summary>
        /// Získá nebo nastaví schopnost domestikace (přidělení) rasy.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Určuje, zda mohou být jedinci rasy zdomácněni nebo kontrolováni.
        /// Vyšší hodnota = lépe se dá s nimi manipulovat.
        /// </para>
        /// </remarks>
        /// <value>Celočíselná hodnota (výchozí: 0).</value>
        public int DomesticationValue { get; internal set; } = 0;

        #endregion

        #region Kultura a Sociální Struktura

        /// <summary>
        /// Získá nebo nastaví seznam speciálních schopností rasy.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Definuje nadpřirozené či rasové schopnosti dostupné všem jedincům rasy.
        /// Mohou zahrnovat magii, speciální útoky, nebo imunity.
        /// </para>
        /// <para>
        /// Příklady:
        /// <list type="bullet">
        /// <item><description>Nočné vidění - Vidět v temnu</description></item>
        /// <item><description>Magická odolnost - Odolnost vůči kouzlům</description></item>
        /// <item><description>Regenerace - Zotavení se ze zranění</description></item>
        /// <item><description>Létání - Přirozené létání</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <value>List objektů <see cref="SpecialAbilities"/> nebo null (volitelné).</value>
        public List<SpecialAbilities>? SpecialAbilities { get; internal set; }

        /// <summary>
        /// Získá nebo nastaví hierarchické systémy v rase.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Definuje sociální strukturu rasy - jak jsou jedinci organizováni.
        /// </para>
        /// <para>
        /// Příklady:
        /// <list type="bullet">
        /// <item><description>Kasta systém</description></item>
        /// <item><description>Vojenská hierarchie</description></item>
        /// <item><description>Rodiny a klany</description></item>
        /// <item><description>Magocracy (vláda kouzelníků)</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <value>List stringů popisujících hierarchii (volitelné).</value>
        public List<string>? RaceHierarchySystem { get; internal set; }

        #endregion

        /// <summary>
        /// Inicializuje novou instanci třídy <see cref="Race"/> s povinnými vlastnostmi.
        /// </summary>
        /// <param name="raceName">Unikátní název rasy (např. "Elf", "Skřet").</param>
        /// <param name="raceCategory">Kategorie rasy (Humanoid, Beast, atd.).</param>
        /// <param name="bodyDimensions">Tělesné rozměry rasy.</param>
        /// <remarks>
        /// <para>
        /// Konstruktor inicializuje:
        /// - Povinné vlastnosti (jméno, kategorie, rozměry)
        /// - Výchozí kolekcemi pro Vulnerabilities a Mobility (naplněny enum hodnotami)
        /// - Volitelné vlastnosti na prázdné/null hodnoty
        /// </para>
        /// <para>
        /// Vulnerabilities se inicializují na 1.0 pro všechny <see cref="VulnerabilityType"/> hodnoty.
        /// Mobility se inicializují na 10 pro všechny <see cref="MobilityType"/> hodnoty.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var bodyDims = new BodyDimension(180, 50, 30);
        /// var race = new Race("Elf", RaceCategory.Humanoid, bodyDims);
        /// </code>
        /// </example>
        public Race(string raceName, RaceCategory raceCategory, BodyDimension bodyDimensions)
        {
            if (string.IsNullOrWhiteSpace(raceName))
                throw new ArgumentException("Název rasy nesmí být prázdný nebo null.", nameof(raceName));
            
            if (bodyDimensions == null)
                throw new ArgumentNullException(nameof(bodyDimensions), "BodyDimensions nesmí být null.");
            
            RaceName = raceName;
            RaceCategory = raceCategory;
            BodyDimensins = bodyDimensions;
            
            foreach (var enumTyp in Enum.GetValues(typeof(VulnerabilityType)))
            {
                Vulnerabilities.Add((VulnerabilityType)enumTyp, 1.0);
            }
            foreach (var enumTyp in Enum.GetValues(typeof(MobilityType)))
            {
                Mobility.Add((MobilityType)enumTyp, 10);
            }
        }

    }

}

