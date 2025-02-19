using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRatesApp.Application.TechnicalInterfaces;

namespace ExchangeRatesApp.Infrastructure.CqrsDispatcherPipelineBehaviors
{
    public sealed class LockedHandlerPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class, IRequest<TResponse>
    {
        private readonly IHandlerContext _handlerContext;
        private readonly ApplicationContext _applicationContext;

        public LockedHandlerPipelineBehavior(IHandlerContext handlerContext, ApplicationContext applicationContext)
        {
            _handlerContext = handlerContext;
            _applicationContext = applicationContext;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (!string.IsNullOrWhiteSpace(_handlerContext.LockId))
            {
                /*
                 * [DESC]
                 * There might be situations where we want to ensure that a given handler is used only once at the same time.
                 * This lock mechanism can achieve that, but we need to carefully analyze the functionality case, as it works in a pessimistic way.
                 * In some sense the lock mechnism plays role of quequing mechnism.
                 * This solution is protected against potential problems by using a timeout.
                 */
                await _applicationContext.WaitLock(_handlerContext.LockId);

                try
                {
                    return await next();
                }
                finally
                {
                    _applicationContext.ReleaseLock(_handlerContext.LockId);
                }
            }
            else
            {
                return await next();
            }
        }
    }
}
