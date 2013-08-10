using System;
using System.Runtime.Serialization;

using Blitz.Common.Agatha;

namespace Blitz.Common.Customer
{
    [DataContract]
    public class ReportRunnerRequest : RequestBase<ReportRunnerResponse>
    {
        [DataMember]
        public DateTime ReportDate { get; set; }
    }
}