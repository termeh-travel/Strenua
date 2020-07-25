using System.Collections.Generic;
using System.Linq;

namespace Strenua.Domain
{
    public abstract class ValueObject
    {
        public static bool operator !=(ValueObject a, ValueObject b)
        {
            return !(a == b);
        }

        public static bool operator ==(ValueObject a, ValueObject b)
        {
            if (a is null && b is null)
            {
                return true;
            }

            if (a is null || b is null)
            {
                return false;
            }

            return a.Equals(b);
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (GetType() != obj.GetType())
            {
                return false;
            }

            return GetEquals().SequenceEqual(((ValueObject) obj).GetEquals());
        }

        public override int GetHashCode()
        {
            return GetEquals().Aggregate(0, (a, b) => (a * 97) + b.GetHashCode());
        }

        protected abstract IEnumerable<object> GetEquals();
    }
}