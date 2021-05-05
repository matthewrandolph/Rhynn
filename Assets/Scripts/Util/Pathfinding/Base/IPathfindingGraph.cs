using System;
using System.Collections.Generic;
using Util.Pathfinding.SearchAlgorithms;

namespace Util.Pathfinding
{
    /// <summary>
    /// Designates that the class has a pathfinding graph that can traversed by a pathfinding agent.
    /// </summary>
    public interface IPathfindingGraph<T>
    {
        IList<GridNode> FindPath<TSearchAlgorithm>(GridNode start, GridNode goal,
            Motility agentMotility) where TSearchAlgorithm : ISearchAlgorithm, new();

        IList<GridNode> FindPath<TSearchAlgorithm>(GridNode start, GridNode goal, 
            Motility agentMotility, Func<GridNode, GridNode, float> heuristic) 
            where TSearchAlgorithm : ISearchAlgorithm, new();

        IDictionary<GridNode, GridNode> FloodFill<TFloodFillAlgorithm>(GridNode start, 
            float searchDepth, Motility agentMotility) where TFloodFillAlgorithm : IFloodFillAlgorithm, new();
    }
}