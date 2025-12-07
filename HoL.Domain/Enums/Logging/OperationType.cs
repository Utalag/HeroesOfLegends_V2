using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoL.Domain.Enums.Logging
{
    public enum OperationType
    {
        Query = 1,
        Command = 2,
        Repository = 3,
        Security = 4,
        External = 5,
    }
}
