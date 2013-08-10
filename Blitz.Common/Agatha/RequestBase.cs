using System.Runtime.Serialization;

using Agatha.Common;

namespace Blitz.Common.Agatha
{
    [DataContract]
    public abstract class RequestBase<TResponse> : Request
        where TResponse : Response
    {
        [DataMember]
        public string Id { get; set; }
    }
}