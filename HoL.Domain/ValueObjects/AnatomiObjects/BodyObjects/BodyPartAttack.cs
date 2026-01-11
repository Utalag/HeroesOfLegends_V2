using HoL.Domain.Enums;
using HoL.Domain.Helpers;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoL.Domain.ValueObjects.Anatomi.Body
{
    /// <summary>
    /// Reprezentuje útočné vlastnosti části těla.
    /// Definuje poškození, typ poškození, iniciativu a schopnost kombinovat útoky.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="BodyPartAttack"/> je value object, který zapouzdřuje útočné charakteristiky
    /// fyzické části těla v herním systému. Implementuje fluent API pro snadné konfigurování vlastností.
    /// </para>
    /// <para>
    /// Útočné vlastnosti zahrnují:
    /// <list type="bullet">
    /// <item><description><see cref="DamageDice"/> - Kostka určující rozsah poškození (např. d6, d8, d10)</description></item>
    /// <item><description><see cref="DamageType"/> - Typ poškození (slashing, piercing, bludgeoning, atd.)</description></item>
    /// <item><description><see cref="Initiative"/> - Iniciativa útoku (rychlost provedení útoku, minimum 1)</description></item>
    /// <item><description><see cref="CanBeUsedWithOtherAttacks"/> - Možnost kombinovat s jinými částmi těla</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// Typické příklady útočných částí:
    /// <list type="bullet">
    /// <item><description>Drápů, tesáků - d4, piercing, nízká iniciativa, lze kombinovat</description></item>
    /// <item><description>Paže (údery pěstí) - d4, bludgeoning, střední iniciativa, lze kombinovat</description></item>
    /// <item><description>Nohy (kopy) - d6, bludgeoning, nízká iniciativa, lze kombinovat</description></item>
    /// <item><description>Zobák - d8, piercing, vysoká iniciativa, nelze kombinovat</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <example>
    /// Vytváření a konfigurace útočného objektu pro část těla:
    /// <code>
    /// // Vytvoření útoku s d6 kostkou, piercing poškozením
    /// var claw = new BodyPartAttack(
    ///     damageDice: new Dice(1, 6),
    ///     damageType: DamageType.Piercing,
    ///     initiative: 2,
    ///     canBeUsedWithOtherAttacks: true
    /// );
    /// 
    /// // Konfigurace fluent API
    /// claw
    ///     .SetDice(new Dice(1, 8))        // Zvýšení poškození
    ///     .SetInitiative(3)               // Zvýšení iniciativy
    ///     .SetCanBeUsedWithOtherAttacks(false);
    /// </code>
    /// </example>
    public class BodyPartAttack
    {
        /// <summary>
        /// Získá nebo nastaví kostku poškození způsobeného touto částí těla.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Kostka poškození (Dice) určuje rozsah poškození, které část těla způsobí při úspěšném útoku.
        /// Skládá se z počtu kostek a typu kostky (např. 1d6, 2d8, 3d10).
        /// </para>
        /// <para>
        /// Typické kostky poškození podle typu útaku:
        /// <list type="bullet">
        /// <item><description>d4 - Malé ostré poranění (drápky, zubby)</description></item>
        /// <item><description>d6 - Střední poranění (bodnutí, údery)</description></item>
        /// <item><description>d8 - Velké poranění (tesáky, silné údery)</description></item>
        /// <item><description>d10+ - Extrémní poranění (obrovské drápy, speciální útoky)</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <value>Objekt <see cref="Dice"/> reprezentující kostku poškození.</value>
        public Dice DamageDice { get; private set; }

        /// <summary>
        /// Získá nebo nastaví typ poškození způsobeného touto částí těla.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Typ poškození ovlivňuje, jak se poškození vypočítává a aplikuje v herních mechanikách.
        /// Různé typy poškození mohou mít různou efektivitu proti různým typům obrany.
        /// </para>
        /// <para>
        /// Typické typy poškození:
        /// <list type="bullet">
        /// <item><description><see cref="DamageType.Slashing"/> - Sečné poranění (tesáky, drápy, meče)</description></item>
        /// <item><description><see cref="DamageType.Piercing"/> - Bodné poranění (ostny, bodci, šípy)</description></item>
        /// <item><description><see cref="DamageType.Bludgeoning"/> - Tupé poranění (údery, kosy)</description></item>
        /// <item><description><see cref="DamageType.Fire"/> - Ohnivé poranění (ohnivý dech, magický plamen)</description></item>
        /// <item><description><see cref="DamageType.Cold"/> - Mrazivé poranění (ledový dech, zmrzlina)</description></item>
        /// <item><description><see cref="DamageType.Lightning"/> - Elektrické poranění (blesk, magickýúder)</description></item>
        /// <item><description><see cref="DamageType.Poison"/> - Jedovaté poranění (jedovatý kus, jedovaté sliny)</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <value>Enum hodnota reprezentující typ poškození.</value>
        public DamageType DamageType { get; private set; }

        /// <summary>
        /// Získá nebo nastaví iniciativu útoku této části těla.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Iniciativa určuje, jak rychle lze tuto část těla použít v útoku.
        /// Vyšší hodnota znamená rychlejší útok, nižší hodnota znamená pomalejší útok.
        /// </para>
        /// <para>
        /// Typické hodnoty iniciativy:
        /// <list type="bullet">
        /// <item><description>1 - Velmi pomalý útok (těžká část těla, koordinace)</description></item>
        /// <item><description>2-3 - Normální útok (průměrná část těla)</description></item>
        /// <item><description>4-5 - Rychlý útok (lehká, pružná část)</description></item>
        /// <item><description>6+ - Velmi rychlý útok (specializovaná, vysoce reaktivní část)</description></item>
        /// </list>
        /// </para>
        /// <para>
        /// Minimální hodnota: 1 (obvyklé minimum v herních systémech)
        /// </para>
        /// </remarks>
        /// <value>Celočíselná hodnota reprezentující iniciativu útoku (1 nebo vyšší).</value>
        public int Initiative { get; private set; } = 1;

        /// <summary>
        /// Získá nebo nastaví příznak indikující, zda mohou být útoky touto částí používány s jinými útoky.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Některé částí těla mohou být použity kombinovně s ostatními útoky (např. dvě paže, obě nohy),
        /// zatímco jiné mohou být použity pouze samostatně (např. zobák nebo ohnivý dech).
        /// </para>
        /// <para>
        /// Příklady:
        /// <list type="bullet">
        /// <item><description><c>true</c> - Drápů lze použít spolu s údery paží, kopy, atd.</description></item>
        /// <item><description><c>true</c> - Paže lze kombinovat s jinými údery těla</description></item>
        /// <item><description><c>false</c> - Zobák je zvláštní útok, který vylučuje ostatní</description></item>
        /// <item><description><c>false</c> - Ohnivý dech vylučuje fyzické útoky v určitém tahu</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <value><c>true</c> pokud mohou být útoky kombinovány; jinak <c>false</c>. Výchozí hodnota: <c>false</c>.</value>
        public bool CanBeUsedWithOtherAttacks { get; private set; } = false;

        /// <summary>
        /// Inicializuje novou instanci třídy <see cref="BodyPartAttack"/> se všemi útočnými vlastnostmi.
        /// </summary>
        /// <param name="damageDice">Kostka určující rozsah poškození. Nesmí být null.</param>
        /// <param name="damageType">Typ poškození (slashing, piercing, atd.).</param>
        /// <param name="initiative">Iniciativa útoku (1 nebo vyšší).</param>
        /// <param name="canBeUsedWithOtherAttacks">Zda mohou být útoky kombinovány s ostatními.</param>
        /// <exception cref="ArgumentNullException">
        /// Vyvolá se, pokud <paramref name="damageDice"/> je null.
        /// </exception>
        /// <remarks>
        /// <para>
        /// Všechny parametry jsou povinné a inicializují odpovídající vlastnosti.
        /// Fluent API metody lze později použít k úpravě jednotlivých vlastností.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// // Vytvoření útoku paží (údery)
        /// var fistAttack = new BodyPartAttack(
        ///     damageDice: new Dice(1, 4),
        ///     damageType: DamageType.Bludgeoning,
        ///     initiative: 2,
        ///     canBeUsedWithOtherAttacks: true
        /// );
        /// 
        /// // Vytvoření speciálního útoku (zobák)
        /// var biteAttack = new BodyPartAttack(
        ///     damageDice: new Dice(1, 8),
        ///     damageType: DamageType.Piercing,
        ///     initiative: 3,
        ///     canBeUsedWithOtherAttacks: false
        /// );
        /// </code>
        /// </example>
        public BodyPartAttack(Dice damageDice, DamageType damageType, int initiative, bool canBeUsedWithOtherAttacks)
        {
            DamageDice = damageDice;
            DamageType = damageType;
            Initiative = initiative;
            CanBeUsedWithOtherAttacks = canBeUsedWithOtherAttacks;
        }

        /// <summary>
        /// Nastaví kostku poškození pro tuto část těla a vrátí stejný objekt pro fluent API.
        /// </summary>
        /// <param name="dice">Nová kostka poškození. Nesmí být null.</param>
        /// <returns>Vrací aktuální instanci (<c>this</c>) pro podporu fluent API chování.</returns>
        /// <remarks>
        /// <para>
        /// Tato metoda implementuje fluent API pattern, který umožňuje řetězení více volání
        /// pro jednoduchou a čitelnou konfiguraci objektu.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var attack = new BodyPartAttack(new Dice(1, 4), DamageType.Piercing, 2, true)
        ///     .SetDice(new Dice(1, 6))        // Zvýšení poškození
        ///     .SetInitiative(3);              // Zvýšení iniciativy
        /// </code>
        /// </example>
        public BodyPartAttack SetDice(Dice dice)
        {
            DamageDice = dice;
            return this;
        }

        /// <summary>
        /// Nastaví typ poškození pro tuto část těla a vrátí stejný objekt pro fluent API.
        /// </summary>
        /// <param name="damageType">Nový typ poškození.</param>
        /// <returns>Vrací aktuální instanci (<c>this</c>) pro podporu fluent API chování.</returns>
        /// <remarks>
        /// <para>
        /// Tato metoda implementuje fluent API pattern pro jednoduché řetězení konfigurací.
        /// Změna typu poškození ovlivňuje, jak se poškození aplikuje v herních mechanikách.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var attack = new BodyPartAttack(new Dice(1, 6), DamageType.Slashing, 2, true)
        ///     .SetDamageType(DamageType.Piercing);  // Změna typu poškození
        /// </code>
        /// </example>
        public BodyPartAttack SetDamageType(DamageType damageType)
        {
            DamageType = damageType;
            return this;
        }

        /// <summary>
        /// Nastaví iniciativu útoku pro tuto část těla a vrátí stejný objekt pro fluent API.
        /// </summary>
        /// <param name="initiative">Nová iniciativa útoku (obvykle 1 nebo vyšší).</param>
        /// <returns>Vrací aktuální instanci (<c>this</c>) pro podporu fluent API chování.</returns>
        /// <remarks>
        /// <para>
        /// Tato metoda implementuje fluent API pattern pro jednoduché řetězení konfigurací.
        /// Vyšší iniciativa znamená rychlejší útok v herním systému.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var attack = new BodyPartAttack(new Dice(1, 6), DamageType.Slashing, 1, true)
        ///     .SetInitiative(4);              // Zvýšení iniciativy
        /// </code>
        /// </example>
        public BodyPartAttack SetInitiative(int initiative)
        {
            Initiative = initiative;
            return this;
        }

        /// <summary>
        /// Nastaví, zda mohou být útoky touto částí verwendet s jinými útoky, a vrátí stejný objekt pro fluent API.
        /// </summary>
        /// <param name="canBeUsed"><c>true</c> pokud mohou být útoky kombinovány; jinak <c>false</c>.</param>
        /// <returns>Vrací aktuální instanci (<c>this</c>) pro podporu fluent API chování.</returns>
        /// <remarks>
        /// <para>
        /// Tato metoda implementuje fluent API pattern pro jednoduché řetězení konfigurací.
        /// Určuje, zda lze tuto část těla použít v kombinaci s jinými útoky v jednom tahu.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var attack = new BodyPartAttack(new Dice(1, 8), DamageType.Piercing, 3, false)
        ///     .SetCanBeUsedWithOtherAttacks(true);  // Povolení kombinování s jinými útoky
        /// </code>
        /// </example>
        public BodyPartAttack SetCanBeUsedWithOtherAttacks(bool canBeUsed)
        {
            CanBeUsedWithOtherAttacks = canBeUsed;
            return this;
        }
    }
}
