using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoL.Domain.Enums;
using HoL.Domain.Helpers;

namespace HoL.Domain.ValueObjects
{
    [ComplexType]
    public class FightingSpirit
    {
        public int DangerNumber { get; set; }
       
        public bool IsFighting()
        {
            var dice = new Dice() { Bonus = 0, Sides = DiceType.D6, Count = 2 };

            if (DangerNumber == 0)
                return false;
           if (dice.Roll(new Random()) < DangerNumber)
                return false;
           return true;
        }
    }
}
