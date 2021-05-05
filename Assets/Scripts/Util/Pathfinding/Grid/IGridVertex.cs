namespace Util.Pathfinding
{
    public interface IGridVertex<T> : IPathfindingEdge<T>
    {
        Vec2 Position { get; }
        
        IGridNode<T> Touches { get; }
        IGridEdge<T> Protrudes { get; }
        IGridVertex<T> Adjacent { get; }
        
        // GridVertex checks its internal motility list and then asks its four protrudes edges and its four touches faces
        bool CanTraverse(Motility motility);
    }
}