using HoL.Domain.Enums;

namespace HoL.Domain.ValueObjects.Anatomi.Body
{
    /// <summary>
    /// Reprezentuje fyzickou část tělaEntity (postava, zvíře, nestvůra).
    /// Zapouzdřuje základní vlastnosti, funkčnost a combat charakteristiky jednotlivých částí těla.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="BodyPart"/> je value object, který slouží k definování jednotlivých fyzických částí těla.
    /// Kombinuje anatomické informace s herními vlastnostmi (útok, obrana, funkce).
    /// </para>
    /// <para>
    /// Povinné vlastnosti (nastavují se v konstruktoru):
    /// <list type="bullet">
    /// <item><description><see cref="Name"/> - Unikátní identifikátor části (např. "hlava", "levé křídlo")</description></item>
    /// <item><description><see cref="BodyPartCategory"/> - Typ/kategorie části (Hlava, Končetina, Zbraň, atd.)</description></item>
    /// <item><description><see cref="Quantity"/> - Počet těchto částí (např. 2 paže, 1 hlava, 4 nohy)</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// Volitelné vlastnosti (nastavují se přes fluent API):
    /// <list type="bullet">
    /// <item><description><see cref="Function"/> - Popis funkce (vidění, pohyb, útok, atd.)</description></item>
    /// <item><description><see cref="Attack"/> - Útočné vlastnosti (pokud je část schopna útoku)</description></item>
    /// <item><description><see cref="Defense"/> - Obranné vlastnosti (pokud je část zranitelná)</description></item>
    /// <item><description><see cref="Appearance"/> - Vizuální popis (barva, textura, zvláštnosti)</description></item>
    /// <item><description><see cref="IsMagical"/> - Příznak magických/speciálních vlastností</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// Typické příklady tělesných částí:
    /// <list type="bullet">
    /// <item><description>Hlava - Hlava, vidění, jediná (Quantity=1), obvykle zranitelná</description></item>
    /// <item><description>Paže - Končetina, útok/úchop, dva kusy (Quantity=2), zranitelné</description></item>
    /// <item><description>Nohy - Pohybový orgán, chůze/běh, čtyři kusy (Quantity=4), zranitelné</description></item>
    /// <item><description>Ocas - Pohybový orgán, rovnováha, jeden kus (Quantity=1), může být zbraní</description></item>
    /// <item><description>Křídla - Pohybový orgán, let, dva kusy (Quantity=2), možná magická</description></item>
    /// <item><description>Rohy - Zbraň, útok, obvykle dva kusy (Quantity=2), s útočnými vlastnostmi</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <example>
    /// Vytváření a konfigurace tělesné části:
    /// <code>
    /// // Vytvoření jednoduché hlavy
    /// var head = new BodyPart("hlava", BodyPartType.Head, 1)
    ///     .SetFunction("vidění a smyslové vnímání")
    ///     .SetDefense(new BodyPartDefense(5).SetVital(true));
    /// 
    /// // Vytvoření paže s možností útoku
    /// var arm = new BodyPart("levá paž", BodyPartType.Limb, 1)
    ///     .SetFunction("úchop a manipulace")
    ///     .SetAttack(new BodyPartAttack(new Dice(1, DiceType.D4, 0), DamageType.Bludgeoning, 2, true))
    ///     .SetDefense(new BodyPartDefense(3));
    /// 
    /// // Vytvoření magického křídla
    /// var wing = new BodyPart("levé křídlo", BodyPartType.MovementOrgan, 1)
    ///     .SetFunction("let")
    ///     .SetAppearance("irridescenční modro-fialové peří")
    ///     .SetAttack(new BodyPartAttack(new Dice(1, DiceType.D6, 0), DamageType.Slashing, 3, false))
    ///     .SetIsMagical(true);
    /// </code>
    /// </example>
    public class BodyPart
    {
        /// <summary>
        /// Získá nebo nastaví unikátní název části těla.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Název je identifikátor části těla, který ji rozlišuje od ostatních.
        /// Může obsahovat umístění (např. "levé", "pravé", "horní", "dolní") nebo označení číslem.
        /// </para>
        /// <para>
        /// Příklady:
        /// <list type="bullet">
        /// <item><description>hlava</description></item>
        /// <item><description>levá paž</description></item>
        /// <item><description>pravá přední noha</description></item>
        /// <item><description>levé křídlo</description></item>
        /// <item><description>ocas</description></item>
        /// <item><description>trup</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <value>String reprezentující název části (nesmí být prázdný či null).</value>
        public string Name { get; private set; } = string.Empty;

