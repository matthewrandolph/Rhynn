using System.Collections.Generic;
using Rhynn.Engine;

namespace Util.Pathfinding
{
    /// <summary>
    /// A node within an <see cref="IPathfindingGraph"/> that a pathfinding agent can traverse to.
    /// </summary>
    /// <typeparam name="T">The object the node represents.</typeparam>
    public interface IPathfindingNode<T>
    {
        //TileType Type { get; set; }
        
        /// <summary>
        /// The running total cost to reach this node of the path most recently calculated by a pathfinding algorithm.
        /// This is reset whenever a new path is calculated.
        /// </summary>
        float PathCost { get; set; }
        //Vec2 Position { get; }
        
        IReadOnlyCollection<IPathfindingNode<T>> Neighbors { get; }

        //IReadOnlyDictionary<IPathfindingNode<T>, IPathfindingEdge<T>> NeighborMap { get; }
        
        T Value { get; set; }

        // Node checks its internal motility and then asks its edge
        bool CanEnter(Motility motility, IPathfindingNode<T> neighbor);

        /// <summary>
        /// Returns the <see cref="IPathfindingEdge{T}"/> that joins this node and the passed in node.
        /// </summary>
        /// <param name="node">The neighboring node.</param>
        IPathfindingEdge<T> JoiningEdge(IPathfindingNode<T> node);


        //void AddNeighbor(IPathfindingNode neighbor, float weight, Direction direction);
        //void AddEdge(IPathfindingEdge edge);

        //bool IsTraversableTo(IPathfindingNode end, Motility motility);
        //void SetIncomingTraversableFlag(Motility motility);
        //void UnsetIncomingTraversableFlag(Motility motility);
        //void SetOutgoingTraversableFlag(Motility motility);
        //void UnsetOutgoingTraversableFlag(Motility motility);
    }
}