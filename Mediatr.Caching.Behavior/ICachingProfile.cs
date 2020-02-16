using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Mediatr.Caching.Behavior
{
    public interface ICachingProfile<in TRequest, TResponse>
    {
        string GetCacheKey(TRequest request);
        DateTime GetExpirationDate(TRequest request);
    }
}
