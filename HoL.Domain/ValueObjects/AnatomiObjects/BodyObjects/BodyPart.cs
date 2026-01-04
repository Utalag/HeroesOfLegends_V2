using HoL.Domain.Enums;

namespace HoL.Domain.ValueObjects.Anatomi.Body
{

    public class BodyPart
    {
        /// <summary>Unikátní název části těla (např. "hlava", "levé křídlo", "ocas")</summary>
        public string Name { get; private set; } = string.Empty;

        /// <summary>Typ části těla (např. Hlava, Končetina, Smyslový orgán, Zbraň, Pohybový orgán)</summary>
        public BodyPartType BodyPartCategory { get; private set; }

        /// <summary>Počet těchto částí u dané rasy</summary>
        public int Quantity { get; private set; } = 1;

        /// <summary>Popis funkce (např. "vidění", "let", "útok", "rovnováha")</summary>
        public string Function { get; private set; } = string.Empty;

        /// <summary>Je-li část těla schopná útoku, zde je definice</summary>
        public BodyPartAttack? Attack { get; private set; }

        /// <summary>Je-li část těla zranitelná, zde je definice</summary>
        public BodyPartDefense? Defense { get; private set; }

        /// <summary>Volitelný vizuální popis (např. "rozeklaný ocas", "zelené šupiny")</summary>
        public string Appearance { get; private set; } = string.Empty;

        /// <summary>Je-li část těla magická nebo speciální</summary>
        public bool IsMagical { get; private set; } = false;

        // _____________________________ Konstruktor ____________________________
        public BodyPart(string name, BodyPartType type, int quantity)
        {
            Name = name;
            BodyPartCategory = type;
            Quantity = quantity;
        }  //✅ 

        // _____________________________ Metody _________________________________
        public BodyPart SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.");
            Name = name;
            return this;
        }   //✅ 
        public BodyPart SetType(BodyPartType bodyTyp)
        {
            BodyPartCategory = bodyTyp;
            return this;
        }  //✅ 
        public BodyPart SetQuantity(int quantity)
        {
            if (quantity < 1)
                throw new ArgumentException("Quantity must be at least 1.");
            Quantity = quantity;
            return this;
        }  //✅
        public BodyPart SetFunction(string function)
        {
            if (string.IsNullOrWhiteSpace(function))
                throw new ArgumentException("Function cannot be empty.");
            Function = function;
            return this;
        }  //✅ 
        public BodyPart SetAppearance(string appearance)
        {
            Appearance = appearance;
            return this;
        }  //✅
        public BodyPart SetIsMagical(bool isMagical)
        {
            IsMagical = isMagical;
            return this;
        }  //✅

        public BodyPart SetAttack(BodyPartAttack attack)
        {
            Attack = attack;
            return this;
        }  //✅
        public BodyPart SetDefense(BodyPartDefense defense)
        {
            Defense = defense;
            return this;
        }  //✅

    }
}
