using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoL.Infrastructure.Data.Models
{
    public class SingleCurrencyDbModel
    {
        public int Id { get;set; }

        /// <summary>
        /// Název měny.
        /// </summary>
        public string Name { get;set; } = string.Empty;

        /// <summary>
        /// Krátký název měny.
        /// </summary>
        public string ShotName { get;set; } = string.Empty;

        /// <summary>
        /// Úroveň (1 = nejvyšší hodnat, vyšší číslo = nižší hodnota).
        /// </summary>
        public int HierarchyLevel { get;set; }

        /// <summary>
        /// Směnný kurz vůči základní denominaci.
        /// </summary>
        public int ExchangeRate { get;set; }
    }
}
