using System.Runtime.Serialization;

namespace Blitz.Common.Customer
{
    [DataContract]
    public class AttributeDto
    {
        [DataMember]
        public string Name { get; set; }
    }
}