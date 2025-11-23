Popis složky `HoL.Domain.Helpers`

Co sem patří:
- Statické pomocné třídy a utilitky, které podporují logiku domény, ale nejsou samotnými doménními hodnotami.
  - např. `StatModifierTable`, převodní/formatovací helpery, konvertory pro serializaci, validátory výjimek
- Výjimky specifické pro doménu, které nejsou součástí pojednání o hodnotových objektech.
- Helpery pro transformace (např. převod `ValueRange` -> `Dice`) pokud jsou čistě implementační záležitostí.

Co sem nepatří:
- Doménní entity nebo Value Objects (ty patří do `HoL.Domain.ValueObjects` nebo `HoL.Domain.Entities`).
- Data access / repository kód (patří do Infrastructure).

Pravidla a doporučení:
- Pomocné třídy by měly být bezstavové (statické) nebo malými službami bez závislosti na persistence.
- Pokud helper implementuje doménní pravidla nebo invariants, zvaž přesun do `ValueObjects` nebo `Domain` — helpers by měly být "implementační" nikoli nosné logiky.
- Namespace: `HoL.Domain.Helpers` nebo podnamespace `HoL.Domain.Helpers.AnatomiHelpers` pokud jde o skupiny utilit.

Příklad typických souborů:
- `StatModifierTable.cs` (tabulka modifikátorů statistik)
- `ValueRange.cs` (pokud jde o konstrukci/konverzi na Dice, ale preferovaně přesunout do ValueObjects)
- `SerializationConverters/*.cs` (např. SafeEnumConverter)
- `AnatomiHelpers/BodyPartHelpers.cs` (pokud jsou čistě utilitární funkce pro práci s BodyPart)