using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoL.Domain.Entities.CurencyEntities;
using Microsoft.EntityFrameworkCore;

namespace HoL.Infrastructure.Data.Models
{
    public class CurrencyGroupDbModel
    {
        public int Id { get;  set; }

        public string GroupName { get; set; } = string.Empty;

        public ICollection<SingleCurrencyDbModel> Currencies { get;  set; } = new List<SingleCurrencyDbModel>();
    }
}
