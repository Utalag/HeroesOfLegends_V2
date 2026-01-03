using HoL.Domain.Enums;
using HoL.Domain.Helpers;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoL.Domain.Helpers.AnatomiHelpers
{
    [Owned]
    public class BodyPartAttack
    {
        public Dice DamageDice { get; private set; }              // Kostka poškození způsobeného touto částí těla
        public DamageType DamageType { get; private set; }                  // Typ poškození způsobeného touto částí těla
        public int Initiative { get; private set; } = 1;                    // Iniciativa útoku této části těla
        public bool CanBeUsedWithOtherAttacks { get; private set; } = false; // Můzou být použity s jinými útoky 


        public BodyPartAttack(Dice damageDice, DamageType damageType, int initiative, bool canBeUsedWithOtherAttacks)
        {
            DamageDice = damageDice;
            DamageType = damageType;
            Initiative = initiative;
            CanBeUsedWithOtherAttacks = canBeUsedWithOtherAttacks;
        }

        public BodyPartAttack SetDice(Dice dice)
        {
            DamageDice = dice;
            return this;
        }
        public BodyPartAttack SetDamageType(DamageType damageType)
        {
            DamageType = damageType;
            return this;
        }
        public BodyPartAttack SetInitiative(int initiative)
        {
            Initiative = initiative;
            return this;
        }
        public BodyPartAttack SetCanBeUsedWithOtherAttacks(bool canBeUsed)
        {
            CanBeUsedWithOtherAttacks = canBeUsed;
            return this;
        }
    }

}
