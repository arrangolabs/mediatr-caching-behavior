using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mediatr.Caching.Behavior
{
    public interface ILogger
    {
        void LogInfo(string info);
        void LogWarning(string warning);
    }
}
