namespace HoL.Aplication.DTOs.ValueObjectDtos
{
    /// <summary>
    /// Data Transfer Object pro reprezentaci bojového ducha entity.
    /// Určuje ochotu entity vstoupit do boje na základě úrovně nebezpečí.
    /// </summary>
    public class FightingSpiritDto
    {
        /// <summary>
        /// Hranice nebezpečí určující ochotu bojovat.
        /// </summary>
        public int DangerNumber { get; set; }
    }
}
