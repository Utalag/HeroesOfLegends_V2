using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace HoL.Aplication.Handlers.Queries.RaceQeries.GetRaceByIds
{
    public class GetRaceByIdsQueryValidator : AbstractValidator<GetRacesByIdsQuery>
    {
        public GetRaceByIdsQueryValidator()
        {
            // Kolekce musí existovat
            RuleFor(x => x.Ids)
                .NotNull().WithMessage("Ids kolekce je povinná.")
                .Must(x => x.Any()).WithMessage("Ids kolekce nesmí být prázdná.");

            // Každé Id > 0
            RuleForEach(x => x.Ids)
                .GreaterThan(0).WithMessage("Každé Id musí být větší než 0.");

            // Žádné duplicity
            RuleFor(x => x.Ids)
                .Must(ids => ids.Distinct().Count() == ids.Count())
                .WithMessage("Ids kolekce nesmí obsahovat duplicity.");


        }
    }
}
