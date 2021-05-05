using System.Collections.ObjectModel;

namespace Util.Pathfinding
{
    /// <summary>
    /// Interface used to represent traversable edges in an <see cref="IPathfindingGrid{T}"/>. Agents cannot end their
    /// movement on a GridEdge.
    /// </summary>
    public interface IGridEdge<T> : IPathfindingEdge<T>
    {
        Vec2 Position { get; }
        /// <summary>
        /// 'N' for the north edge or 'W' for the west edge. The 'S' and 'E' edges are represented by the 'N' and 'W'
        /// edges of connecting tiles.
        /// </summary>
        char Annotation { get; }
        
        /// <summary>
        /// The <see cref="IGridNode{T}"/>s an agent can reach through this <see cref="IGridEdge{T}"/>.
        /// <example>
        /// <see cref="Joins"/> will return both NodeB's, assuming the edge is EdgeA:
        /// # | NodeB | #
        /// - · EdgeA · -
        /// # | NodeB | #
        /// </example>
        /// </summary>
        ReadOnlyCollection<IGridNode<T>> Joins();

        /// <summary>
        /// The <see cref="IGridEdge{T}"/>s that share an <see cref="IGridVertex{T}"/> with this edge.
        /// <example>
        /// <see cref="Connects"/> will return both EdgeB's, assuming the edge is EdgeA:
        ///   #   |   #   |   #
        /// EdgeB . EdgeA . EdgeB
        ///   #   |   #   |   #
        /// </example>
        /// </summary>
        ReadOnlyCollection<IGridEdge<T>> Connects();

        /// <summary>
        /// The endpoint <see cref="IGridVertex{T}"/>s of the <see cref="IGridEdge{T}"/>.
        /// <example>
        /// <see cref="Endpoints"/> will return both VertB's, assuming the edge is EdgeA:
        ///   |     #    |
        /// VertB EdgeA VertB
        ///   |     #    |
        /// </example>
        /// </summary>
        ReadOnlyCollection<IGridVertex<T>> Endpoints();
        
        /// <summary>
        /// Returns <c>true</c> if this edge can be traversed with one or more of the given
        /// <see cref="Motility">motilities</see>.
        /// </summary>
        /// <param name="motility">The motility of the agent.</param>
        /// <returns><c>true</c> if the agent motility overlaps with the edge's motility.</returns>
        bool CanTraverse(Motility motility);
    }
}