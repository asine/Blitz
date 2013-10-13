using System;
using System.Runtime.Serialization;

using Naru.Agatha;

namespace Blitz.Common.Customer
{
    [DataContract]
    public class ReportRunnerRequest : Request<ReportRunnerResponse>
    {
        [DataMember]
        public DateTime ReportDate { get; set; }
    }
}