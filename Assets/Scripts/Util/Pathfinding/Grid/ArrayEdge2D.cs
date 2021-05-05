using System.Collections.Generic;
using System.Collections.ObjectModel;
using Rhynn.Engine;

namespace Util.Pathfinding
{
    public class ArrayEdge2D<T> : IGridEdge<GridTile>
    {
        public Vec2 Position { get; }
        public char Annotation { get; }
        
        public float Weight { get; }
       
        public ICollection<IPathfindingNode<GridTile>> Joins { get; }

        public bool IsTraversable(Motility motility)
        {
            // TODO: Check its own internal motility (based on if an object is on the edge)
            
            // TODO: Ask both of its joining nodes if they are traversable.
            
            throw new System.NotImplementedException();
        }

        ReadOnlyCollection<IPathfindingNode<GridTile>> IPathfindingEdge<GridTile>.Joins()
        {
            throw new System.NotImplementedException();
        }

        ReadOnlyCollection<IGridNode<GridTile>> IGridEdge<GridTile>.Joins()
        {
            throw new System.NotImplementedException();
        }

        public ReadOnlyCollection<IGridEdge<GridTile>> Connects()
        {
            throw new System.NotImplementedException();
        }

        public ReadOnlyCollection<IGridVertex<GridTile>> Endpoints()
        {
            throw new System.NotImplementedException();
        }

        public bool CanTraverse(Motility motility)
        {
            throw new System.NotImplementedException();
        }
        
        private Motility _motility;
    }
}