using System.Collections.Generic;

namespace HoL.Aplication.DTOs.ValueObjectDtos
{
    public class TreasureDto
    {
        public int CurrencySetId { get; set; }
        public Dictionary<int, int> Amounts { get; set; } = new();
    }

}
