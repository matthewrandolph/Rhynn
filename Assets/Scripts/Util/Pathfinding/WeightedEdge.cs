using System.Collections.ObjectModel;
using Rhynn.Engine;

namespace Util.Pathfinding
{
    public class WeightedEdge<T> : IPathfindingEdge<T>
    {
        #region IPathfindingEdge Members

        public float Weight { get; }

        public IPathfindingNode<T> Start { get; }

        public IPathfindingNode<T> End { get; }

        public Direction Direction { get; }

        public WeightedEdge(IPathfindingNode<T> start, IPathfindingNode<T> end, float weight, Direction direction)
        {
            Start = start;
            End = end;
            Weight = weight;
            Direction = direction;
        }

        public ReadOnlyCollection<IPathfindingNode<T>> Joins()
        {
            throw new System.NotImplementedException();
        }

        public bool IsTraversable(Motility motility)
        {
            return _motility.Contains(motility);
        }

        public void SetMotilityFlag(Motility motility)
        {
            _motility |= motility;
        }

        public void UnsetMotilityFlag(Motility motility)
        {
            _motility -= motility;
        }

        #endregion

        private Motility _motility = Motility.None;
    }
}