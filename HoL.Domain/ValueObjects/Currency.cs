using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoL.Domain.ValueObjects
{
    [ComplexType]
    public class Currency
    {
        public int ?Gold { get; set; }
        public int ?Silver { get; set; }
        public int ?Copper { get; set; }
        public int[] ConveredRatio { get; set; } = new int[3] { 1, 10, 100 };
    }
}
