using HoL.Domain.Enums;
using HoL.Domain.Helpers;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoL.Domain.Helpers.AnatomiHelpers
{
    [Owned]
    public class BodyPartAttack
    {
        public Dice DamageDice { get; set; } = new();                     // Kostka poškození způsobeného touto částí těla
        public DamageType DamageType { get; set; }                  // Typ poškození způsobeného touto částí těla
        public int Initiative { get; set; } = 1;                    // Iniciativa útoku této části těla
        public bool CanBeUsedWithOtherAttacks { get; set; } = false; // Můzou být použity s jinými útoky 
    }

}
