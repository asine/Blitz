using System;
using System.Runtime.Serialization;

using Naru.Agatha;

namespace Blitz.Common.Trading.Quote.Edit
{
    [DataContract]
    public class GetQuoteRequest : Request<GetQuoteResponse>
    {
        [DataMember]
        public Guid Id { get; set; }
    }
}