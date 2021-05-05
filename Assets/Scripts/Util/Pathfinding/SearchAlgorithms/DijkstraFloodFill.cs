using System.Collections.Generic;
using Util.BlueRaja;

namespace Util.Pathfinding.SearchAlgorithms
{
    public class DijkstraFloodFill : IFloodFillAlgorithm
    {
        public IDictionary<GridNode, GridNode> Fill(GridNode start, float searchDepth, Motility motility)
        {
            Dictionary<GridNode, GridNode> parentMap = new Dictionary<GridNode, GridNode>();
            SimplePriorityQueue<GridNode> frontier = new SimplePriorityQueue<GridNode>();

            frontier.Enqueue(start, 0);

            while (frontier.Count > 0)
            {
                GridNode current = frontier.Dequeue();

                foreach (GridNode neighbor in current.Neighbors)
                {
                    IPathfindingEdge edge = current.JoiningEdge(neighbor);
                    
                    //if (!edge.IsTraversable(edge, motility)) continue;
                    
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