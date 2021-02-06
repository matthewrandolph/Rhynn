using Engine;

namespace Util
{
    /// <summary>
    /// Interface used to represent directional edges in a pathfinding graph.
    /// </summary>
    public interface IPathfindingEdge
    {
        /// <summary>
        /// Cost associated with traversing this edge.
        /// </summary>
        float Weight { get; }

        /// <summary>
        /// An agent can traverse this edge from this node.
        /// </summary>
        IPathfindingNode Start { get; }

        /// <summary>
        /// An agent will end in this node upon traversal.
        /// </summary>
        IPathfindingNode End { get; }

        /// <summary>
        /// The Direction from the start node to the end node. Any non-traditional edges (such as portals, etc.)
        /// will return "Direction.None".
        /// </summary>
        Direction Direction { get; }

        bool IsTraversable(Traversable traversability);

        void SetTraversableFlag(Traversable traversability);

        void UnsetTraversableFlag(Traversable traversability);
    }
}