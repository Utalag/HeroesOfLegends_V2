using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoL.Domain.ValueObjects
{
    [Owned]
    public class SpecialAbilities
    {
        public string AbilityName { get; set; } = string.Empty;
        public string AbilityDescription { get; set; } = string.Empty;
    }
}
