using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoL.Aplication.LogHelpers.LogInterfaces.ICommandLog
{
    public interface ILogUpdated : ICommonLog, ILogExceptions
    {
        void LogUpdatingEntityInfo(object id);
        void LogEntityNotFound(object id);
        void LogEntityUpdatedSuccessfully(object id);
    }
}
