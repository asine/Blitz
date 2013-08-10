using System.Collections.Generic;
using System.Runtime.Serialization;

using Agatha.Common;

namespace Blitz.Common.Customer
{
    [DataContract]
    public class ReportRunnerResponse : Response
    {
        [DataMember]
        public ICollection<ReportDto> Results { get; set; }
    }
}