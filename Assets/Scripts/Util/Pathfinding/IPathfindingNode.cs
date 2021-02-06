using System.Collections.Generic;
using Engine;

namespace Util
{
    public interface IPathfindingNode 
    {
        float PathCost { get; set; }
        Vec2 Position { get; set; }
        
        IDictionary<IPathfindingNode, IPathfindingEdge> NeighborMap { get; }

        void AddNeighbor(IPathfindingNode neighbor, float weight, Direction direction);
        //void AddEdge(IPathfindingEdge edge);

        bool IsTraversableTo(Traversable traversable, IPathfindingNode end);
        void SetTraversableFlag(Traversable traversability);
        void UnsetTraversableFlag(Traversable traversability);
    }
}