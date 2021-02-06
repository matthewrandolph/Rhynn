using System;
using System.Collections;
using System.Collections.Generic;

namespace Util
{
    /// <summary>
    /// A 2D integer rectangle class. Similar to UnityEngine.Rect, but has some more features.
    /// </summary>
    [Serializable]
    public struct Rect : IEquatable<Rect>, IEnumerable<Vec2>
    {
        /// <summary>
        /// Gets the empty rectangle.
        /// </summary>
        public static readonly Rect Empty;

        /// <summary>
        /// Creates a new rectangle a single row in height, as wide as the given size.
        /// </summary>
        /// <param name="size">The width of the rectangle</param>
        /// <returns>The new rectangle.</returns>
        public static Rect Row(int size)
        {
            return new Rect(0, 0, size, 1);
        }

        /// <summary>
        /// Creates a new rectangle a single row in height, as wide as the given size,
        /// starting at the given top-left corner.
        /// </summary>
        /// <param name="x">The left edge of the rectangle.</param>
        /// <param name="y">The top of the rectangle.</param>
        /// <param name="size">The width of the rectangle.</param>
        /// <returns>The new rectangle.</returns>
        public static Rect Row(int x, int y, int size)
        {
            return new Rect(x, y, size, 1);
        }

        /// <summary>
        /// Creates a new rectangle a single row in height, as wide as the given size,
        /// starting at the given top-left corner.
        /// </summary>
        /// <param name="position">The top-left corner of the rectangle</param>
        /// <param name="size">The width of the rectangle.</param>
        /// <returns>The new rectangle.</returns>
        public static Rect Row(Vec2 position, int size)
        {
            return new Rect(position.x, position.y, size, 1);
        }

        /// <summary>
        /// Creates a new rectangle a single column in width, as tall as the given size.
        /// </summary>
        /// <param name="size">The height of the rectangle</param>
        /// <returns>The new rectangle</returns>
        public static Rect Column(int size)
        {
            return new Rect(0, 0, 1, size);
        }

        /// <summary>
        /// Creates a new rectangle a single column in width, as tall as the given size,
        /// starting at the given top-left corner.
        /// </summary>
        /// <param name="x">The left edge of the rectangle.</param>
        /// <param name="y">The top of the rectangle.</param>
        /// <param name="size">The height of the rectangle.</param>
        /// <returns>The new rectangle.</returns>
        public static Rect Column(int x, int y, int size)
        {
            return new Rect(x, y, 1, size);
        }

        /// <summary>
        /// Creates a new rectangle a single column in width, as tall as the given size,
        /// starting at the given top-left corner.
        /// </summary>
        /// <param name="position">The top-left corner of the rectangle</param>
        /// <param name="size">The height of the rectangle.</param>
        /// <returns>The new rectangle.</returns>
        public static Rect Column(Vec2 position, int size)
        {
            return new Rect(position.x, position.y, 1, size);
        }

        #region Operators

        public static bool operator ==(Rect r1, Rect r2)
        {
            return r1.Equals(r2);
        }

        public static bool operator !=(Rect r1, Rect r2)
        {
            return !r1.Equals(r2);
        }

        public static Rect operator +(Rect r1, Vec2 v2)
        {
            return new Rect(r1.Position + v2, r1.Size);
        }

        public static Rect operator +(Vec2 v1, Rect r2)
        {
            return new Rect(r2.Position + v1, r2.Size);
        }

        public static Rect operator -(Rect r1, Vec2 v2)
        {
            return new Rect(r1.Position - v2, r1.Size);
        }

        #endregion

        public Vec2 Position => _size;
        public Vec2 Size => _size;

        public int x => _position.x;
        public int y => _position.y;
        public int Width => _size.x;
        public int Height => _size.y;

        public int Left => x;
        public int Top => y;
        public int Right => x + Width;
        public int Bottom => y + Height;
        
        public Vec2 TopLeft => new Vec2(Left, Top);
        public Vec2 TopRight => new Vec2(Right, Top);
        public Vec2 BottomLeft => new Vec2(Left, Bottom);
        public Vec2 BottomRight => new Vec2(Right, Bottom);
        
        public Vec2 Center => new Vec2((Left + Right) / 2, (Top + Bottom) / 2);

        public int Area => _size.Area;

        public Rect(Vec2 position, Vec2 size)
        {
            _position = position;
            _size = size;
        }

        public Rect(Vec2 size) : this(Vec2.Zero, size) { }
        
        public Rect(int x, int y, int width, int height) : this(new Vec2(x, y), new Vec2(width, height)) { }

        public Rect(Vec2 position, int width, int height) : this(position, new Vec2(width, height)) { }
        
        public Rect(int width, int height) : this(new Vec2(width, height)) { }
        
        public Rect(int x, int y, Vec2 size) : this(new Vec2(x, y), size) { }

        public override string ToString()
        {
            return $"({_position})-({_size})";
        }

        public override bool Equals(object obj)
        {
            if (obj is Rect) return Equals((Rect)obj);

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return _position.GetHashCode() + _size.GetHashCode();
        }

        public Rect Inflate(int distance)
        {
            return new Rect(_position.Offset(-distance, -distance), _size.Offset(distance * 2, distance * 2));
        }

        public bool Contains(Vec2 position)
        {
            if (position.x < _position.x) return false;
            if (position.x >= _position.x + _size.x) return false;
            if (position.y < _position.y) return false;
            if (position.y >= _position.y + _size.y) return false;

            return true;
        }

        public bool Contains(Rect rect)
        {
            // all sides must be within
            if (rect.Left < Left) return false;
            if (rect.Right > Right) return false;
            if (rect.Top < Top) return false;
            if (rect.Bottom > Bottom) return false;

            return true;
        }

        public bool Overlaps(Rect rect)
        {
            // fail if they do not overlap on any axis
            if (Left > rect.Right) return false;
            if (Right < rect.Left) return false;
            if (Top > rect.Bottom) return false;
            if (Bottom < rect.Top) return false;

            return true;
        }

        public IEnumerable<Vec2> Trace()
        {
            if ((Width > 1) && (Height > 1))
            {
                // trace all four sides
                foreach (Vec2 top in Row(TopLeft, Width - 1)) yield return top;
                foreach (Vec2 right in Column(TopRight.OffsetX(-1), Height - 1)) yield return right;
            } 
            else if ((Width > 1) && (Height == 1))
            {
                // a single row
            }
            else if ((Width == 1) && (Height >= 1))
            {
                // a single column or one unit
            }
            
            // otherwise, the rect doesn't have a positive size, so there's nothing to trace
        }

        #region IEquatable<Rect> Members

        public bool Equals(Rect other)
        {
            //if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _position.Equals(other._position) && _size.Equals(other._size);
        }

        #endregion

        #region IEnumerable<Vec2> Members

        public IEnumerator<Vec2> GetEnumerator()
        {
            if (_size.x < 0) throw new ArgumentOutOfRangeException("Cannot enumerate a Rectangle with a negative width.");
            if (_size.y < 0) throw new ArgumentOutOfRangeException("Cannot enumerate a Rectangle with a negative height.");

            for (int y = _position.y; y < _position.y + _size.y; y++)
            {
                for (int x = _position.x; x < _position.x + _size.x; x++)
                {
                    yield return new Vec2(x, y);
                }
            }
        }

        #endregion
        
        #region IEnumerable Members
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        #endregion

        private Vec2 _position;
        private Vec2 _size;
    }
}