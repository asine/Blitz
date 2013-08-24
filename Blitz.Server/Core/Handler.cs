using Agatha.Common;
using Agatha.ServiceLayer;

using Blitz.Common.Agatha;
using Blitz.Common.Core;

namespace Blitz.Server.Core
{
    public abstract class Handler<TRequest, TResponse> : RequestHandler<TRequest, TResponse> 
        where TRequest : Request<TResponse> 
        where TResponse : Response, new()
    {
        protected readonly ILog Log;

        protected Handler(ILog log)
        {
            Log = log;
        }

        public override Response Handle(TRequest request)
        {
            using (var performanceTester = new PerformanceTester())
            {
                Log.Info("Started processing request {0}, Id - {1}", typeof (TRequest).FullName, request.Id);

                var response = Execute(request);

                Log.Info("Finished processing request {0}, Id - {1}. Duration {2}", 
                    typeof (TRequest).FullName,
                    request.Id,
                    performanceTester.Result.Milliseconds);

                return response;
            }
        }

        protected abstract TResponse Execute(TRequest request);
    }
}