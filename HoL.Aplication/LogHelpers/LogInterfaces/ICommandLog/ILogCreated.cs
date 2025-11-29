using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoL.Aplication.LogHelpers.LogInterfaces.ICommandLog
{
    public interface ILogCreated : ICommonLog, ILogExceptions
    {
        void LogCreatingEntity();
        void LogEntityCreatedSuccessfully();
    }
}
