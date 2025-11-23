namespace HoL.Aplication.DTOs.ValueObjectDtos
{
    /// <summary>
    /// Data Transfer Object pro reprezentaci měnových hodnot v herním systému.
    /// </summary>

   
    public class CurrencyDto
    {
        /// <summary>
        /// Počet zlatých mincí (nejvyšší měnová jednotka).
        /// </summary>
        /// <value>
        /// Hodnota musí být >= 0 pokud je zadána.
        /// <c>null</c> znamená že zlaté mince nejsou součástí této měny.
        /// 1 Gold = 10 Silver = 100 Copper.
        /// </value>
        public int? Gold { get; set; }

        /// <summary>
        /// Počet stříbrných mincí (střední měnová jednotka).
        /// </summary>
        /// <value>
        /// Hodnota musí být >= 0 pokud je zadána.
        /// <c>null</c> znamená že stříbrné mince nejsou součástí této měny.
        /// 1 Silver = 10 Copper, 10 Silver = 1 Gold.
        /// </value>
        public int? Silver { get; set; }

        /// <summary>
        /// Počet měděných mincí (nejnižší měnová jednotka).
        /// </summary>
        /// <value>
        /// Hodnota musí být >= 0 pokud je zadána.
        /// <c>null</c> znamená že měděné mince nejsou součástí této měny.
        /// 100 Copper = 1 Gold, 10 Copper = 1 Silver.
        /// </value>
        public int? Copper { get; set; }


        /// <summary>
        /// Prevodni pomer mezi mincemi (Gold, Silver, Copper).
        /// </summary>
        public int[] ConveredRatio { get; set; } = new int[3] { 1, 10, 100 };


    }
}
