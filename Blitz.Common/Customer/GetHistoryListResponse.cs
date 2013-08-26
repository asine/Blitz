using System.Collections.Generic;
using System.Runtime.Serialization;

using Agatha.Common;

namespace Blitz.Common.Customer
{
    [DataContract]
    public class GetHistoryListResponse : Response
    {
        [DataMember]
        public ICollection<HistoryDto> Results { get; set; }
    }
}