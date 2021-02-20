using System.Collections.Generic;
using Rhynn.Engine;

namespace Util.Pathfinding
{
    public interface IPathfindingNode 
    {
        TileType Type { get; set; }
        float PathCost { get; set; }
        Vec2 Position { get; }
        
        IDictionary<IPathfindingNode, IPathfindingEdge> NeighborMap { get; }

        void AddNeighbor(IPathfindingNode neighbor, float weight, Direction direction);
        //void AddEdge(IPathfindingEdge edge);

        bool IsTraversableTo(IPathfindingNode end, Motility motility);
        void SetIncomingTraversableFlag(Motility motility);
        void UnsetIncomingTraversableFlag(Motility motility);
        void SetOutgoingTraversableFlag(Motility motility);
        void UnsetOutgoingTraversableFlag(Motility motility);
    }
}