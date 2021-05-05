using System.Collections.Generic;
using System.Collections.ObjectModel;
using Rhynn.Engine;

namespace Util.Pathfinding
{
    /// <summary>
    /// Interface used to represent traversable edges in an <see cref="IPathfindingGraph{T}"/>. Agents cannot end their
    /// movement on an Edge.
    /// </summary>
    public interface IPathfindingEdge<T>
    {
        /// <summary>
        /// Cost associated with traversing this edge.
        /// </summary>
        float Weight { get; }
        
        /// <summary>
        /// The <see cref="IPathfindingNode{T}"/>s an agent can reach through this <see cref="IPathfindingEdge{T}"/>.
        /// <example>
        /// <see cref="Joins"/> will return both NodeB's, assuming the edge is EdgeA:
        /// # | NodeB | #
        /// - · EdgeA · -
        /// # | NodeB | #
        /// </example>
        /// </summary>
        ReadOnlyCollection<IPathfindingNode<T>> Joins();

        /// <summary>
        /// The Direction from the start node to the end node. Any non-traditional edges (such as portals, etc.)
        /// will return "Direction.None".
        /// </summary>
        //Direction Direction { get; }

        bool IsTraversable(Motility motility);

        //void SetMotilityFlag(Motility motility);

        //void UnsetMotilityFlag(Motility motility);
    }

    /// <summary>
    /// Interface used to represent traversable edges in a pathfinding graph. Agents cannot end their movement on an Edge.
    /// </summary>
    public interface IPathfindingEdge
    {
        /// <summary>
        /// Cost associated with traversing this edge.
        /// </summary>
        float Weight { get; }
        
        //IPathfindingEdge JoiningEdge(GridTile node);

        bool IsTraversable(Motility motility);
    }
}