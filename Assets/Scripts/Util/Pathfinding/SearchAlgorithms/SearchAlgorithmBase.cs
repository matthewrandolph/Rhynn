using System;
using System.Collections.Generic;
using Rhynn.Engine;
using UnityEngine;

namespace Util.Pathfinding.SearchAlgorithms
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
        /// <param name="motility">The traversability of the agent. Any edges that don't contain one of
        ///     these flags is thrown out.</param>
        /// <returns>The path from the start node to the goal node, inclusive.</returns>
        IList<IPathfindingNode> Search(IPathfindingNode start, IPathfindingNode goal, Motility motility);
        /// <summary>
        /// This algorithm returns a path from the start to end node, following only edges that are traversable.
        /// </summary>
        /// <param name="start">The agent's starting node.</param>
        /// <param name="goal">The agent's goal node.</param>
        /// <param name="motility">The traversability of the agent. Any edges that don't contain one of
        ///     these flags is thrown out.</param>
        /// <param name="heuristic"></param>
        /// <returns></returns>
        IList<IPathfindingNode> Search(IPathfindingNode start, IPathfindingNode goal, Motility motility,
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
         * types! -- Cost = BaseMovementCost + FastestMovementSpeed / FastestMovementSpeedOnEdgeThatTheAgentCanUse
         * NOTE: A possible solution for the "cant end movement in a particular tile" problem would be to have a
         * traversability flag Traversable.CantEndMovementHere and if the (pathCost % (mod) moveSpeed == 0) then reject
         * that edge in the IsTraversable function, or alternatively in a separate check.
         */ 
        public virtual IList<IPathfindingNode> Search(IPathfindingNode start, IPathfindingNode goal, 
            Motility motility)
        {
            throw new NotImplementedException(
                "\"A heuristic is required for this algorithm. Use \"Search(IPathfindingNode start, IPathfindingNode end, Traversable traversability, Func<IPathfindingNode, IPathfindingNode, float> heuristic)\" instead.");
        }

        public virtual IList<IPathfindingNode> Search(IPathfindingNode start, IPathfindingNode goal, 
            Motility motility, Func<IPathfindingNode, IPathfindingNode, float> heuristic)
        {
            throw new NotImplementedException(
                "\"This algorithm does not use a heuristic. Use \"Search(IPathfindingNode start, IPathfindingNode end, Traversable traversability)\" instead.");
        }

        /// <summary>
        /// Checks if a given edge is traversable given the motility flags passed in. Will check both movement
        /// motility as well as filter out edges based on the most permissive n-WayNeighbor's flag set. If no
        /// neighbor flag is set, it defaults to Motility.AllNeighbors.
        /// </summary>
        /// <param name="edge">The edge attempting traversal.</param>
        /// <param name="agentMotility">The agent's ability to traverse edges.</param>
        public static bool IsTraversable(IPathfindingEdge edge, Motility agentMotility)
        {
            // Identify the graph structure for this agent and reject extraneous neighbors.
            if (agentMotility.Contains(Motility.AllNeighbors))
            {
                // Most permissive option is true, so continue to next step, skipping the other checks.
            }
            else if (agentMotility.Contains(Motility.EightWayNeighbors))
            {
                // Reject only special nodes (such as portals)
                if (!Direction.Clockwise.Contains(edge.Direction))
                {
                    return false;
                }
            }
            else if (agentMotility.Contains(Motility.FourWayNeighbors))
            {
                // Reject the intercardinal directions (diagonals) as well as any special nodes (such as portals)
                if (!Direction.CardinalDirections.Contains(edge.Direction))
                {
                    return false;
                }
            }
            else
            {
                // This is an implicit "has no neighbors flag" flag, which is just a lack of any neighbor flag at all.
                // In this case we default to Motility.AllNeighbors and continue.
            }

            // If the agent has Motility.Unconstrained, then we return, no need to compare to edge Motility.
            if (agentMotility.Contains(Motility.Unconstrained)) return true;

            // Check the agent's movement Motility compared to the edge Motility
            return edge.IsTraversable(agentMotility);
        }

        /// <summary>
        /// Takes the path in parentMap, inverts it (so it goes from start to goal) and returns it as an IList
        ///     instead of an IDictionary.
        /// </summary>
        /// <returns>The list of nodes from start to goal (inclusive).</returns>
        protected IList<IPathfindingNode> ReconstructPath(IDictionary<IPathfindingNode, IPathfindingNode> parentMap,
            IPathfindingNode start, IPathfindingNode goal)
        {
            if (parentMap.Count == 0) throw new NoPathFoundException($"Cannot find a path between nodes {start.Position} and {goal.Position}");
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

        public class NoPathFoundException : Exception
        {
            public NoPathFoundException(string message) : base (message) { }
        }
    }
}