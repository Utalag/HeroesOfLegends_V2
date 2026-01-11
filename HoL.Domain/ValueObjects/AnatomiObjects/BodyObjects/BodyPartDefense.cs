using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoL.Domain.ValueObjects.Anatomi.Body
{
    /// <summary>
    /// Reprezentuje obranné vlastnosti části těla.
    /// Definuje brnění, vitálnost a přirozenou ochranu specifické části těla.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="BodyPartDefense"/> je value object, který zapouzdřuje obranné charakteristiky
    /// fyzické části těla v herním systému. Implementuje fluent API pro snadné konfigurování vlastností.
    /// </para>
    /// <para>
    /// Obranné vlastnosti zahrnují:
    /// <list type="bullet">
    /// <item><description><see cref="ArmorValue"/> - Poskytuje trvalou redukci poškození (brníček, kůže, atd.)</description></item>
    /// <item><description><see cref="IsVital"/> - Indikuje, že zničení této části má závažné následky (srdce, mozek, atd.)</description></item>
    /// <item><description><see cref="IsProtected"/> - Znamená, že část je přirozeně chráněna nebo je obrácena krunýřem</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// Validace a invarianty:
    /// <list type="bullet">
    /// <item><description><see cref="ArmorValue"/> nemůže být záporné (minimálně 0)</description></item>
    /// <item><description>Všechny vlastnosti lze měnit prostřednictvím fluent API metod</description></item>
    /// <item><description>Výchozí hodnota <see cref="IsVital"/> a <see cref="IsProtected"/> je <c>false</c></description></item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <example>
    /// Vytváření a konfigurace obranného objektu pro část těla:
    /// <code>
    /// // Vytvoření se základní hodnotou brnění
    /// var defense = new BodyPartDefense(armorValue: 5);
    /// 
    /// // Konfigurace fluent API
    /// defense
    ///     .SetVital(true)           // Tato část je důležitá pro přežití
    ///     .SetProtected(true)       // Je přirozeně chráněna
    ///     .SetArmorValue(8);        // Zvýšení brnění
    /// </code>
    /// </example>
    public class BodyPartDefense
    {
        /// <summary>
        /// Získá nebo nastaví hodnotu brnění poskytovaného touto částí těla.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Brnění (armor value) představuje schopnost části těla absorbovat nebo zmírňovat poškození.
        /// Vyšší hodnota znamená větší ochranu. Brnění je obvykle poskytnuto:
        /// <list type="bullet">
        /// <item><description>Přirozenou strukturou (kost, kůže, muskuly)</description></item>
        /// <item><description>Nošeným vybavením (brníček, zbroj, štít)</description></item>
        /// </list>
        /// </para>
        /// <para>
        /// Minimální hodnota: 0 (bez ochrany)
        /// Maximální hodnota: Teoreticky bez limitu, záleží na herní vyvážení
        /// </para>
        /// </remarks>
        /// <value>Celočíselná hodnota reprezentující úroveň brnění (0 nebo vyšší).</value>
        public int ArmorValue { get; private set; }

        /// <summary>
        /// Získá nebo nastaví příznak indikující, zda je tato část těla vitální pro přežití.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Vitální část těla je taková, jejíž zničení nebo vážné zranění má katastrofální následky
        /// pro zdraví nebo funkčnost bytosti. Typické vitální části:
        /// <list type="bullet">
        /// <item><description>Srdce - kritické pro oběh krve</description></item>
        /// <item><description>Mozek - kritické pro vědomí a neuronální funkce</description></item>
        /// <item><description>Játra - kritické pro detoxikaci</description></item>
        /// <item><description>Plíce - kritické pro dýchání</description></item>
        /// </list>
        /// </para>
        /// <para>
        /// Zranění vitální části může spustit speciální mechaniky, jako např. krvácení, ztrátu vědomí
        /// nebo okamžitou smrt, v závislosti na herních pravidlech.
        /// </para>
        /// </remarks>
        /// <value><c>true</c> pokud je část vitální; jinak <c>false</c>. Výchozí hodnota: <c>false</c>.</value>
        public bool IsVital { get; private set; } = false;

        /// <summary>
        /// Získá nebo nastaví příznak indikující, zda je tato část těla přirozeně chráněna.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Chráněná část těla je ta, která má zlepšenou obranu díky:
        /// <list type="bullet">
        /// <item><description>Přirozené anatomii (krunýř je chráněn skořápkou či schránkou)</description></item>
        /// <item><description>Nošenému vybavení (brnění, štít, krunýř)</description></item>
        /// <item><description>Magické ochraně nebo speciálním vlastnostem</description></item>
        /// </list>
        /// </para>
        /// <para>
        /// Tento příznak se často používá v herních mechanikách k určení, zda lze část těla
        /// přiblížit nebo zranit určitými typy útoků.
        /// </para>
        /// </remarks>
        /// <value><c>true</c> pokud je část chráněna; jinak <c>false</c>. Výchozí hodnota: <c>false</c>.</value>
        public bool IsProtected { get; private set; } = false;

        /// <summary>
        /// Inicializuje novou instanci třídy <see cref="BodyPartDefense"/> se zadanou hodnotou brnění.
        /// </summary>
        /// <param name="armorValue">Počáteční hodnota brnění. Musí být 0 nebo vyšší.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Vyvolá se, pokud <paramref name="armorValue"/> je menší než 0.
        /// </exception>
        /// <remarks>
        /// <para>
        /// Vlastnosti <see cref="IsVital"/> a <see cref="IsProtected"/> se inicializují na výchozí hodnotu <c>false</c>.
        /// Ty lze změnit později pomocí fluent API metod <see cref="SetVital"/> a <see cref="SetProtected"/>.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// // Vytvoření obranného objektu s brnění hodnotou 5
        /// var defense = new BodyPartDefense(armorValue: 5);
        /// 
        /// // Vytvoření bez brnění
        /// var unarmored = new BodyPartDefense(armorValue: 0);
        /// </code>
        /// </example>
        public BodyPartDefense(int armorValue)
        {
            if (armorValue < 0)
                throw new ArgumentOutOfRangeException(nameof(armorValue), "Armor value cannot be negative.");
            ArmorValue = armorValue;
        }

        /// <summary>
        /// Nastaví, zda je tato část těla vitální pro přežití, a vrátí stejný objekt pro fluent API.
        /// </summary>
        /// <param name="isVital"><c>true</c> pokud je část těla vitální; jinak <c>false</c>.</param>
        /// <returns>Vrací aktuální instanci (<c>this</c>) pro podporu fluent API chování.</returns>
        /// <remarks>
        /// <para>
        /// Tato metoda implementuje fluent API pattern, který umožňuje řetězení více volání
        /// pro jednoduchou a čitelnou konfiguraci objektu.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var defense = new BodyPartDefense(5)
        ///     .SetVital(true)           // Nastaví vitálnost
        ///     .SetProtected(true);      // Nastaví ochranu
        /// </code>
        /// </example>
        public BodyPartDefense SetVital(bool isVital)
        {
            IsVital = isVital;
            return this;
        }

        /// <summary>
        /// Nastaví, zda je tato část těla přirozeně chráněna, a vrátí stejný objekt pro fluent API.
        /// </summary>
        /// <param name="isProtected"><c>true</c> pokud je část těla chráněna; jinak <c>false</c>.</param>
        /// <returns>Vrací aktuální instanci (<c>this</c>) pro podporu fluent API chování.</returns>
        /// <remarks>
        /// <para>
        /// Tato metoda implementuje fluent API pattern pro jednoduché řetězení konfigurací.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var defense = new BodyPartDefense(8)
        ///     .SetProtected(true);      // Označí část jako chráněnou
        /// </code>
        /// </example>
        public BodyPartDefense SetProtected(bool isProtected)
        {
            IsProtected = isProtected;
            return this;
        }

        /// <summary>
        /// Nastaví hodnotu brnění pro tuto část těla a vrátí stejný objekt pro fluent API.
        /// </summary>
        /// <param name="armorValue">Nová hodnota brnění. Musí být 0 nebo vyšší.</param>
        /// <returns>Vrací aktuální instanci (<c>this</c>) pro podporu fluent API chování.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Vyvolá se, pokud <paramref name="armorValue"/> je menší než 0.
        /// </exception>
        /// <remarks>
        /// <para>
        /// Tato metoda umožňuje změnit hodnotu brnění po inicializaci objektu.
        /// Stejně jako ostatní fluent metody, vrací <c>this</c> pro snadné řetězení volání.
        /// </para>
        /// <para>
        /// Validace zajistí, že hodnota brnění zůstane logicky konzistentní
        /// (brnění nemůže být záporné).
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var defense = new BodyPartDefense(5)
        ///     .SetArmorValue(10)        // Zvýšení brnění
        ///     .SetVital(true);          // Označení jako vitální
        /// </code>
        /// </example>
        public BodyPartDefense SetArmorValue(int armorValue)
        {
            if (armorValue < 0)
                throw new ArgumentOutOfRangeException(nameof(armorValue), "Armor value cannot be negative.");
            ArmorValue = armorValue;
            return this;
        }
    }
}
