using System.Runtime.Serialization;

using Naru.Agatha;

namespace Blitz.Common.Trading.Quote.Blotter
{
    [DataContract]
    public class GetQuotesRequest : Request<GetQuotesResponse>
    {
    }
}