        /// <summary>
        /// Získá nebo nastaví typ/kategorii části těla.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Kategorie určuje roli a funkci části v anatomické struktuře.
        /// </para>
        /// <para>
        /// Typické kategorie:
        /// <list type="bullet">
        /// <item><description><see cref="BodyPartType.Head"/> - Hlava, senzorické centrum</description></item>
        /// <item><description><see cref="BodyPartType.Limb"/> - Končetina (paže, nohy)</description></item>
        /// <item><description><see cref="BodyPartType.Torso"/> - Trup, centrální část těla</description></item>
        /// <item><description><see cref="BodyPartType.Tail"/> - Ocas nebo podobný dodatek</description></item>
        /// <item><description><see cref="BodyPartType.Wing"/> - Křídlo nebo létající orgán</description></item>
        /// <item><description><see cref="BodyPartType.Weapon"/> - Přirozená zbraň (rohy, zuby, drápy)</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <value>Enum hodnota reprezentující typ části.</value>
        public BodyPartType BodyPartCategory { get; private set; }

        /// <summary>
        /// Získá nebo nastaví počet těchto částí u entity.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Označuje, kolik instancí dané části těla má entita.
        /// Například člověk má 2 paže, 2 nohy, 1 hlavu, 1 trup, atd.
        /// </para>
        /// <para>
        /// Minimální hodnota: 1 (alespoň jedna část musí existovat)
        /// </para>
        /// </remarks>
        /// <value>Celočíselná hodnota reprezentující počet (1 nebo vyšší).</value>
        public int Quantity { get; private set; } = 1;

        /// <summary>
        /// Získá nebo nastaví popis funkce/účelu části těla.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Funkce popisuje herní a biologickou roli části těla.
        /// </para>
        /// <para>
        /// Příklady:
        /// <list type="bullet">
        /// <item><description>vidění a smyslové vnímání (oči)</description></item>
        /// <item><description>chůze a běh (nohy)</description></item>
        /// <item><description>úchop a manipulace (paže)</description></item>
        /// <item><description>kousání a žvýkání (tlama)</description></item>
        /// <item><description>rovnováha a steerage (ocas)</description></item>
        /// <item><description>létání (křídla)</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <value>String popis funkce.</value>
        public string Function { get; private set; } = string.Empty;

        //----------------------------------------------------------------------

        /// <summary>
        /// Získá nebo nastaví útočné vlastnosti části těla.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Pokud je část těla schopna útoku (např. paže, rohy, kousání),
        /// zde jsou definovány její útočné charakteristiky.
        /// </para>
        /// <para>
        /// Null pokud část není schopna útoku (např. oči, nohy v pasivním kontextu).
        /// </para>
        /// </remarks>
        /// <value>Objekt <see cref="BodyPartAttack"/> nebo null.</value>
        public BodyPartAttack? Attack { get; private set; }

        /// <summary>
        /// Získá nebo nastaví obranné vlastnosti části těla.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Definuje zranitelnost a obranné schopnosti části.
        /// Obsahuje informace o brnění a vitálnosti.
        /// </para>
        /// <para>
        /// Null pokud je část imunní vůči poškození (např. nehmotné části).
        /// </para>
        /// </remarks>
        /// <value>Objekt <see cref="BodyPartDefense"/> nebo null.</value>
        public BodyPartDefense? Defense { get; private set; }

        /// <summary>
        /// Získá nebo nastaví vizuální popis části těla.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Volitelný popis pro flavor text a detailní charakteristiku vzhledu.
        /// </para>
        /// <para>
        /// Příklady:
        /// <list type="bullet">
        /// <item><description>zelené šupiny se zlatými okraji</description></item>
        /// <item><description>rozeklaný ocas s trny</description></item>
        /// <item><description>irridescenční modro-fialové peří</description></item>
        /// <item><description>ostrý pazurek s černými nehty</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <value>String reprezentující vizuální popis (prázdný řetězec pokud není nastaveno).</value>
        public string Appearance { get; private set; } = string.Empty;

