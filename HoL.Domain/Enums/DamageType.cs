namespace HoL.Domain.Enums
{
    [Flags]
    public enum DamageType
    {
        // Fyzické typy
        Bludgeoning,    // tupé zbraně, pád, úder
        Piercing,       // šípy, bodce, zuby
        Slashing,       // meče, drápy, sekery

        // Elementární typy
        Fire,       // oheň
        Cold,       // led
        Lightning,  // blesk
        Acid,       // kyselina
        Poison,     // toxické látky

        // Magické / mentální
        Psychic,    // mentální útoky
        Necrotic,   // smrtící magie
        Radiant,    // svatá energie
        Force,      // čistá magická síla
        Thunder,    // zvukové vlny

        // Speciální
        Magical,        // obecné magické poškození
        Divine,         // božské zásahy
        Shadow,         // stínová energie
        Chaos,          // nestabilní, mutační síla

        // Stavové / podmíněné
        Disease,        // nemoc
        Exhaustion,     // vyčerpání
        Corruption,     // zkažení, morální poškození

        // Technické / univerzální
        TrueDamage,     // ignoruje obranu, zbroj, odolnost
        Custom          // pro rozšíření nebo specifické efekty
    }

}
