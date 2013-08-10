using System.Runtime.Serialization;

namespace Blitz.Common.Customer
{
    [DataContract]
    public class GetHistoryRequest : RequestBase<GetHistoryResponse>
    {
    }
}