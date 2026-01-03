using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoL.Domain.Helpers.AnatomiHelpers
{
    public class BodyPartDefense
    {
        public int ArmorValue { get;private set; }             // Hodnota brnění poskytovaná touto částí těla
        public bool IsVital { get; private set; } = false;      // Pokud je tato část těla zničena, může to mít vážné následky
        public bool IsProtected { get;private  set; } = false;  // Pokud je část těla přirozeně chráněna (např. krunýřem)

        public BodyPartDefense(int armorValue)
        {
            if (armorValue < 0)
                throw new ArgumentOutOfRangeException(nameof(armorValue), "Armor value cannot be negative.");
            ArmorValue = armorValue;
        }  //✅

        public BodyPartDefense SetVital(bool isVital)
        {
            IsVital = isVital;
            return this;
        }  //✅
        public BodyPartDefense SetProtected(bool isProtected)
        {
            IsProtected = isProtected;
            return this;
        }  //✅
        public BodyPartDefense SetArmorValue(int armorValue)
        {
            if (armorValue < 0)
                throw new ArgumentOutOfRangeException(nameof(armorValue), "Armor value cannot be negative.");
            ArmorValue = armorValue;
            return this;
        }  //✅
    }

}
