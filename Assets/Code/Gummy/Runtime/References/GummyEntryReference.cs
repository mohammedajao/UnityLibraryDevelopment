using System;
using System.Runtime.CompilerServices;
using Gummy.Entries;

namespace Gummy.References
{
    [Serializable]
    public struct GummyEntryReference : IEquatable<GummyEntryReference>
    {
        public int id;

        public bool HasValue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => id != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(GummyEntryReference other)
        {
            return id == other.id;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            return obj is GummyEntryReference other && Equals(other);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return id;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(GummyEntryReference lhs, GummyEntryReference rhs)
        {
            return lhs.id == rhs.id;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(GummyEntryReference lhs, GummyEntryReference rhs)
        {
            return lhs.id != rhs.id;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator int(GummyEntryReference reference)
        {
            return reference.id;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator GummyEntryReference(int identifier)
        {
            return new GummyEntryReference(identifier);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator GummyEntryReference(GummyBaseEntry entry)
        {
            return new GummyEntryReference(entry.id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(GummyEntryReference lhs, GummyEntryReference rhs)
        {
            return lhs.id < rhs.id;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(GummyEntryReference lhs, GummyEntryReference rhs)
        {
            return lhs.id > rhs.id;
        }

        public GummyEntryReference(int identifier)
        {
            if(identifier == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(identifier), "ID cannot be 0.");
            }
            id = identifier;
        }
    }
}
