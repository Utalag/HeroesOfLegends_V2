using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoL.Aplication.LogHelpers.LogInterfaces
{
    public interface ICommonLog
    {
        void LogInfo(string message);
        void LogWarning(string message);
    }
}
