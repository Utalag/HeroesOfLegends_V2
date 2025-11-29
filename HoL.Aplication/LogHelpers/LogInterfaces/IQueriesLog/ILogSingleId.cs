using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoL.Aplication.LogHelpers.LogInterfaces.IQueriesLog
{
    public interface ILogSingleId : ICommonLog, ILogExceptions
    {
        public void LogInvalidId(object id);
        public void LogRetrievingEntity(object id);
        public void LogEntityNotFound(object id);
        public void LogEntityRetrievedSuccessfully(object id);
    }
}
