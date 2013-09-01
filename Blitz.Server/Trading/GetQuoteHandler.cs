using Blitz.Common.Core;
using Blitz.Common.Trading.Quote;
using Blitz.Common.Trading.Quote.Edit;
using Blitz.Server.Core;

namespace Blitz.Server.Trading
{
    public class GetQuoteHandler : Handler<GetQuoteRequest, GetQuoteResponse>
    {
        public GetQuoteHandler(ILog log)
            : base(log)
        {
        }

        protected override GetQuoteResponse Execute(GetQuoteRequest request)
        {
            var response = CreateTypedResponse();

            var quote = new QuoteDto
            {
                Id = request.Id,
                QuoteReference = string.Format("xxxxx{0}", request.Id)
            };

            response.Result = quote;

            return response;
        }
    }
}