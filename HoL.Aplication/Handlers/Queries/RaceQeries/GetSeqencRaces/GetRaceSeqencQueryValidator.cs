using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace HoL.Aplication.Handlers.Queries.RaceQeries.GetSeqencRaces
{
    public class GetRaceSeqencQueryValidator : AbstractValidator<GetRaceSeqencQuery>
    {
        public GetRaceSeqencQueryValidator()
        {
            RuleFor(x => x.page).GreaterThanOrEqualTo(1);
            RuleFor(x => x.size).InclusiveBetween(1, 100);
            RuleFor(x => x.SortBy).Must(s => new[] { "RaceName", "Id", "RaceCategory" }.Contains(s))
                                  .When(x => !string.IsNullOrWhiteSpace(x.SortBy));
            
        }
    }
}
