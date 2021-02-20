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