using Rhynn.Engine;

namespace Util.Pathfinding
{
    public class WeightedEdge : IPathfindingEdge
    {
        #region IPathfindingEdge Members

        public float Weight { get; }

        public IPathfindingNode Start { get; }

        public IPathfindingNode End { get; }

        public Direction Direction { get; }

        public WeightedEdge(IPathfindingNode start, IPathfindingNode end, float weight, Direction direction)
        {
            Start = start;
            End = end;
            Weight = weight;
            Direction = direction;
        }
        
        public bool IsTraversable(Traversable traversable)
        {
            return (_traversable & traversable) != 0;
        }

        public void SetTraversableFlag(Traversable traversable)
        {
            _traversable |= traversable;
        }

        public void UnsetTraversableFlag(Traversable traversable)
        {
            _traversable &= ~traversable;
        }

        #endregion

        private Traversable _traversable;
    }
}