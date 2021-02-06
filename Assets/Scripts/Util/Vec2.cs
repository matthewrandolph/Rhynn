using System;

namespace Util
{
    /// <summary>
    /// A 2D integer vector class. Similar to UnityEngine.Vector2Int but with
    /// some other features (and shorter to write to boot).
    /// </summary>
    [Serializable]
    public struct Vec2 : IEquatable<Vec2>
    {
        /// <summary>
        /// Gets the zero vector.
        /// </summary>
        public static readonly Vec2 Zero = new Vec2(0, 0);

        public static readonly Vec2 One = new Vec2(1, 1);

        #region Operators

        public static bool operator ==(Vec2 v1, Vec2 v2)
        {
            return v1.Equals(v2);
        }
        
        public static bool operator !=(Vec2 v1, Vec2 v2)
        {
            return !v1.Equals(v2);
        }

        public static Vec2 operator +(Vec2 v1, Vec2 v2)
        {
            return new Vec2(v1.x + v2.x, v1.y + v2.y);
        }
        
        public static Vec2 operator +(Vec2 v1, int i2)
        {
            return new Vec2(v1.x + i2, v1.y + i2);
        }
        
        public static Vec2 operator +(int i1, Vec2 v2)
        {
            return new Vec2(i1 + v2.x, i1 + v2.y);
        }
        
        public static Vec2 operator -(Vec2 v1, Vec2 v2)
        {
            return new Vec2(v1.x - v2.x, v1.y - v2.y);
        }
        
        public static Vec2 operator -(Vec2 v1, int i2)
        {
            return new Vec2(v1.x - i2, v1.y - i2);
        }
        
        public static Vec2 operator -(int i1, Vec2 v2)
        {
            return new Vec2(i1 - v2.x, i1 - v2.y);
        }

        public static Vec2 operator *(Vec2 v1, int i2)
        {
            return new Vec2(v1.x * i2, v1.y * i2);
        } 
        
        public static Vec2 operator *(int i1, Vec2 v2)
        {
            return new Vec2(i1 * v2.x, i1 * v2.y);
        }
        
        public static Vec2 operator /(Vec2 v1, int i2)
        {
            return new Vec2(v1.x / i2, v1.y / i2);
        } 

        #endregion

        /// <summary>
        /// Initializes a new instance of Vec2 with the given coordinates.
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        public Vec2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int x;
        public int y;

        /// <summary>
        /// Gets the area of a rectangle with opposite corners at (0, 0) and this Vec.
        /// </summary>
        public int Area => x * y;

        #region IEquatable<Vec2> Members
        
        public bool Equals(Vec2 other)
        {
            //if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return x == other.x && y == other.y;
        }

        public override bool Equals(object obj)
        {
            //if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Vec2) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (x * 397) ^ y;
            }
        }

        public Vec2 Offset(int x, int y)
        {
            return new Vec2(this.x + x, this.y + y);
        }

        /// <summary>
        /// Returns a new Vec2 whose coordinates are the coordinates of this Vec2 with the
        /// given value added to the X coordinate. This Vec2 is not modified.
        /// </summary>
        /// <param name="offset">Distance to offset the X coordinate</param>
        /// <returns>A new Vec2 offset by the given X coordinate.</returns>
        public Vec2 OffsetX(int offset)
        {
            return new Vec2(x + offset, y);
        }

        /// <summary>
        /// Returns a new Vec2 whose coordinates are the coordinates of this Vec2 with the
        /// given value added to the Y coordinate. This Vec2 is not modified.
        /// </summary>
        /// <param name="offset">Distance to offset the Y coordinate</param>
        /// <returns>A new Vec2 offset by the given Y coordinate.</returns>
        public Vec2 OffsetY(int offset)
        {
            return new Vec2(x, y + offset);
        }
        
        #endregion


    }
}