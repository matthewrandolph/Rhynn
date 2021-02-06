using System;
using System.Collections.Generic;
using Engine;

namespace Util
{
    /// <summary>
    /// Interface to represent graph searching algorithms used by a pathfinding agent.
    /// </summary>
    public interface ISearchAlgorithm
    {
        /// <summary>
        /// This algorithm returns a path from the start to end node, following only edges that are traversable.
        /// </summary>
        /// <param name="start">The agent's starting node.</param>
        /// <param name="goal">The agent's goal node.</param>
        /// <param name="traversability">The traversability of the agent. Any edges that don't contain one of
        ///     these flags is thrown out.</param>
        /// <returns>The path from the start node to the goal node, inclusive.</returns>
        IList<IPathfindingNode> Search(IPathfindingNode start, IPathfindingNode goal, Traversable traversability);
        /// <summary>
        /// This algorithm returns a path from the start to end node, following only edges that are traversable.
        /// </summary>
        /// <param name="start">The agent's starting node.</param>
        /// <param name="goal">The agent's goal node.</param>
        /// <param name="traversability">The traversability of the agent. Any edges that don't contain one of
        ///     these flags is thrown out.</param>
        /// <param name="heuristic"></param>
        /// <returns></returns>
        IList<IPathfindingNode> Search(IPathfindingNode start, IPathfindingNode goal, Traversable traversability,
            Func<IPathfindingNode, IPathfindingNode, float> heuristic);
    }

    /// <summary>
    /// A base class for other SearchAlgorithms to inherit from.
    /// </summary>
    public abstract class SearchAlgorithmBase : ISearchAlgorithm
    {
        /* TODO: When multiple movement types are added, the pathfinding algorithms will have to account for the fact
         * that there are "preferences" for movement - i.e. if you have a land speed of 6 and a climb speed
         * of 3 the costs of climbing are twice as high as the cost of walking. This will probably need to have the
         * traversabilities given in a Dictionary format with the traversability as the key and the cost as the value.
         * This would also add another bonus: implicitly giving whether or not an agent has a particular movement
         * type by way of if they have a movement speed listed, instead of needing to explicitly define their movement
         * types!
         * NOTE: A possible solution for the "cant end movement in a particular tile" problem would be to have a
         * traversability flag Traversable.CantEndMovementHere and if the (pathCost % (mod) moveSpeed == 0) then reject
         * that edge in the IsTraversable function, or alternatively in a seperate check.
         */ 
        public virtual IList<IPathfindingNode> Search(IPathfindingNode start, IPathfindingNode goal, 
            Traversable traversability)
        {
            throw new NotImplementedException(
                "\"A heuristic is required for this algorithm. Use \"Search(IPathfindingNode start, IPathfindingNode end, Traversable traversability, Func<IPathfindingNode, IPathfindingNode, float> heuristic)\" instead.");
        }

        public virtual IList<IPathfindingNode> Search(IPathfindingNode start, IPathfindingNode goal, 
            Traversable traversability, Func<IPathfindingNode, IPathfindingNode, float> heuristic)
        {
            throw new NotImplementedException(
                "\"This algorithm does not use a heuristic. Use \"Search(IPathfindingNode start, IPathfindingNode end, Traversable traversability)\" instead.");
        }

        /// <summary>
        /// Checks if a given edge is traversable given the traversability flags passed in. Will check both movement
        /// traversability as well as filter out edges based on the most permissive n-WayNeighbor's flag set.
        /// </summary>
        /// <param name="edge">The edge attempting traversal.</param>
        /// <param name="agentTraversability">The agent's ability to traverse edges.</param>
        protected bool IsTraversable(IPathfindingEdge edge, Traversable agentTraversability)
        {
            // Identify the graph structure for this agent and reject extraneous neighbors.
            if (agentTraversability.HasFlag(Traversable.AllNeighbors))
            {
                // Most permissive option is true, so continue to next step, skipping the other checks.
            }
            else if (agentTraversability.HasFlag(Traversable.EightWayNeighbors))
            {
                // Reject only special nodes (such as portals)
                if (!Direction.Clockwise.Contains(edge.Direction))
                {
                    return false;
                }
            }
            else if (edge.IsTraversable(Traversable.FourWayNeighbors))
            {
                // Reject the diagonals and any special nodes (such as portals)
                if (!Direction.CardinalDirections.Contains(edge.Direction))
                {
                    return false;
                }
            }

            // Check the agent's movement traversability compared to the edge traversability
            return edge.IsTraversable(agentTraversability);
        }

        /// <summary>
        /// Takes the path in parentMap, inverts it (so it goes from start to goal) and returns it as an IList
        ///     instead of an IDictionary.
        /// </summary>
        /// <returns>The list of nodes from start to goal (inclusive).</returns>
        protected IList<IPathfindingNode> ReconstructPath(IDictionary<IPathfindingNode, IPathfindingNode> parentMap,
            IPathfindingNode start, IPathfindingNode goal)
        {
            List<IPathfindingNode> path = new List<IPathfindingNode>();
            IPathfindingNode current = goal;

            while (current != start)
            {
                path.Add(current);
                current = parentMap[current];
            }
            
            path.Add(start);
            path.Reverse();
            return path;
        }
    }
}