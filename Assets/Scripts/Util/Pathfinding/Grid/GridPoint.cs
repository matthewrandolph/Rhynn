using Rhynn.Engine;

namespace Util.Pathfinding
{
    public struct GridPoint
    {
        public Vec2 Position { get; }
        public Direction Annotation { get; }

        /// <summary>
        /// Initializes a new instance of GridPoint with the given coordinates and annotation.
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="annotation">The direction from the grid's center position.</param>
        public GridPoint(int x, int y, Direction annotation) : this (new Vec2(x, y), annotation) { }

        /// <summary>
        /// Initializes a new instance of GridPoint with the given coordinates and annotation.
        /// </summary>
        /// <param name="position">X,Y coordinate</param>
        /// <param name="annotation">The direction from the grid's center position.</param>
        public GridPoint(Vec2 position, Direction annotation)
        {
            Position = position;
            Annotation = annotation;
        }
    }
}