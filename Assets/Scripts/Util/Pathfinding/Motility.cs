using System;
using System.Diagnostics.Contracts;

namespace Util.Pathfinding
{
    public struct Motility : IEquatable<Motility>
    {
        /// <summary>
        /// A default value that indicates the agent cannot traverse at all.
        /// </summary>
        public static readonly Motility None = new Motility(0 << 0);
        
        public static readonly Motility Land = new Motility(1 << 0);       //   1 in decimal
        public static readonly Motility Swim = new Motility(1 << 1);       //   2 in decimal
        public static readonly Motility Climb = new Motility(1 << 2);      //   4 in decimal
        public static readonly Motility Fly = new Motility(1 << 3);        //   8 in decimal (...etc)
        public static readonly Motility Burrow = new Motility(1 << 4);       
        public static readonly Motility Incorporeal = new Motility(1 << 5); 
        
        /// <summary>
        /// Special case where the agent can ignore movement constraints (used in map generation, etc).
        /// </summary>
        public static readonly Motility Unconstrained = new Motility(1 << 6); 
        
        /// <summary>
        /// Indicates that the agent is constrained by edges that block Line of Sight (ranged attacks, etc).
        /// </summary>
        public static readonly Motility LineOfSight = new Motility(1 << 7);
        
        /// <summary>
        /// Indicates that the agent is constrained by edges that block Line of Effect (magic effects for the most part).
        /// </summary>
        public static readonly Motility LineOfEffect = new Motility(1 << 8);
        
        /// <summary>
        /// Indicates that the agent can move along any connected edge. This is the default movement.
        /// </summary>
        public static readonly Motility AllNeighbors = new Motility(1 << 9);
        
        /// <summary>
        /// Indicates that the agent can only move in the cardinal direction (useful for map generation).
        /// </summary>
        public static readonly Motility FourWayNeighbors = new Motility(1 << 10);
        
        /// <summary>
        /// Indicates that the agent can move in both the cardinal and intercardinal directions (but not in specialty
        /// directions like through portals).
        /// </summary>
        public static readonly Motility EightWayNeighbors = new Motility(1 << 11);
        
        /// <summary>
        /// A dummy entry where all bits are set.
        /// </summary>
        public static readonly Motility Everything = new Motility(~0); //  -1 in decimal

        public Motility(int bitMask)
        {
            _bitMask = bitMask;
        }

        /// <summary>
        /// Returns whether any of the flags match between the motilities.
        /// </summary>
        /// <param name="motility">The other Motility to compare to.</param>
        [Pure]
        public bool Contains(Motility motility)
        {
            return (_bitMask & motility._bitMask) != 0;
        }

        #region Operators

        public static bool operator ==(Motility m1, Motility m2)
        {
            //if (m1 is null) return m2 is null;
            return m1.Equals(m2);
        }
        
        public static bool operator !=(Motility m1, Motility m2)
        {
            //if (m1 is null) return !(m2 is null);
            return !m1.Equals(m2);
        }
        
        /// <summary>
        /// Creates a new Motility that combines all the motilities from m1 and m2
        /// </summary>
        public static Motility operator |(Motility m1, Motility m2)
        {
            return new Motility(m1._bitMask | m2._bitMask);
        }

        /// <summary>
        /// Creates a new Motility containing all of the motilities from m1 exceptfor the motilities in m2.
        /// </summary>
        public static Motility operator -(Motility m1, Motility m2)
        {
            return new Motility(m1._bitMask & ~m2._bitMask);
        }

        /// <summary>
        /// Intersection operator. Creates a new Motility that contains the motilities contained in both m1 and m2
        /// (but not if they are in only one or the other).
        /// </summary>
        public static Motility operator &(Motility m1, Motility m2)
        {
            return new Motility(m1._bitMask & m2._bitMask);
        }
        
        #endregion
        
        #region IEquatable<Motility> Members

        public bool Equals(Motility other)
        {
            //if (ReferenceEquals(null, other)) return false;
            //if (ReferenceEquals(this, other)) return true;
            return _bitMask == other._bitMask;
        }
        
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            //if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Motility) obj);
        }

        public override int GetHashCode()
        {
            return _bitMask.GetHashCode();
        }
        
        #endregion
        
        /// <remarks>
        /// A bitmask implemented as an Int32. Could be switched to a long if we need more values!
        /// </remarks>
        private readonly int _bitMask;
    }
}