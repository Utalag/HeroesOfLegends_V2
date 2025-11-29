using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoL.Aplication.LogHelpers.LogInterfaces
{
    public interface ILogExceptions
    {
        public void LogOperationCanceled(object? id = null);
        public void LogUnexpectedError(Exception exception, object? id = null);
        public void LogValidationError(string validationMessage);
    }
}
