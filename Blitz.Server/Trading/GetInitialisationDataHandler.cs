using System.Collections.Generic;

using Blitz.Common.Core;
using Blitz.Common.Trading.Quote.Edit;
using Blitz.Server.Core;

namespace Blitz.Server.Trading
{
    public class GetInitialisationDataHandler : Handler<GetInitialisationDataRequest, GetInitialisationDataResponse>
    {
        public GetInitialisationDataHandler(ILog log)
            : base(log)
        {
        }

        protected override GetInitialisationDataResponse Execute(GetInitialisationDataRequest request)
        {
            var response = CreateTypedResponse();

            var instruments = new List<LookupValue>();
            for (var index = 0; index < 10; index++)
            {
                var instrument = new LookupValue
                {
                    Id = index,
                    Value = "Instrument" + index
                };
                instruments.Add(instrument);
            }

            response.Instruments = instruments;

            return response;
        }
    }
}