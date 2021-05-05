using System.Collections.Generic;

namespace Util.Pathfinding.SearchAlgorithms
{
    public class BreadthFirstSearchAlgorithm : SearchAlgorithmBase
    {
        public override IList<GridNode> Search(GridNode start, GridNode goal, Motility motility)
        {
            Dictionary<GridNode, GridNode> parentMap = new Dictionary<GridNode, GridNode>();
            Queue<GridNode> frontier = new Queue<GridNode>();
            
            frontier.Enqueue(start);

            while (frontier.Count > 0)
            {
                GridNode current = frontier.Dequeue();

                if (current == goal) break;

                foreach (GridNode neighbor in current.Neighbors)
                {
                    IPathfindingEdge edge = current.JoiningEdge(neighbor);

                    //if (!edge.IsTraversable(edge, motility)) continue;

                    if (parentMap.ContainsKey(neighbor)) continue;
                    
                    frontier.Enqueue(neighbor);
                    parentMap.Add(neighbor, current);
                }
            }

            IList<GridNode> path = ReconstructPath(parentMap, start, goal);
            return path;
        }
    }
}