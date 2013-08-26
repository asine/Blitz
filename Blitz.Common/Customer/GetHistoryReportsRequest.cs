using System;
using System.Runtime.Serialization;

using Blitz.Common.Agatha;

namespace Blitz.Common.Customer
{
    [DataContract]
    public class GetHistoryReportsRequest : Request<GetHistoryReportsResponse>
    {
        [DataMember]
        public long Id { get; set; }
    }
}