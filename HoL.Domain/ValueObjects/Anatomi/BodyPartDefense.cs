using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoL.Domain.Helpers.AnatomiHelpers
{
    [Owned]
    public class BodyPartDefense
    {
        public int ArmorValue { get; set; }             // Hodnota brnění poskytovaná touto částí těla
        public bool IsVital { get; set; } = false;      // Pokud je tato část těla zničena, může to mít vážné následky
        public bool IsProtected { get; set; } = false;  // Pokud je část těla přirozeně chráněna (např. krunýřem)
    }

}
