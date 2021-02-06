using System;
using System.Collections.Generic;
using Engine;
using UnityEngine;

namespace Util
{
    /// <summary>
    /// A fixed-size two-dimensional grid class that can be used as a pathfinding graph.
    /// </summary>
    /// <typeparam name="T">The grid element type.</typeparam>
    [Serializable]
    public class Grid2D<T> : IPathfindingGraph where T : class, IPathfindingNode
    {
        /// <summary>
        /// Initializes a new instance of Grid2D with the given dimensions.
        /// </summary>
        public Grid2D(int width, int height)
        {
            if (width < 0) throw new ArgumentOutOfRangeException("Width must be greater than zero.");
            if (height < 0) throw new ArgumentOutOfRangeException("Height must be greater than zero.");

            _width = width;
            _values = new T[width * height];
        }
        
        /// <summary>
        /// Initializes a new instance of Grid2D with the given size.
        /// </summary>
        public Grid2D(Vec2 size) : this(size.x, size.y) { }
        
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
        public int Height => _values.Length / _width;

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
            get => _values[GetIndex(x, y)];
            set => _values[GetIndex(x, y)] = value;
        }

        /// <summary>
        /// Fills all of the elements in the array with values returned by the given callback
        /// </summary>
        /// <param name="callback"/>The functon to call for each element in the array</param>
        public void Fill(Func<Vec2, T> callback)
        {
            foreach (Vec2 position in new Rect(Size))
            {
                this[position] = callback(position);
            }
        }

        private int GetIndex(int x, int y)
        {
            return (y * _width) + x;
        }
        
        #region IPathfindingGraph Members
        
        public IPathfindingNode GetNodeAt(Vec2 coordinates)
        {
            return this[coordinates.x, coordinates.y];
        }

        public IList<IPathfindingNode> Pathfinder<TSearchAlgorithm>(IPathfindingNode start, IPathfindingNode goal, 
            Traversable agentTraversability) where TSearchAlgorithm : ISearchAlgorithm, new()
        {
            var searchAlgorithm = new TSearchAlgorithm();
            InitializeCosts(start);
            return searchAlgorithm.Search(start, goal, agentTraversability);
        }

        public IList<IPathfindingNode> Pathfinder<TSearchAlgorithm>(IPathfindingNode start, IPathfindingNode goal, 
            Traversable agentTraversability, Func<IPathfindingNode, IPathfindingNode, float> heuristic) 
            where TSearchAlgorithm : ISearchAlgorithm, new()
        {
            var searchAlgorithm = new TSearchAlgorithm();
            InitializeCosts(start);
            return searchAlgorithm.Search(start, goal, agentTraversability, heuristic);
        }

        public IDictionary<IPathfindingNode, IPathfindingNode> Pathfinder<TFloodFillAlgorithm>(IPathfindingNode start, 
            float searchDepth, Traversable agentTraversability) where TFloodFillAlgorithm : IFloodFillAlgorithm, new()
        {
            var fillAlgorithm = new TFloodFillAlgorithm();
            InitializeCosts(start);
            return fillAlgorithm.Fill(start, searchDepth, agentTraversability);
        }

        private void InitializeCosts(IPathfindingNode start)
        {
            foreach (T node in _values)
            {
                node.PathCost = Mathf.Infinity;
            }
        }

        #endregion
        
        private int _width;
        private readonly T[] _values;
    }
}