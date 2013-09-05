using System;
using System.Runtime.Serialization;

using Blitz.Common.Agatha;

namespace Blitz.Common.Trading.Security.Chart
{
    [DataContract]
    public class GetHistoricDataRequest : Request<GetHistoricDataResponse>
    {
        [DataMember]
        public string Ticker { get; set; }

        [DataMember]
        public DateTime From { get; set; }

        [DataMember]
        public DateTime To { get; set; }
    }
}