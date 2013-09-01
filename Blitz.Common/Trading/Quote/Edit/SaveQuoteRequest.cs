using System.Runtime.Serialization;

using Blitz.Common.Agatha;

namespace Blitz.Common.Trading.Quote.Edit
{
    [DataContract]
    public class SaveQuoteRequest : Request<SaveQuoteResponse>
    {
        [DataMember]
        public QuoteDto Quote { get; set; }
    }
}