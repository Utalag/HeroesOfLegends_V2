using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoL.Aplication.LogHelpers.LogInterfaces.ICommandLog
{
    public interface ILogDeleted : ICommonLog, ILogExceptions
    {
        void LogDeletingEntityInfo( object id);
        void LogEntityDeletedSuccessfully(object id);
        void LogEntityNotFound(object id);
    }
}
