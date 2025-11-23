using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using HoL.Domain.Enums;

namespace HoL.Domain.Helpers
{
    [ComplexType]
    public class Dice
    {
        public int Count { get; set; } = 1;
        public DiceType Sides { get; set; } = DiceType.D6;
        public int Bonus { get; set; } = 0;

        public int Roll(Random rng)
        =>
        Enumerable.Range(0, Count).Sum(_ => rng.Next(1, (int)Sides + 1)) + Bonus;


    }
}
