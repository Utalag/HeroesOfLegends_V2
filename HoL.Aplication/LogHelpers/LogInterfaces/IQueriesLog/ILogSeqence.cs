using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoL.Aplication.LogHelpers.LogInterfaces.IQueriesLog
{
    public interface ILogSequence : ICommonLog, ILogExceptions
    {
        public void LogSequenceQueryStart(int page, int size, string? sortBy, string sortDir);
        public void LogSequenceQueryCompleted(int count);
        public void LogNoEntitiesFound();

    }
}
