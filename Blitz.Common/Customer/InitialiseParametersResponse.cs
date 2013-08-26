using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Agatha.Common;

namespace Blitz.Common.Customer
{
    [DataContract]
    public class InitialiseParametersResponse : Response
    {
        public InitialiseParametersResponse()
        {
        }

        [DataMember]
        public ICollection<DateTime> AvailableDates { get; set; } 
    }
}