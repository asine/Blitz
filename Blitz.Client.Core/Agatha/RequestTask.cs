﻿using System;
using System.Threading.Tasks;

using Agatha.Common;

using Blitz.Common.Agatha;
using Blitz.Common.Core;

namespace Blitz.Client.Core.Agatha
{
    public interface IRequestTask
    {
        Task<TResponse> Get<TRequest, TResponse>(TRequest request)
            where TRequest : RequestBase<TResponse>
            where TResponse : Response;

        Task<TResult> Get<TRequest, TResponse, TResult>(TRequest request, Func<TResponse, TResult> selector)
            where TRequest : RequestBase<TResponse>
            where TResponse : Response;

        Task<TResponse> GetUnstarted<TRequest, TResponse>(TRequest request)
            where TRequest : RequestBase<TResponse>
            where TResponse : Response;
    }

    public class RequestTask : IRequestTask
    {
        private readonly ILog _log;
        private readonly Func<IRequestDispatcher> _requestDispatcher;

        public RequestTask(ILog log, Func<IRequestDispatcher> requestDispatcher)
        {
            _log = log;
            _requestDispatcher = requestDispatcher;
        }

        public Task<TResponse> Get<TRequest, TResponse>(TRequest request)
            where TRequest : RequestBase<TResponse>
            where TResponse : Response
        {
            return Task.Factory.StartNew(() => Execute<TRequest, TResponse>(request));
        }

        public Task<TResult> Get<TRequest, TResponse, TResult>(TRequest request, Func<TResponse, TResult> selector) 
            where TRequest : RequestBase<TResponse> 
            where TResponse : Response
        {
            return Task.Factory.StartNew(() =>
            {
                var response = Execute<TRequest, TResponse>(request);
                return selector(response);
            });
        }

        public Task<TResponse> GetUnstarted<TRequest, TResponse>(TRequest request) 
            where TRequest : RequestBase<TResponse> 
            where TResponse : Response
        {
            return new Task<TResponse>(() => Execute<TRequest, TResponse>(request));
        }

        private TResponse Execute<TRequest, TResponse>(TRequest request)
            where TRequest : RequestBase<TResponse>
            where TResponse : Response
        {
            using (var tester = new PerformanceTester())
            {
                request.Id = Guid.NewGuid().ToString();

                _log.Info("Start RequestTask {0}, Id - {1}", typeof (TRequest), request.Id);

                var response = _requestDispatcher().Get<TResponse>(request);

                _log.Info("Finished RequestTask {0}, Id - {1}. Duration {2}",
                    typeof (TRequest),
                    request.Id,
                    tester.Result.Milliseconds);

                return response;
            }
        }
    }
}