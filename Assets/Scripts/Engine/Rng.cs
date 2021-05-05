using System;

namespace Rhynn.Engine
{
    /// <summary>
    /// The Random Number Generator (i.e. the Random Number God)
    /// </summary>
    public static class Rng
    {
        public static void SetSeed(int seed)
        {
            _random = new Random(seed);
            Seed = seed;
        }
        
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
        /// Rolls the given number of dice with the given number of sides on each die and returns the sum. The values
        /// on each side range from 1 to the number of sides.
        /// </summary>
        /// <param name="dice">Number of dice to roll.</param>
        /// <param name="sides">Number of sides on each die.</param>
        /// <returns>The sum of the dice rolled.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws if <see cref="dice"/> or <see cref="sides"/> are
        /// less than or equal to 0.</exception>
        public static int Roll(int dice, int sides)
        {
            if (dice <= 0) throw new ArgumentOutOfRangeException(nameof(dice), "The argument \"dice\" must be greater than zero.");
            if (sides <= 0) throw new ArgumentOutOfRangeException(nameof(dice), "The argument \"sides\" must be greater than zero.");

            int total = 0;

            for (int i = 0; i < dice; i++)
            {
                total += Int(1, sides + 1);
            }

            return total;
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

        /// <summary>
        /// Returns the value rolled of a typical "check"-type roll such as an attack roll, saving throw, or skill check.
        /// </summary>
        /// <remarks>The typical "check"-type roll is currently implemented as 3d8-3, returning a result between 0 and 21.</remarks>
        /// <param name="bonus">The bonus or penalty to add to (or subtract from) the roll.</param>
        /// <returns>The value rolled</returns>
        public static int RollCheck(int bonus)
        {
            return Roll(3, 8) - 3 + bonus;
        }

        private static Random _random = new Random();
        public static int Seed { get; private set; }
    }
}