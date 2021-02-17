using System;
using System.Collections.Generic;
using Util.Pathfinding.SearchAlgorithms;

namespace Util.Pathfinding
{
    /// <summary>
    /// Designates that the class has a pathfinding graph that can traversed by a pathfinding agent.
    /// </summary>
    public interface IPathfindingGraph
    {
        IPathfindingNode GetNodeAt(Vec2 coordinates);

        IList<IPathfindingNode> Pathfinder<TSearchAlgorithm>(IPathfindingNode start, IPathfindingNode goal,
            Traversable agentTraversability) where TSearchAlgorithm : ISearchAlgorithm, new();

        IList<IPathfindingNode> Pathfinder<TSearchAlgorithm>(IPathfindingNode start, IPathfindingNode goal, 
            Traversable agentTraversability, Func<IPathfindingNode, IPathfindingNode, float> heuristic) 
            where TSearchAlgorithm : ISearchAlgorithm, new();

        IDictionary<IPathfindingNode, IPathfindingNode> Pathfinder<TFloodFillAlgorithm>(IPathfindingNode start, 
            float searchDepth, Traversable agentTraversability) where TFloodFillAlgorithm : IFloodFillAlgorithm, new();
    }
}