﻿using System.Collections.Generic;
using BlueRaja;

namespace Util
{
    public class DijkstraSearchAlgorithm : SearchAlgorithmBase
    {
        public override IList<IPathfindingNode> Search(IPathfindingNode start, IPathfindingNode goal,
            Traversable traversability)
        {
            Dictionary<IPathfindingNode, IPathfindingNode> parentMap = new Dictionary<IPathfindingNode, IPathfindingNode>();
            SimplePriorityQueue<IPathfindingNode> frontier = new SimplePriorityQueue<IPathfindingNode>();
            
            frontier.Enqueue(start, start.PathCost);

            while (frontier.Count > 0)
            {
                IPathfindingNode current = frontier.Dequeue();

                if (current == goal) break;

                foreach (KeyValuePair<IPathfindingNode, IPathfindingEdge> entry in current.NeighborMap)
                {
                    IPathfindingEdge edge = entry.Value;
                    IPathfindingNode neighbor = entry.Key;

                    if (!IsTraversable(edge, traversability)) continue;

                    float newCost = current.PathCost + edge.Weight;
                    float neighborCost = neighbor.PathCost;
                    bool containsKey = parentMap.ContainsKey(neighbor);

                    if (containsKey && (newCost >= neighborCost)) continue;

                    neighbor.PathCost = newCost;
                    parentMap[neighbor] = current; // UPSERT dictionary function
                    float priority = newCost;
                    frontier.Enqueue(neighbor, priority);
                }
            }

            IList<IPathfindingNode> path = ReconstructPath(parentMap, start, goal);
            return path;
        }
    }
}