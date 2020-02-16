using System;
using System.Threading.Tasks;

namespace Mediatr.Caching.Behavior
{
    public interface ICacheProvider
    {
        TResponse Get<TResponse>(string cacheKey, DateTime absoluteExpiryDate, Func<TResponse> getData);

        Task<TResponse> GetAsync<TResponse>(string cacheKey, DateTime absoluteExpiryDate, Func<Task<TResponse>> getData);
    }
}