using System;
using System.Collections.Generic;
using Util.BlueRaja;

namespace Util.Pathfinding.SearchAlgorithms
{
    public class AStarSearchAlgorithm : SearchAlgorithmBase
    {
        public override IList<GridNode> Search(GridNode start, GridNode goal, Motility motility, 
            Func<GridNode, GridNode, float> heuristic)
        {
            Dictionary<GridNode, GridNode> parentMap = new Dictionary<GridNode, GridNode>();
            SimplePriorityQueue<GridNode> frontier = new SimplePriorityQueue<GridNode>();
            
            frontier.Enqueue(start, start.PathCost);

            while (frontier.Count > 0)
            {
                GridNode current = frontier.Dequeue();

                if (current == goal) break;

                foreach (GridNode neighbor in current.Neighbors)
                {
                    IPathfindingEdge edge = current.JoiningEdge(neighbor);

                    //if (!IsTraversable(edge, motility)) continue; // TODO: This line should be made irrelevant

                    float newCost = current.PathCost + edge.Weight;
                    float neighborCost = neighbor.PathCost;
                    bool containsKey = parentMap.ContainsKey(neighbor);

                    if (containsKey && (newCost >= neighborCost)) continue;

                    neighbor.PathCost = newCost;
                    parentMap[neighbor] = current; // UPSERT dictionary function
                    float priority = newCost + heuristic(goal, neighbor);
                    frontier.Enqueue(neighbor, priority);
                }
            }

            IList<GridNode> path = ReconstructPath(parentMap, start, goal);
            return path;
        }
    }
}