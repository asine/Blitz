using System;
using System.Runtime.Serialization;

namespace Blitz.Common.Customer
{
    [DataContract]
    public class ReportRunnerRequest : RequestBase<ReportRunnerResponse>
    {
        [DataMember]
        public DateTime ReportDate { get; set; }
    }
}