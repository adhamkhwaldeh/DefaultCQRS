using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;


namespace AlJawad.DefaultCQRS.CQRS.Handlers
{
    public abstract class RequestHandlerBase<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
         where TRequest : IRequest<TResponse>
    {
        protected RequestHandlerBase(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }
        protected ILogger Logger { get; }

        public virtual async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            try
            {
                Logger.LogInformation("Processing request '{request}' ...", request);
                var watch = Stopwatch.StartNew();
                var response = await ProcessAsync(request, cancellationToken).ConfigureAwait(false);
                watch.Stop();
                Logger.LogInformation("Processed request '{requestName}': {elapsed} ms", request, watch.ElapsedMilliseconds);
                return response;
            }

            catch (Exception ex)
            {
                Logger.LogError(ex, "Error processing request '{requestName}': {errorMessage}", request, ex.Message);
                throw;
            }
        }

        protected abstract Task<TResponse> ProcessAsync(TRequest request, CancellationToken cancellationToken);
    }
}

