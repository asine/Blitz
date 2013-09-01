using System;

namespace Blitz.Common.Trading.Quote.Edit
{
    public class LookupValue : IEquatable<LookupValue>
    {
        public long Id { get; set; }

        public string Value { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((LookupValue) obj);
        }
        public bool Equals(LookupValue other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id && string.Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Id.GetHashCode()*397) ^ (Value != null ? Value.GetHashCode() : 0);
            }
        }
    }
}