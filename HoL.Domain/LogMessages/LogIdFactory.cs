using HoL.Domain.Enums.Logging;
using Microsoft.Extensions.Logging;

namespace HoL.Domain.LogMessages
{
    public static class LogIdFactory
    {
        // Format: P_O_L_VV

        /// <summary>
        /// Vytvoří EventId pro standardní vrstvy projektu
        /// </summary>
        public static EventId Create(
            ProjectLayerType layer,     // P
            OperationType op,           // O
            LogLevelCodeType level,     // L 
            EventVariantType variant)   // VV
        {
            var id = (int)layer * 10000
                   + (int)op * 1000
                   + (int)level * 100
                   + (int)variant;

            return new EventId(id, variant.ToString());
        }

    }
}
