using System.Collections.ObjectModel;
using Rhynn.Engine;

namespace Util.Pathfinding
{
    public interface IGridNode<T>
    {
        Vec2 Position { get; }
        
        ReadOnlyCollection<IGridNode<T>> Neighbors(IGridNode<T> tile);

        ReadOnlyCollection<IGridEdge<T>> Borders(IGridNode<T> tile);

        ReadOnlyCollection<IGridVertex<T>> Corners(IGridNode<T> tile);

        /// <summary>
        /// Returns the <see cref="IGridEdge{T}"/> that joins this tile and the passed in tile.
        /// </summary>
        /// <param name="tile">The neighboring tile.</param>
        IGridEdge<T> JoiningEdge(IGridNode<T> tile);
        
        bool CanEnter(Motility motility);
    }
}