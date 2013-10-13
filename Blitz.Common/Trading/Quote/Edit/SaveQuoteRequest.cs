using System.Runtime.Serialization;

using Naru.Agatha;

namespace Blitz.Common.Trading.Quote.Edit
{
    [DataContract]
    public class SaveQuoteRequest : Request<SaveQuoteResponse>
    {
        [DataMember]
        public QuoteDto Quote { get; set; }
    }
}