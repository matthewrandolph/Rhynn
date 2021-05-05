/*using System;
using System.Collections.Generic;
using Util.Pathfinding.SearchAlgorithms;

namespace Util.Pathfinding
{
    public class PathfindingGraph<T>
    {
        public IList<PathfindingNode<T>> FindPath<TSearchAlgorithm>(PathfindingNode<T> start, PathfindingNode<T> goal,
            Motility agentMotility) where TSearchAlgorithm : ISearchAlgorithm<T>, new()
        {
            throw new NotImplementedException();
        }

        public IList<PathfindingNode<T>> FindPath<TSearchAlgorithm>(PathfindingNode<T> start, PathfindingNode<T> goal, 
            Motility agentMotility, Func<PathfindingNode<T>, PathfindingNode<T>, float> heuristic) 
            where TSearchAlgorithm : ISearchAlgorithm<T>, new()
        {
            throw new NotImplementedException();
        }

        public IDictionary<PathfindingNode<T>, PathfindingNode<T>> FloodFill<TFloodFillAlgorithm>(
            PathfindingNode<T> start, float searchDepth, Motility agentMotility) 
            where TFloodFillAlgorithm : IFloodFillAlgorithm<T>, new()
        {
            throw new NotImplementedException();
        }
    }
}*/