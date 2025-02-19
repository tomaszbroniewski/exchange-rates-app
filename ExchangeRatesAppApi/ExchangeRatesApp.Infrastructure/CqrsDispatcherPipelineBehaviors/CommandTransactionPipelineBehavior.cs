using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRatesApp.Application.TechnicalInterfaces;

namespace ExchangeRatesApp.Infrastructure.CqrsDispatcherPipelineBehaviors
{
    /*
     * [DESC]
     * We are using MediatR's PipelineBehavior to ensure database transactions at the handler level.
     */
    public sealed class CommandTransactionPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class, IRequest<TResponse>
    {
        private readonly IRequestHandler<TRequest, TResponse> _requestHandler;
        private readonly IUnitOfWork _uow;

        public CommandTransactionPipelineBehavior(IRequestHandler<TRequest, TResponse> requestHandler, IUnitOfWork uow)
        {
            _requestHandler = requestHandler;
            _uow = uow;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            TResponse response;

            if (_requestHandler.GetType().Name.EndsWith("CommandHandler"))
            {
                using (var trScope = _uow.CreateTransactionScope())
                {
                    response = await next();

                    await _uow.CommitAsync();
                    trScope.Complete();
                }

                await ExecuteActionsAfterDbCommitSuccess();
            }
            else
            {
                response = await next();
            }

            return response;
        }

        private static Task ExecuteActionsAfterDbCommitSuccess()
        {
            // Here run actions (needs to be implemeneted) 
            return Task.CompletedTask;
        }
    }
}
