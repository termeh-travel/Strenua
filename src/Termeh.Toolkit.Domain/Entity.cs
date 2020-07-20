using System;

namespace Termeh.Toolkit.Domain
{
    public interface IEntity
    {
        
    }

    public interface IEntity<out TIdentifier> : IEntity where TIdentifier : notnull
    {
        TIdentifier Id { get; }
        DateTime CreatedAt { get; }
        DateTime UpdatedAt { get; }
        public event EventHandler IdentifierSetEvent;
    }

    public abstract class Entity<TIdentifier> : IEntity<TIdentifier>, IEquatable<Entity<TIdentifier>> where TIdentifier : notnull
    {
        private TIdentifier _id = default;
        public TIdentifier Id
        {
            get => _id;
            protected set
            {
                _id = value;
                IdentifierSetEvent?.Invoke(this, null);
            }
        }

        public DateTime CreatedAt { get; protected set; }

        public DateTime UpdatedAt { get; protected set; }

        public event EventHandler IdentifierSetEvent;

        public static bool operator !=(Entity<TIdentifier> a, Entity<TIdentifier> b)
        {
            return !(a == b);
        }

        public static bool operator ==(Entity<TIdentifier> a, Entity<TIdentifier> b)
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

        public override bool Equals(object? obj)
        {
            return !(obj is null) && Equals(obj as Entity<TIdentifier>);
        }

        public bool Equals(Entity<TIdentifier>? other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (GetType() != other.GetType())
            {
                return false;
            }

            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (GetType().GetHashCode() * 97) ^ Id.GetHashCode();
            }
        }
    }
}