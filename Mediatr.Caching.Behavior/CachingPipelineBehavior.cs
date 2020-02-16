using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Mediatr.Caching.Behavior
{
    public class CachingPipelineBehavior<TRequest,TResponse>: IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IList<ICachingProfile<TRequest, TResponse>> _cacheableRequests;
        private readonly ILogger _logger;
        private readonly ICacheProvider _cacheProvider;

        public CachingPipelineBehavior(
            IList<ICachingProfile<TRequest, TResponse>> cacheableRequests, 
            ILogger logger,
            ICacheProvider cacheProvider)
        {
            _logger = logger;
            _cacheProvider = cacheProvider;
            this._cacheableRequests = cacheableRequests;
            
        }

        public async Task<TResponse> Handle(
            TRequest request, 
            CancellationToken cancellationToken, 
            RequestHandlerDelegate<TResponse> next)
        {
            var requestTypeFullName = request.GetType().FullName;
            _logger.LogInfo($"Entering CachingPipelineBehavior for {requestTypeFullName}");
            
            if (_cacheableRequests == null || !_cacheableRequests.Any())
            {
                _logger.LogInfo($"No caching profiles found for {requestTypeFullName}");
                _logger.LogInfo($"Exiting CachingPipelineBehavior for {requestTypeFullName}");
                return await next();
            }

            var cacheProfile = _cacheableRequests.FirstOrDefault();
            _logger.LogInfo($"Caching profile {cacheProfile.GetType().FullName} is being used for caching");

            return await _cacheProvider.GetAsync<TResponse>(cacheProfile.GetCacheKey(request),
                cacheProfile.GetExpirationDate(request), async () => await next());
        }
    }
}