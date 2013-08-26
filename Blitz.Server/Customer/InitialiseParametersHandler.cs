using System;
using System.Linq;

using Blitz.Common.Core;
using Blitz.Common.Customer;
using Blitz.Server.Core;

namespace Blitz.Server.Customer
{
    public class InitialiseParametersHandler : Handler<InitialiseParametersRequest, InitialiseParametersResponse>
    {
        public InitialiseParametersHandler(ILog log) 
            : base(log)
        {
        }

        protected override InitialiseParametersResponse Execute(InitialiseParametersRequest request)
        {
            var response = CreateTypedResponse();

            var dates = Enumerable.Range(0, 5)
                .Select(x => DateTime.Now.AddDays(-x))
                .ToList();

            response.AvailableDates = dates;

            return response;
        }
    }
}