using System.Collections.Generic;

namespace Mediatr.Caching.Behavior.Tests
{
    public class Logger:ILogger
    {
        
        public IList<string> InfoLogs => new List<string>();
        public IList<string> WarningLogs => new List<string>();
        public void LogInfo(string info)
        {
            InfoLogs.Add(info);
        }

        public void LogWarning(string warning)
        {
            WarningLogs.Add(warning);
        }
    }
}
