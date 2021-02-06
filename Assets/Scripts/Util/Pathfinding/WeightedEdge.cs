using Engine;

namespace Util
{
    public class WeightedEdge : IPathfindingEdge
    {
        #region IPathfindingEdge Members

        public float Weight => _weight;
        public IPathfindingNode Start => _start;
        public IPathfindingNode End => _end;
        public Direction Direction => _direction;
        
        public WeightedEdge(IPathfindingNode start, IPathfindingNode end, float weight, Direction direction)
        {
            _start = start;
            _end = end;
            _weight = weight;
            _direction = direction;
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
            _traversable &= traversable;
        }

        #endregion

        private float _weight;
        private IPathfindingNode _start;
        private IPathfindingNode _end;
        private Direction _direction;
        private Traversable _traversable;
    }
}