        /// <summary>
        /// Získá nebo nastaví, zda je část těla magická nebo speciální.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Příznak indikuje, že část má magické vlastnosti, která ji odlišuje od normálních částí.
        /// Může ovlivňovat Combat mechaniky, interakce s magií, nebo speciální efekty.
        /// </para>
        /// </remarks>
        /// <value><c>true</c> pokud je část magická; jinak <c>false</c>. Výchozí hodnota: <c>false</c>.</value>
        public bool IsMagical { get; private set; } = false;

        /// <summary>
        /// Inicializuje novou instanci třídy <see cref="BodyPart"/> s povinnými vlastnostmi.
        /// </summary>
        /// <param name="name">Unikátní název části těla. Nesmí být prázdný či null.</param>
        /// <param name="type">Typ/kategorie části těla.</param>
        /// <param name="quantity">Počet těchto částí (1 nebo vyšší).</param>
        /// <exception cref="ArgumentException">
        /// Vyvolá se, pokud <paramref name="name"/> je prázdný či obsahuje jen whitespace.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Vyvolá se, pokud <paramref name="quantity"/> je menší než 1.
        /// </exception>
        /// <remarks>
        /// <para>
        /// Konstruktor nastavuje pouze povinné vlastnosti.
        /// Volitelné vlastnosti lze nastavit později prostřednictvím fluent API metod.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// // Vytvoření jednoduché hlavy
        /// var head = new BodyPart("hlava", BodyPartType.Head, 1);
        /// 
        /// // Vytvoření paží (dva kusy)
        /// var arms = new BodyPart("paž", BodyPartType.Limb, 2);
        /// </code>
        /// </example>
        public BodyPart(string name, BodyPartType type, int quantity)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.");
            if (quantity < 1)
                throw new ArgumentException("Quantity must be at least 1.");
            
            Name = name;
            BodyPartCategory = type;
            Quantity = quantity;
        }

