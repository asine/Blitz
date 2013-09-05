using System.Collections.Generic;
using System.Runtime.Serialization;

using Agatha.Common;

namespace Blitz.Common.Trading.Security.Chart
{
    [DataContract]
    public class GetHistoricDataResponse : Response
    {
        [DataMember]
        public ICollection<HistoricalDataDto> Results { get; set; }
    }
}