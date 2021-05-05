/*using System;
using System.Collections.ObjectModel;
using Rhynn.Engine;

namespace Util.Pathfinding
{
    public interface IPathfindingGrid<T> : IPathfindingGraph<T>
    {
        Vec2 Size { get; }
        
        #region Grid Functions
        
        #region Face Relationships

        ReadOnlyCollection<IGridNode<T>> Neighbors(IGridNode<T> tile);

        ReadOnlyCollection<IGridEdge<T>> Borders(IGridNode<T> tile);

        ReadOnlyCollection<IGridVertex<T>> Corners(IGridNode<T> tile);

        #endregion

        #region Edge Relationships

        /// <summary>
        /// The <see cref="IGridNode{T}"/>s an agent can reach through this <see cref="IGridEdge{T}"/>.
        /// <example>
        /// <see cref="Joins"/> will return both NodeB's, assuming the edge is EdgeA:
        /// # | NodeB | #
        /// - · EdgeA · -
        /// # | NodeB | #
        /// </example>
        /// </summary>
        ReadOnlyCollection<IGridNode<T>> Joins(IGridEdge<T> edge);

        /// <summary>
        /// The <see cref="IGridEdge{T}"/>s that share an <see cref="IGridVertex{T}"/> with this edge.
        /// <example>
        /// <see cref="Connects"/> will return both EdgeB's, assuming the edge is EdgeA:
        ///   #   |   #   |   #
        /// EdgeB . EdgeA . EdgeB
        ///   #   |   #   |   #
        /// </example>
        /// </summary>
        ReadOnlyCollection<IGridEdge<T>> Connects(IGridEdge<T> edge);

        /// <summary>
        /// The endpoint <see cref="IGridVertex{T}"/>s of the <see cref="IGridEdge{T}"/>.
        /// <example>
        /// <see cref="Endpoints"/> will return both VertB's, assuming the edge is EdgeA:
        ///   |     #    |
        /// VertB EdgeA VertB
        ///   |     #    |
        /// </example>
        /// </summary>
        ReadOnlyCollection<IGridVertex<T>> Endpoints(IGridEdge<T> edge);

        #endregion

        #region Vertex Relationships

        ReadOnlyCollection<IGridNode<T>> Touches(IGridVertex<T> tile);

        ReadOnlyCollection<IGridEdge<T>> Protrudes(IGridVertex<T> vertex);

        ReadOnlyCollection<IGridVertex<T>> Adjacent(IGridVertex<T> vertex);

        #endregion
        
        #endregion
    }
}*/