        /// <summary>
        /// Nastaví název části těla a vrátí stejný objekt pro fluent API.
        /// </summary>
        /// <param name="name">Nový název. Nesmí být prázdný či null.</param>
        /// <returns>Vrací aktuální instanci (<c>this</c>) pro podporu fluent API chování.</returns>
        /// <exception cref="ArgumentException">
        /// Vyvolá se, pokud <paramref name="name"/> je prázdný či obsahuje jen whitespace.
        /// </exception>
        /// <example>
        /// <code>
        /// var part = new BodyPart("paž", BodyPartType.Limb, 1)
        ///     .SetName("levá paž");
        /// </code>
        /// </example>
        public BodyPart SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.");
            Name = name;
            return this;
        }

        /// <summary>
        /// Nastaví typ/kategorii části těla a vrátí stejný objekt pro fluent API.
        /// </summary>
        /// <param name="bodyTyp">Nový typ části.</param>
        /// <returns>Vrací aktuální instanci (<c>this</c>) pro podporu fluent API chování.</returns>
        /// <example>
        /// <code>
        /// var part = new BodyPart("část", BodyPartType.Limb, 1)
        ///     .SetType(BodyPartType.Weapon);
        /// </code>
        /// </example>
        public BodyPart SetType(BodyPartType bodyTyp)
        {
            BodyPartCategory = bodyTyp;
            return this;
        }

        /// <summary>
        /// Nastaví počet těchto částí a vrátí stejný objekt pro fluent API.
        /// </summary>
        /// <param name="quantity">Nový počet (1 nebo vyšší).</param>
        /// <returns>Vrací aktuální instanci (<c>this</c>) pro podporu fluent API chování.</returns>
        /// <exception cref="ArgumentException">
        /// Vyvolá se, pokud <paramref name="quantity"/> je menší než 1.
        /// </exception>
        /// <example>
        /// <code>
        /// var part = new BodyPart("noha", BodyPartType.Limb, 2)
        ///     .SetQuantity(4);
        /// </code>
        /// </example>
        public BodyPart SetQuantity(int quantity)
        {
            if (quantity < 1)
                throw new ArgumentException("Quantity must be at least 1.");
            Quantity = quantity;
            return this;
        }

        /// <summary>
        /// Nastaví funkci/účel části těla a vrátí stejný objekt pro fluent API.
        /// </summary>
        /// <param name="function">Popis funkce. Nesmí být prázdný či null.</param>
        /// <returns>Vrací aktuální instanci (<c>this</c>) pro podporu fluent API chování.</returns>
        /// <exception cref="ArgumentException">
        /// Vyvolá se, pokud <paramref name="function"/> je prázdný či obsahuje jen whitespace.
        /// </exception>
        /// <example>
        /// <code>
        /// var part = new BodyPart("hlava", BodyPartType.Head, 1)
        ///     .SetFunction("vidění a smyslové vnímání");
        /// </code>
        /// </example>
        public BodyPart SetFunction(string function)
        {
            if (string.IsNullOrWhiteSpace(function))
                throw new ArgumentException("Function cannot be empty.");
            Function = function;
            return this;
        }

        /// <summary>
        /// Nastaví vizuální popis části těla a vrátí stejný objekt pro fluent API.
        /// </summary>
        /// <param name="appearance">Popis vzhledu (může být prázdný).</param>
        /// <returns>Vrací aktuální instanci (<c>this</c>) pro podporu fluent API chování.</returns>
        /// <remarks>
        /// <para>
        /// Na rozdíl od jiných metod, tato akceptuje i prázdné stringy
        /// pro případ, kdy chcete odebrat vizuální popis.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var part = new BodyPart("křídlo", BodyPartType.Wing, 2)
        ///     .SetAppearance("irridescenční modro-fialové peří");
        /// </code>
        /// </example>
        public BodyPart SetAppearance(string appearance)
        {
            Appearance = appearance;
            return this;
        }

        /// <summary>
        /// Nastaví, zda je část těla magická, a vrátí stejný objekt pro fluent API.
        /// </summary>
        /// <param name="isMagical"><c>true</c> pokud je část magická; jinak <c>false</c>.</param>
        /// <returns>Vrací aktuální instanci (<c>this</c>) pro podporu fluent API chování.</returns>
        /// <example>
        /// <code>
        /// var part = new BodyPart("křídlo", BodyPartType.Wing, 2)
        ///     .SetIsMagical(true);
        /// </code>
        /// </example>
        public BodyPart SetIsMagical(bool isMagical)
        {
            IsMagical = isMagical;
            return this;
        }

        /// <summary>
        /// Nastaví útočné vlastnosti části těla a vrátí stejný objekt pro fluent API.
        /// </summary>
        /// <param name="attack">Objekt s útočnými vlastnostmi. Může být null.</param>
        /// <returns>Vrací aktuální instanci (<c>this</c>) pro podporu fluent API chování.</returns>
        /// <remarks>
        /// <para>
        /// Nastavení null odebere útočné vlastnosti.
        /// Tato metoda pouze nastavuje referenci; validace je v odpovědnosti objektu <see cref="BodyPartAttack"/>.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var attack = new BodyPartAttack(new Dice(1, DiceType.D6, 0), DamageType.Slashing, 2, true);
        /// var part = new BodyPart("paž", BodyPartType.Limb, 1)
        ///     .SetAttack(attack);
        /// </code>
        /// </example>
        public BodyPart SetAttack(BodyPartAttack attack)
        {
            Attack = attack;
            return this;
        }

        /// <summary>
        /// Nastaví obranné vlastnosti části těla a vrátí stejný objekt pro fluent API.
        /// </summary>
        /// <param name="defense">Objekt s obranými vlastnostmi. Může být null.</param>
        /// <returns>Vrací aktuální instanci (<c>this</c>) pro podporu fluent API chování.</returns>
        /// <remarks>
        /// <para>
        /// Nastavení null indikuje, že část není zranitelná (imunní vůči poškození).
        /// Tato metoda pouze nastavuje referenci; validace je v odpovědnosti objektu <see cref="BodyPartDefense"/>.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var defense = new BodyPartDefense(5).SetVital(true);
        /// var part = new BodyPart("hlava", BodyPartType.Head, 1)
        ///     .SetDefense(defense);
        /// </code>
        /// </example>
        public BodyPart SetDefense(BodyPartDefense defense)
        {
            Defense = defense;
            return this;
        }
    }
}
