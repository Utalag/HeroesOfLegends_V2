using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HoL.Domain.Enums;
using HoL.Domain.Helpers;

namespace HoL.Domain.ValueObjects.Stat
{

    [ComplexType]
    // Nová implementace: Min + DiceCount model (Max je odvozené)
    public class ValueRange
    {
        /// <summary>
        /// Minimální možná hodnota (např. minimální součet hodů).
        /// </summary>
        public int Min { get; set; }

        /// <summary>
        /// Počet hodů (počet kostek), např. 2 znamená 2 hodů danou kostkou.
        /// </summary>
        public int DiceCount { get; set; } = 1;

        /// <summary>
        /// Typ kostky (D6, D20 ...).
        /// </summary>
        public DiceType DiceType { get; set; } = DiceType.D6;

        /// <summary>
        /// Max je odvozené tak, aby span = Max - Min = DiceCount * (sides - 1)
        /// </summary>
        public int Max => Min + DiceCount * ((int)DiceType - 1);

        /// <summary>
        /// Popis kostky
        /// </summary>
        public Dice Dice { get => ToDice();}

        
        
        
        
        /// <summary>
        /// Převod na popis hodů (Dice) — validuje vstupy.
        /// </summary>
        private Dice ToDice()
        {
            if (DiceCount <= 0)
                throw new InvalidValueRangeException("DiceCount must be greater than zero.");

            int sides = (int)DiceType;
            if (sides <= 1)
                throw new InvalidValueRangeException($"Invalid DiceType: {DiceType}");

            int bonus = Min - DiceCount; // protože minimální součet n kostek = n
            return new Dice { Count = DiceCount, Sides = DiceType, Bonus = bonus };
        }

        /// <summary>
        /// Výjimka vyhozená při neplatném rozsahu hodnot.
        /// (ponecháno z předchozí implementace pro kompatibilitu)
        /// </summary>
        public class InvalidValueRangeException : ArgumentException
        {
            public InvalidValueRangeException(string message) : base(message) { }
        }

        /// <summary>
        /// Pohodlné vlastnosti pro kompatibilitu s původním API.
        /// </summary>
        public Dice DiceRoll => ToDice();
    }
}

/*  
  // --- Původní implementace (zachována zakomentovaná pro referenci) ---
  public class ValueRange
  {
      private int max;
      private int span;
      private int sides;
      private int unit ;
      private int rolls;
      private int rest ;

      [Required] public int Min { get; set; }
      //[Required] public int Max { get => max; set => max = value; }
      [Required] public int Max 
      { get => max;
        set =>RecalculateMax(value); 
      }
      public DiceType DiceType { get; set; } = DiceType.D6;
      public Dice DiceRoll { get => ToDice(); }

      /// <summary>
      /// Výjimka vyhozená při neplatném rozsahu hodnot.
      /// </summary>
      public class InvalidValueRangeException : ArgumentException
      {
          public InvalidValueRangeException(string message) : base(message) { }
      }

      /// <summary>
      /// Převede rozsah na konfiguraci hodů (počet kostek, typ, bonus).
      /// Pokud je Max menší než Min, vyhodí InvalidValueRangeException.
      /// </summary>

      public Dice ToDice()
      {
          // Ensure Max is recalculated according to current DiceType and Min
          RecalculateMax(max);

          span = Max - Min;
          sides = (int)DiceType;
          unit = sides - 1;
          rolls = span / unit;
          int bonus = Min - rolls;
          return new Dice { Count = rolls, Sides = DiceType, Bonus = bonus };
      }

      private void RecalculateMax(int value)
      {
          if (value < Min)
          {
              string message = $"ValueRange.RecalculateMax: Invalid range - Max ({value}) < Min ({Min}).";
              throw new InvalidValueRangeException(message);
          }

          span = value - Min;
          sides = (int)DiceType;
          if (sides <= 1)
          {
              string message = $"ValueRange.RecalculateMax: Invalid DiceType ({DiceType}).";
              throw new InvalidValueRangeException(message);
          }

          unit = sides - 1;
          rolls = span / unit;
          rest = span % unit;
          if (rest != 0)
          {
              // round to nearest: if remainder is at least half of unit, round up
              if (rest * 2 >= unit)
              {
                  rolls += 1; // round up
              }
              // set max based on rolls (either rounded up or down)
              max = Min + rolls * unit;
          }
          else
          {
              max = value;
          }
      }
  }
  // --- Konec původní implementace ---
  */