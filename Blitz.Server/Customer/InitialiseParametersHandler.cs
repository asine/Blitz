using System;
using System.Linq;

using Agatha.Common;
using Agatha.ServiceLayer;

using Blitz.Common.Customer;

namespace Blitz.Server.Customer
{
    public class InitialiseParametersHandler : RequestHandler<InitialiseParametersRequest, InitialiseParametersResponse>
    {
        public override Response Handle(InitialiseParametersRequest request)
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