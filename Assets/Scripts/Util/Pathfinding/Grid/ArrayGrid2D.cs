using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Util.Pathfinding.SearchAlgorithms;

namespace Util.Pathfinding
{
    /// <summary>
    /// A fixed-size two-dimensional grid class designed to be used as an <see cref="IPathfindingGrid{T}"/>.
    /// </summary>
    [Serializable]
    public class ArrayGrid2D<T> : IPathfindingGraph<T>
    {
        public T Value { get; set; }

        /// <summary>
        /// Initializes a new instance of ArrayGrid2D with the given dimensions.
        /// </summary>
        public ArrayGrid2D(int width, int height)
        {
            if (width < 0) throw new ArgumentOutOfRangeException(nameof(width),"Width must be greater than zero.");
            if (height < 0) throw new ArgumentOutOfRangeException(nameof(height),"Height must be greater than zero.");

            _width = width;
            _tiles = new IPathfindingNode<T>[width * height];
        }

        /// <summary>
        /// Initializes a new instance of ArrayGrid2D with the given size.
        /// </summary>
        public ArrayGrid2D(Vec2 size) : this(size.x, size.y)
        {
        }

        /// <summary>
        /// Gets the size of the grid.
        /// </summary>
        public Vec2 Size => new Vec2(Width, Height);

        /// <summary>
        /// Gets the width of the grid.
        /// </summary>
        public int Width => _width;

        /// <summary>
        /// Gets the height of the grid.
        /// </summary>
        public int Height => _tiles.Length / _width;

        /// <summary>
        /// Gets and sets the array element at the given position.
        /// </summary>
        /// <param name="position">The position of the element. Must be within bounds.</param>
        /// <exception cref="IndexOutOfBoundsException">if the position is out of bounds.</exception>
        public T this[Vec2 position]
        {
            get => this[position.x, position.y];
            set => this[position.x, position.y] = value;
        }

        public T this[int x, int y]
        {
            get => _tiles[GetIndex(x, y)].Value;
            set => _tiles[GetIndex(x, y)].Value = value;
        }

        /// <summary>
        /// Fills all of the elements in the array with values returned by the given callback
        /// </summary>
        /// <param name="callback">The function to call for each element in the array.</param>
        public void Fill(Func<Vec2, T> callback)
        {
            foreach (Vec2 position in new Rect(Size))
            {
                //this[position] = callback(position);
            }
        }

        private int GetIndex(int x, int y)
        {
            return (y * _width) + x;
        }

        #region IPathfindingGrid Members
        
        #region Face Relationships

        public ReadOnlyCollection<IGridNode<T>> Neighbors(IGridNode<T> tile)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IGridEdge<T>> Borders(IGridNode<T> tile)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IGridVertex<T>> Corners(IGridNode<T> tile)
        {
            throw new NotImplementedException();
        }
        
        #endregion
        
        #region Edge Relationships

        /// <summary>
        /// The <see cref="IPathfindingNode{T}"/>s an agent can reach through this edge.
        /// <example>
        /// <see cref="Joins"/> will return both NodeB's, assuming the edge is EdgeA:
        /// # | NodeB | #
        /// - · EdgeA · -
        /// # | NodeB | #
        /// </example>
        /// </summary>
        public ReadOnlyCollection<IGridNode<T>> Joins(IGridEdge<T> edge)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The <see cref="IPathfindingEdge{T}"/>s that share an <see cref="EndPoints">endpoint</see> with this edge.
        /// <example>
        /// <see cref="Connects"/> will return both EdgeB's, assuming the edge is EdgeA.
        ///   #   |   #   |   #
        /// EdgeB . EdgeA . EdgeB
        ///   #   |   #   |   #
        /// </example>
        /// </summary>
        public ReadOnlyCollection<IGridEdge<T>> Connects(IGridEdge<T> edge)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The endpoints of the Edge.
        /// <example>
        /// Given a square grid, <see cref="Endpoints"/> will return both VertB's, assuming the edge is EdgeA.
        ///   |     #    |
        /// VertB EdgeA VertB
        ///   |     #    |
        /// </summary>
        public ReadOnlyCollection<IGridVertex<T>> Endpoints(IGridEdge<T> edge)
        {
            throw new NotImplementedException();
        }
        
        #endregion

        #region Vertex Relationships
        
        public ReadOnlyCollection<IGridNode<T>> Touches(IGridVertex<T> tile)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IGridEdge<T>> Protrudes(IGridVertex<T> vertex)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IGridVertex<T>> Adjacent(IGridVertex<T> vertex)
        {
            throw new NotImplementedException();
        }
        
        #endregion

        #endregion
        
        #region IPathfindingGraph Members
        
        public IList<GridNode> FindPath<TSearchAlgorithm>(GridNode start, 
            GridNode goal, Motility agentMotility) where TSearchAlgorithm : ISearchAlgorithm, new()
        {
            throw new NotImplementedException();
            
            var searchAlgorithm = new TSearchAlgorithm();
            InitializeCosts(start);
            return searchAlgorithm.Search(start, goal, agentMotility);
        }

        public IList<GridNode> FindPath<TSearchAlgorithm>(GridNode start, 
            GridNode goal, Motility agentMotility, Func<GridNode, 
                GridNode, float> heuristic) where TSearchAlgorithm : ISearchAlgorithm, new()
        {
            throw new NotImplementedException();
            
            var searchAlgorithm = new TSearchAlgorithm();
            InitializeCosts(start);
            return searchAlgorithm.Search(start, goal, agentMotility, heuristic);
        }

        public IDictionary<GridNode, GridNode> FloodFill<TFloodFillAlgorithm>(
            GridNode start, float searchDepth, Motility agentMotility) 
            where TFloodFillAlgorithm : IFloodFillAlgorithm, new()
        {
            throw new NotImplementedException();
            
            var fillAlgorithm = new TFloodFillAlgorithm();
            InitializeCosts(start);
            return fillAlgorithm.Fill(start, searchDepth, agentMotility);
        }

        private void InitializeCosts(GridNode start)
        {
            foreach (GridNode tile in _tiles)
            {
                tile.PathCost = Mathf.Infinity;
            }

            start.PathCost = 0;
        }

        #endregion

        private readonly int _width;
        private readonly IPathfindingNode<T>[] _tiles;
        private readonly IGridEdge<T>[] _edges;
        private readonly IGridVertex<T>[] _vertices;
    }
}