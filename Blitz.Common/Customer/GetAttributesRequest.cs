using System.Runtime.Serialization;

using Naru.Agatha;

namespace Blitz.Common.Customer
{
    [DataContract]
    public class GetAttributesRequest : Request<GetAttributesResponse>
    {
    }
}