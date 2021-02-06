using System;

namespace Engine
{
    /// <summary>
    /// The Random Number Generator (i.e. the Random Number God)
    /// </summary>
    public static class Rng
    {
        /// <summary>
        /// Gets a random int between 0 and max (half-inclusive).
        /// </summary>
        public static int Int(int max)
        {
            return _random.Next(max);
        }

        /// <summary>
        /// Gets a random int between min and max (half-inclusive).
        /// </summary>
        public static int Int(int min, int max)
        {
            return Int(max - min) + min;
        }

        /// <summary>
        /// Gets a random int between 0 and max (inclusive).
        /// </summary>
        public static int IntInclusive(int max)
        {
            return _random.Next(max + 1);
        }

        /// <summary>
        /// Gets a random int between min and max (inclusive).
        /// </summary>
        public static int IntInclusive(int min, int max)
        {
            return IntInclusive(max - min) + min;
        }

        /// <summary>
        /// Gets a random float between 0 and max.
        /// </summary>
        public static float Float(float max)
        {
            if (max < 0.0f) throw new ArgumentOutOfRangeException("The max must be zero or greater.");

            return (float) _random.NextDouble() * max;
        }
        
        /// <summary>
        /// Gets a random float between min and max.
        /// </summary>
        public static float Float(float min, float max)
        {
            if (max < min) throw new ArgumentOutOfRangeException("The max must be min or greater.");

            return Float(max - min) + min;
        }

        /// <summary>
        /// Gets a random number centered around "center" with range "range" (inclusive)
        /// using a triangular distribution. For example getTriInt(8, 4) will return values
        /// between 4 and 12 (inclusive) with greater distribution towards 8.
        /// </summary>
        /// <remarks>
        /// This means output values will range from (center - range) to (center + range)
        /// inclusive, with most values near the center, but not with a normal distribution.
        /// Think of it as a poor man's bell curve.
        ///
        /// The algorithm works essentially by choosing a random point inside the triangle,
        /// and then calculating the x coordinate of that point. It works like this:
        ///
        /// Consider Center 4, Range 3:
        /// 
        ///         *
        ///       * | *
        ///     * | | | *
        ///   * | | | | | *
        /// --+-----+-----+--
        /// 0 1 2 3 4 5 6 7 8
        ///  -r     c     r
        /// 
        /// Now flip the left half of the triangle (from 1 to 3) vertically and move it
        /// over to the right so that we have a square.
        /// 
        ///     +-------+
        ///     |       V
        ///     |
        ///     |   R L L L
        ///     | . R R L L
        ///     . . R R R L
        ///   . . . R R R R
        /// --+-----+-----+--
        /// 0 1 2 3 4 5 6 7 8
        /// 
        /// Choose a point in that square. Figure out which half of the triangle the
        /// point is in, and then remap the point back out to the original triangle.
        /// The result is the x coordinate of the point in the original triangle.
        /// </remarks>
        public static int TriangleInt(int center, int range)
        {
            if (range < 0) throw new ArgumentOutOfRangeException($"The argument \"{range}\" must be zero or greater.");
            
            // Pick a point in the square
            int x = IntInclusive(range);
            int y = IntInclusive(range);
            
            // Figure out which triangle we are in
            if (x <= y)
            {
                // larger triangle
                return center + x;
            }
            else
            {
                // smaller triangle
                return center - range - 1 + x;
            }
        }
        
        private static Random _random = new Random();
    }
}