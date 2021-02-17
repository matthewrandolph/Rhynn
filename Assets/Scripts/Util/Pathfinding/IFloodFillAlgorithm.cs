using System.Collections.Generic;

namespace Util.Pathfinding
{
    /// <summary>
    /// Interface to represent graph flood-filling style algorithms used by a pathfinding agent.
    /// </summary>
    public interface IFloodFillAlgorithm
    {
        /// <summary>
        /// Given a starting node and a search depth, returns all nodes that are reachable from that location.
        /// </summary>
        /// <param name="start">The start node.</param>
        /// <param name="searchDepth">The cost of the path at which to stop searching along that path.</param>
        /// <param name="traversability">The traversability of the agent. Any edges that don't contain one of
        ///     these flags is thrown out.</param>
        /// <returns>All nodes where the traversal cost is less than or equal to searchDepth.</returns>
        IDictionary<IPathfindingNode, IPathfindingNode> Fill(IPathfindingNode start, float searchDepth,
            Traversable traversability);
    }
}