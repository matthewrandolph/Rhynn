using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Util.Pathfinding
{
    public class PathfindingNode<T> // This is expected to be replaced with a new node when the Value or motility changes
    {
        public float PathCost { get; set; } // Running cost of traversing to this node of the most recent path found. if not part of the path, it will be Mathf.Infinity.
        public T Value { get; } // The thing the node represents
        
        public PathfindingNode(T value, Motility motility)
        {
            Motility = motility;
            Value = value;
        }
        
        public PathfindingNode(T value, float weight) : this(value, Motility.Unconstrained) { }

        public virtual bool CanEnter(Motility motility, PathfindingNode<T> neighbor)
        {
            throw new System.NotImplementedException();
            
            // Check internal motility
            
            // Check motility of edge connecting this and neighbor
        }

        public virtual PathfindingEdge<T> JoiningEdge(PathfindingNode<T> node)
        {
            throw new System.NotImplementedException();
            
            // return the edge connecting this node and the passed in neighbor. Throw an exception if they are not connected.
        }
        
        public virtual ReadOnlyCollection<PathfindingNode<T>> Neighbors(PathfindingNode<T> tile)
        {
            throw new System.NotImplementedException();
            
            // return all the neighbors to this node
        }

        // The motility based on the type of tile as well as the tile's contents
        protected Motility Motility;
    }
}