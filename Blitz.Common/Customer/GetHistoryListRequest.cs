using System.Runtime.Serialization;

using Blitz.Common.Agatha;

namespace Blitz.Common.Customer
{
    [DataContract]
    public class GetHistoryListRequest : Request<GetHistoryListResponse>
    {
    }
}