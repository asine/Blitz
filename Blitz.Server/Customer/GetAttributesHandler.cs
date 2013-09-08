using System.Collections.Generic;

using Blitz.Common.Customer;
using Blitz.Server.Core;

using Common.Logging;

namespace Blitz.Server.Customer
{
    public class GetAttributesHandler : Handler<GetAttributesRequest, GetAttributesResponse>
    {
        public GetAttributesHandler(ILog log) 
            : base(log)
        {
        }

        protected override GetAttributesResponse Execute(GetAttributesRequest request)
        {
            var response = CreateTypedResponse();

            var dimensions = new List<AttributeDto>();
            for (var index = 0; index < 10; index++)
            {
                dimensions.Add(new AttributeDto {Name = index.ToString()});
            }
            response.Dimensions = dimensions;

            var measures = new List<AttributeDto>();
            for (var index = 0; index < 10; index++)
            {
                measures.Add(new AttributeDto { Name = index.ToString() });
            }
            response.Measures = measures;

            return response;
        }
    }
}