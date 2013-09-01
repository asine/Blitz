using System.Runtime.Serialization;

using Blitz.Common.Agatha;

namespace Blitz.Common.Trading.Quote.Blotter
{
    [DataContract]
    public class GetQuotesRequest : Request<GetQuotesResponse>
    {
    }
}