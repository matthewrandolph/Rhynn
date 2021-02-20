using System.Collections.Generic;
using Util.BlueRaja;

namespace Util.Pathfinding.SearchAlgorithms
{
    public class DijkstraFloodFill : IFloodFillAlgorithm
    {
        public IDictionary<IPathfindingNode, IPathfindingNode> Fill(IPathfindingNode start, float searchDepth, Motility motility)
        {
            Dictionary<IPathfindingNode, IPathfindingNode> parentMap = new Dictionary<IPathfindingNode, IPathfindingNode>();
            SimplePriorityQueue<IPathfindingNode> frontier = new SimplePriorityQueue<IPathfindingNode>();

            frontier.Enqueue(start, 0);

            while (frontier.Count > 0)
            {
                IPathfindingNode current = frontier.Dequeue();

                foreach (KeyValuePair<IPathfindingNode, IPathfindingEdge> entry in current.NeighborMap)
                {
                    IPathfindingEdge edge = entry.Value;
                    IPathfindingNode neighbor = entry.Key;
                    
                    if (!SearchAlgorithmBase.IsTraversable(edge, motility)) continue;
                    
                    float newCost = current.PathCost + edge.Weight;
                    float neighborCost = neighbor.PathCost;
                    bool containsKey = parentMap.ContainsKey(neighbor);

                    if (containsKey && (newCost >= neighborCost) || newCost > searchDepth) continue;

                    neighbor.PathCost = newCost;
                    parentMap[neighbor] = current; // UPSERT dictionary function
                    float priority = newCost;
                    frontier.Enqueue(neighbor, priority);
                }
            }

            return parentMap;
        }
    }
}