using Microsoft.EntityFrameworkCore;

namespace HoL.Infrastructure.Data.Models
{
    /// <summary>
    /// Databázový model pro poklad, který může padnout z tvora rasy.
    /// </summary>
    [Owned]
    public class TreasureDbModel
    {
        /// <summary>
        /// JSON serialized Dictionary<int, int> pro mapování měnových typů na množství.
        /// </summary>
        public string CoinQuantitiesJson { get; set; } = string.Empty;

        /// <summary>
        /// Cizí klíč na sadu měny.
        /// </summary>
        public int CurrencyId { get; set; }
        /// <summary>
        ///  Navigační vlastnost na měnovou sadu.
        /// </summary>
        public CurrencyGroupDbModel? CurrencyGroup { get; set; }

    }
}