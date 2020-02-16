using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Mediatr.Caching.Behavior
{
    public interface ICacheableRequest<in TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        string GetCacheKey(TRequest request);
        DateTime GetExpirationDate(TRequest request);
    }
}
