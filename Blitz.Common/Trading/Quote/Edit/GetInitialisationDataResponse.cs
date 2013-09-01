using System.Collections.Generic;
using System.Runtime.Serialization;

using Agatha.Common;

namespace Blitz.Common.Trading.Quote.Edit
{
    [DataContract]
    public class GetInitialisationDataResponse : Response
    {
        [DataMember]
        public ICollection<LookupValue> Instruments { get; set; }
    }
}