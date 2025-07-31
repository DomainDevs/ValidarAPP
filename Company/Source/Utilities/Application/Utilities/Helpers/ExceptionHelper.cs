using Sistran.Core.Framework.Logging;

namespace Sistran.Company.Application.Utilities.Helpers
{
    public static class ExceptionHelper
    {
        public static void LogError(string logMessage)
        {
            EventLogLogger eventLogLogger = new EventLogLogger(null);
            eventLogLogger.Name = "EventLog";
            eventLogLogger.LogError(logMessage);
        }
    }
}
