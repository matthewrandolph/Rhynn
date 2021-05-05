using System.Collections.ObjectModel;

namespace Util.Pathfinding
{
    public class PathfindingEdge<T>
    {
        public float Weight { get; }
        public T Value { get; }
        
        public PathfindingEdge(T value, float weight, Motility motility)
        {
            Motility = motility;
            Weight = weight;
            Value = value;
        }
        
        public PathfindingEdge(T value, float weight) : this(value, weight, Motility.Unconstrained) { }
        
        public virtual bool IsTraversable(Motility motility)
        {
            throw new System.NotImplementedException();
            
            // Check internal motility
        }
        
        public virtual ReadOnlyCollection<PathfindingNode<T>> Joins()
        {
            throw new System.NotImplementedException();
            
            // return the nodes that this edge connects
        }
        
        // The motility based on the contents of the edge -- consider having it read from the motility of T
        protected Motility Motility;
    }
}