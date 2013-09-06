﻿using System;
using System.Runtime.Serialization;

using Blitz.Common.Agatha;

namespace Blitz.Common.Trading.Quote.Edit
{
    [DataContract]
    public class GetQuoteRequest : Request<GetQuoteResponse>
    {
        [DataMember]
        public Guid Id { get; set; }
    }
}