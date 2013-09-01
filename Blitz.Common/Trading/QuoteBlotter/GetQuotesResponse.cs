using System.Collections.Generic;
using System.Runtime.Serialization;

using Agatha.Common;

namespace Blitz.Common.Trading.QuoteBlotter
{
    [DataContract]
    public class GetQuotesResponse : Response
    {
        [DataMember]
        public ICollection<QuoteDto> Results { get; set; }
    }
}