using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Score.Domain.Logging;
using Serilog;

namespace Scores.Infrastructure.MediatR.Behaviours
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger _logger;

        public LoggingBehaviour(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            if (request is IExposeLoggingInfo info)
            {
                _logger.Information("Handling {query} {@logInfo}", typeof(TRequest).Name, info.GetLoggingInfo());
            }
            else
            {
                _logger.Information("Handling {Name}", typeof(TRequest).Name);
            }

            var response = await next();

            _logger.Information("Handled {Name}", typeof(TResponse).Name);

            return response;
        }
    }
}
