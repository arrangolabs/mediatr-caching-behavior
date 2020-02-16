using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Mediatr.Caching.Behavior
{
    public class CachingPipelineBehavior<TRequest,TResponse>: IPipelineBehavior<TRequest, TResponse>
    {
        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            throw new System.NotImplementedException();
        }
    }
}