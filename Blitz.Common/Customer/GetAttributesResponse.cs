using System.Collections.Generic;
using System.Runtime.Serialization;

using Agatha.Common;

namespace Blitz.Common.Customer
{
    [DataContract]
    public class GetAttributesResponse : Response
    {
        [DataMember]
        public ICollection<AttributeDto> Dimensions { get; set; }

        [DataMember]
        public ICollection<AttributeDto> Measures { get; set; }
    }
}