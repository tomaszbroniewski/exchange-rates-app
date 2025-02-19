using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRatesApp.Application.TechnicalInterfaces;

namespace ExchangeRatesApp.Infrastructure.CqrsDispatcherPipelineBehaviors
{
    public sealed class LoggingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class, IRequest<TResponse>
    {
        private readonly ILogger<LoggingPipelineBehavior<TRequest, TResponse>> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRequestHandler<TRequest, TResponse> _requestHandler;
        private readonly ICurrentUser _currentUser;

        public LoggingPipelineBehavior(ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger, IHttpContextAccessor httpContextAccessor,
            IRequestHandler<TRequest, TResponse> requestHandler, ICurrentUser currentUser)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _requestHandler = requestHandler;
            _currentUser = currentUser;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var (executionContextId, handlerName, currentUser, inputData) = FetchLoggingData(request);

            using (_logger.BeginScope(LoggingHelper.GetScopedDictionary(executionContextId, handlerName, currentUser, inputData)))
            {
                _httpContextAccessor.HttpContext.SetScopedLoggingData(handlerName, currentUser, inputData);

                var logMessage = $"Executing handler: | {LoggingHelper.ScopedLogMessageForHandler}";
                _logger.LogInformation(logMessage);

                return await next();
            }
        }

        private (string executionContextId, string handlerName, string currentUser, string inputData) FetchLoggingData(TRequest request)
        {
            var handlerName = _requestHandler.GetType().Name ?? "N/A";
            var currentUser = _currentUser.IsAuthenticated ? _currentUser.UserId! : "N/A";
            var inputData = request.SerializeToJsonForLog();
            var executionContextId = _httpContextAccessor?.HttpContext?.TraceIdentifier ?? "N/A";

            return (executionContextId, handlerName, currentUser, inputData);
        }
    }
}
