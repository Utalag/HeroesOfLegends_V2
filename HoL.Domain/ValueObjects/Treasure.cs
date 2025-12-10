using HoL.Domain.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoL.Domain.ValueObjects
{
    [Owned]
    public class Treasure
    {
        public string GlobalCurrencyName { get; set; } = "Gold";
        public int Currency1 { get; set; }
        public int Currency2 { get; set; }
        public int Currency3 { get; set; }
        public int Currency4 { get; set; }
        public int Currency5 { get; set; }
        
        // Foreign Key
        public int CurrencyId { get; set; }
        public Currency? Currency { get; set; }
    }
}
