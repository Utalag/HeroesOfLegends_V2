using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoL.Domain.Enums;
using HoL.Domain.Helpers.AnatomiHelpers;

namespace HoL.Domain.ValueObjects.Anatomi
{
    [ComplexType]
    public class BodyPart
    {
        /// <summary>Unikátní název části těla (např. "hlava", "levé křídlo", "ocas")</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Typ části těla (např. Hlava, Končetina, Smyslový orgán, Zbraň, Pohybový orgán)</summary>
        public BodyPartType Type { get; set; }

        /// <summary>Počet těchto částí u dané rasy</summary>
        public int Count { get; set; } = 1;

        /// <summary>Popis funkce (např. "vidění", "let", "útok", "rovnováha")</summary>
        public string Function { get; set; } = string.Empty;

        /// <summary>Je-li část těla schopná útoku, zde je definice</summary>
        public BodyPartAttack? Attack { get; set; }

        /// <summary>Je-li část těla zranitelná, zde je definice</summary>
        public BodyPartDefense? Defense { get; set; }

        /// <summary>Volitelný vizuální popis (např. "rozeklaný ocas", "zelené šupiny")</summary>
        public string Appearance { get; set; } = string.Empty;

        /// <summary>Je-li část těla magická nebo speciální</summary>
        public bool IsMagical { get; set; } = false;
    }

}
