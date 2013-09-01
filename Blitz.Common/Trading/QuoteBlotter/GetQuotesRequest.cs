using System.Runtime.Serialization;

using Blitz.Common.Agatha;

namespace Blitz.Common.Trading.QuoteBlotter
{
    [DataContract]
    public class GetQuotesRequest : Request<GetQuotesResponse>
    {
    }
}