using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoL.Domain.Helpers
{
    public class Currency
    {
        public int Id { get; set; }
        public string Currency1Name { get; set; }= string.Empty;
        public string Currency2Name { get; set; }= string.Empty;
        public string Currency3Name { get; set; }= string.Empty;
        public string Currency4Name { get; set; }= string.Empty;
        public string Currency5Name { get; set; } = string.Empty;

        public override string? ToString()
        {
            return String.Format("{Currency1Name}, {Currency2Name}, {Currency3Name}, {Currency4Name}, {Currency5Name}"
                                  ,Currency1Name,Currency2Name,Currency3Name,Currency4Name,Currency5Name);
        }
    }
}
