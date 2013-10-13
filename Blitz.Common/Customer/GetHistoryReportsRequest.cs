using System.Runtime.Serialization;

using Naru.Agatha;

namespace Blitz.Common.Customer
{
    [DataContract]
    public class GetHistoryReportsRequest : Request<GetHistoryReportsResponse>
    {
        [DataMember]
        public long Id { get; set; }
    }
}