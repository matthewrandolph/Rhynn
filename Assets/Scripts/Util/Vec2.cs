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
        /// Gets whether the distance between the two given <see cref="Vec2">Vecs</see> is within the given distance.
        /// </summary>
        /// <param name="a">First Vec2</param>
        /// <param name="b">Second Vec2</param>
        /// <param name="distance">Maximum distance between them</param>
        /// <returns><c>true</c> if the distance between <c>a</c> and <c>b</c> is less than or equal to <c>distance</c></returns>
        public static bool IsDistanceWithin(Vec2 a, Vec2 b, int distance)
        {
            Vec2 offset = a - b;

            return offset.LengthSquared <= (distance * distance);
        }

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

        /// <summary>
        /// Gets the absolute magnitude of the Vec2 squared.
        /// </summary>
        public int LengthSquared => (x * x) + (y * y);

        /// <summary>
        /// Gets the absolute magnitude of the Vec2.
        /// </summary>
        public float Length => (float)Math.Sqrt(LengthSquared);

        /// <summary>
        /// Gets the rook length of the Vec2, which is the number of squares a rook on a chessboard would need to
        /// move from (0, 0) to reach the endpoint of the Vec. Also known as Manhattan or taxicab distance.
        /// </summary>
        public int RookLength => Math.Abs(x) + Math.Abs(y);

        /// <summary>
        /// Gets the king length of the Vec2, which is the number of squares a king on a chessboard would need to move
        /// in order to move from (0, 0) to reach the endpoint of the Vec2. Also known as Chebyshev distance.
        /// </summary>
        public int KingLength => Math.Max(Math.Abs(x), Math.Abs(y));

        public override string ToString()
        {
            return $"({x.ToString()}, {y.ToString()})";
        }

        #region IEquatable<Vec2> Members
        
        public bool Equals(Vec2 other)
        {
            //if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return x == other.x && y == other.y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
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
        
        #endregion

        /// <summary>
        /// Gets whether the given vector is within a rectangle from (0, 0) to this vector (half-inclusive).
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public bool Contains(Vec2 vec)
        {
            if (vec.x < 0) return false;
            if (vec.x >= x) return false;
            if (vec.y < 0) return false;
            if (vec.y >= y) return false;

            return true;
        }

        public bool IsAdjacentTo(Vec2 other)
        {
            // not adjacent to the exact same position
            if (this == other) return false;

            Vec2 offset = this - other;

            return (Math.Abs(offset.x) <= 1) && (Math.Abs(offset.y) <= 1);
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

        public Vec2 Each(Func<int, int> function)
        {
            if (function == null) throw new ArgumentNullException("function");
            
            return new Vec2(function(x), function(y));
        }
    }
}