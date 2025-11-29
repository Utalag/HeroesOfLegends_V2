using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace HoL.Aplication.Handlers.Queries.RaceQeries.AllRaces
{
    public class GetRaceAllQueryValidator : AbstractValidator<GetRaceAllQuery>
    {
        public GetRaceAllQueryValidator()
        {
            
        }
    }
}
