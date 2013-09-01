using System.Runtime.Serialization;

using Agatha.Common;

namespace Blitz.Common.Trading.Quote.Edit
{
    [DataContract]
    public class GetQuoteResponse : Response
    {
        [DataMember]
        public QuoteDto Result { get; set; }
    }
}