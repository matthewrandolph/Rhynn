using System;
using System.Collections.Generic;
using Util;

namespace Rhynn.Engine
{
    /// <summary>
    /// Represents one of the directions on a compass (or no direction).
    /// </summary>
    public struct Direction : IEquatable<Direction>
    {
        /// <summary>
        /// Gets the "none" direction.
        /// </summary>
        public static Direction None => new Direction(Vec2.Zero);
        
        public static Direction N => new Direction(new Vec2(0, -1));
        public static Direction NE => new Direction(new Vec2(1, -1));
        public static Direction E => new Direction(new Vec2(1, 0));
        public static Direction SE => new Direction(new Vec2(1, 1));
        public static Direction S => new Direction(new Vec2(0, 1));
        public static Direction SW => new Direction(new Vec2(-1, 1));
        public static Direction W => new Direction(new Vec2(-1, 0));
        public static Direction NW => new Direction(new Vec2(-1, -1));

        /// <summary>
        /// Adds the offset of the given Direction to the Vec2.
        /// </summary>
        /// <param name="v1">Vector to add the Direction to.</param>
        /// <param name="d2">Direction to offset the vector.</param>
        /// <returns>A new Vec2.</returns>
        public static Vec2 operator +(Vec2 v1, Direction d2)
        {
            return v1 + d2.Offset;
        }
        
        /// <summary>
        /// Adds the offset of the given Direction to the Vec2.
        /// </summary>
        /// <param name="v1">Vector to add the Direction to.</param>
        /// <param name="d2">Direction to offset the vector.</param>
        /// <returns>A new Vec2.</returns>
        public static Vec2 operator +(Direction d1, Vec2 v2)
        {
            return d1.Offset + v2;
        }

        /// <summary>
        /// Enumerates the direction in clockwise order, starting with <see cref="N"/>.
        /// </summary>
        public static IList<Direction> Clockwise => new List<Direction> { N, NE, E, SE, S, SW, W, NW };
        
        /// <summary>
        /// Enumerates the direction in counterclockwise order, starting with <see cref="N"/>.
        /// </summary>
        public static IList<Direction> CounterClockwise => new List<Direction> { N, NW, W, SW, S, SE, E, NE };

        /// <summary>
        /// Enumerates the four main cardinal compass directions
        /// </summary>
        public static IList<Direction> CardinalDirections => new List<Direction> {N, S, E, W};
        
        /// <summary>
        /// Enumerates the four intercardinal compass directions
        /// </summary>
        public static IList<Direction> IntercardinalDirections => new List<Direction> {NW, SE, NE, SW};

        /// <summary>
        /// The <see cref="Direction"/> a given <see cref="Vec2"/> is pointing.
        /// </summary>
        public static Direction Towards(Vec2 position)
        {
            Vec2 offset = Vec2.Zero;

            if (position.x < 0) offset.x = -1;
            if (position.x > 0) offset.x = 1;
            if (position.y < 0) offset.y = -1;
            if (position.y > 0) offset.y = 1;
            
            return new Direction(offset);
        }

        public static bool operator ==(Direction left, Direction right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Direction left, Direction right)
        {
            return !left.Equals(right);
        }

        public Vec2 Offset => _offset;
        
        /// <summary>
        /// Gets the <see cref="Direction"/> following this one in clockwise order. Will wrap around. If
        /// this direction is None, then returns None.
        /// </summary>
        public Direction Next 
        {
            get
            {
                if (this == N) return NE;
                else if (this == NE) return E;
                else if (this == E) return SE;
                else if (this == SE) return S;
                else if (this == S) return SW;
                else if (this == SW) return W;
                else if (this == W) return NW;
                else if (this == NW) return N;
                else return None;
            }
        }
        
        /// <summary>
        /// Gets the <see cref="Direction"/> following this one in counterclockwise order. Will
        /// wrap around. If this direction is None, then returns None.
        /// </summary>
        public Direction Previous
        {
            get
            {
                if (this == N) return NW;
                else if (this == NE) return N;
                else if (this == E) return NE;
                else if (this == SE) return E;
                else if (this == S) return SE;
                else if (this == SW) return S;
                else if (this == W) return SW;
                else if (this == NW) return W;
                else return None;
            }
        }

        public Direction RotateLeft90 => Previous.Previous;

        public Direction RotateRight90 => Next.Next;

        public Direction Rotate180 => new Direction(_offset * -1);

        public override string ToString()
        {
            if (this == N) return "N";
            if (this == NE) return "NE";
            if (this == E) return "E";
            if (this == SE) return "SE";
            if (this == S) return "S";
            if (this == SW) return "SW";
            if (this == W) return "W";
            if (this == NW) return "NW";
            if (this == None) return "None";
            
            return Offset.ToString();
        }

        private Direction(Vec2 offset)
        {
            _offset = offset;
        }

        #region IEquatable<Direction> Members

        public bool Equals(Direction other)
        {
            return Offset.Equals(other.Offset);
        }

        public override bool Equals(object obj)
        {
            return obj is Direction other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Offset.GetHashCode();
        }

        #endregion

        private Vec2 _offset;
    }
}