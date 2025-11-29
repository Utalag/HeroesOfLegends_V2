using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoL.Aplication.LogHelpers.LogInterfaces.IQueriesLog
{
    public interface ILogMultipleIds : ICommonLog, ILogExceptions
    {
        public void LogNullIdsCollection();
        public void LogNoValidIds();
        public void LogProcessingIds(int rawCount, int normalizedCount, IEnumerable<object> ids);
        public void LogQueryCompleted(int requestedCount, int returnedCount);
        public void LogNoEntitiesFound();
        public void LogEntityNotFound(object id);
    }
}
